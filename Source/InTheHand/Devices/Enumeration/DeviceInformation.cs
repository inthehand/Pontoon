//-----------------------------------------------------------------------
// <copyright file="DeviceInformation.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;
#if __ANDROID__
using Android.Bluetooth;
using Android.Bluetooth.LE;
#elif __IOS__
using CoreBluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents a picker flyout that contains a list of devices for the user to choose from.
    /// </summary>
    public sealed class DeviceInformation
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Enumeration.DeviceInformation _deviceInformation;

        internal DeviceInformation(Windows.Devices.Enumeration.DeviceInformation deviceInformation)
        {
            _deviceInformation = deviceInformation;
        }
#elif __ANDROID__
        private static BluetoothLeScanner _scanner;
        static DeviceInformation()
        {
            BluetoothManager bluetoothManager = (BluetoothManager)Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.GetSystemService(Android.MainApplication.BluetoothService);
            _scanner = bluetoothManager.Adapter.BluetoothLeScanner;

        }
#elif __IOS__
        internal CBPeripheral _peripheral;
        private static CBCentralManager _manager = new CBCentralManager();
        //private static EventHandler<CBPeripheralsEventArgs> _retrieved = new EventHandler<CBPeripheralsEventArgs>(_manager_RetrievedConnectedPeripherals);
        internal static List<DeviceInformation> _devices = new List<DeviceInformation>();
        static DeviceInformation()
        {
            //_manager.RetrievedConnectedPeripherals += _retrieved;
            _manager.DiscoveredPeripheral += _manager_DiscoveredPeripheral;
        }

        private static bool retrieving = false;

        private static void _manager_RetrievedConnectedPeripherals(object sender, CBPeripheralsEventArgs e)
        {
            foreach(CBPeripheral p in e.Peripherals )
            {
                _devices.Add(new DeviceInformation(p));
            }

            retrieving = false;
        }

        private static void _manager_DiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            _devices.Add(new DeviceInformation(e.Peripheral));
        }

        internal DeviceInformation(CBPeripheral peripheral)
        {
            _peripheral = peripheral;
        }
#endif

        public static async Task<IReadOnlyCollection<DeviceInformation>> FindAllAsync(string aqsFilter)
        {
#if __IOS__
            retrieving = true;
            //_manager.RetrieveConnectedPeripherals();
            _manager.ScanForPeripherals(CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.Battery.ToByteArray()));
            while (_manager.IsScanning)
            {
                await Task.Delay(100);
            }

            return _devices.AsReadOnly();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            var devs = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(aqsFilter);
            List<DeviceInformation> all = new List<DeviceInformation>();
            foreach(Windows.Devices.Enumeration.DeviceInformation di in devs)
            {
                all.Add(new DeviceInformation(di));
            }
            return new ReadOnlyCollection<DeviceInformation>(all);

#else
            return new ReadOnlyCollection<DeviceInformation>(new List<DeviceInformation>());
#endif
        }

        public static async Task<IReadOnlyCollection<DeviceInformation>> FindAllAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            var devs = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync();
            List<DeviceInformation> all = new List<DeviceInformation>();
            foreach(Windows.Devices.Enumeration.DeviceInformation di in devs)
            {
                all.Add(new DeviceInformation(di));
            }
            return new ReadOnlyCollection<DeviceInformation>(all);

#else
            return new ReadOnlyCollection<DeviceInformation>(new List<DeviceInformation>());
#endif
        }

        public string Id
        {
            get
            {
#if __IOS__
                return _peripheral.Identifier.ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _deviceInformation.Id;
#else
                return string.Empty;
#endif
            }
        }

        public string Name
        {
            get
            {
#if __IOS__
                return _peripheral.Identifier.ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _deviceInformation.Name;
#else
                return string.Empty;
#endif
            }
        }
    }
}