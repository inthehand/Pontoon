//-----------------------------------------------------------------------
// <copyright file="RfcommDeviceService.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

#if WIN32
using System.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Sockets;
#endif

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    /// <summary>
    /// Represents an instance of a service on a remote Bluetooth device.
    /// </summary>
    public sealed class RfcommDeviceService
    {
        /// <summary>
        /// Gets an Advanced Query Syntax (AQS) string for identifying instances of an RFCOMM service.
        /// </summary>
        /// <param name="serviceId">The service id for which to query.</param>
        /// <returns>An AQS string for identifying RFCOMM service instances.</returns>
        public static string GetDeviceSelector(RfcommServiceId serviceId)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService.GetDeviceSelector(serviceId);

#elif WIN32
            return serviceId.Uuid.ToString();

#else
            return string.Empty;
#endif
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService _service;

        private RfcommDeviceService(Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService service)
        {
            _service = service;
        }

        public static implicit operator Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService(RfcommDeviceService service)
        {
            return service._service;
        }

        public static implicit operator RfcommDeviceService(Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService service)
        {
            return new RfcommDeviceService(service);
        }

#else
        private ulong _address;
        private RfcommServiceId _service;

        internal RfcommDeviceService(ulong address, RfcommServiceId service)
        {
            _address = address;
            _service = service;
        }
#endif

        /// <summary>
        /// Gets the RfcommServiceId of this RFCOMM service instance.
        /// </summary>
        /// <value>The RfcommServiceId of the RFCOMM service instance.</value>
        public RfcommServiceId ServiceId
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _service.ServiceId;

#else
                return _service;
#endif
            }
        }

        /// <summary>
        /// Connects to the remote service and returns a read/write Stream to communicate over.
        /// </summary>
        /// <returns>A <see cref="Stream"/> for reading and writing from the remote service. 
        /// Remember to Dispose of this Stream when you've finished working.</returns>
        public Task<Stream> OpenStreamAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Task.Run<Stream>(async () =>
            {
                Windows.Networking.Sockets.StreamSocket socket = new Windows.Networking.Sockets.StreamSocket();
                await socket.ConnectAsync(_service.ConnectionHostName, _service.ConnectionServiceName);
                return new InTheHand.Networking.Sockets.NetworkStream(socket);
            });
#elif WIN32
            return Task.Run<Stream>(() =>
            {
                var socket = new Socket(AddressFamily32.Bluetooth, SocketType.Stream, BluetoothProtocolType.RFComm);
                socket.Connect(new BluetoothEndPoint(_address, _service.Uuid));
                return new NetworkStream(socket);
            });
#else
                return Task.FromResult<Stream>(null);
#endif
        }
    }
}