// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VibrationDevice.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if __ANDROID__
using Android.Content;
using Android.OS;
#elif __IOS__
using AudioToolbox;
using Foundation;
#endif

using System;

namespace InTheHand.Phone.Devices.Notification
{
    /// <summary>
    /// Vibrates the phone.
    /// </summary>
    /// <remarks>Phone devices include a vibration controller.
    /// Your app can vibrate the phone for up to 5 seconds to notify the user of an important event.
    /// Use the vibration feature in moderation.
    /// Do not rely on the vibration feature for critical notifications, because the user can disable vibration
    /// <para>To test an app that uses the vibration controller effectively, you have to test it on a physical device.
    /// The emulator cannot simulate vibration and does not provide any audible or visual feedback that vibration is occurring.
    /// <para>An app that is running in the background cannot vibrate the phone.</para>
    /// If your code tries to use vibration while the app is running in the background, nothing happens, but no exception is raised.
    /// If you want to vibrate the phone while your app is running in the background, you have to implement a toast notification.</para></remarks>
    public sealed class VibrationDevice
    {
        private static VibrationDevice  _default;
        /// <summary>
        /// Gets an instance of the VibrationDevice class.
        /// </summary>
        /// <returns></returns>
        public static VibrationDevice GetDefault()
        {
            if(_default == null)
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP
#if WINDOWS_UWP
                if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1))
#endif
                {
                    _default = new VibrationDevice(Windows.Phone.Devices.Notification.VibrationDevice.GetDefault());
                }
#elif __IOS__ || WINDOWS_PHONE
                _default = new VibrationDevice();
#else
#endif
            }

            return _default;
        }

#if WINDOWS_UWP || WINDOWS_PHONE_APP
        private Windows.Phone.Devices.Notification.VibrationDevice _device;

        private VibrationDevice(Windows.Phone.Devices.Notification.VibrationDevice device)
        {
            _device = device;
        }

        [CLSCompliant(false)]
        public static implicit operator Windows.Phone.Devices.Notification.VibrationDevice(VibrationDevice device)
        {
            return device._device;
        }
#else
#if __ANDROID__
        private Vibrator _vibrator;
#endif
        private VibrationDevice()
        {
#if __ANDROID__
            _vibrator = (Vibrator)Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.GetSystemService(Context.VibratorService);
#endif
        }
#endif

        /// <summary>
        /// Vibrates the phone for the specified duration (from 0 to 5 seconds).
        /// </summary>
        /// <param name="duration">The duration (from 0 to 5 seconds) for which the phone vibrates. A value that is less than 0 or greater than 5 raises an exception. Ignored on iOS.</param>
        public void Vibrate(TimeSpan duration)
        {
            if(duration < TimeSpan.Zero || duration.TotalSeconds > 5.0)
            {
                throw new ArgumentOutOfRangeException("duration");
            }
#if __ANDROID__
            if (_vibrator.HasVibrator)
            {
                _vibrator.Vibrate(Convert.ToInt64(duration.TotalMilliseconds));
            }
#elif __IOS__
            SystemSound.Vibrate.PlaySystemSound();
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
            _device.Vibrate(duration);
#elif WINDOWS_PHONE
            Microsoft.Devices.VibrateController.Default.Start(duration);
#else
#endif
        }

        /// <summary>
        /// Stops the vibration of the phone.
        /// </summary>
        public void Cancel()
        {
#if __ANDROID__
            _vibrator.Cancel();
#elif __IOS__
            SystemSound.Vibrate.Close();
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
            _device.Cancel();
#elif WINDOWS_PHONE
            Microsoft.Devices.VibrateController.Default.Stop();
#else
#endif
        }
    }
}