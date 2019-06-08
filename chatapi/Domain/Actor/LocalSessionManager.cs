using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using chatapi.Controllers;
using chatapi.Dto;
using chatapi.Message.REST;
using chatapi.Message.WS;
using chatapi.Module.JSON;

namespace chatapi.Domain.Actor
{
    public class SessionSockInfo
    {
        public SessionInfo Session { get; set; }
        public string SockID { get; set; }
        public CSChatHandler SockHandeler { get; set; }
    }

    public class LocalSessionManager : ReceiveActor
    {
        private Dictionary<string, SessionSockInfo> sessionList = new Dictionary<string, SessionSockInfo>();
        
        protected SessionSockInfo AddSockInfo(string sessionKey,SessionSockInfo sessionSockInfo)
        {
            sessionList[sessionKey] = sessionSockInfo;
            return sessionSockInfo;
        }

        protected SessionSockInfo GetSockInfo(string sessionKey)
        {
            SessionSockInfo result = null; ;
            if (sessionList.ContainsKey(sessionKey))
            {
                result =  sessionList[sessionKey];
            }
            return result;
        }


        public LocalSessionManager()
        {
            Receive<BaseSocketData>(command =>
            {
                if (command.Pid.Equals(WSMsg.LoginInfo))
                {
                    LoginInfo loginInfo = JSONConvert.JsonToObject<LoginInfo>(command.Data);
                    if (loginInfo!=null)
                    {
                        // 인증된 사용자만이 채팅방 개설가능....
                        if(loginInfo.session.Channel.Contains("test") && loginInfo.session.ClientId.Contains("psmon"))
                        {
                            string SessionKey = $"{loginInfo.session.Channel}_{loginInfo.session.ClientId}";
                            SessionSockInfo sessionSock = new SessionSockInfo
                            {
                                Session = loginInfo.session,
                                SockID = command.SocketId,
                                SockHandeler = command.SocketHandler as CSChatHandler
                            };
                            
                            if (sessionList.ContainsKey(SessionKey))
                            {
                                SessionSockInfo sesionSocket = GetSockInfo(SessionKey);
                                sesionSocket.SockHandeler.SendMessageAsync(sesionSocket.SockID, "duplicated login - logout");                                
                            }
                            AddSockInfo(SessionKey,sessionSock).SockHandeler.SendMessageAsync(command.SocketId, "auth-OK");                
                        }
                    }
                }
                else if(command.Pid.Equals(WSMsg.DisconnectInfo))
                {

                }                
            });

            Receive<UserChatData>(command =>
            {
                string SessionKey = $"{command.Session.Channel }_{command.Session.ClientId}";               
                if (sessionList.ContainsKey(SessionKey))
                {
                    SessionSockInfo sesionSocket = GetSockInfo(SessionKey);
                    sesionSocket.SockHandeler.SendMessageAsync(sesionSocket.SockID, command.Message);
                }
            });


        }        
    }
}
