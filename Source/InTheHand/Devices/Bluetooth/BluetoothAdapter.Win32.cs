//-----------------------------------------------------------------------
// <copyright file="BluetoothAdapter.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothAdapter
    {
        private static BluetoothAdapter s_default;
        
        private static Task<BluetoothAdapter> GetDefaultAsyncImpl()
        {
            return Task.Run(() =>
            {
                if (s_default == null)
                {
                    NativeMethods.BLUETOOTH_FIND_RADIO_PARAMS p = new NativeMethods.BLUETOOTH_FIND_RADIO_PARAMS();
                    p.dwSize = Marshal.SizeOf(p);
                    IntPtr radioHandle;
                    IntPtr findHandle = NativeMethods.BluetoothFindFirstRadio(ref p, out radioHandle);
                    if (findHandle != IntPtr.Zero)
                    {
                        NativeMethods.BLUETOOTH_RADIO_INFO radioInfo = new NativeMethods.BLUETOOTH_RADIO_INFO();
                        radioInfo.dwSize = Marshal.SizeOf(radioInfo);
                        if (NativeMethods.BluetoothGetRadioInfo(radioHandle, ref radioInfo) == 0)
                        {
                            s_default = new BluetoothAdapter(radioHandle, radioInfo);
                        }

                        NativeMethods.BluetoothFindRadioClose(findHandle);
                    }
                }

                return s_default;
            });
        }

        private IntPtr _handle;
        private NativeMethods.BLUETOOTH_RADIO_INFO _radioInfo;
        private BluetoothMessageWindow _messageWindow;
        private int _notifyHandle;

        internal BluetoothAdapter(IntPtr radioHandle, NativeMethods.BLUETOOTH_RADIO_INFO radioInfo)
        {
            _handle = radioHandle;
            _radioInfo = radioInfo;
            _messageWindow = new BluetoothMessageWindow();

            // register for connection events
            NativeMethods.DEV_BROADCAST_HANDLE filter = new NativeMethods.DEV_BROADCAST_HANDLE();
            filter.dbch_size = Marshal.SizeOf(filter);
            filter.dbch_handle = radioHandle;
            filter.dbch_devicetype = NativeMethods.DBT_DEVTYP.HANDLE;
            filter.dbch_eventguid = NativeMethods.GUID_BLUETOOTH_L2CAP_EVENT;

            _notifyHandle = NativeMethods.RegisterDeviceNotification(_messageWindow.Handle, ref filter, NativeMethods.DEVICE_NOTIFY.WINDOWS_HANDLE);
            filter.dbch_eventguid = NativeMethods.GUID_BLUETOOTH_HCI_EVENT;
            int notifyHandle = NativeMethods.RegisterDeviceNotification(_messageWindow.Handle, ref filter, NativeMethods.DEVICE_NOTIFY.WINDOWS_HANDLE);
            NativeMethods.PostMessage(_messageWindow.Handle, 0x401, IntPtr.Zero, IntPtr.Zero);

        }


        private event EventHandler<ulong> _connectionChanged;

        internal event EventHandler<ulong> ConnectionChanged
        {
            add
            {
                if(_connectionChanged == null)
                {
                    _messageWindow.ConnectionStateChanged += BluetoothMessageWindow_ConnectionStateChanged;
                }

                _connectionChanged += value;
            }
            remove
            {
                _connectionChanged -= value;

                if (_connectionChanged == null)
                {
                    _messageWindow.ConnectionStateChanged -= BluetoothMessageWindow_ConnectionStateChanged;
                }
            }
        }

        private void BluetoothMessageWindow_ConnectionStateChanged(object sender, ulong e)
        {
            _connectionChanged?.Invoke(this, e);
        }

        private ulong GetBluetoothAddress()
        {
            return _radioInfo.address;
        }
        
        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice(_radioInfo.ulClassofDevice);
        }

        private bool GetIsClassicSupported()
        {
            return true;
        }

        private bool GetIsLowEnergySupported()
        {
            return false;
        }

        private string GetName()
        {
            return _radioInfo.szName;
        }
    }
}