//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainerSettings.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-16 In The Hand Ltd. All rights reserved.
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
#elif WINDOWS_PHONE
using System.IO.IsolatedStorage;
#elif __IOS__
using Foundation;
using global::System.Globalization;
#endif

//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Storage.ApplicationDataContainerSettings))]
//#elif WINDOWS_PHONE
//// not used in 8.0
//#else
namespace InTheHand.Storage
{

    /// <summary>
    /// Provides access to the settings in a settings container.
    /// </summary>
    public sealed class ApplicationDataContainerSettings :
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
#if __ANDROID__
            _preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
#elif WINDOWS_PHONE
            applicationSettings = global::System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Deactivated += Current_Deactivated;
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Closing += Current_Closing;
#elif __IOS__
            switch(locality)
            {
                case ApplicationDataLocality.Roaming:
                    _store = NSUbiquitousKeyValueStore.DefaultStore;
                    break;

                case ApplicationDataLocality.SharedLocal:
                    _defaults = new NSUserDefaults(sharedName, NSUserDefaultsType.SuiteName);
                    if(_defaults == null)
                    {
                        throw new ArgumentException("name");
                    }
                    break;

                default:
                    _defaults = NSUserDefaults.StandardUserDefaults;
                    break;
            }
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
#if __ANDROID__
                    _preferences.RegisterOnSharedPreferenceChangeListener(this);
#elif __IOS__
                    if (!IsRoaming)
                    {
                        _observer = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("NSUserDefaultsDidChangeNotification"), (n) =>
                        {
                            if (_mapChanged != null)
                            {
                            // indicate a reset change (because we can't determine the specific key)
                            _mapChanged(this, new ApplicationDataMapChangedEventArgs(null, CollectionChange.Reset));
                            }
                        });
                    }
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
                    _preferences.UnregisterOnSharedPreferenceChangeListener(this);
#endif
                }
            }
        }

#if __ANDROID__
        private ISharedPreferences _preferences;

        void ISharedPreferencesOnSharedPreferenceChangeListener.OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            if(_mapChanged != null)
            {
                _mapChanged(this, new ApplicationDataMapChangedEventArgs(key, CollectionChange.Reset));
            }
        }
#elif __IOS__
        private NSUserDefaults _defaults;
        private NSUbiquitousKeyValueStore _store;

        private NSObject _observer;

#elif WINDOWS_PHONE
        private IsolatedStorageSettings applicationSettings;

        void rootFrame_Navigating(object sender, global::System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            applicationSettings.Save();
        }

        void Current_Deactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            applicationSettings.Save();
        }

        void Current_Closing(object sender, Microsoft.Phone.Shell.ClosingEventArgs e)
        {
            applicationSettings.Save();
        }

#endif

#region IDictionary<string,object> Members

        /// <summary>
        /// Adds an item to the <see cref="ApplicationDataContainerSettings"/>. 
        /// </summary>
        /// <param name="key">The key of the item to add.</param>
        /// <param name="value">The item value to add.</param>
        public void Add(string key, object value)
        {
#if __ANDROID__
            ISharedPreferencesEditor editor = _preferences.Edit();
            List<string> pkg = new List<string>();
            pkg.Add(value.GetType().Name);
            pkg.Add(value.ToString());
            editor.PutStringSet(key, pkg);
            editor.Commit();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _settings.Add(key,value);
#elif WINDOWS_PHONE
            if (value is DateTimeOffset)
            {
                DateTimeOffset offset = (DateTimeOffset)value;
                value = offset.UtcDateTime;
            }

            applicationSettings.Add(key, value);
#elif __IOS__
            this[key] = value;

            /*if (value == null)
            {
                _defaults.SetNilValueForKey(new NSString(key));
            }
            else
            {
                TypeCode code = Type.GetTypeCode(value.GetType());
                switch (code)
                {
                    case TypeCode.String:
                        _defaults.SetString(value.ToString(), key);
                        break;
                    case TypeCode.Int32:
                        _defaults.SetInt((int)value, key);
                        break;
                    case TypeCode.Double:
                        _defaults.SetDouble((double)value, key);
                        break;
                    case TypeCode.Single:
                        _defaults.SetFloat((float)value, key);
                        break;
                    case TypeCode.Boolean:
                        _defaults.SetBool((bool)value, key);
                        break;
                    case TypeCode.DateTime:
                        NSDate nsd = NSDate.FromTimeIntervalSince1970(((DateTime)value).Ticks);
                        _defaults.SetValueForKey(nsd, new NSString(key));
                        break;

                    default:
                        break;
                }
            }*/
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
#if __ANDROID__
            object o = null;
            bool success = TryGetValue(key, out o);
            return success;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _settings.ContainsKey(key);
#elif WINDOWS_PHONE
            return applicationSettings.Contains(key);
#elif __IOS__
            if (IsRoaming)
            {
                return _store.ValueForKey(new NSString(key)) != null;
            }
            else
            {
                // TODO: see if there is a more efficient way of checking the key exists
                return _defaults.ValueForKey(new NSString(key)) != null;
            }
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
                Collection<string> genericKeys = new Collection<string>();
#if __ANDROID__
                foreach(KeyValuePair<string,object> entry in _preferences.All)
                {
                    genericKeys.Add(entry.Key);
                }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _settings.Keys;
#elif WINDOWS_PHONE
                foreach (string key in applicationSettings.Keys)
                {
                    genericKeys.Add(key);
                }
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
#if __ANDROID__
            ISharedPreferencesEditor editor = _preferences.Edit();
            editor.Remove(key);
            bool removed = editor.Commit();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            bool removed = _settings.Remove(key);
#elif WINDOWS_PHONE
            bool removed = applicationSettings.Remove(key);
#elif __IOS__
            bool removed = true;
            if (IsRoaming)
            {
                _store.Remove(key);
            }
            else
            {
                _defaults.RemoveObject(key);
            }
#else
            bool removed = false;
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
#if __ANDROID__
            ICollection<string> vals = _preferences.GetStringSet(key, new List<string> { "null", "" });
            string type = string.Empty;
            string val = string.Empty;
            foreach (string v in vals)
            {
                if (string.IsNullOrEmpty(type))
                {
                    type = v;
                }
                else
                {
                    val = v;
                    break;
                }
            }

            //todo deserialise type
            switch (type)
            {
                case "null":
                    value = null;
                    return false;
                case "System.Boolean":
                    value = bool.Parse(val);
                    return true;
                case "System.Int32":
                    value = int.Parse(val);
                    return true;
                case "System.Int64":
                    value = long.Parse(val);
                    return true;
                case "System.Single":
                    value = float.Parse(val);
                    return true;
                case "System.DateTimeOffset":
                    value = DateTimeOffset.Parse(val);
                    return true;
                default:
                    value = val;
                    return true;
            }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _settings.TryGetValue(key, out value);
#elif WINDOWS_PHONE
            return applicationSettings.TryGetValue<object>(key, out value);
#elif __IOS__
            NSObject obj = null;
            if (IsRoaming)
            {
                obj = _store.ValueForKey(new NSString(key));
            }
            else
            {
                obj = _defaults.ValueForKey(new NSString(key));
            }
            value = IOSTypeConverter.ConvertToObject(obj);
            return obj != null;
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets an ICollection object containing the values of the <see cref="ApplicationDataContainerSettings"/>.
        /// </summary>
        public ICollection<object> Values
        {
            get
            {
                Collection<object> genericValues = new Collection<object>();
#if __ANDROID__
                foreach(KeyValuePair<string,object> kvp in _preferences.All)
                {
                    ICollection<string> rawVal = kvp.Value as ICollection<string>;
                    if(rawVal != null)
                    {
                        string type = string.Empty;
                        string val = string.Empty;
                        foreach (string v in rawVal)
                        {
                            if (string.IsNullOrEmpty(type))
                            {
                                type = v;
                            }
                            else
                            {
                                val = v;
                                break;
                            }
                        }

                        //todo deserialise type
                        switch (type)
                        {
                            case "System.Boolean":
                                genericValues.Add( bool.Parse(val));
                                break;
                            case "System.Int32":
                                genericValues.Add(int.Parse(val));
                                break;
                            case "System.Int64":
                                genericValues.Add(long.Parse(val));
                                break;
                            case "System.Single":
                                genericValues.Add(float.Parse(val));
                                break;
                            case "System.DateTimeOffset":
                                genericValues.Add(DateTimeOffset.Parse(val));
                                break;
                            default:
                                genericValues.Add(val);
                                break;
                        }
                    }
                }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _settings.Values;
#elif WINDOWS_PHONE
                foreach (object value in applicationSettings.Values)
                {
                    genericValues.Add(value);
                }
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
#if __ANDROID__
               ICollection<string> vals =  _preferences.GetStringSet(key, new List<string> { "null", "" });
                string type = string.Empty;
                string val = string.Empty;
                foreach(string v in vals)
                {
                    if(string.IsNullOrEmpty(type))
                    {
                        type = v;
                    }
                    else
                    {
                        val = v;
                        break;
                    }
                }

                //todo deserialise type
                switch(type)
                {
                    case "null":
                        return null;

                    default:
                        return val;
                }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _settings[key];
#elif WINDOWS_PHONE
                object value = applicationSettings[key];
                if (value is DateTime)
                {
                    DateTime dateTime = (DateTime)value;
                    value = new DateTimeOffset(dateTime);
                }
                
                return value;
#elif __IOS__
                NSObject obj = null;
                if (IsRoaming)
                {
                    obj = _store.ValueForKey(new NSString(key));
                }
                else
                {
                    obj = _defaults.ValueForKey(new NSString(key));
                }
                return IOSTypeConverter.ConvertToObject(obj);
#else
                throw new PlatformNotSupportedException();
#endif

            }

            set
            {
#if __ANDROID__
                Add(key, value);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _settings[key] = value;
#elif WINDOWS_PHONE
                // temporary workaround while investigating datetimeoffset behaviour in isostore
                if (value is DateTimeOffset)
                {
                    DateTimeOffset offset = (DateTimeOffset)value;
                    value = offset.UtcDateTime;
                }

                if (applicationSettings.Contains(key))
                {
                    if (applicationSettings[key] != value)
                    {
                        applicationSettings[key] = value;
                        //OnMapChanged(key, CollectionChange.ItemChanged);
                    }
                }
                else
                {
                    // if not present add a new value (matches RT behaviour)
                    Add(key, value);
                }
#elif __IOS__
                if (value == null)
                {
                    if (IsRoaming)
                    {
                        _store.Remove(key);
                    }
                    else
                    {
                        _defaults.RemoveObject(key);
                    }
                }
                else
                {
                    TypeCode code = Type.GetTypeCode(value.GetType());
                    switch (code)
                    {
                        case TypeCode.String:
                            if (IsRoaming)
                            {
                                _store.SetString(key, value.ToString());
                            }
                            else
                            {
                                _defaults.SetString(value.ToString(), key);
                            }
                            break;
                        case TypeCode.Int32:
                            if (IsRoaming)
                            {
                                _store.SetLong(key, (long)value);
                            }
                            else
                            {
                                _defaults.SetInt((int)value, key);
                            }
                            break;
                        case TypeCode.Double:
                            if (IsRoaming)
                            {
                                _store.SetDouble(key, (double)value);
                            }
                            else
                            {
                                _defaults.SetDouble((double)value, key);
                            }
                            break;
                        case TypeCode.Single:
                            if (IsRoaming)
                            {
                                _store.SetDouble(key, (double)value);
                            }
                            else
                            {
                                _defaults.SetFloat((float)value, key);
                            }
                            break;
                        case TypeCode.Boolean:
                            if (IsRoaming)
                            {
                                _store.SetBool(key, (bool)value);
                            }
                            else
                            {
                                _defaults.SetBool((bool)value, key);
                            }
                            break;

                        default:
                            if (IsRoaming)
                            {
                                _store.SetValueForKey(IOSTypeConverter.ConvertToNSObject(value), new NSString(key));
                            }
                            else
                            {
                                _defaults.SetValueForKey(IOSTypeConverter.ConvertToNSObject(value), new NSString(key));
                            }
                            break;
                    }
                }
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
#if __ANDROID__
            ISharedPreferencesEditor editor = _preferences.Edit();
            editor.Clear();
            editor.Commit();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _settings.Clear();
#elif WINDOWS_PHONE
            applicationSettings.Clear();
#elif __IOS__
            if (IsRoaming)
            {
                _store.Init();
            }
            else
            {
                _defaults.Init();
            }
#else
            throw new PlatformNotSupportedException();
#endif
            //OnMapChanged(null, CollectionChange.Reset);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified key-value pair exists in the ApplicationDataContainerSettings.
        /// </summary>
        /// <param name="item">The key-value pair to check for in the ApplicationDataContainerSettings.</param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
#if __ANDROID__
            object o = null;
            bool success = TryGetValue(item.Key, out o);
            return item.Value == o;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _settings.Contains(item);
#elif WINDOWS_PHONE
            if (applicationSettings.Contains(item.Key))
            {
                object value = applicationSettings[item.Key];
                if (value is DateTime)
                {
                    DateTime dateTime = (DateTime)value;
                    value = new DateTimeOffset(dateTime);
                }

                if (value == item.Value)
                {
                    return true;
                }
            }
#else
            throw new PlatformNotSupportedException();
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
#if __ANDROID__
                return _preferences.All.Count;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _settings.Count;
#elif WINDOWS_PHONE
                return applicationSettings.Count;
#elif __IOS__
                return -1;
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
            get { return false; }
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
            return new ApplicationDataContainerEnumerator();
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
            return new ApplicationDataContainerEnumerator();
#else
            throw new NotSupportedException();
#endif
        }

#endregion
    }

#if WINDOWS_PHONE
    internal sealed class ApplicationDataContainerEnumerator : IEnumerator<KeyValuePair<string, object>>
    {

        private global::System.IO.IsolatedStorage.IsolatedStorageSettings settings;
        private IEnumerator keyEnumerator;

        internal ApplicationDataContainerEnumerator()
        {
            settings = global::System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            keyEnumerator = settings.Keys.GetEnumerator();
        }

        public KeyValuePair<string, object> Current
        {
            get {
                object val = settings[keyEnumerator.Current.ToString()];
                return new KeyValuePair<string, object>(keyEnumerator.Current.ToString(), val);
            }
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            return keyEnumerator.MoveNext();
        }

        public void Reset()
        {
            keyEnumerator.Reset();
        }

        public void Dispose()
        {
            keyEnumerator.Reset();
        }
    }
#endif

#if __IOS__
    internal static class IOSTypeConverter
    {
        public static object ConvertToObject(NSObject obj)
        {
            object val = null;

            if (obj != null)
            {
                if(obj is NSString)
                {
                    return obj.ToString();
                }

                if(obj is NSDate)
                {
                    return DateTimeOffsetHelper.FromNSDate((NSDate)obj);
                }

                if(obj is NSUuid)
                {
                    return new Guid(((NSUuid)obj).GetBytes());
                }

                if(obj is NSDecimalNumber)
                {
                    return decimal.Parse(obj.ToString(), CultureInfo.InvariantCulture);
                }

                if(obj is NSNumber)
                {
                    var x = (NSNumber)obj;
                    switch(x.ObjCType)
                    {
                        case "c":
                            return x.BoolValue;
                        case "l":
                        case "i":
                            return x.Int32Value;
                        case "s":
                            return x.Int16Value;
                        case "q":
                            return x.Int64Value;
                        case "Q":
                            return x.UInt64Value;
                        case "C":
                            return x.ByteValue;
                        case "L":
                        case "I":
                            return x.UInt32Value;
                        case "S":
                            return x.UInt16Value;
                        case "f":
                            return x.FloatValue;
                        case "d":
                            return x.DoubleValue;
                        case "B":
                            return x.BoolValue;
                        default:
                            return x.ToString();
                    }
                }

                if (obj.GetType() == typeof(NSString))
                {
                    val = ((NSString)obj).ToString();
                }
                else if (obj.GetType() == typeof(NSDate))
                {
                    val = DateTimeOffsetHelper.FromNSDate((NSDate)obj);
                }
            }

            return val;
        }

        public static NSObject ConvertToNSObject(object obj)
        {
            if(obj != null)
            {
                if(obj is Boolean)
                {
                    return NSNumber.FromBoolean((bool)obj);
                }
                else if (obj is Byte)
                {
                    return NSNumber.FromByte((byte)obj);
                }
                else if (obj is SByte)
                {
                    return NSNumber.FromSByte((sbyte)obj);
                }
                else if (obj is Int16)
                {
                    return NSNumber.FromInt16((short)obj);
                }
                else if (obj is Int32)
                {
                    return NSNumber.FromInt32((int)obj);
                }
                else if (obj is Int64)
                {
                    return NSNumber.FromInt64((long)obj);
                }
                else if (obj is UInt16)
                {
                    return NSNumber.FromUInt16((ushort)obj);
                }
                else if (obj is UInt32)
                {
                    return NSNumber.FromUInt32((uint)obj);
                }
                else if (obj is UInt64)
                {
                    return NSNumber.FromUInt64((ulong)obj);
                }
                else if (obj is Single)
                {
                    return NSNumber.FromFloat((float)obj);
                }
                else if (obj is Double)
                {
                    return NSNumber.FromDouble((double)obj);
                }
                else if (obj is string)
                {
                    return new NSString(obj.ToString());
                }
                else if(obj is Guid)
                {
                    return new NSUuid(((Guid)obj).ToByteArray());
                }
                else if(obj is DateTimeOffset)
                {
                    return ((DateTimeOffset)obj).ToNSDate();
                }
            }

            return null;
        }
    }
#endif

    internal sealed class ApplicationDataMapChangedEventArgs : IMapChangedEventArgs<string>
    {
        internal ApplicationDataMapChangedEventArgs(string key, CollectionChange change)
        {
            this.Key = key;
            this.CollectionChange = change;
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
//#endif