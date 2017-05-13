//-----------------------------------------------------------------------
// <copyright file="BluetoothLEDevice.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Threading.Tasks;
using InTheHand.Devices.Enumeration;
using InTheHand.Devices.Bluetooth.GenericAttributeProfile;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InTheHand.Foundation;
using System.Threading;
using Android.Bluetooth;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothLEDevice
    {


        private static async Task<BluetoothLEDevice> FromBluetoothAddressAsyncImpl(ulong bluetoothAddress)
        {
            return Android.Bluetooth.BluetoothAdapter.DefaultAdapter.GetRemoteDevice(BitConverter.GetBytes(bluetoothAddress));
        }

        private static async Task<BluetoothLEDevice> FromIdAsyncImpl(string deviceId)
        {
            return Android.Bluetooth.BluetoothAdapter.DefaultAdapter.GetRemoteDevice(deviceId);
        }

        private static async Task<BluetoothLEDevice> FromDeviceInformationAsyncImpl(DeviceInformation deviceInformation)
        {
            return deviceInformation._device;
        }

        private static string GetDeviceSelectorImpl()
        {
            return string.Empty;
        }

        internal Android.Bluetooth.BluetoothDevice _device;
        private BluetoothGatt _bluetoothGatt;
        private GattCallback _gattCallback;

        private BluetoothLEDevice(Android.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator Android.Bluetooth.BluetoothDevice(BluetoothLEDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothLEDevice(Android.Bluetooth.BluetoothDevice device)
        {
            return new BluetoothLEDevice(device);
        }



        private ulong GetBluetoothAddress()
        {
            return ulong.Parse(_device.Address.Replace(":", ""), NumberStyles.HexNumber);
        }

        private BluetoothConnectionStatus GetConnectionStatus()
        {
            Android.Bluetooth.ProfileState state = DeviceInformation.Manager.GetConnectionState(_device, Android.Bluetooth.ProfileType.Gatt);
            return state == Android.Bluetooth.ProfileState.Connected ? BluetoothConnectionStatus.Connected : BluetoothConnectionStatus.Disconnected;
        }


        private string GetDeviceId()
        {
            return _device.Address;
        }

        private IReadOnlyList<GattDeviceService> GetGattServices()
        {
            if (_bluetoothGatt == null)
            {
                _gattCallback = new GattCallback(this);
                _bluetoothGatt = _device.ConnectGatt(Android.App.Application.Context, true, _gattCallback);
            }
            return null;
        }

    }

    internal class GattCallback : BluetoothGattCallback
    {
        private BluetoothLEDevice _owner;

        internal GattCallback(BluetoothLEDevice owner)
        {
            _owner = owner;
        }
    }
}