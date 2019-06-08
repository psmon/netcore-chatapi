using Akka.Actor;
using chatapi.Dto;
using chatapi.Message.WS;
using chatapi.Module.JSON;
using chatapi.Module.WebSocketManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static chatapi.ActorProviders;

namespace chatapi.Controllers
{
    public class CSChatHandler : WebSocketHandler
    {
        IActorRef _sessionManagerActor;
        bool _tipMode;

        public CSChatHandler(WebSocketConnectionManager webSocketConnectionManager, SessionManagerProvider sessionManager) : base(webSocketConnectionManager)
        {
            _sessionManagerActor = sessionManager();
            _tipMode = true; //웹소켓 응답해야할 가이드가 웹소켓모드로 작동됨 -개발모드
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var socketId = WebSocketConnectionManager.GetId(socket);
            
            BaseSocketData baseSocketData = new BaseSocketData()
            {
                Pid= WSMsg.ConnectInfo,
                Data=""
            };
            await SendMessageAsync(socketId, JSONConvert.ObjectToJson<BaseSocketData>(baseSocketData));

            if (_tipMode)
            {
                SessionInfo sessionInfo = new SessionInfo()
                {
                    Channel = "test",
                    ClientId = "psmon"
                };
                LoginInfo loginInfo = new LoginInfo()
                {
                    session = sessionInfo
                };
                BaseSocketData tipLoginData = new BaseSocketData()
                {
                    Pid = WSMsg.LoginInfo,
                    Data = JSONConvert.ObjectToJson<LoginInfo>(loginInfo)
                };

                await SendMessageAsync(socketId, JSONConvert.ObjectToJson<BaseSocketData>(tipLoginData));
            }
            
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);           
            BaseSocketData socketData = new BaseSocketData();
            socketData.Pid = WSMsg.DisconnectInfo;          
            socketData.SocketHandler = this;
            socketData.SocketId = socketId;
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            var socketDataString = $"{Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            var message = "";
            BaseSocketData socketData = new BaseSocketData();
            if (socketData.ReadToObject(socketDataString))
            {
                socketData.SocketHandler = this;
                socketData.SocketId = socketId;
                _sessionManagerActor.Tell(socketData);
                message = $"{socketId} said Json : {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            }
            else
            {
                message = $"{socketId} said PlainText(Bad Request) : {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            }            
            //await SendMessageToAllAsync(message);
        }
    }
}
