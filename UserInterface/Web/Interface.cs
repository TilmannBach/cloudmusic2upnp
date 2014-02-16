using System;
using System.Collections.Generic;

using cloudmusic2upnp.UserInterface.Web.Protocol;
using cloudmusic2upnp;
using cloudmusic2upnp.DeviceController;

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
        private Dictionary<IWebClient,IDevice> Clients;
        private Dictionary<String,IDevice> KnownDevices;


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
            Clients = new Dictionary<IWebClient, IDevice>();
            KnownDevices = new Dictionary<string, IDevice>();
            
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
            foreach (KeyValuePair<IWebClient,IDevice> client in Clients)
            {
                client.Key.SendMessage(message);
            }
        }


        /*
         * Eventhandler
         */
        private void HandleDeviceDiscovery(object sender, cloudmusic2upnp.DeviceController.DeviceEventArgs e)
        {
            SendMessageAll(new Protocol.DeviceNotification(Controller));

            if (e.Action == DeviceController.DeviceEventArgs.DeviceEventActions.Added)
            {
                KnownDevices.Add(e.Device.Udn, e.Device);
                foreach (IWebClient webClient in new LinkedList<IWebClient>(Clients.Keys))
                {
                    if (Clients[webClient] == null)
                    {
                        Clients[webClient] = e.Device;
                        webClient.SendMessage(new Protocol.SelectDeviceNotification(e.Device));
                    }
                }
            }
            else
            {
                KnownDevices.Remove(e.Device.Udn);
                foreach (IWebClient webClient in new LinkedList<IWebClient>(Clients.Keys))
                {
                    if (Clients[webClient] == e.Device)
                    {
                        if (KnownDevices.Count > 0)
                        {
                            Clients[webClient] = new LinkedList<IDevice>(KnownDevices.Values).Last.Value;
                            webClient.SendMessage(new Protocol.SelectDeviceNotification(e.Device));
                        }
                        else
                            Clients[webClient] = null;
                    }
                }
            }
            e.Device.MuteChanged += Device_MuteChanged;
            e.Device.VolumeChanged += Device_VolumeChanged;
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
            lock (KnownDevices)
            {
                if (KnownDevices.Count != 0)
                {
                    Clients.Add(client, new LinkedList<IDevice>(KnownDevices.Values).Last.Value);
                    client.SendMessage(new Protocol.SelectDeviceNotification(new LinkedList<IDevice>(KnownDevices.Values).Last.Value));
                }
                else
                    Clients.Add(client, null);
            }
            client.SendMessage(new Protocol.ProviderNotification(Providers));
            client.SendMessage(new Protocol.DeviceNotification(Controller));
            client.SendMessage(new Protocol.PlaylistNotification(Playlist.Active));
        }


        private void HandleClientDisconnect(IWebClient client)
        {
            Utils.Logger.Log("Web client disconnected.");
            Clients.Remove(client);
        }

        #region Handle incoming web requests

        private void HandleClientMessage(IWebClient client, Message message)
        {
            if (message.GetType() == typeof(SearchRequest))
                OnSearchRequest(client, (SearchRequest)message);
            else if (message.GetType() == typeof(PlayRequest))
                OnPlayRequest(client, (PlayRequest)message);
            else if (message.GetType() == typeof(SetMuteRequest))
            {
                HandleOnSetMuteRequest((SetMuteRequest)message, client);
            }
            else if (message.GetType() == typeof(SelectDeviceRequest))
                Clients[client] = KnownDevices[((SelectDeviceRequest)message).Udn];
            else if (message.GetType() == typeof(PlaylistRemoveItemRequest))
                Playlist.Active.Remove(((PlaylistRemoveItemRequest)message).PlaylistItem);
        }

        private void HandleOnSetMuteRequest(SetMuteRequest request, IWebClient client)
        {
            Utils.Logger.Log("Requested to change mute state to: '" + request.SetMuted + "'.");
            if (Clients[client] != null)
            {
                if (request.SetMuted)
                    Clients[client].Mute();
                else
                    Clients[client].Unmute();
            }
            else
            {
                Utils.Logger.Log(Utils.Logger.Level.Warning, "WebClient does not control any devices - no mute state was changed!");
            }
        }

        #endregion
    }
}