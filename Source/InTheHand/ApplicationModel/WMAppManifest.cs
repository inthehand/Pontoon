// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WMAppManifest.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides information about a Silverlight app package.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Windows;
using System.Xml;
using Windows.ApplicationModel;

namespace InTheHand.ApplicationModel
{
    internal sealed class WMAppManifest
    {
        public WMAppManifest()
        {
            // parse the xml manifest once

            // parse the package xml to get attributes
            XmlReader xr = XmlReader.Create("WMAppManifest.xml");
            if (xr.ReadToDescendant("Deployment"))
            {
                // if built as an 8.1 app defer to platform for most properties
                IsSilverlight81 = new Version(xr["AppPlatformVersion"]) > new Version(8, 0);


                /*if (isEightPointOne)
                {
                    this.Id = new PackageId(nativeCurrent.Id);
                }
                else
                {
                    this.Id = new PackageId();
                }*/


                if (xr.ReadToDescendant("App"))
                {
                    ProductID = xr["ProductID"];
                    DisplayName = xr["Title"];
                    Description = xr["Description"];

                    if (DisplayName.StartsWith("@"))
                    {
                        try
                        {
                            object[] attributes = Assembly.Load(Deployment.Current.EntryPointAssembly).GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                            if (attributes.Length > 0)
                            {
                                AssemblyProductAttribute productAttribute = attributes[0] as AssemblyProductAttribute;
                                if (productAttribute != null)
                                {
                                    DisplayName = productAttribute.Product;
                                }
                            }
                        }
                        catch
                        {
                            // will fail from background task
                        }
                    }


                    
                    Version = new Version(xr["Version"]).ToPackageVersion();

                    PublisherDisplayName = xr["Publisher"];
                    PublisherId = xr["PublisherID"];
                    
                    
                    if (xr.ReadToDescendant("IconPath"))
                    {
                        Logo = ImagePathToUri(xr);
                    }

                    if (xr.ReadToFollowing("Capabilities"))
                    {
                        bool more = xr.ReadToDescendant("Capability");
                        while (more)
                        {
                            Capability newCapability = (Capability)0;
                            DeviceCapability newDeviceCapability = (DeviceCapability)0;
                            switch (xr["Name"])
                            {
                                case "ID_CAP_NETWORKING":
                                    newCapability = Capability.Internet;
                                    break;
                                /*case "ID_CAP_IDENTITY_DEVICE":
                                    newCapability = Capability.IdentityDevice;
                                    break;
                                case "ID_CAP_IDENTITY_USER":
                                    newCapability = Capability.IdentityUser;
                                    break;
                                case "ID_FUNCCAP_EXTEND_MEM":
                                    newCapability = Capability.ExtendedMemory;
                                    break;*/
                                case "ID_CAP_LOCATION":
                                    newDeviceCapability = DeviceCapability.Location;
                                    break;
                                case "ID_CAP_SENSORS":
                                    newDeviceCapability = DeviceCapability.Sensors;
                                    break;
                                case "ID_CAP_MICROPHONE":
                                    newDeviceCapability = DeviceCapability.Microphone;
                                    break;
                                //case "ID_CAP_MEDIALIB":
                                //    newCapability = Capability.MediaLibrary;
                                //    break;
                                case "ID_CAP_MEDIALIB_AUDIO":
                                    newCapability = Capability.Music;
                                    break;
                                case "ID_CAP_MEDIALIB_PHOTO":
                                    newCapability = Capability.Pictures;
                                    break;
                                /*case "ID_CAP_MEDIALIB_PLAYBACK":
                                    newCapability = Capability.MusicLibraryPlayback;
                                    break;
                                case "ID_CAP_GAMERSERVICES":
                                    newCapability = Capability.GamerServices;
                                    break;*/
                                case "ID_CAP_PHONEDIALER":
                                    newCapability = Capability.PhoneDialer;
                                    break;
                                case "ID_CAP_PUSH_NOTIFICATION":
                                    newCapability = Capability.PushNotification;
                                    break;
                                case "ID_CAP_REMOVABLE_STORAGE":
                                    newCapability = Capability.RemovableStorage;
                                    break;
                                /*case "ID_CAP_WEBBROWSERCOMPONENT":
                                    newCapability = Capability.WebBrowserComponent;
                                    break;
                                case "ID_CAP_RINGTONE_ADD":
                                    newCapability = Capability.RingtoneAdd;
                                    break;*/
                                case "ID_CAP_PROXIMITY":
                                    newDeviceCapability = DeviceCapability.Proximity;
                                    break;
                                case "ID_CAP_SPEECH_RECOGNITION":
                                    newCapability = Capability.SpeechRecognition;
                                    break;
                                case "ID_CAP_VOIP":
                                    newCapability = Capability.Voip;
                                    break;

                                case "ID_CAP_WALLET":
                                    newCapability = Capability.Wallet;
                                    break;
                                /*case "ID_CAP_WALLET_PAYMENTINSTRUMENTS":
                                    newCapability = Capability.WalletPaymentInstruments;
                                    break;
                                case "ID_CAP_WALLET_SECUREELEMENT":
                                    newCapability = Capability.WalletSecureElement;
                                    break;*/

                                case "ID_CAP_APPOINTMENTS":
                                    newCapability = Capability.Appointments;
                                    break;
                                case "ID_CAP_CONTACTS":
                                    newCapability = Capability.Contacts;
                                    break;
                                case "ID_CAP_ISV_CAMERA":
                                    newDeviceCapability = DeviceCapability.Camera;
                                    break;

                            }

                            global::System.Diagnostics.Debug.WriteLine((int)newCapability);
                            Capabilities |= newCapability;
                            DeviceCapabilities |= newDeviceCapability;
                            more = xr.ReadToNextSibling("Capability");
                        }
                    }
                }
            }

            xr.Dispose();
        }

        public bool IsSilverlight81
        {
            get;
            private set;
        }

        public Capability Capabilities
        {
            get;
            private set;
        }

        public DeviceCapability DeviceCapabilities
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public string ProductID
        {
            get;
            private set;
        }

        public Uri Logo
        {
            get;
            private set;
        }

        public string PublisherDisplayName
        {
            get;
            private set;
        }

        public string PublisherId
        {
            get;
            private set;
        }

        public PackageVersion Version
        {
            get;
            private set;
        }

        private static Uri ImagePathToUri(XmlReader reader)
        {
            UriKind kind = bool.Parse(reader["IsRelative"]) ? UriKind.Relative : UriKind.Absolute;
            reader.MoveToElement();
            string path = reader.ReadElementContentAsString().Replace('\\', '/');
            return new Uri(path, kind);
        }
    }
}
