// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallManager.cs" company="In The Hand Ltd">
//     Copyright (c) 2014-2018 In The Hand Ltd, All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
// <summary>
//   Provides methods for launching the built-in phone call UI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using InTheHand.System;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace InTheHand.ApplicationModel.Calls
{
    /// <summary>
    /// Provides methods for launching the built-in phone call UI.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 4.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public static partial class PhoneCallManager
    {
#if WINDOWS_APP
        private static Type _type10;

        static PhoneCallManager()
        {
            _type10 = Type.GetType("Windows.ApplicationModel.Calls.PhoneCallManager, Windows, ContentType=WindowsRuntime");
        }
#endif

    /// <summary>
    /// Launches the built-in phone call UI with the specified phone number and display name.
    /// </summary>
    /// <param name="phoneNumber">A phone number.
    /// This should be in international format e.g. +12345678901</param>
    /// <param name="displayName">A display name.</param>
    public static void ShowPhoneCallUI(string phoneNumber, string displayName)
        {
#if __ANDROID__ || __IOS__ || TIZEN || WINDOWS_PHONE
            DoShowPhoneCallUI(phoneNumber, displayName, false);

#elif WINDOWS_UWP || WINDOWS_PHONE_APP
#if WINDOWS_UWP
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Calls.PhoneCallManager"))
            {
#endif
                Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(phoneNumber, displayName);
#if WINDOWS_UWP
            }
            else
            {
                Windows.System.Launcher.LaunchUriAsync(new Uri("tel:" + phoneNumber));
            }
#endif
#elif WINDOWS_APP || WIN32 || __MAC__
            InTheHand.UI.Popups.MessageDialog prompt = new InTheHand.UI.Popups.MessageDialog(string.Format("Dial {0} at {1}?", displayName, phoneNumber), "Phone");
            prompt.Commands.Add(new InTheHand.UI.Popups.UICommand("Call", async (c) =>
                {
                        // OS may prompt the user for an app e.g. Skype, Lync etc
                        await Launcher.LaunchUriAsync(new Uri("tel:" + CleanPhoneNumber(phoneNumber)));
                }));
            prompt.Commands.Add(new InTheHand.UI.Popups.UICommand("Cancel", null));
            prompt.ShowAsync();
            
#else
            throw new PlatformNotSupportedException();
#endif
        }

        internal static string CleanPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Replace(" ", "");
        }
    }
}