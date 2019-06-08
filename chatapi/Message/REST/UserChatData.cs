using chatapi.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace chatapi.Message.REST
{
    public enum ChatMessageType
    {
        [Description("New")]
        New,
        [Description("Continue")]
        Continue,
        [Description("Close")]
        Close
    }

    public class UserChatData
    {
        public SessionInfo Session { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        
    }

}
