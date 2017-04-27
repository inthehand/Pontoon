//-----------------------------------------------------------------------
// <copyright file="BluetoothAdapter.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Globalization;

namespace InTheHand.Devices.Bluetooth
{
    public sealed partial class BluetoothAdapter
    {
        private static Task<BluetoothAdapter> GetDefaultAsyncImpl()
        {
            return Task.Run<BluetoothAdapter>(() =>
            {
                return new BluetoothAdapter(Android.Bluetooth.BluetoothAdapter.DefaultAdapter);
            });
        }
        
        private Android.Bluetooth.BluetoothAdapter _adapter;

        internal BluetoothAdapter(Android.Bluetooth.BluetoothAdapter adapter)
        {
            _adapter = adapter;
        }
        
        private ulong GetBluetoothAddress()
        {
            return ulong.Parse(_adapter.Address.Replace(":", ""), NumberStyles.HexNumber);
        }
        
        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice(0);
        }
        
        private string GetName()
        {
            return _adapter.Name;
        }
    }
}