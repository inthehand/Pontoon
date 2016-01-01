//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainerSettings.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using global::System;
using global::System.Collections;
using global::System.Collections.Generic;
using global::System.Collections.Specialized;
using global::System.Collections.ObjectModel;
#if __ANDROID__
using Android.Content;
using Android.Preferences;
#elif WINDOWS_PHONE
using System.IO.IsolatedStorage;
using Windows.Foundation.Collections;
#elif __IOS__
using Foundation;
using global::System.Globalization;
#endif

namespace InTheHand.Storage
{

    /// <summary>
    /// Provides access to the settings in a settings container.
    /// </summary>
    public sealed class ApplicationDataContainerSettings : IDictionary<string,object>, IEnumerable<KeyValuePair<string,object>>
    {
        internal ApplicationDataContainerSettings()
        {
#if __ANDROID__
            _preferences = PreferenceManager.GetDefaultSharedPreferences(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity);
#elif WINDOWS_APP || WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _settings = Windows.Storage.ApplicationData.Current.LocalSettings.Values;
#elif WINDOWS_PHONE
            applicationSettings = global::System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Deactivated += Current_Deactivated;
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Closing += Current_Closing;
#elif __IOS__
            _defaults = NSUserDefaults.StandardUserDefaults;
#endif
        }

#if __ANDROID__
        private ISharedPreferences _preferences;
#elif __IOS__
        private NSUserDefaults _defaults;
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
        private IPropertySet _settings;
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
            _settings.Add(key, value);
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
            return _settings.ContainsKey(key);
#elif WINDOWS_PHONE
            return applicationSettings.Contains(key);
#elif __IOS__
            // TODO: see if there is a more efficient way of checking the key exists
            return _defaults.ValueForKey(new NSString(key)) != null;
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
            bool removed = _settings.Remove(key);
#elif WINDOWS_PHONE
            bool removed = applicationSettings.Remove(key);
#elif __IOS__
            bool removed = true;
            _defaults.RemoveObject(key);
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
            return _settings.TryGetValue(key, out value);
#elif WINDOWS_PHONE
            return applicationSettings.TryGetValue<object>(key, out value);
#elif __IOS__
            NSObject obj = _defaults.ValueForKey(new NSString(key));
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
                return _settings.Values;
#elif WINDOWS_PHONE
                foreach (object value in applicationSettings.Values)
                {
                    genericValues.Add(value);
                }
#else
                throw new PlatformNotSupportedException();
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
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
                NSObject obj = _defaults.ValueForKey(new NSString(key));
                return IOSTypeConverter.ConvertToObject(obj);
#else
                throw new PlatformNotSupportedException();
#endif

            }

            set
            {
#if __ANDROID__
                Add(key, value);
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
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
                    _defaults.RemoveObject(key);
                    //_defaults.SetNilValueForKey(new NSString(key));
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

                        default:
                            _defaults.SetValueForKey(IOSTypeConverter.ConvertToNSObject(value), new NSString(key));
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
            Add(item.Key, item.Value);
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
            _settings.Clear();
#elif WINDOWS_PHONE
            applicationSettings.Clear();
#elif __IOS__
            _defaults.Init();
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
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
            throw new NotSupportedException();
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
                return _settings.Count;
#elif WINDOWS_PHONE
                return applicationSettings.Count;
#elif __IOS__
                // TODO: fix to compile with 8.4
                return 0;// Convert.ToInt32(_defaults.k);
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
            return Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,object>> Members

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string,object>>.GetEnumerator()
        {
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
            return _settings.GetEnumerator();
#else
            return new ApplicationDataContainerEnumerator();
#endif
        }

#endregion

#region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
            return _settings.GetEnumerator();
#else
            return new ApplicationDataContainerEnumerator();
#endif
        }

#endregion
    }

    internal sealed class ApplicationDataContainerEnumerator : IEnumerator<KeyValuePair<string, object>>
    {
#if WINDOWS_PHONE
        private global::System.IO.IsolatedStorage.IsolatedStorageSettings settings;
#elif __IOS__
        private NSUserDefaults _defaults;
#endif
        private IEnumerator keyEnumerator;

        internal ApplicationDataContainerEnumerator()
        {
#if WINDOWS_PHONE
            settings = global::System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            keyEnumerator = settings.Keys.GetEnumerator();
#elif __IOS__
            _defaults = NSUserDefaults.StandardUserDefaults;
            // TODO: temp fix for old xamarin version
            keyEnumerator = null;// _defaults.ToDictionary().Keys.GetEnumerator();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public KeyValuePair<string, object> Current
        {
            get {
#if WINDOWS_PHONE
                object val = settings[keyEnumerator.Current.ToString()];
#elif __IOS__
                object val = null;
                NSObject obj = _defaults.ValueForKey(new NSString(keyEnumerator.Current.ToString()));
                val = IOSTypeConverter.ConvertToObject(obj);
#else
                object val = null;
#endif
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
}
