//-----------------------------------------------------------------------
// <copyright file="BluetoothAdapter.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothAdapter
    {
        private static Task<BluetoothAdapter> GetDefaultAsyncImpl()
        {
            return Task.Run<BluetoothAdapter>(() =>
            {
                if (s_default == null)
                {
                    s_default = new BluetoothAdapter();
                }

                return s_default;
            });
        }
        

        internal BluetoothAdapter()
        {
        }

        private ulong GetBluetoothAddress()
        {
            return 0;
        }

        private BluetoothClassOfDevice GetClassOfDevice()
        {
            
            return BluetoothClassOfDevice.FromRawValue(0);
        }

        private bool GetIsClassicSupported()
        {
            return true;
        }

        private bool GetIsLowEnergySupported()
        {
            return true;
        }

        private string GetName()
        {
            return string.Empty;
        }
    }
}