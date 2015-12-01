using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI;

namespace InTheHand.ApplicationModel
{
    internal sealed class AppxManifest
    {

        public AppxManifest()
        {
            Task t = Task.Run(async () =>
            {
                Uri u = new Uri("ms-appx:///AppxManifest.xml");
                var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(u);
                var stream = await file.OpenStreamForReadAsync();
                var streamReader = new StreamReader(stream);
                var xml = await streamReader.ReadToEndAsync();
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xml);

                //parse xml
                IXmlNode nameNode = xdoc.SelectSingleNodeNS("/appx:Package/appx:Properties/appx:DisplayName", "xmlns:appx=\"http://schemas.microsoft.com/appx/2010/manifest\"");
                if (nameNode != null)
                {
                    DisplayName = nameNode.InnerText;
                }
                IXmlNode publisherNode = xdoc.SelectSingleNodeNS("/appx:Package/appx:Properties/appx:PublisherDisplayName", "xmlns:appx=\"http://schemas.microsoft.com/appx/2010/manifest\"");
                if (publisherNode != null)
                {
                    PublisherDisplayName = publisherNode.InnerText;
                }
               
                IXmlNode visualElementsNode = xdoc.SelectSingleNodeNS("/appx:Package/appx:Applications/appx:Application/uap:VisualElements", "xmlns:appx=\"http://schemas.microsoft.com/appx/manifest/foundation/windows10\" xmlns:uap=\"http://schemas.microsoft.com/appx/manifest/uap/windows10\"");
                if (visualElementsNode == null)
                {
                    visualElementsNode = xdoc.SelectSingleNodeNS("/appx:Package/appx:Applications/appx:Application/m3:VisualElements", "xmlns:appx=\"http://schemas.microsoft.com/appx/2010/manifest\" xmlns:m3=\"http://schemas.microsoft.com/appx/2014/manifest\"");
                }
                if (visualElementsNode == null)
                {
                    visualElementsNode = xdoc.SelectSingleNodeNS("/appx:Package/appx:Applications/appx:Application/m2:VisualElements", "xmlns:appx=\"http://schemas.microsoft.com/appx/2010/manifest\" xmlns:m2=\"http://schemas.microsoft.com/appx/2013/manifest\"");
                }

                if (visualElementsNode != null)
                {
                    /*IXmlNode toastCapableNode = visualElementsNode.Attributes.GetNamedItem("ToastCapable");
                    if (toastCapableNode != null)
                    {
                        bool toastCapable = bool.Parse(toastCapableNode.InnerText);
                        if (toastCapable)
                        {
                            Capabilities |= Capability.PushNotification;
                        }
                    }*/

                    IXmlNode bgColorNode = visualElementsNode.Attributes.GetNamedItem("BackgroundColor");
                    if (bgColorNode != null)
                    {
                        string color = bgColorNode.InnerText;
                        if (color.StartsWith("#"))
                        {
                            // hex color
                            if (color.Length == 7)
                            {
                                var r = byte.Parse(color.Substring(1, 2), global::System.Globalization.NumberStyles.HexNumber);
                                var g = byte.Parse(color.Substring(3, 2), global::System.Globalization.NumberStyles.HexNumber);
                                var b = byte.Parse(color.Substring(5, 2), global::System.Globalization.NumberStyles.HexNumber);
                                BackgroundColor = Color.FromArgb(0xff, r, g, b);
                            }
                            else if (color.Length == 9)
                            {
                                var a = byte.Parse(color.Substring(1, 2), global::System.Globalization.NumberStyles.HexNumber);
                                var r = byte.Parse(color.Substring(3, 2), global::System.Globalization.NumberStyles.HexNumber);
                                var g = byte.Parse(color.Substring(5, 2), global::System.Globalization.NumberStyles.HexNumber);
                                var b = byte.Parse(color.Substring(7, 2), global::System.Globalization.NumberStyles.HexNumber);
                                BackgroundColor = Color.FromArgb(a, r, g, b);
                            }
                        }
                        else
                        {
                            // color name
                            foreach (PropertyInfo pi in typeof(Colors).GetRuntimeProperties())
                            {
                                if (pi.Name.ToLower() == color)
                                {
                                    BackgroundColor = (Color)pi.GetValue(null);
                                    break;
                                }
                            }
                        }
                    }
                    IXmlNode descriptionNode = visualElementsNode.Attributes.GetNamedItem("Description");
                    if(descriptionNode != null)
                    {
                        Description = descriptionNode.InnerText;
                    }
                    
                }

                IXmlNode logoNode = xdoc.SelectSingleNodeNS("/appx:Package/appx:Properties/appx:Logo", "xmlns:appx=\"http://schemas.microsoft.com/appx/2010/manifest\"");
                if (logoNode != null)
                {
                    Logo = new Uri("ms-appx:///" + logoNode.InnerText.Replace("\\", "/"));
                }

                IXmlNode capabilitiesNode = xdoc.SelectSingleNodeNS("/appx:Package/appx:Capabilities", "xmlns:appx=\"http://schemas.microsoft.com/appx/2010/manifest\"");
                if (capabilitiesNode != null)
                {
                    foreach (IXmlNode element in capabilitiesNode.ChildNodes)
                    {
                        if (element.NodeType == NodeType.ElementNode)
                        {
                            IXmlNode capNode = element.Attributes.GetNamedItem("Name");
                            string cap = capNode.InnerText;

                            switch (cap)
                            {
                                case "appointments":
                                    Capabilities |= Capability.Appointments;
                                    break;
                                case "contacts":
                                    Capabilities |= Capability.Contacts;
                                    break;


                                case "musicLibrary":
                                    Capabilities |= Capability.Music;
                                    break;
                                case "picturesLibrary":
                                    Capabilities |= Capability.Pictures;
                                    break;
                                case "videosLibrary":
                                    Capabilities |= Capability.Videos;
                                    break;


                                case "internetClient":
                                    Capabilities |= Capability.Internet;
                                    break;

                                case "internetClientServer":
                                    Capabilities |= Capability.Internet;
                                    break;


                                case "location":
                                    DeviceCapabilities |= DeviceCapability.Location;
                                    break;

                                case "proximity":
                                    DeviceCapabilities |= DeviceCapability.Proximity;
                                    break;

                                case "microphone":
                                    DeviceCapabilities |= DeviceCapability.Microphone;
                                    break;

                                case "webcam":
                                    DeviceCapabilities |= DeviceCapability.Camera;
                                    break;


                                case "removableStorage":
                                    Capabilities |= Capability.RemovableStorage;
                                    break;


                                case "enterpriseAuthentication":
                                    Capabilities |= Capability.EnterpriseAuthentication;
                                    break;

                                case "sharedUserCertificates":
                                    Capabilities |= Capability.SharedUserCertificates;
                                    break;
                            }
                        }
                    }
                }

            });
            t.Wait();
        }

        public Windows.UI.Color BackgroundColor
        {
            get;
            private set;
        }

        public Capability Capabilities
        {
            get;
            private set;
        }

        public string Description
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
    }
}
