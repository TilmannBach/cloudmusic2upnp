using System;
using System.Collections.Generic;

using cloudmusic2upnp.UserInterface.Web.Protocol;
using cloudmusic2upnp.Session;

namespace cloudmusic2upnp.UserInterface.Web
{
    public class Interface
    {
        /*
         * Properties
         */
        private const int WEBSOCKET_PORT = 5009;
        private WebSocket.Manger WebSocketManager;
        private DeviceController.IController Controller;
        private ContentProvider.Providers Providers;
        private List<IWebClient> Clients;


        public Http.WebServer WebServer;


        /*
         * Events
         */
        public event EventHandler InterfaceShutdownRequest;


        /*
         * Methods
         */
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


        /*
         * Eventhandler
         */
        private void HandleDeviceDiscovery(object sender, cloudmusic2upnp.DeviceController.DeviceEventArgs e)
        {
            SendMessageAll(new Protocol.DeviceNotification(Controller));
        }


        private void HandleClientConnect(IWebClient client)
        {
            Utils.Logger.Log("Web client connected.");
            Clients.Add(client);

            client.SendMessage(new Protocol.ProviderNotification(Providers));
            client.SendMessage(new Protocol.DeviceNotification(Controller));
        }


        private void HandleClientDisconnect(IWebClient client)
        {
            Utils.Logger.Log("Web client disconnected.");
            Clients.Remove(client);
        }


        private void HandleClientMessage(IWebClient client, Message message)
        {
            if (message.GetType() == typeof(SearchRequest))
            {
                HandleSearchRequest(client, (SearchRequest)message);

            } else if (message.GetType() == typeof(PlayRequest))
            {
                HandlePlayRequest(client, (PlayRequest)message);
            }
        }


        private void HandleSearchRequest(IWebClient client, SearchRequest request)
        {
            Utils.Logger.Log("Requested search for: '" + request.Query + "'.");

            var tracks = Providers.Plugins ["Soundcloud"].Search(request.Query);
            var response = new SearchResponse(request.Query, tracks);
            client.SendMessage(response);

            Utils.Logger.Log("Sent response for search for: '" + response.Query + "'.");
        }


        private void HandlePlayRequest(IWebClient client, PlayRequest request)
        {
            Utils.Logger.Log("Requested play for: '" + request.MediaUrl + "'.");

            foreach (var device in Controller.GetDevices())
            {
                Playlist.Active.Prepend(request.MediaUrl);
                device.SetMediaUrl(new Uri(request.MediaUrl));
                device.Play();
            }
        }

    }
}