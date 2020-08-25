//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2020 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2020 .NET Foundation <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Configuration;
using ChatMessages;

namespace ChatServer
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
            port = 8081
            hostname = 0.0.0.0
            public-hostname = localhost
        }
    }
}
");

            using (var system = ActorSystem.Create("MyServer", config))
            {
                system.ActorOf(Props.Create(() => new ChatServerActor()), "ChatServer");

                Console.ReadLine();
            }
        }
    }

    class ChatServerActor : ReceiveActor, ILogReceive
    {
        private readonly ActorSelection _proxy = Context.ActorSelection("akka.tcp://AkkaProxy@localhost:9090/user/ChatProxy");
        private readonly HashSet<string> _clients = new HashSet<string>();

        public ChatServerActor()
        {
            Receive<SayRequest>(message =>
            {
                foreach (var client in _clients)
                {
                    var response = new SayResponse
                    {
                        Username = message.Username,
                        Text = message.Text,
                        Target = client
                    };
                    _proxy.Tell(response, Self);
                }
            });

            Receive<ConnectRequest>(message =>
            {
                _clients.Add(message.Sender);
                _proxy.Tell(new ConnectResponse
                {
                    Message = "Hello and welcome to Akka.NET chat example",
                    Target = message.Sender
                }, Self);
            });

            Receive<NickRequest>(message =>
            {
                foreach (var client in _clients)
                {
                    var response = new NickResponse
                    {
                        OldUsername = message.OldUsername,
                        NewUsername = message.NewUsername,
                        Target = client
                    };

                    _proxy.Tell(response, Self);
                }
            });
        }
    }
}

