using System;
using Akka.Actor;
using Akka.Configuration;
using ChatMessages;

namespace ChatProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(@"
akka {  
    actor {
        provider = remote
    }
    remote {
        dot-netty.tcp {
            port = 9090
            hostname = 0.0.0.0
            public-hostname = localhost
        }
    }
}
");

            using (var system = ActorSystem.Create("AkkaProxy", config))
            {
                system.ActorOf(Props.Create(() => new ChatProxyActor()), "ChatProxy");

                Console.ReadLine();
            }
        }
    }

    class ChatProxyActor : ReceiveActor, ILogReceive
    {
        private readonly ActorSelection _server = Context.ActorSelection("akka.tcp://MyServer@localhost:8081/user/ChatServer");

        public ChatProxyActor()
        {
            Receive<BaseProxyRequestMessage>(message =>
            {
                message.Sender = Sender.Path.ToStringWithAddress();
                _server.Tell(message, Self);
            });

            Receive<BaseProxyResponseMessage>(message =>
            {
                Context.ActorSelection(message.Target).Tell(message, Self);
            });
        }
    }
}
