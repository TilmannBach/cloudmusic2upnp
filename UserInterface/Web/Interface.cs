using System;
using System.Collections.Generic;

using cloudmusic2upnp.UserInterface.Web.Protocol;

namespace cloudmusic2upnp.UserInterface.Web
{
    public class Interface : IInterface
    {
        private const int WEBSOCKET_PORT = 5009;


        private WebSocket.Manger WebSocketManager;
        public Http.WebServer WebServer;

        private DeviceController.IController Controller;
        private ContentProvider.Providers Providers;

        private List<IWebClient> Clients;

        public event EventHandler InterfaceShutdownRequest;


        public Interface(DeviceController.IController controller,
                          ContentProvider.Providers providers)
        {
            Controller = controller;
            Providers = providers;
            Clients = new List<IWebClient>();

            WebSocketManager = new WebSocket.Manger(WEBSOCKET_PORT);
            WebSocketManager.ClientConnect += HandleClientConnect;
            WebSocketManager.ClientMessage += HandleClientMessage;

            Controller.DeviceDiscovery += HandleDeviceDiscovery;
        }


        public void Start()
        {
            WebSocketManager.Start();

            WebServer = new Http.WebServer();
            WebServer.Start();
        }


        public void Stop()
        {
            WebSocketManager.Stop();
            WebServer.Stop();
        }


        public void SendMessageAll(Protocol.Message message)
        {
            foreach (IWebClient client in Clients)
            {
                client.SendMessage(message);
            }
        }


        private void HandleDeviceDiscovery(object sender, cloudmusic2upnp.DeviceController.DeviceEventArgs e)
        {
            SendMessageAll(new Protocol.DeviceNotification(Controller));
        }


        private void HandleClientConnect(object manager, ClientConnectEventArgs args)
        {
            Utils.Logger.Log("Got new web connection.");
            var client = args.Client;
            Clients.Add(client);

            client.SendMessage(new Protocol.ProviderNotification(Providers));
            client.SendMessage(new Protocol.DeviceNotification(Controller));
        }


        private void HandleClientMessage(object sender, ClientMessageEventArgs e)
        {
            if (e.Message.GetType() == typeof(SearchRequest))
            {
                HandleSearchRequest((SearchRequest)e.Message);
            }
        }


        private void HandleSearchRequest(SearchRequest request)
        {
            Utils.Logger.Log("Requested search request for: '" + request.Query + "'.");
        }


    }
}