using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;

namespace chatapi
{
    public class ActorProviders
    {
        public delegate IActorRef SessionManagerProvider();
    }
}
