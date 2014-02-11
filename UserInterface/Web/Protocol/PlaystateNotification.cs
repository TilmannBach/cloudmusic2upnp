using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;

using cloudmusic2upnp.ContentProvider;
using cloudmusic2upnp.DeviceController;

namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    public class DeviceStateNotification : Message
    {
        [DataMember]
        public String MuteActive { get; private set; }

        /// <summary>
        /// Volume of the master channel as integer from 0 to 100
        /// </summary>
        [DataMember]
        public String VolumeMaster { get; private set; }

        [DataMember]
        public String ShuffleActive { get; private set; }

        [DataMember]
        public String RepeatActive { get; private set; }

        public DeviceStateNotification(DeviceState playstate)
        {
            MuteActive = playstate.muteActive;
            VolumeMaster = playstate.volumeMaster;
            ShuffleActive = playstate.shuffleActive;
            RepeatActive = playstate.repeatActive;
        }

        public override String ToJson()
        {
            return Header<DeviceStateNotification>.ToJson(this);
        }
    }


    public class DeviceState
    {
        public String muteActive;
        public String shuffleActive;
        public String repeatActive;
        public String volumeMaster;
    }
}

