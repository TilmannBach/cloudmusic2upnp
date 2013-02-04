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
            WebSocketManager.ClientDisconnect += HandleClientDisconnect;
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


        private void HandleClientConnect(object manager, ClientEventArgs args)
        {
            Utils.Logger.Log("Web client connected.");
            var client = args.Client;
            Clients.Add(client);

            client.SendMessage(new Protocol.ProviderNotification(Providers));
            client.SendMessage(new Protocol.DeviceNotification(Controller));
        }


        void HandleClientDisconnect(object sender, ClientEventArgs e)
        {
            Utils.Logger.Log("Web client disconnected.");
            Clients.Remove(e.Client);
        }


        private void HandleClientMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.GetType() == typeof(SearchRequest))
            {
                HandleSearchRequest(e.Client, (SearchRequest)e.Message);

            } else if (e.Message.GetType() == typeof(PlayRequest))
            {
                HandlePlayRequest(e.Client, (PlayRequest)e.Message);
            }
        }


        private void HandleSearchRequest(IWebClient client, SearchRequest request)
        {
            Utils.Logger.Log("Requested search for: '" + request.Query + "'.");

            var tracks = Providers.Plugins ["Soundcloud"].Search(request.Query);
            var response = new SearchResponse(request.Query, tracks);
            client.SendMessage(response);
        }

        private void HandlePlayRequest(IWebClient client, PlayRequest request)
        {
            Utils.Logger.Log("Requested play for: '" + request.MediaUrl + "'.");

            foreach (var device in Controller.GetDevices())
            {
                device.SetMediaUrl(new Uri(request.MediaUrl));
                device.Play();
            }
        }

    }
}