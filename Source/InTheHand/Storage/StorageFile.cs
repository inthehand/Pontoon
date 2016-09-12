//-----------------------------------------------------------------------
// <copyright file="StorageFile.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
[assembly: TypeForwardedTo(typeof(Windows.Storage.IStorageItem))]
[assembly: TypeForwardedTo(typeof(Windows.Storage.IStorageFile))]
[assembly: TypeForwardedTo(typeof(Windows.Storage.StorageFile))]
[assembly: TypeForwardedTo(typeof(Windows.Storage.StorageItemTypes))]
#else

namespace Windows.Storage
{

    /// <summary>
    /// Manipulates storage items (files and folders) and their contents, and provides information about them.
    /// </summary>
    public interface IStorageItem
    {
        /// <summary>
        /// Deletes the current item. 
        /// </summary>
        /// <returns></returns>
        IAsyncAction DeleteAsync();

        /// <summary>
        /// Determines whether the current IStorageItem matches the specified StorageItemTypes value.
        /// </summary>
        /// <param name="type">The value to match against.</param>
        /// <returns></returns>
        bool IsOfType(StorageItemTypes type);

        /// <summary>
        /// Gets the attributes of a storage item.
        /// </summary>
        FileAttributes Attributes { get; }

        /// <summary>
        /// Gets the date and time when the current item was created. 
        /// </summary>
        DateTimeOffset DateCreated { get; }

        /// <summary>
        /// Gets the name of the item including the file name extension if there is one.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Gets the full file-system path of the item, if the item has a path.
        /// </summary>
        string Path { get; }
    }

    /// <summary>
    /// Describes whether an item that implements the <see cref="IStorageItem"/> interface is a file or a folder.
    /// </summary>
    public enum StorageItemTypes
    {
        /// <summary>
        /// A storage item that is neither a file nor a folder.
        /// </summary>
        None = 0,

        /// <summary>
        /// A file that is represented as a <see cref="StorageFile"/> instance.
        /// </summary>
        File = 1,

        /// <summary>
        /// A folder that is represented as a <see cref="StorageFolder"/> instance.
        /// </summary>
        Folder = 2,
    }

    /// <summary>
    /// Represents a file.
    /// Provides information about the file and its contents, and ways to manipulate them.
    /// </summary>
    public interface IStorageFile : IStorageItem
    {
        /// <summary>
        /// Creates a copy of the file in the specified folder.
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <returns></returns>
        IAsyncOperation<StorageFile> CopyAsync(IStorageFolder destinationFolder);

        /// <summary>
        /// Creates a copy of the file in the specified folder, using the desired name.
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <param name="desiredNewName"></param>
        /// <returns></returns>
        IAsyncOperation<StorageFile> CopyAsync(IStorageFolder destinationFolder, string desiredNewName);

        /// <summary>
        /// Moves the current file to the location of the specified file and replaces the specified file in that location.
        /// </summary>
        /// <param name="fileToReplace"></param>
        /// <returns></returns>
        IAsyncAction MoveAndReplaceAsync(IStorageFile fileToReplace);

        /// <summary>
        /// Moves the current file to the specified folder.
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <returns></returns>
        IAsyncAction MoveAsync(IStorageFolder destinationFolder);

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according to the desired name.
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <param name="desiredNewName"></param>
        /// <returns></returns>
        IAsyncAction MoveAsync(IStorageFolder destinationFolder, string desiredNewName);
        
        /// <summary>
        /// Gets the MIME type of the contents of the file.
        /// </summary>
        /// <value>The MIME type of the file contents.
        /// For example, a music file might have the "audio/mpeg" MIME type.</value>
        string ContentType { get; }

        /// <summary>
        /// Gets the type (file name extension) of the file.
        /// </summary>
        string FileType { get; }
    }
    /// <summary>
    /// Represents a file.
    /// Provides information about the file and its content, and ways to manipulate them.
    /// </summary>
    public sealed class StorageFile : IStorageFile, IStorageItem
    {
        /// <summary>
        /// Gets a StorageFile object to represent the file at the specified path.
        /// </summary>
        /// <param name="path">The path of the file to get a StorageFile to represent.
        /// If your path uses slashes, make sure you use backslashes(\).
        /// Forward slashes(/) are not accepted by this method.</param>
        /// <returns>When this method completes, it returns the file as a StorageFile.</returns>
        public static IAsyncOperation<StorageFile> GetFileFromPathAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
#if __ANDROID__ || __IOS__
            return Task.FromResult<StorageFile>(new StorageFile(path)).AsAsyncOperation<StorageFile>();
#else
            return null;
#endif
        }
        
        private string _path;
    
        internal StorageFile(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Creates a copy of the file in the specified folder.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the copy of the file is created.</param>
        /// <returns></returns>
        public IAsyncOperation<StorageFile> CopyAsync(IStorageFolder destinationFolder)
        {
#if __ANDROID__ || __IOS__
            return CopyAsync(destinationFolder, global::System.IO.Path.GetFileName(Path));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Creates a copy of the file in the specified folder and renames the copy.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the copy of the file is created.</param>
        /// <param name="desiredNewName">The new name for the copy of the file created in the destinationFolder.</param>
        /// <returns></returns>
        public IAsyncOperation<StorageFile> CopyAsync(IStorageFolder destinationFolder, string desiredNewName)
        {
#if __ANDROID__ || __IOS__
            return Task.Run<StorageFile>(() =>
            {
                string newPath = global::System.IO.Path.Combine(destinationFolder.Path, desiredNewName);
                global::System.IO.File.Copy(Path, newPath);
                return new Storage.StorageFile(newPath);
            }).AsAsyncOperation<StorageFile>();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Deletes the current file.
        /// </summary>
        /// <returns></returns>
        public IAsyncAction DeleteAsync()
        {
#if __ANDROID__ || __IOS__
            return Task.Run(() =>
            {
                global::System.IO.File.Delete(Path);
            }).AsAsyncAction();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the parent folder of the current file.
        /// </summary>
        /// <returns></returns>
        public IAsyncOperation<StorageFolder> GetParentAsync()
        {
#if __ANDROID__ || __IOS__
            return Task.Run<StorageFolder>(() =>
            {
                var parent = global::System.IO.Directory.GetParent(Path);
                return parent == null ? null : new StorageFolder(parent.FullName);
            }).AsAsyncOperation<StorageFolder>();
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
            var parentPath = global::System.IO.Path.GetPathRoot(Path.TrimEnd('\\'));
            Windows.Storage.StorageFolder parent = null;
            if (!string.IsNullOrEmpty(parentPath))
            {
                parent = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(parentPath);
            }
            return parent == null ? null : new StorageFolder(parent);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the file is moved.</param>
        /// <returns></returns>
        public IAsyncAction MoveAsync(IStorageFolder destinationFolder)
        {
#if __ANDROID__ || __IOS__
            return MoveAsync(destinationFolder, global::System.IO.Path.GetFileName(Path));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the file is moved.</param>
        /// <param name="desiredNewName">The desired name of the file after it is moved.</param>
        /// <returns></returns>
        public IAsyncAction MoveAsync(IStorageFolder destinationFolder, string desiredNewName)
        {
#if __ANDROID__ || __IOS__
            return Task.Run(() =>
            {
                string newPath = global::System.IO.Path.Combine(destinationFolder.Path, desiredNewName);
                global::System.IO.File.Move(Path, newPath);
            }).AsAsyncAction();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileToReplace"></param>
        /// <returns></returns>
        public IAsyncAction MoveAndReplaceAsync(IStorageFile fileToReplace)
        {
            if(fileToReplace == null)
            {
                throw new ArgumentNullException("fileToReplace");
            }
#if __ANDROID__ || __IOS__
            return Task.Run(async () =>
            {
                string fileName = fileToReplace.Name;
                string folder = global::System.IO.Path.GetDirectoryName(fileToReplace.Path);
                await fileToReplace.DeleteAsync();
                await this.MoveAsync(await StorageFolder.GetFolderFromPathAsync(folder), fileName);
            }).AsAsyncAction();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the attributes of a file.
        /// </summary>
        [CLSCompliant(false)]
        public FileAttributes Attributes
        {
            get
            {
#if __ANDROID__ || __IOS__
                return FileAttributesHelper.FromIOFileAttributes(global::System.IO.File.GetAttributes(Path));
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the date and time when the current file was created. 
        /// </summary>
        public DateTimeOffset DateCreated
        {
            get
            {
#if __ANDROID__ || __IOS__
                var utc = global::System.IO.File.GetCreationTimeUtc(Path);
                var local = global::System.IO.File.GetCreationTime(Path);
                var offset = local - utc;
                return new DateTimeOffset(local, offset);
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        public string ContentType
        {
            get
            {
#if __IOS__
                string mime = string.Empty;

                string utref = MobileCoreServices.UTType.CreatePreferredIdentifier(MobileCoreServices.UTType.TagClassFilenameExtension, FileType.Substring(1).ToLower(), "");
                if (!string.IsNullOrEmpty(utref))
                {
                    mime = MobileCoreServices.UTType.GetPreferredTag(utref, MobileCoreServices.UTType.TagClassMIMEType);
                }
                return mime;
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the type (file name extension) of the file.
        /// </summary>
        public string FileType
        {
            get
            {
#if __ANDROID__ || __IOS__
                return global::System.IO.Path.GetExtension(Path);
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the name of the file including the file name extension.
        /// </summary>
        public string Name
        {
            get
            {
#if __ANDROID__ || __IOS__
                return global::System.IO.Path.GetFileName(Path);
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the full file-system path of the current file, if the file has a path.
        /// </summary>
        public string Path
        {
            get
            {
#if __ANDROID__ || __IOS__
                return _path;
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Determines whether the current StorageFile matches the specified <see cref="StorageItemTypes"/> value.
        /// </summary>
        /// <param name="type">The value to match against.</param>
        /// <returns>True if the StorageFile matches the specified value; otherwise false.</returns>
        public bool IsOfType(StorageItemTypes type)
        {
            return type == StorageItemTypes.File;
        }
    }
}
#endif