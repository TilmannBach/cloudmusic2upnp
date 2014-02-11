using System;
using System.Collections.Generic;

using cloudmusic2upnp.UserInterface.Web.Protocol;
using cloudmusic2upnp;

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
        private List<cloudmusic2upnp.DeviceController.IDevice> KnownDevices;


        public Http.WebServer WebServer;


        /*
         * Events
         */
        public event Action<IWebClient, SearchRequest> OnSearchRequest;
        public event Action<IWebClient, PlayRequest> OnPlayRequest;


        /*
         * Methods
         */
        public Interface(DeviceController.IController controller,
                          ContentProvider.Providers providers)
        {
            Controller = controller;
            Providers = providers;
            Clients = new List<IWebClient>();
            KnownDevices = new List<DeviceController.IDevice>();


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
            if (e.Action == DeviceController.DeviceEventArgs.DeviceEventActions.Added)
                KnownDevices.Add(e.Device);
            else
                KnownDevices.Remove(e.Device);
            e.Device.MuteChanged += Device_MuteChanged;
            e.Device.VolumeChanged += Device_VolumeChanged;
            SendMessageAll(new Protocol.DeviceNotification(Controller));
        }

        void Device_VolumeChanged(object sender, DeviceController.DeviceVolumeEventArgs e)
        {
            DeviceState _playState = new DeviceState();
            _playState.volumeMaster = e.Volume.ToString();
            SendMessageAll(new Protocol.DeviceStateNotification(_playState));
        }

        void Device_MuteChanged(object sender, DeviceController.DeviceMuteEventArgs e)
        {
            DeviceState _playState = new DeviceState();
            _playState.muteActive = (e.MuteState == DeviceController.DeviceMuteEventArgs.MuteStates.Muted) ? "true" : "false";
            SendMessageAll(new Protocol.DeviceStateNotification(_playState));
        }


        private void HandleClientConnect(IWebClient client)
        {
            Utils.Logger.Log("Web client connected.");
            Clients.Add(client);

            client.SendMessage(new Protocol.ProviderNotification(Providers));
            client.SendMessage(new Protocol.DeviceNotification(Controller));
            client.SendMessage(new Protocol.PlaylistNotification(Playlist.Active));
        }


        private void HandleClientDisconnect(IWebClient client)
        {
            Utils.Logger.Log("Web client disconnected.");
            Clients.Remove(client);
        }


        private void HandleClientMessage(IWebClient client, Message message)
        {
            if (message.GetType() == typeof(SearchRequest))
                OnSearchRequest(client, (SearchRequest)message);
            else if (message.GetType() == typeof(PlayRequest))
                OnPlayRequest(client, (PlayRequest)message);
            else if (message.GetType() == typeof(SetMuteRequest))
            {
                HandleOnSetMuteRequest((SetMuteRequest)message);
            }

        }

        private void HandleOnSetMuteRequest(SetMuteRequest request)
        {
            Utils.Logger.Log("Requested to change mute state to: '" + request.SetMuted + "'.");
            foreach(cloudmusic2upnp.DeviceController.IDevice device in KnownDevices)
            {
                if (request.SetMuted)
                    device.Mute();
                else
                    device.Unmute();
            }
        }
    }
}