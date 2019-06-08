using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatapi.Dto
{
    public class SessionInfo
    {
        public string Channel { get; set; }
        public string SubChannel { get; set; }
        public long RoomNo { get; set; }
        public string ClientType { get; set; }
        public string ClientId { get; set; }
    }
}
