using System;

namespace cloudmusic2upnp.UserInterface.Web
{
    public class Interface : IInterface
    {
        private const int WEBSOCKET_PORT = 5009;


        private WebSocketManger WebSocket;
        public Http.WebServer WebServer;

        private DeviceController.IController Controller;
        private ContentProvider.Providers Providers;

        public event EventHandler InterfaceShutdownRequest;

        public Interface(DeviceController.IController controller,
                          ContentProvider.Providers providers)
        {
            Controller = controller;
            Providers = providers;

            WebSocket = new WebSocketManger(WEBSOCKET_PORT);
        }


        public void Start()
        {
            WebSocket.Start();

            WebServer = new Http.WebServer();
            WebServer.Start();
        }

        public void Stop()
        {
            WebSocket.Stop();
            WebServer.Stop();
        }
    }
}