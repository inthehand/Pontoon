// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OverloadAttribute.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.OverloadAttribute))]
#else

using System;

namespace Windows.Foundation.Metadata
{
    [ContractVersion(typeof(FoundationContract), 65536U)]
    [AttributeUsage(global::System.AttributeTargets.Method)]
    public sealed class OverloadAttribute : Attribute
    {
        public OverloadAttribute(string method) { }
    }
}

#endif