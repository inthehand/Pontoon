// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extensions for List[T]
    /// </summary>
    public static class InTheHandListExtensions
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this List<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }
    }
}