using System;
using System.Net.Sockets;
using System.Collections.Generic;

using Bauglir.Ex;


namespace cloudmusic2upnp.UserInterface.Web.WebSocket
{
    public class HackedWebSocketServer : WebSocketServer
    {
        public override WebSocketServerConnection GetConnectionInstance(
		    TcpClient aClient, WebSocketHeaders aHeaders, string aHost,
			string aPort, string aResourceName, string aOrigin, string aCookie,
			string aVersion, ref string aProtocol, ref string aExtension,
		    ref int aHttpCode
        )
        {
            aProtocol = "-";
            aExtension = "-";
            return new WebSocketServerConnection(aClient, this);
        }
    }


    public class Manger : IWebManager
    {
        private HackedWebSocketServer Server;
        private int Port;
        private Dictionary<WebSocketConnection, Client> Clients;

        public event EventHandler<ConnectionOpenEventArgs> OnConnectionOpen;
        public event EventHandler<ConnectionReadEventArgs> OnConnectionRead;


        public Manger(int port)
        {
            Port = port;
            Clients = new Dictionary<WebSocketConnection, Client>();

            Server = new HackedWebSocketServer();
            Server.AfterAddConnection += HandleAfterAddConnection;
        }


        public void Start()
        {
            Server.Start(System.Net.IPAddress.Any, Port);
        }


        public void Stop()
        {
            Server.Stop();
        }


        private void HandleAfterAddConnection(WebSocketServer aServer, WebSocketServerConnection aConnection)
        {
            aConnection.ConnectionOpen += HandleConnectionOpen;
            aConnection.ConnectionRead += HandleConnectionRead;
        }


        private void HandleConnectionOpen(WebSocketConnection aConnection)
        {
            var client = new Client(aConnection);
            Clients.Add(aConnection, client);
            OnConnectionOpen(this, new ConnectionOpenEventArgs(client));
        }


        void HandleConnectionRead(WebSocketConnection aConnection, bool aFinal, bool aRes1, bool aRes2, bool aRes3, int aCode, System.IO.MemoryStream aData)
        {
            var client = Clients [aConnection];
            OnConnectionRead(this, new ConnectionReadEventArgs(client, aData));
        }
    }
}

