using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudmusic2upnp.DeviceController
{
    public class DeviceEventArgs : EventArgs
    {
        public DeviceEventArgs(IDevice device, DeviceEventActions action)
        {
            Device = device;
            Action = action;
        }
        public IDevice Device;

        public DeviceEventActions Action;
        public enum DeviceEventActions {
            Added = 0,
            Removed = 1
        }
    }
}
