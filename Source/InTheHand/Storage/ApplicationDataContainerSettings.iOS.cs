//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainerSettings.iOS.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-18 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Foundation;

using InTheHand.Foundation.Collections;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;

namespace InTheHand.Storage
{
    partial class ApplicationDataContainerSettings :
        IPropertySet, IDictionary<string, object>, IEnumerable<KeyValuePair<string, object>>, IObservableMap<string, object>
    {

        private NSUserDefaults _defaults;
        private NSUbiquitousKeyValueStore _store;

        private NSObject _observer;

        private void Initialize(string name)
        {
            switch (_locality)
            {
                case ApplicationDataLocality.Roaming:
                    _store = NSUbiquitousKeyValueStore.DefaultStore;
                    break;

                case ApplicationDataLocality.SharedLocal:
                    _defaults = new NSUserDefaults(name, NSUserDefaultsType.SuiteName);
                    if(_defaults == null)
                    {
                        throw new ArgumentException("name");
                    }
                    break;

                default:
                    _defaults = NSUserDefaults.StandardUserDefaults;
                    break;
            }
        }

        private void AddMapChanged()
        {
            if (!IsRoaming)
            {
                _observer = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("NSUserDefaultsDidChangeNotification"), (n) =>
                {
                    NSUserDefaults defaults = n.Object as NSUserDefaults;

                    if (defaults == _defaults)
                    {
                        // indicate a reset change (because we can't determine the specific key)
                        _mapChanged?.Invoke(this, new ApplicationDataMapChangedEventArgs(null, CollectionChange.Reset));
                    }
                });
            }
        }

        private void DoAdd(string key, object value)
        {
            this[key] = value;
        }
        
        private bool DoContainsKey(string key)
        {
            if (IsRoaming)
            {
                return _store.ValueForKey(new NSString(key)) != null;
            }
            else
            {
                // TODO: see if there is a more efficient way of checking the key exists
                return _defaults.ValueForKey(new NSString(key)) != null;
            }
        }
        
        private bool DoRemove(string key)
        {
            if (IsRoaming)
            {
                _store.Remove(key);
            }
            else
            {
                _defaults.RemoveObject(key);
            }

            return true;
        }
        
        private bool DoTryGetValue(string key, out object value)
        {
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
        }
        
        private object GetItem(string key)
        {
           
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

            }

        private void SetItem(string key, object value)
        {
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

                    case TypeCode.Int64:
                        if (IsRoaming)
                        {
                            _store.SetLong(key, (long)value);
                        }
                        else
                        {
                            _defaults.SetInt((nint)value, key);
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
        }
        
        private void DoClear()
        {
#if __IOS__ || __TVOS__ || __WATCHOS__
            if (IsRoaming)
            {
                _store.Init();
            }
            else
            {
                _defaults.Init();
            }
#endif
        }
       
        private bool DoContains(string key, object value)
        {
            if(DoContainsKey(key))
            {
                if(GetItem(key) == value)
                {
                    return true;
                }
            }

            return false;
        }

        private int GetCount()
        {
            if (IsRoaming)
            {
                return (int)_store.ToDictionary().Count;
            }
            else
            {
                return (int)_defaults.ToDictionary().Count;
            }
        }

        private ICollection<string> GetKeys()
        {
            ICollection<string> genericKeys = new Collection<string>();
            if (IsRoaming)
            {
                foreach (NSObject item in _store.ToDictionary().Keys)
                {
                    genericKeys.Add(item.ToString());
                }
            }
            else
            {
                foreach (NSObject item in _defaults.ToDictionary().Keys)
                {
                    genericKeys.Add(item.ToString());
                }
            }

            return genericKeys;
        }

        private ICollection<object> GetValues()
        {
            ICollection<object> vals = new Collection<object>();

            foreach (KeyValuePair<string, object> item in this)
            {
                vals.Add(item.Value);
            }

            return vals;
        }

        private IEnumerator<KeyValuePair<string, object>> DoGetEnumerator()
        {
            if (IsRoaming)
            {
                return new ApplicationDataContainerEnumerator(_store.ToDictionary());
            }
            else
            {
                return new ApplicationDataContainerEnumerator(_defaults.ToDictionary());
            }
        }

    }

    internal sealed class ApplicationDataContainerEnumerator : IEnumerator<KeyValuePair<string, object>>
    {
        private NSDictionary _dictionary;
        private IEnumerator<KeyValuePair<NSObject,NSObject>> _enumerator;

        internal ApplicationDataContainerEnumerator(NSDictionary dictionary)
        {
            _dictionary = dictionary;
            _enumerator = _dictionary.GetEnumerator();
        }

        public KeyValuePair<string, object> Current
        {
            get
            {
                var current = _enumerator.Current;
                return new KeyValuePair<string, object>(current.Key.ToString(), IOSTypeConverter.ConvertToObject(current.Value));
            }
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        public void Dispose()
        {
            _enumerator.Reset();
        }
    }

    internal static class IOSTypeConverter
    {
        public static object ConvertToObject(NSObject obj)
        {
            object val = null;

            if (obj != null)
            {
                if(obj is NSString)
                {
                    string s = obj.ToString();

                    //allows us to persist datetimes with offsets in a roundtrippable format (NSDate doesn't do timezones)
                    DateTimeOffset dt;
                    if(DateTimeOffset.TryParse(s, out dt))
                    {
                        return dt;
                    }

                    return s;
                }
                else if(obj is NSDate)
                {
                    return DateTimeOffsetHelper.FromNSDate((NSDate)obj);
                }
                else if(obj is NSUuid)
                {
                    return new Guid(((NSUuid)obj).GetBytes());
                }
                else if(obj is NSDecimalNumber)
                {
                    return decimal.Parse(obj.ToString(), CultureInfo.InvariantCulture);
                }
                else if(obj is NSNumber)
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
                else if (obj.GetType() == typeof(NSDate))
                {
                    val = DateTimeOffsetHelper.FromNSDate((NSDate)obj);
                }
                else if(obj is NSArray)
                {
                    NSArray array = obj as NSArray;
                    object[] oarray = new object[array.Count];
                    for(int i = 0; i < (int)array.Count; i++)
                    {
                        oarray[i] = ConvertToObject(array.GetItem<NSObject>((nuint)i));
                    }
                    val = oarray;
                }
                else if(obj is NSNull)
                {
                    val = null;
                }
                else
                {
                    val = obj;
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
                else if(obj is DateTime)
                {
                    return ((DateTime)obj).ToNSDate();
                }
                else if(obj is DateTimeOffset)
                {
                    return new NSString(((DateTimeOffset)obj).ToString("O"));
                }
            }

            return null;
        }
    }
}