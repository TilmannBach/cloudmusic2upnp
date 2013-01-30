// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.38967
//    <NameSpace>cloudmusic2upnp</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>False</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net40</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>False</GenerateXMLAttributes><EnableEncoding>False</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>False</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace cloudmusic2upnp.DeviceController.UPnP.AvtEvent
{
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.Collections.Generic;


    public partial class rootType
    {

        private List<InstanceIDtype> itemsField;

        public rootType()
        {
            this.itemsField = new List<InstanceIDtype>();
        }

        public List<InstanceIDtype> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    public partial class InstanceIDtype
    {

        private List<object> itemsField;

        private List<ItemsChoiceType> itemsElementNameField;

        private uint valField;

        public InstanceIDtype()
        {
            this.itemsElementNameField = new List<ItemsChoiceType>();
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public List<ItemsChoiceType> ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }

        public uint val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class AVTransportURItype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class DRMStatetype
    {

        private DRMStatetypeVal valField;

        public DRMStatetypeVal val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public enum DRMStatetypeVal
    {

        /// <remarks/>
        OK,

        /// <remarks/>
        UNKNOWN,

        /// <remarks/>
        PROCESSING_CONTENT_KEY,

        /// <remarks/>
        CONTENT_KEY_FAILURE,

        /// <remarks/>
        ATTEMPTING_AUTHENTICATION,

        /// <remarks/>
        FAILED_AUTHENTICATION,

        /// <remarks/>
        NOT_AUTHENTICATED,

        /// <remarks/>
        DEVICE_REVOCATION,
    }

    public partial class restrictedUnsignedIntVal
    {

        private uint valField;

        public uint val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class intVal
    {

        private int valField;

        public int val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class CurrentTransportActionstype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class NextAVTransportURItype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class CurrentTrackURItype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class CurrentTrackMetadatatype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class durationtype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class CurrentTracktype
    {

        private uint valField;

        public uint val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class NumberOfTrackstype
    {

        private uint valField;

        public uint val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class CurrentRecordQualityModetype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class RecordMediumWriteStatustype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class TransportPlaySpeedtype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class CurrentPlayModetype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class storageMediumtype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class CurrentMediaCategorytype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class TransportStatustype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class TransportStatetype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public enum ItemsChoiceType
    {

        /// <remarks/>
        AVTransportURI,

        /// <remarks/>
        AbsoluteCounterPosition,

        /// <remarks/>
        AbsoluteTimePosition,

        /// <remarks/>
        CurrentMediaCategory,

        /// <remarks/>
        CurrentMediaDuration,

        /// <remarks/>
        CurrentPlayMode,

        /// <remarks/>
        CurrentRecordQualityMode,

        /// <remarks/>
        CurrentTrack,

        /// <remarks/>
        CurrentTrackDuration,

        /// <remarks/>
        CurrentTrackMetadata,

        /// <remarks/>
        CurrentTrackURI,

        /// <remarks/>
        CurrentTransportActions,

        /// <remarks/>
        DRMState,

        /// <remarks/>
        NextAVTransportURI,

        /// <remarks/>
        NumberOfTracks,

        /// <remarks/>
        PlaybackStorageMedium,

        /// <remarks/>
        PossiblePlaybackStorageMedia,

        /// <remarks/>
        PossibleRecordQualityMode,

        /// <remarks/>
        PossibleRecordStorageMedia,

        /// <remarks/>
        RecordMediumWriteStatus,

        /// <remarks/>
        RecordStorageMedium,

        /// <remarks/>
        RelativeCounterPosition,

        /// <remarks/>
        RelativeTimePosition,

        /// <remarks/>
        TransportPlaySpeed,

        /// <remarks/>
        TransportState,

        /// <remarks/>
        TransportStatus,
    }

    public partial class AVTransportURIMetaDatatype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class NextAVTransportURIMetaDatatype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }

    public partial class nametype
    {

        private string valField;

        public string val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }
}