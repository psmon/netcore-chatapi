using chatapi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatapi.Message
{
    public enum ChatEventType { New,Message,End };
    public enum ChatFrom { CS,CUSTOMER };
    public enum ChatContentType { Text,Image };
    public enum ChatProvider { KakaoI,NaverTokTok }

    public class ChatEvent : ActorEvent
    {
        public SessionInfo Session { get; set; }        
        public ChatEventType EventType { get; set; }
        public ChatFrom From { get; set; }
            
    }
}
