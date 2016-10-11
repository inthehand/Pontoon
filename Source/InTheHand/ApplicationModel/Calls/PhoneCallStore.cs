// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallStore.cs" company="In The Hand Ltd">
//   Copyright (c) 2014-2016 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides methods for launching the built-in phone call UI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.Calls.PhoneCallStore))]
#else
using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Foundation;

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

namespace Windows.ApplicationModel.Calls
{
    /// <summary>
    /// Represents a collection of information about the phone lines available on a device.
    /// </summary>
    public sealed class PhoneCallStore
    {
#if __IOS__
        internal PhoneCallStore()
        {
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
        public IAsyncOperation<Guid> GetDefaultLineAsync()
        {
#if WINDOWS_APP || WINDOWS_PHONE_APP
            return _type.GetRuntimeMethod("GetDefaultLineAsync", new Type[0]).Invoke(_store, new object[0]) as IAsyncOperation<Guid>;
#else
            return Task.FromResult<Guid>(Guid.Empty).AsAsyncOperation<Guid>();
#endif
        }
    }
}
#endif