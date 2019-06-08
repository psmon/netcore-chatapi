using chatapi.Module.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace chatapi.Message.WS
{
    public static class WSMsg
    {
        static public  string ConnectInfo = "ConnectInfo";
        static public string DisconnectInfo = "DisconnectInfo";
        static public  string LoginInfo = "LoginInfo";
    }

    public class BaseSocketData
    {
        public string Pid { get; set; }
        public string Data { get; set; }

        public string SocketId { get; set; }
        public object SocketHandler { get; set; }
        
        public bool ReadToObject(string json)
        {
            bool isJsonData = false;
            BaseSocketData readData = JSONConvert.JsonToObject<BaseSocketData>(json);
            if (readData != null)
            {
                this.Pid = readData.Pid;
                this.Data = readData.Data;
                isJsonData = true;
            }           
            return isJsonData;
        }

    }
}
