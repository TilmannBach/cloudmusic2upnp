using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudmusic2upnp.DeviceController
{
    public class DevicePlaystateEventArgs : EventArgs
    {
        public DevicePlaystateEventArgs(IDevice device, DevicePlaystate playstate, int timeOffset)
        {
            Device = device;
            Playstate = playstate;
            TimeOffset = timeOffset;
        }

        public IDevice Device;

        public DevicePlaystate Playstate;

        public int TimeOffset;

        public enum DevicePlaystate { Playing, Stopped, Paused, Unloaded, Loaded, ReachedEnd }
    }
}
