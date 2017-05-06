//-----------------------------------------------------------------------
// <copyright file="DevicePicker.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InTheHand.Foundation;
using InTheHand.UI.Popups;
using Android.Content;
using Android.App;
using System.Threading;

namespace InTheHand.Devices.Enumeration
{
    partial class DevicePicker
    {
        //private static DevicePickerReceiver s_devicePickerReceiver = new DevicePickerReceiver();
        //private static IntentFilter s_intentFilter = new IntentFilter("android.bluetooth.devicepicker.action.DEVICE_SELECTED");
        internal EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal Android.Bluetooth.BluetoothDevice _device;
        internal static DevicePicker _current;

        static DevicePicker()
        {
            //Application.Context.RegisterReceiver(s_devicePickerReceiver, s_intentFilter);
        }

        private Task<DeviceInformation> DoPickSingleDeviceAsync()
        {
            _current = this;

            return Task.Run<DeviceInformation>(() =>
            {
                bool paired = true;
                // parse filters
                foreach (string filter in Filter.SupportedDeviceSelectors)
                {
                    var parts = filter.Split(':');
                    switch (parts[0])
                    {
                        case "bluetoothPairing":
                            paired = bool.Parse(parts[1]);
                            break;
                    }
                }
                Intent i = new Intent("android.bluetooth.devicepicker.action.LAUNCH");
                i.PutExtra("android.bluetooth.devicepicker.extra.LAUNCH_PACKAGE", Application.Context.PackageName);
                //TODO: how to get this identifier programmatically
                i.PutExtra("android.bluetooth.devicepicker.extra.DEVICE_PICKER_LAUNCH_CLASS", "md55064263052223d9e87420d8cd886ad7c.DevicePickerReceiver");
                if (!paired)
                {
                    i.PutExtra("android.bluetooth.devicepicker.extra.NEED_AUTH", false);
                }
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivityForResult(i,1);
                _handle.WaitOne();

                return Task.FromResult<DeviceInformation>(_device);
            });
        }


       
    }

    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "android.bluetooth.devicepicker.action.DEVICE_SELECTED" })]
    internal class DevicePickerReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var dev = (Android.Bluetooth.BluetoothDevice)intent.Extras.Get("android.bluetooth.device.extra.DEVICE");
            DevicePicker._current._device = dev;
            DevicePicker._current._handle.Set();
        }
    }
}