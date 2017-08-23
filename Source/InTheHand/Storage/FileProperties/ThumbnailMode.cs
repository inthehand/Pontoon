//-----------------------------------------------------------------------
// <copyright file="ThumbnailMode.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Storage.FileProperties
{
    /// <summary>
    /// Describes the purpose of the thumbnail to determine how to adjust the thumbnail image to retrieve.
    /// </summary>
    public enum ThumbnailMode
    {
        /// <summary>
        /// To display previews of picture or video files.
        /// </summary>
        MediaView = 0,

        /// <summary>
        /// To display previews of files (or other items) in a list.
        /// </summary>
        ListView = 4,

        /// <summary>
        /// To display a preview of any single item (like a file, folder, or file group).
        /// </summary>
        SingleItem = 5,
    }
}