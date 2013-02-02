using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudmusic2upnp.DeviceController
{
    public class DeviceVolumeEventArgs : EventArgs
    {
        public DeviceVolumeEventArgs(int volume)
        {
            Volume = volume;
        }

        public int Volume;
    }
}
