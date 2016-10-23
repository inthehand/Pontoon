// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extensions for <see cref="List{T}"/>.
    /// </summary>
    public static class InTheHandListExtensions
    {
        /// <summary>
        /// Returns a read-only <see cref="ReadOnlyCollection{T}"/> wrapper for the current collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>An object that acts as a read-only wrapper around the current <see cref="List{T}"/>.</returns>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this List<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }
    }
}