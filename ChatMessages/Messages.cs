//-----------------------------------------------------------------------
// <copyright file="Messages.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2020 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2020 .NET Foundation <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using Akka.Actor;

namespace ChatMessages
{
    public abstract class BaseProxyRequestMessage
    {
        public string Sender { get; set; }
    }

    public abstract class BaseProxyResponseMessage
    {
        public string Target { get; set; }
    }

    public class ConnectRequest : BaseProxyRequestMessage
    {
        public string Username { get; set; }
    }

    public class ConnectResponse : BaseProxyResponseMessage
    {
        public string Message { get; set; }
    }

    public class NickRequest : BaseProxyRequestMessage
    {
        public string OldUsername { get; set; }
        public string NewUsername { get; set; }
    }

    public class NickResponse : BaseProxyResponseMessage
    {
        public string OldUsername { get; set; }
        public string NewUsername { get; set; }
    }

    public class SayRequest : BaseProxyRequestMessage
    {
        public string Username { get; set; }
        public string Text { get; set; }
    }

    public class SayResponse : BaseProxyResponseMessage
    {
        public string Username { get; set; }
        public string Text { get; set; }
    }
}
