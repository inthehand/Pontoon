//-----------------------------------------------------------------------
// <copyright file="DevicePicker.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace InTheHand.Devices.Enumeration
{
    public sealed partial class DevicePicker
    {
        private NativeMethods.PFN_DEVICE_CALLBACK _callback;

        private async Task<DeviceInformation> PickSingleDeviceAsyncImpl()
        {
            NativeMethods.BLUETOOTH_SELECT_DEVICE_PARAMS sdp = new NativeMethods.BLUETOOTH_SELECT_DEVICE_PARAMS();
            sdp.dwSize = Marshal.SizeOf(sdp);
            sdp.fShowAuthenticated = true;

            sdp.hwndParent = NativeMethods.GetForegroundWindow();

            if (Filter.SupportedDeviceSelectors.Count > 0)
            {
                _callback = new NativeMethods.PFN_DEVICE_CALLBACK(FilterDevices);
                sdp.pfnDeviceCallback = _callback;
                //sdp.pvParam = Marshal.AllocHGlobal(4);
                //Marshal.WriteInt32(sdp.pvParam, 1);
            }

            bool success = NativeMethods.BluetoothSelectDevices(ref sdp);

            if (success)
            {
                BLUETOOTH_DEVICE_INFO info = Marshal.PtrToStructure<BLUETOOTH_DEVICE_INFO>(sdp.pDevices);
                NativeMethods.BluetoothSelectDevicesFree(ref sdp);

                return new DeviceInformation(info);
            }

            return null;
        }

        private bool FilterDevices(IntPtr param, ref BLUETOOTH_DEVICE_INFO info)
        {
            Guid[] services = GetRemoteServices(info);
            if (services.Length > 0)
            {
                bool match = false;
                foreach (string filter in Filter.SupportedDeviceSelectors)
                {
                    Guid service = Guid.Parse(filter);
                    for (int i = 0; i < services.Length; i++)
                    {
                        if (services[i] == service)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private Guid[] GetRemoteServices(BLUETOOTH_DEVICE_INFO info)
        {
            Guid[] services = new Guid[16];
            int ns = services.Length;
            int error = NativeMethods.BluetoothEnumerateInstalledServices(IntPtr.Zero, ref info, ref ns, services);
            if(error == 0)
            {
                Guid[] enumeratedServices = new Guid[ns];
                for(int i = 0; i < ns; i++)
                {
                    enumeratedServices[i] = services[i];
                }

                return enumeratedServices;
            }

            return new Guid[0];
        }

        private static class NativeMethods
        {
            [DllImport("User32", SetLastError = true)]
            internal static extern IntPtr GetForegroundWindow();

            [DllImport("IrProps.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothSelectDevices(
                ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

            [DllImport("IrProps.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothSelectDevicesFree(
                ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

            [DllImport("IrProps.cpl", SetLastError = true)]
            internal static extern int BluetoothEnumerateInstalledServices(
                IntPtr hRadio,
                ref BLUETOOTH_DEVICE_INFO pbtdi, 
                ref int pcServices, 
                Guid[] pGuidServices);

            [StructLayout(LayoutKind.Sequential)]
            internal struct BLUETOOTH_SELECT_DEVICE_PARAMS
            {
                internal int dwSize;
                uint numOfClasses;
                IntPtr /*BLUETOOTH_COD_PAIRS**/ prgClassOfDevices;
                [MarshalAs(UnmanagedType.LPWStr)]
                string info;
                internal IntPtr hwndParent;
                [MarshalAs(UnmanagedType.Bool)]
                bool fForceAuthentication;
                [MarshalAs(UnmanagedType.Bool)]
                internal bool fShowAuthenticated;
                [MarshalAs(UnmanagedType.Bool)]
                bool fShowRemembered;
                [MarshalAs(UnmanagedType.Bool)]
                bool fShowUnknown;
                [MarshalAs(UnmanagedType.Bool)]
                bool fAddNewDeviceWizard;
                [MarshalAs(UnmanagedType.Bool)]
                bool fSkipServicesPage;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                internal PFN_DEVICE_CALLBACK pfnDeviceCallback;
                internal IntPtr pvParam;
                internal uint numDevices;
                internal IntPtr /*PBLUETOOTH_DEVICE_INFO*/ pDevices;
            }
            
            [return:MarshalAs(UnmanagedType.Bool)]
            internal delegate bool PFN_DEVICE_CALLBACK(IntPtr param, ref BLUETOOTH_DEVICE_INFO device);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct BLUETOOTH_DEVICE_INFO
    {
        internal int dwSize;
        internal ulong Address;
        internal uint ulClassofDevice;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fConnected;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fRemembered;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fAuthenticated;
        internal SYSTEMTIME stLastSeen;
        internal SYSTEMTIME stLastUsed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst =248)]
        internal string szName;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SYSTEMTIME
    {
        private ushort year;
        private short month;
        private short dayOfWeek;
        private short day;
        private short hour;
        private short minute;
        private short second;
        private short millisecond;
    }
}