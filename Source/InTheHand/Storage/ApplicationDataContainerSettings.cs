//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainerSettings.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using InTheHand.Foundation.Collections;
using InTheHand;

#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Preferences;
#elif TIZEN
using Tizen.Applications;
#endif

namespace InTheHand.Storage
{

    /// <summary>
    /// Provides access to the settings in a settings container.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
    /// </remarks>
    public sealed partial class ApplicationDataContainerSettings :
#if __ANDROID__
        Java.Lang.Object, ISharedPreferencesOnSharedPreferenceChangeListener, 
#endif
        IPropertySet, IDictionary<string, object>, IEnumerable<KeyValuePair<string, object>>, IObservableMap<string, object>
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Storage.ApplicationDataContainerSettings _settings;

        internal ApplicationDataContainerSettings(Windows.Foundation.Collections.IPropertySet settings)
        {
            _settings = (Windows.Storage.ApplicationDataContainerSettings)settings;
        }

        public static implicit operator Windows.Storage.ApplicationDataContainerSettings(ApplicationDataContainerSettings s)
        {
            return s._settings;
        }
#else
        private ApplicationDataLocality _locality;

        internal ApplicationDataContainerSettings(ApplicationDataLocality locality, string name)
        {
            _locality = locality;
#if __ANDROID__ || WINDOWS_PHONE
            Initialize();
#elif __UNIFIED__
            Initialize(name);
#endif
        }

        private bool IsRoaming
        {
            get
            {
                return _locality == ApplicationDataLocality.Roaming;
            }
        }
#endif


        private event MapChangedEventHandler<string, object> _mapChanged;
        /// <summary>
        /// Occurs when the map changes.
        /// </summary>
        public event MapChangedEventHandler<string, object> MapChanged
        {
            add
            {
                if (_mapChanged == null)
                {
#if __ANDROID__ || __UNIFIED__
                    AddMapChanged();
#endif
                }
                _mapChanged += value;
            }

            remove
            {
                _mapChanged -= value;

                if(_mapChanged == null)
                {
#if __ANDROID__
                    RemoveMapChanged();
#endif
                }
            }
        }

        #region IDictionary<string,object> Members

        /// <summary>
        /// Adds an item to the <see cref="ApplicationDataContainerSettings"/>. 
        /// </summary>
        /// <param name="key">The key of the item to add.</param>
        /// <param name="value">The item value to add.</param>
        public void Add(string key, object value)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _settings.Add(key,value);

#elif __ANDROID__ || WINDOWS_PHONE || __UNIFIED__
            DoAdd(key, value);

#elif TIZEN
            Preference.Set(key, value);
#else
            throw new PlatformNotSupportedException();
#endif
            //OnMapChanged(key, CollectionChange.ItemInserted);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified key exists in the <see cref="ApplicationDataContainerSettings"/>.
        /// </summary>
        /// <param name="key">The key to check for in the <see cref="ApplicationDataContainerSettings"/>.</param>
        /// <returns>true if an item with that key exists in the <see cref="ApplicationDataContainerSettings"/>; otherwise, false. </returns>
        public bool ContainsKey(string key)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _settings.ContainsKey(key);

#elif __ANDROID__ || WINDOWS_PHONE || __UNIFIED__
            return DoContainsKey(key);

#elif TIZEN
            return Preference.Contains(key);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets an ICollection object containing the keys of the <see cref="ApplicationDataContainerSettings"/>.
        /// </summary>
        public ICollection<string> Keys
        {
            get
            {
                ICollection<string> genericKeys = new Collection<string>();
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _settings.Keys;

#elif __ANDROID__ || WINDOWS_PHONE
                return GetKeys();

#elif TIZEN
                genericKeys = new List<string>(Preference.Keys);
#endif
                return genericKeys;
            }
        }

        /// <summary>
        /// Removes a specific item from the <see cref="ApplicationDataContainerSettings"/>.
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        /// <returns>true if the item was removed, otherwise false.</returns>
        public bool Remove(string key)
        {
            bool removed = false;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            removed = _settings.Remove(key);

#elif __ANDROID__ || WINDOWS_PHONE || __UNIFIED__
            removed = DoRemove(key);

#elif TIZEN
            Preference.Remove(key);
            removed = true;
#else
            throw new PlatformNotSupportedException();
#endif
            /*if (removed)
            {
                OnMapChanged(key, CollectionChange.ItemRemoved);
            }*/

            return removed;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified key exists in the <see cref="ApplicationDataContainerSettings"/>.
        /// If an item with that key exists, the item is retrieved as an out parameter.
        /// </summary>
        /// <param name="key">The key to check for in the <see cref="ApplicationDataContainerSettings"/>.</param>
        /// <param name="value">The item, if it exists.
        /// Contains null if the item does not exist in the <see cref="ApplicationDataContainerSettings"/>.</param>
        /// <returns>true if an item with that key exists in the <see cref="ApplicationDataContainerSettings"/>; otherwise, false.</returns>
        public bool TryGetValue(string key, out object value)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _settings.TryGetValue(key, out value);

#elif __ANDROID__ || WINDOWS_PHONE || __UNIFIED__
            return DoTryGetValue(key, out value);

#elif TIZEN
            try
            {
                value = Preference.Get<object>(key);
                return value != null;
            }
            catch
            {
                value = null;
                return false;
            }
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets an <see cref="ICollection{T}"/> object containing the values of the <see cref="ApplicationDataContainerSettings"/>.
        /// </summary>
        public ICollection<object> Values
        {
            get
            {
                Collection<object> genericValues = new Collection<object>();
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _settings.Values;

#elif __ANDROID__ || WINDOWS_PHONE
                return GetValues();

#else
                //throw new PlatformNotSupportedException();
#endif
                return genericValues;
            }
        }

        /// <summary>
        /// Gets or sets the element value at the specified key index.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The element value at the specified key index.</returns>
        public object this[string key]
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _settings[key];

#elif __ANDROID__ || WINDOWS_PHONE || __UNIFIED__
                return GetItem(key);

#elif TIZEN
                object value;
                if(TryGetValue(key, out value))
                {
                    return value;
                }

                return null;
#else
                throw new PlatformNotSupportedException();
#endif

            }

            set
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _settings[key] = value;

#elif  __ANDROID__ || WINDOWS_PHONE || __UNIFIED__
                SetItem(key, value);

#elif TIZEN
                Add(key, value);
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

#endregion

#region ICollection<KeyValuePair<string,object>> Members

        /// <summary>
        /// Adds a new key-value pair to the ApplicationDataContainerSettings. 
        /// </summary>
        /// <param name="item">The key-value pair to add.</param>
        public void Add(KeyValuePair<string, object> item)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _settings.Add(item);
#else
            Add(item.Key, item.Value);
#endif
        }

        /// <summary>
        /// Removes all related application settings.
        /// </summary>
        public void Clear()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _settings.Clear();

#elif __ANDROID__ || WINDOWS_PHONE || __UNIFIED__
            DoClear();

#elif TIZEN
            Preference.RemoveAll();

#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Returns a value that indicates whether a specified key-value pair exists in the ApplicationDataContainerSettings.
        /// </summary>
        /// <param name="item">The key-value pair to check for in the ApplicationDataContainerSettings.</param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _settings.Contains(item);

#elif __ANDROID__ || WINDOWS_PHONE
            return DoContains(item.Key, item.Value);

#else
            if(ContainsKey(item.Key))
            {
                if(this[item.Key] == item.Value)
                {
                    return true;
                }
            }
#endif
            return false;
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at a particular array index. 
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from the collection.
        /// The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins. </param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _settings.CopyTo(array, arrayIndex);

#elif __ANDROID__ || TIZEN
            int index = arrayIndex;

            foreach(string key in Keys)
            {
                array[index++] = new KeyValuePair<string, object>(key, this[key]);
            }
#else
            throw new NotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _settings.Count;

#elif __ANDROID__ || WINDOWS_PHONE
                return GetCount();

#elif __UNIFIED__
                return -1;

#elif TIZEN
                int count = 0;
                foreach(string key in Preference.Keys)
                {
                    count++;
                }

                return count;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets a value indicating whether the dictionary is read-only. 
        /// </summary>
        /// <value>true if the dictionary is read-only; otherwise, false.</value>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a specific key-value pair from the <see cref="ApplicationDataContainerSettings"/>. 
        /// </summary>
        /// <param name="item">The key-value pair to remove.</param>
        /// <returns>true if the item was removed, otherwise false.</returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _settings.Remove(item);
#else
            return Remove(item.Key);
#endif
        }

#endregion

#region IEnumerable<KeyValuePair<string,object>> Members

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string,object>>.GetEnumerator()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return ((IEnumerable<KeyValuePair<string,object>>)_settings).GetEnumerator();
#elif WINDOWS_PHONE
            return DoGetEnumerator();
#else
            throw new NotSupportedException();
#endif
        }

#endregion

#region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return ((IEnumerable)_settings).GetEnumerator();

#elif WINDOWS_PHONE
            return DoGetEnumerator();

#else
            throw new NotSupportedException();
#endif
        }

#endregion
    }



    internal sealed class ApplicationDataMapChangedEventArgs : IMapChangedEventArgs<string>
    {
        internal ApplicationDataMapChangedEventArgs(string key, CollectionChange change)
        {
            Key = key;
            CollectionChange = change;
        }

        public CollectionChange CollectionChange
        {
            get;
            private set;
        }

        public string Key
        {
            get;
            private set;
        }
    }
}