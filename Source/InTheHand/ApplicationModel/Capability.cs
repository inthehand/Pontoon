// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Capability.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-14 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   App Capability constants.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.ApplicationModel
{
    /// <summary>
    /// Application Capabilities
    /// </summary>
    [Flags(), CLSCompliant(false)]
    public enum Capability : uint
    {
        /// <summary>
        /// The Appointments capability provides access to the user’s appointment store. 
        /// This capability allows read access to appointments obtained from the synced network accounts and to other apps that write to the appointment store
        /// With this capability, your app can create new calendars and write appointments to calendars that it creates.
        /// </summary>
        Appointments,

        /// <summary>
        /// The Contacts capability provides access to the aggregated view of the contacts from various contacts stores.
        /// This capability gives the app limited access (network permitting rules apply) to contacts that were synced from various networks and the local contact store.
        /// </summary>
        Contacts,

        /// <summary>
        /// The MusicLibrary capability provides programmatic access to the user's Music, allowing the app to enumerate and access all files in the library without user interaction
        /// This capability is typically used in jukebox apps that need to access the entire Music library. 
        /// </summary>
        Music,
        /// <summary>
        /// The PicturesLibrary capability provides programmatic access to the user's Pictures, allowing the app to enumerate and access all files in the library without user interaction.
        /// This capability is typically used in photo playback apps that need to access the entire Pictures library. 
        /// </summary>
        Pictures, 
        /// <summary>
        /// The VideosLibrary capability provides programmatic access to the user's Videos, allowing the app to enumerate and access all files in the library without user interaction.
        /// This capability is typically used in movie playback apps that need access to the entire Videos library. 
        /// </summary>
        Videos,

        /// <summary>
        /// Application requires network access.
        /// </summary>
        Internet,

        /// <summary>
        /// Application requires the ability to make phone calls.
        /// <para>Silverlight Only</para>
        /// </summary>
        PhoneDialer,

        /// <summary>
        /// Application requires access to push notifications.
        /// <para>Silverlight Only</para>
        /// </summary>
        PushNotification,

        /// <summary>
        /// The RemovableStorage capability provides programmatic access to files on removable storage, such as USB keys and external hard drives, filtered to the file type associations declared in the package manifest.
        /// For example, if a DOC reader app declared a .doc file type association, it can open .doc files on the removable storage device, but not other types of files.
        /// Be careful when declaring this capability, because users may include a variety of info in their removable storage devices, and will expect the app to provide a valid justification for programmatic access to the removable storage for the entire file type.
        /// </summary>
        RemovableStorage,
        
        /// <summary>
        /// <para>Silverlight Only</para>
        /// </summary>
        SpeechRecognition,
        
        /// <summary>
        /// <para>Silverlight Only</para>
        /// </summary>
        Voip,

        /// <summary>
        /// <para>Silverlight Only</para>
        /// </summary>
        Wallet,

        /// <summary>
        /// 
        /// </summary>
        EnterpriseAuthentication,
        /// <summary>
        /// 
        /// </summary>
        SharedUserCertificates,

        /// <summary>
        /// The DocumentsLibrary capability provides programmatic access to the user's Documents, filtered to the file type associations declared in the package manifest, to support offline access to OneDrive.
        /// For example, if a DOC reader app declared a .doc file type association, it can open .doc files in Documents, but not other types of files. 
        /// </summary>
        DocumentsLibrary,
    }

    /// <summary>
    /// Device capabilities allow your app to access peripheral and internal devices. Device capabilities are specified with the DeviceCapability element in your app package manifest.
    /// This element may require additional child elements and some device capabilities need to be added to the package manifest manually
    /// </summary>
    [Flags, CLSCompliant(false)]
    public enum DeviceCapability : uint
    {
        /// <summary>
        /// The Location capability provides access to location functionality, which you get from dedicated hardware like a GPS sensor in the PC or is derived from available network info.
        /// Apps must handle the case where the user has disabled location services from settings.
        /// </summary>
        Location,

        /// <summary>
        /// The Microphone capability provides access to the microphone’s audio feed, which allows the app to record audio from connected microphones.
        /// </summary>
        Microphone,

        /// <summary>
        /// The Proximity capability enables multiple devices in close proximity to communicate with one another.
        /// This capability is typically used in casual multi-player games and in apps that exchange information.
        /// Devices attempt to use the communication technology that provides the best possible connection, including Bluetooth, WiFi, and the internet.
        /// This capability is used only to initiate communication between the devices.
        /// </summary>
        Proximity,

        /// <summary>
        /// The Camera capability provides access to the video feed of a built-in camera or external webcam, which allows the app to capture photos and videos.
        /// On Windows, apps must handle the case where the user has disabled the camera from the Settings charm.
        /// </summary>
        Camera,

        /// <summary>
        /// The Usb device capability enables access to APIs in the Windows.Devices.Usb namespace. 
        /// By using the namespace, you can write an app that talks to a custom USB device. 
        /// "Custom" in this context means, a peripheral device for which Microsoft does not provide an in-box class driver.
        /// </summary>
        Usb,

        /// <summary>
        /// The HumanInterfaceDevice device capability enables access to APIs in the Windows.Devices.HumanInterfaceDevice namespace. 
        /// This namespace lets your app access devices that support the Human Interface Device (HID) protocol.
        /// </summary>
        HumanInterfaceDevice,

        /// <summary>
        /// The bluetooth device capability enables access to APIs in the Windows.Devices.Bluetooth.GenericAttributeProfile and Windows.Devices.Bluetooth.Rfcomm namespaces.
        /// </summary>
        Bluetooth,

        /// <summary>
        /// The PointOfService device capability enables access to APIs in the Windows.Devices.PointOfService namespace.
        /// This namespace lets your Windows Store app access Point of Service (POS) barcode scanners and magnetic stripe readers.
        /// The namespace provides a vendor-neutral interface for accessing POS devices from various manufacturers from a Windows Store app.
        /// </summary>
        PointOfService,
        
        /// <summary>
        /// Application requires access to the accelerometer.
        /// <para>Silverlight Only</para>
        /// </summary>
        Sensors,
    }
}
