using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudmusic2upnp.DeviceController
{
    public class DeviceMuteEventArgs : EventArgs
    {
        public DeviceMuteEventArgs(MuteStates muteState)
        {
            MuteState = muteState;
        }

        public MuteStates MuteState;

        public enum MuteStates { Muted, UnMuted }
    }
}
