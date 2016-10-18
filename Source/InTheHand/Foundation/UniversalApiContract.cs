// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UniversalApiContract.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(Windows.Foundation.UniversalApiContract))]
#else

using Windows.Foundation.Metadata;

namespace Windows.Foundation
{
    [ContractVersion(65536U)]
    [ApiContract]
    public struct UniversalApiContract
    {
    }
}
#endif