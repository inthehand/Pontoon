//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Bluetooth;
using Android.Content;
using Android.OS;
using InTheHand.Devices.Bluetooth.Rfcomm;
using InTheHand.Devices.Enumeration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothDevice
    {
        internal Android.Bluetooth.BluetoothDevice _device;

        private BluetoothDevice(Android.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator Android.Bluetooth.BluetoothDevice(BluetoothDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothDevice(Android.Bluetooth.BluetoothDevice device)
        {
            return new BluetoothDevice(device);
        }

        private static async Task<BluetoothDevice> FromBluetoothAddressAsyncImpl(ulong bluetoothAddress)
        {
            byte[] buffer = new byte[6];
            var addressBytes = BitConverter.GetBytes(bluetoothAddress);
            for (int i = 0; i < 6; i++)
            {
                buffer[i] = addressBytes[i];
            }

            var device = DeviceInformation.Manager.Adapter.GetRemoteDevice(buffer);

            if (device.Type.HasFlag(BluetoothDeviceType.Classic))
            {
                return device;
            }

            return null;
        }

        private static async Task<BluetoothDevice> FromIdAsyncImpl(string deviceId)
        {
            var device = Android.Bluetooth.BluetoothAdapter.DefaultAdapter.GetRemoteDevice(deviceId);

            if (device.Type.HasFlag(BluetoothDeviceType.Classic))
            {
                return device;
            }

            return null;
        }

        private static async Task<BluetoothDevice> FromDeviceInformationAsyncImpl(DeviceInformation deviceInformation)
        {
            return deviceInformation._device;
        }

        private static string GetDeviceSelectorImpl()
        {
            return string.Empty;
        }

        private static string GetDeviceSelectorFromClassOfDeviceImpl(BluetoothClassOfDevice classOfDevice)
        {
            return "bluetoothClassOfDevice:" + classOfDevice.RawValue.ToString("X12");
        }

        private static string GetDeviceSelectorFromPairingStateImpl(bool pairingState)
        {
            return "bluetoothPairingState:" + pairingState.ToString();
        }

        private ulong GetBluetoothAddress()
        {
            return ulong.Parse(_device.Address.Replace(":", ""), NumberStyles.HexNumber);
        }

        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice((uint)_device.BluetoothClass.DeviceClass);
        }

        private BluetoothConnectionStatus _connectionStatus = BluetoothConnectionStatus.Disconnected;
        private BluetoothConnectionStatus GetConnectionStatus()
        {
            return _connectionStatus;
        }

        private string GetDeviceId()
        {
            return _device.Address;
        }

        private string GetName()
        {
            return _device.Name;
        }

        private void GetRfcommServices(List<RfcommDeviceService> services)
        {
            ParcelUuid[] uuids = _device.GetUuids();
            if (uuids != null)
            {
                foreach (ParcelUuid g in uuids)
                {
                    services.Add(new RfcommDeviceService(this, RfcommServiceId.FromUuid(new Guid(g.Uuid.ToString()))));
                }
            }
        }

        private BluetoothDeviceReceiver _connectedStateReceiver;
        private void ConnectionStatusChangedAdd()
        {
            IntentFilter filter = new IntentFilter();
            filter.AddAction("ACTION_ACL_CONNECTED");
            filter.AddAction("ACTION_ACL_DISCONNECTED");
            _connectedStateReceiver = new BluetoothDeviceReceiver(this);
            Android.App.Application.Context.RegisterReceiver(_connectedStateReceiver, filter);
        }

        private void ConnectionStatusChangedRemove()
        {
            if(_connectedStateReceiver != null)
            {
                Android.App.Application.Context.UnregisterReceiver(_connectedStateReceiver);
                _connectedStateReceiver = null;
            }
        }

        private sealed class BluetoothDeviceReceiver : BroadcastReceiver
        {
            private BluetoothDevice _parent;

            internal BluetoothDeviceReceiver(BluetoothDevice parent)
            {
                _parent = parent;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                switch(intent.Action)
                {
                    case "ACTION_ACL_CONNECTED":
                        _parent._connectionStatus = BluetoothConnectionStatus.Connected;
                        _parent.RaiseConnectionStatusChanged();
                        break;

                    case "ACTION_ACL_DISCONNECTED":
                        _parent._connectionStatus = BluetoothConnectionStatus.Disconnected;
                        _parent.RaiseConnectionStatusChanged();
                        break;
                }
            }
        }
    }
}