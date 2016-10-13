// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExclusiveToAttribute.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.ExclusiveToAttribute))]
#else

using System;

namespace Windows.Foundation.Metadata
{
    [ContractVersion(typeof(FoundationContract), 65536U)]
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class ExclusiveToAttribute : Attribute
    {
        public ExclusiveToAttribute(Type typeName) { }
    }
}
#endif