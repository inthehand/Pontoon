// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IObservableMap.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace InTheHand.Foundation.Collections
{
    /// <summary>
    /// Notifies listeners of dynamic changes to a map, such as when items are added or removed.
    /// </summary>
    /// <typeparam name="K">The type of the keys in the map.</typeparam>
    /// <typeparam name="V">The type of the values in the map.</typeparam>
    public interface IObservableMap<K,V>
    {
        /// <summary>
        /// Occurs when the map changes.
        /// </summary>
        event MapChangedEventHandler<K, V> MapChanged;
    }

    /// <summary>
    /// Represents the method that handles the changed event of an observable map.
    /// </summary>
    /// <typeparam name="K">The type of the keys in the map.</typeparam>
    /// <typeparam name="V">The type of the values in the map.</typeparam>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    public delegate void MapChangedEventHandler<K, V>(IObservableMap<K, V> sender, IMapChangedEventArgs<K> eventArgs);

    /// <summary>
    /// Provides data for the changed event of a map collection.
    /// </summary>
    /// <typeparam name="K">The type of the keys in the map.</typeparam>
    public interface IMapChangedEventArgs<K>
    {
        /// <summary>
        /// Gets the type of change that occurred in the map.
        /// </summary>
        CollectionChange CollectionChange { get; }

        /// <summary>
        /// Gets the key of the item that changed.
        /// </summary>
        K Key { get; }
    }

    /// <summary>
    /// Represents a collection of key-value pairs, correlating several other collection interfaces.
    /// </summary>
    public interface IPropertySet : IObservableMap<string, object>, IDictionary<string, object>, IEnumerable<KeyValuePair<string,object>>
    {

    }

    /// <summary>
    /// Describes the action that causes a change to a collection.
    /// </summary>
    public enum CollectionChange
    {
        /// <summary>
        /// The collection is changed.
        /// </summary>
        Reset = 0,

        /// <summary>
        /// An item is added to the collection.
        /// </summary>
        ItemInserted = 1,

        /// <summary>
        /// An item is removed from the collection.
        /// </summary>
        ItemRemoved = 2,

        /// <summary>
        /// An item is changed in the collection.
        /// </summary>
        ItemChanged = 3,
    }
}