// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllowMultipleAttribute.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.AllowMultipleAttribute))]
#else

using System;

namespace Windows.Foundation.Metadata
{
    [AttributeUsage(AttributeTargets.Class)]
    [ContractVersion(typeof(FoundationContract), 65536U)]
    public sealed class AllowMultipleAttribute : Attribute
    {
    }
}
#endif