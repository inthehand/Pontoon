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
using Foundation;
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
        private static CBCentralManager _manager;
        private static System.Threading.EventWaitHandle stateHandle;
        private static System.Threading.EventWaitHandle retrievedHandle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
        //private static EventHandler<CBPeripheralsEventArgs> _retrieved = new EventHandler<CBPeripheralsEventArgs>(_manager_RetrievedConnectedPeripherals);
        internal static List<DeviceInformation> _devices = new List<DeviceInformation>();
        static DeviceInformation()
        {
            stateHandle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset);
            _manager = new CBCentralManager();
            //_manager.RetrievedConnectedPeripherals += _manager_RetrievedConnectedPeripherals;
            //_manager.RetrievedPeripherals += _manager_RetrievedPeripherals;
            _manager.UpdatedState += _manager_UpdatedState;
            _manager.DiscoveredPeripheral += _manager_DiscoveredPeripheral;
        }

        private static void _manager_RetrievedPeripherals(object sender, CBPeripheralsEventArgs e)
        {
            foreach (CBPeripheral p in e.Peripherals)
            {
                _devices.Add(new DeviceInformation(p,string.Empty));
            }

            retrievedHandle.Set();
        }

        private static void _manager_UpdatedState(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(_manager.State);
            if(_manager.State == CBCentralManagerState.PoweredOn)
            {
                stateHandle.Set();
            }
            else
            {
                stateHandle.Reset();
            }
        }
        
       /* private static void _manager_RetrievedConnectedPeripherals(object sender, CBPeripheralsEventArgs e)
        {
            foreach(CBPeripheral p in e.Peripherals )
            {
                _devices.Add(new DeviceInformation(p));
            }

            retrievedHandle.Set();
        }*/

        private static void _manager_DiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            foreach(KeyValuePair<NSObject,NSObject> kvp in e.AdvertisementData)
            {
                System.Diagnostics.Debug.WriteLine(kvp.Key.ToString() + " " + kvp.Value.ToString());
            }

            //e.Peripheral.
            System.Diagnostics.Debug.WriteLine(e.RSSI.ToString());
            _devices.Add(new DeviceInformation(e.Peripheral, e.AdvertisementData["kCBAdvDataLocalName"].ToString()));
        }

        internal DeviceInformation(CBPeripheral peripheral, string name)
        {
            _peripheral = peripheral;
            _name = name;
        }

        internal sealed class DeviceInformationCentralManagerDelegate : CoreBluetooth.CBCentralManagerDelegate
        {
            public override void UpdatedState(CBCentralManager central)
            {
                if(central.State == CBCentralManagerState.PoweredOn)
                {
                    stateHandle.Set();
                }
                else
                {
                    stateHandle.Reset();
                }
            }
            public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
            {
                base.DiscoveredPeripheral(central, peripheral, advertisementData, RSSI);
            }

            public override void RetrievedConnectedPeripherals(CBCentralManager central, CBPeripheral[] peripherals)
            {
                base.RetrievedConnectedPeripherals(central, peripherals);
            }
        }
#endif

        public static async Task<IReadOnlyCollection<DeviceInformation>> FindAllAsync(string aqsFilter)
        {
#if __IOS__
            return await Task.Run<IReadOnlyCollection<DeviceInformation>>(async () =>
            {
                if (_manager.State != CBCentralManagerState.PoweredOn)
                {
                    stateHandle.WaitOne();
                }

            //CBPeripheral[] peripherals = _manager.RetrieveConnectedPeripherals(CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAttribute.ToByteArray()));

                /*CBPeripheral[] peripherals = _manager.RetrievePeripheralsWithIdentifiers(null);// new CBUUID[] { CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAttribute.ToByteArray()), CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAccess.ToByteArray()), CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.Battery.ToByteArray()) });

                foreach (CBPeripheral p in peripherals)
                    {
                        _devices.Add(new DeviceInformation(p));
                    }*/
                //retrievedHandle.WaitOne();
                //_manager.ScanForPeripherals(CBUUID.FromString("180a"));
                _manager.ScanForPeripherals(peripheralUuids: null);
             //_manager.ScanForPeripherals( new CBUUID[] { CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAttribute.ToByteArray()), CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAccess.ToByteArray()), CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.Battery.ToByteArray()) });
                await Task.Delay(12000);
                    _manager.StopScan();
           

                return _devices.AsReadOnly();
            });

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
                return _peripheral.Identifier.AsString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _deviceInformation.Id;
#else
                return string.Empty;
#endif
            }
        }

#if __IOS__
        private string _name;
#endif
        public string Name
        {
            get
            {
#if __IOS__
                if (string.IsNullOrEmpty(_peripheral.Name))
                {
                    return _name;
                }

                return _peripheral.Name;

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _deviceInformation.Name;
#else
                return string.Empty;
#endif
            }
        }
    }
}