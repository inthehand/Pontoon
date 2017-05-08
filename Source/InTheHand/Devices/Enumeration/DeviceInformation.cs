//-----------------------------------------------------------------------
// <copyright file="DeviceInformation.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;

#if __ANDROID__
using Android.Bluetooth;
using Android.Bluetooth.LE;
#elif __UNIFIED__
using CoreBluetooth;
using Foundation;
using System.Threading;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents a picker flyout that contains a list of devices for the user to choose from.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class DeviceInformation
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Enumeration.DeviceInformation _deviceInformation;

        private DeviceInformation(Windows.Devices.Enumeration.DeviceInformation deviceInformation)
        {
            _deviceInformation = deviceInformation;
        }

        public static implicit operator Windows.Devices.Enumeration.DeviceInformation(DeviceInformation deviceInformation)
        {
            return deviceInformation._deviceInformation;
        }

        public static implicit operator DeviceInformation(Windows.Devices.Enumeration.DeviceInformation deviceInformation)
        {
            return new DeviceInformation(deviceInformation);
        }

#elif __ANDROID__
        private static BluetoothManager _manager;

        static DeviceInformation()
        {
            _manager = (BluetoothManager)global::Android.App.Application.Context.GetSystemService(Android.App.Application.BluetoothService);

        }

        internal static BluetoothManager Manager
        {
            get
            {
                return _manager;
            }
        }

        internal Android.Bluetooth.BluetoothDevice _device;

        private DeviceInformation(Android.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator Android.Bluetooth.BluetoothDevice(DeviceInformation deviceInformation)
        {
            return deviceInformation._device;
        }

        public static implicit operator DeviceInformation(Android.Bluetooth.BluetoothDevice device)
        {
            return new DeviceInformation(device);
        }

#elif __UNIFIED__
        internal CBPeripheral _peripheral;
        private static CBCentralManager _manager;

        internal static CBCentralManager Manager
        {
            get
            {
                if(_manager == null)
                {
                    _manager = new CBCentralManager();
                }

                return _manager;
            }
        }
        private static EventWaitHandle stateHandle;
        private static EventWaitHandle retrievedHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        //private static EventHandler<CBPeripheralsEventArgs> _retrieved = new EventHandler<CBPeripheralsEventArgs>(_manager_RetrievedConnectedPeripherals);
        internal static List<DeviceInformation> _devices = new List<DeviceInformation>();
        static DeviceInformation()
        {
            stateHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            _manager = new CBCentralManager();
#if !__TVOS__
            _manager.RetrievedConnectedPeripherals += _manager_RetrievedConnectedPeripherals;
            _manager.RetrievedPeripherals += _manager_RetrievedPeripherals;
#endif
            _manager.UpdatedState += _manager_UpdatedState;
            _manager.DiscoveredPeripheral += _manager_DiscoveredPeripheral;
        }

      

        private static void _manager_UpdatedState(object sender, EventArgs e)
        {
            Debug.WriteLine(_manager.State);
            if(_manager.State == CBCentralManagerState.PoweredOn)
            {
                stateHandle.Set();
            }
            else
            {
                stateHandle.Reset();
            }
        }
#if !__TVOS__
        private static void _manager_RetrievedPeripherals(object sender, CBPeripheralsEventArgs e)
        {
            foreach (CBPeripheral p in e.Peripherals)
            {
                _devices.Add(new DeviceInformation(p, string.Empty));
            }

            retrievedHandle.Set();
        }

        private static void _manager_RetrievedConnectedPeripherals(object sender, CBPeripheralsEventArgs e)
        {
            foreach(CBPeripheral p in e.Peripherals )
            {
                _devices.Add(new DeviceInformation(p, string.Empty));
            }

            retrievedHandle.Set();
        }
#endif

        private static void _manager_DiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            foreach(KeyValuePair<NSObject,NSObject> kvp in e.AdvertisementData)
            {
                Debug.WriteLine(kvp.Key.ToString() + " " + kvp.Value.ToString());
            }

            //e.Peripheral.
            Debug.WriteLine(e.RSSI.ToString());
            string name = e.AdvertisementData.ContainsKey(new NSString("kCBAdvDataLocalName")) ? e.AdvertisementData["kCBAdvDataLocalName"].ToString() : e.Peripheral.Name;
            _devices.Add(new DeviceInformation(e.Peripheral, name));
        }

        internal DeviceInformation(CBPeripheral peripheral, string name)
        {
            _peripheral = peripheral;
            _name = name;
        }

        public static implicit operator CBPeripheral(DeviceInformation deviceInformation)
        {
            return deviceInformation._peripheral;
        }

        internal sealed class DeviceInformationCentralManagerDelegate : CBCentralManagerDelegate
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
            /*public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
            {
                base.DiscoveredPeripheral(central, peripheral, advertisementData, RSSI);
            }

            public override void RetrievedConnectedPeripherals(CBCentralManager central, CBPeripheral[] peripherals)
            {
                base.RetrievedConnectedPeripherals(central, peripherals);
            }*/
        }
#endif

        public static async Task<IReadOnlyCollection<DeviceInformation>> FindAllAsync(string aqsFilter)
        {
            List<DeviceInformation> all = new List<DeviceInformation>();

#if __ANDROID__
            // Step 1: Return all paired devices
            foreach(Android.Bluetooth.BluetoothDevice d in _manager.Adapter.BondedDevices)
            {
                all.Add(new DeviceInformation(d));
            }

#elif __UNIFIED__
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
                _manager.ScanForPeripherals(CBUUID.FromString("0x180A"));
                var peripherals = _manager.RetrieveConnectedPeripherals(CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAccess.ToByteArray()), CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAttribute.ToByteArray()), CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.DeviceInformation.ToByteArray()));

                //var peripherals2 = _manager.RetrievePeripheralsWithIdentifiers(new NSUuid(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAttribute.ToByteArray()));
                //retrievedHandle.WaitOne();


                //_manager.RetrieveConnectedPeripherals(CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAccess.ToByteArray()));
                                          //_manager.ScanForPeripherals(peripheralUuids: null);
                                          //_manager.ScanForPeripherals( new CBUUID[] { CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAttribute.ToByteArray()), CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAccess.ToByteArray()), CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.Battery.ToByteArray()) });
                await Task.Delay(5000);
                    _manager.StopScan();
           

                return _devices.AsReadOnly();
            });

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            var devs = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(aqsFilter);
            
            foreach(Windows.Devices.Enumeration.DeviceInformation di in devs)
            {
                all.Add(new DeviceInformation(di));
            }
#elif WIN32
            FindAllAsyncImpl(aqsFilter, all);

#endif
            return all.AsReadOnly();
        }

        /// <summary>
        /// Enumerates all DeviceInformation objects.
        /// </summary>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<DeviceInformation>> FindAllAsync()
        {
            List<DeviceInformation> all = new List<DeviceInformation>();

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            var devs = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync();

            foreach(Windows.Devices.Enumeration.DeviceInformation di in devs)
            {
                all.Add(new DeviceInformation(di));
            }

#elif WIN32
            FindAllAsyncImpl(null, all);

#endif
            return all.AsReadOnly();
        }

        /// <summary>
        /// A string representing the identity of the device.
        /// </summary>
        public string Id
        {
            get
            {
#if __ANDROID__
                return "BLUETOOTH#" + _device.Address;

#elif __UNIFIED__
                return _peripheral.Identifier.AsString();

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _deviceInformation.Id;

#elif WIN32
                return GetId();

#else
                return string.Empty;
#endif
            }
        }

#if __UNIFIED__
        private string _name;
#endif
        /// <summary>
        /// The name of the device.
        /// </summary>
        public string Name
        {
            get
            {
#if __ANDROID__
                return _device.Name;

#elif __UNIFIED__
                if (string.IsNullOrEmpty(_peripheral.Name))
                {
                    return _name;
                }

                return _peripheral.Name;

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _deviceInformation.Name;

#elif WIN32
                return GetName();

#else
                return string.Empty;
#endif
            }
        }

#if __ANDROID__ || WINDOWS_UWP || WIN32
        /// <summary>
        /// Gets the information about the capabilities for this device to pair.
        /// </summary>
        /// <value>The pairing information for this device.</value>
        public DeviceInformationPairing Pairing
        {
            get
            {
#if __ANDROID__
                return new DeviceInformationPairing(_device);

#elif WINDOWS_UWP
                return _deviceInformation.Pairing;

#elif WIN32
                return new DeviceInformationPairing(_deviceInfo);

#else
                return null;
#endif
            }
        }
#endif

        public override string ToString()
        {
            return Id;
        }
    }
}