// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallStore.cs" company="In The Hand Ltd">
//   Copyright (c) 2014-2016 In The Hand Ltd, All rights reserved.
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
using Windows.Foundation;
using Windows.UI.Popups;
#elif WINDOWS_PHONE_APP
using Windows.Foundation;
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
#if __IOS__
        internal PhoneCallStore()
        {
        }
#elif WINDOWS_UWP
        private Windows.ApplicationModel.Calls.PhoneCallStore _store;

        internal PhoneCallStore(Windows.ApplicationModel.Calls.PhoneCallStore store)
        {
            _store = store;
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

        public async Task<Guid> GetDefaultLineAsync()
        {
#if WINDOWS_UWP
            return await _store.GetDefaultLineAsync();
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            return await (_type.GetRuntimeMethod("GetDefaultLineAsync", new Type[0]).Invoke(_store, new object[0]) as IAsyncOperation<Guid>);
#else
            return Guid.Empty;
#endif   
        }
    }
}
