using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatapi.Dto
{
    public enum RoomStatus { Unassigned, Assigned, Close }

    public class RoomInfo
    {
        public SessionInfo Session { get; set; }

        public String Name { get; set; }

        public RoomStatus Status { get; set; }
        
    }
}
