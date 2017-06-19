// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallStore.cs" company="In The Hand Ltd">
//   Copyright (c) 2014-2017 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides methods for launching the built-in phone call UI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Reflection;
using System.Threading.Tasks;

#if __ANDROID__
using Android.App;
using Android.Content;
#elif WINDOWS_APP
using Windows.UI.Popups;
#elif WINDOWS_PHONE_APP
using Windows.ApplicationModel.Calls;
#elif WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace InTheHand.ApplicationModel.Calls
{
    /// <summary>
    /// Represents a collection of information about the phone lines available on a device.
    /// </summary>
    public sealed class PhoneCallStore
    {
#if __UNIFIED__ || __ANDROID__
        internal PhoneCallStore()
        {
        }

#elif WINDOWS_UWP
        private Windows.ApplicationModel.Calls.PhoneCallStore _store;

        private PhoneCallStore(Windows.ApplicationModel.Calls.PhoneCallStore store)
        {
            _store = store;
        }

        public static implicit operator Windows.ApplicationModel.Calls.PhoneCallStore(PhoneCallStore s)
        {
            return s._store;
        }

        public static implicit operator PhoneCallStore(Windows.ApplicationModel.Calls.PhoneCallStore s)
        {
            return new PhoneCallStore(s);
        }

#elif WINDOWS_APP || WINDOWS_PHONE_APP
        private object _store;
        private Type _type;

        internal PhoneCallStore(object store)
        {
            _store = store;
            _type = _store.GetType();
        }
#endif

        /// <summary>
        /// Gets the line ID for the current default phone line.
        /// </summary>
        /// <returns></returns>
        public Task<Guid> GetDefaultLineAsync()
        {
#if WINDOWS_UWP
            return _store.GetDefaultLineAsync().AsTask();
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            return (_type.GetRuntimeMethod("GetDefaultLineAsync", new Type[0]).Invoke(_store, new object[0]) as Windows.Foundation.IAsyncOperation<Guid>).AsTask<Guid>();
#else
            return Task.FromResult<Guid>(Guid.Empty);
#endif
        }
    }
}