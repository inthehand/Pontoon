//-----------------------------------------------------------------------
// <copyright file="DevicePicker.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Devices.Enumeration
{
    partial class DevicePicker
    {
        internal static EventWaitHandle s_handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal static DevicePicker s_current;
        internal Android.Bluetooth.BluetoothDevice _device;
        
        private Task<DeviceInformation> DoPickSingleDeviceAsync()
        {
            s_current = this;

            Intent i = new Intent(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity, typeof(DevicePickerActivity));
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(i);

            return Task.Run<DeviceInformation>(() =>
            {                
                s_handle.WaitOne();

                if (_device != null)
                {
                    return Task.FromResult<DeviceInformation>(_device);
                }
                else
                {
                    return Task.FromResult<DeviceInformation>(null);
                }
            });
        }

        [Activity(NoHistory = false, LaunchMode = LaunchMode.Multiple)]
        private sealed class DevicePickerActivity : Activity
        {
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                bool paired = true;
                // parse filters
                foreach (string filter in s_current.Filter.SupportedDeviceSelectors)
                {
                    var parts = filter.Split(':');
                    switch (parts[0])
                    {
                        case "bluetoothPairingState":
                            paired = bool.Parse(parts[1]);
                            break;
                    }
                }
                Intent i = new Intent("android.bluetooth.devicepicker.action.LAUNCH");
                i.PutExtra("android.bluetooth.devicepicker.extra.LAUNCH_PACKAGE", Application.Context.PackageName);
                // TODO: how to get this identifier programmatically
                i.PutExtra("android.bluetooth.devicepicker.extra.DEVICE_PICKER_LAUNCH_CLASS", "md55064263052223d9e87420d8cd886ad7c.DevicePickerReceiver");
                i.PutExtra("android.bluetooth.devicepicker.extra.NEED_AUTH", paired);

                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivityForResult(i, 1);

            }

            // set the handle when the picker has completed and return control straight back to the calling activity
            protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                base.OnActivityResult(requestCode, resultCode, data);
                
                s_handle.Set();

                Finish();
            }
        }

    }

    [BroadcastReceiver(Enabled = true)]
    internal class DevicePickerReceiver : BroadcastReceiver
    {
        // receive broadcast if a device is selected and store the device.
        public override void OnReceive(Context context, Intent intent)
        {
            var dev = (Android.Bluetooth.BluetoothDevice)intent.Extras.Get("android.bluetooth.device.extra.DEVICE");
            DevicePicker.s_current._device = dev;
        }
    }
}