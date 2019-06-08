using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Akka.Actor;
using static chatapi.ActorProviders;
using chatapi.Message.REST;

namespace chatapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatPushController : ControllerBase
    {
        private readonly IActorRef _sessionManagerActor;

        public ChatPushController(SessionManagerProvider sessionManager)
        {
            _sessionManagerActor = sessionManager();
        }

        // POST api/values
        [HttpPost]
        public void PostChat(UserChatData value)
        {
            _sessionManagerActor.Tell(value);
        }
    }
}