using System;

namespace cloudmusic2upnp.UserInterface.Web
{
    public class Interface : IInterface
    {
        private const int WEBSOCKET_PORT = 5009;


        private WebSocket.Manger WebSocketManager;
        public Http.WebServer WebServer;

        private DeviceController.IController Controller;
        private ContentProvider.Providers Providers;

        public event EventHandler InterfaceShutdownRequest;

        public Interface(DeviceController.IController controller,
                          ContentProvider.Providers providers)
        {
            Controller = controller;
            Providers = providers;

            WebSocketManager = new WebSocket.Manger(WEBSOCKET_PORT);
            WebSocketManager.OnConnectionOpen += HandleConnectionOpen;
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


        public void HandleConnectionOpen(object manager, ConnectionOpenEventArgs args)
        {
            Logger.Log("Got new web connection.");
        }

    }
}