// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraCaptureUIMode.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Media.Capture
{
    /// <summary>
    /// Determines whether the user interface for capturing from the attached camera allows capture of photos, videos, or both photos and videos.
    /// </summary>
    public enum CameraCaptureUIMode
    {
        /// <summary>
        /// Either a photo or video can be captured.
        /// </summary>
        PhotoOrVideo = 0,

        /// <summary>
        /// The user can only capture a photo.
        /// </summary>
        Photo = 1,

        /// <summary>
        /// The user can only capture a video. 
        /// </summary>
        Video = 2,
    }
}