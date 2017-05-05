// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkStream.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;

#if __ANDROID__
using Android.Bluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Networking.Sockets;
#endif

namespace InTheHand.Networking.Sockets
{
    /// <summary>
    /// Provides the underlying stream of data for network access.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    /// <seealso cref="System.Net.Sockets.NetworkStream"/>
    public sealed class NetworkStream : Stream
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private StreamSocket _socket;
        private Stream _inputStream;
        private Stream _outputStream;

        public NetworkStream(StreamSocket socket)
        {
            _socket = socket;
            _inputStream = _socket.InputStream.AsStreamForRead();
            _outputStream = _socket.OutputStream.AsStreamForWrite();
        }
#elif __ANDROID__
        private BluetoothSocket _socket;

        public NetworkStream(Android.Bluetooth.BluetoothSocket socket)
        {
            _socket = socket;
        }
#endif

        public override bool CanRead
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _inputStream.CanRead;
#elif __ANDROID__
                return _socket.InputStream.CanRead;
#else
                return false;
#endif
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _outputStream.CanWrite;
#elif __ANDROID__
                return _socket.OutputStream.CanWrite;
#else
                return false;
#endif
            }
        }

        public override long Length
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _inputStream.Length;
#elif __ANDROID__
                return _socket.InputStream.Length;
#else
                return 0;
#endif
            }
        }

        public override long Position
        {
            get
            {
                return 0;
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public override void Flush()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _outputStream.Flush();
#elif __ANDROID__
            _socket.OutputStream.Flush();
#endif
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _inputStream.Read(buffer, offset, count);
#elif __ANDROID__
            return _socket.InputStream.Read(buffer, offset, count);
#else
            return 0;
#endif
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _outputStream.Write(buffer, offset, count);
#elif __ANDROID__
            _socket.OutputStream.Write(buffer, offset, count);
#endif
        }

        protected override void Dispose(bool disposing)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _outputStream?.Dispose();
            _outputStream = null;
            _inputStream?.Dispose();
            _outputStream = null;
            _socket?.Dispose();
            _socket = null;
#elif __ANDROID__
            _socket?.Dispose();
            _socket = null;
#endif

            base.Dispose(disposing);
        }
    }
}