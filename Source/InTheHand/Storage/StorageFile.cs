//-----------------------------------------------------------------------
// <copyright file="StorageFile.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;

using System.Threading.Tasks;
using InTheHand.Storage.FileProperties;
#if __ANDROID__
using Android.Webkit;
#endif

namespace InTheHand.Storage
{
    /// <summary>
    /// Represents a file.
    /// Provides information about the file and its content, and ways to manipulate them.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed class StorageFile : IStorageFile, IStorageItem
    {
        /// <summary>
        /// Gets a StorageFile object to represent the file at the specified path.
        /// </summary>
        /// <param name="path">The path of the file to get a StorageFile to represent.
        /// If your path uses slashes, make sure you use backslashes(\).
        /// Forward slashes(/) are not accepted by this method.</param>
        /// <returns>When this method completes, it returns the file as a StorageFile.</returns>
        public static Task<StorageFile> GetFileFromPathAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFile>(async () =>
            {
                var f = await Windows.Storage.StorageFile.GetFileFromPathAsync(path);

                return f == null ? null : new StorageFile(f);
            });
#else
            return Task.FromResult<StorageFile>(new StorageFile(path));
#endif
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Storage.StorageFile _file;

        private StorageFile(Windows.Storage.StorageFile file)
        {
            _file = file;
        }

        public static implicit operator StorageFile(Windows.Storage.StorageFile f)
        {
            return new StorageFile(f);
        }

        public static implicit operator Windows.Storage.StorageFile(StorageFile f)
        {
            return f._file;
        }
#else
        private string _path;
    
        internal StorageFile(string path)
        {
            _path = path;
        }
#endif

        /// <summary>
        /// Replaces the specified file with a copy of the current file.
        /// </summary>
        /// <param name="fileToReplace"></param>
        /// <returns></returns>
        public Task CopyAndReplaceAsync(IStorageFile fileToReplace)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _file.CopyAndReplaceAsync((Windows.Storage.StorageFile)((StorageFile)fileToReplace)).AsTask();
#elif __ANDROID__ || __UNIFIED__ || WIN32
            return Task.Run(() =>
            {
                File.Replace(Path, fileToReplace.Path, null);
            });
#elif TIZEN
            return Task.Run(() =>
            {
                File.Delete(fileToReplace.Path);
                File.Copy(Path, fileToReplace.Path);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }
        /// <summary>
        /// Creates a copy of the file in the specified folder.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the copy of the file is created.</param>
        /// <returns></returns>
        public Task<StorageFile> CopyAsync(IStorageFolder destinationFolder)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFile>(async () =>
            {
                var f = await _file.CopyAsync((Windows.Storage.StorageFolder)((StorageFolder)destinationFolder));

                return f == null ? null : new StorageFile(f);
            });
#else
            return CopyAsync(destinationFolder, global::System.IO.Path.GetFileName(Path));
#endif
        }

        /// <summary>
        /// Creates a copy of the file in the specified folder and renames the copy.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the copy of the file is created.</param>
        /// <param name="desiredNewName">The new name for the copy of the file created in the destinationFolder.</param>
        /// <returns></returns>
        public Task<StorageFile> CopyAsync(IStorageFolder destinationFolder, string desiredNewName)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFile>(async () =>
            {
                var f = await _file.CopyAsync((Windows.Storage.StorageFolder)((StorageFolder)destinationFolder), desiredNewName);

                return f == null ? null : new StorageFile(f);
            });
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run<StorageFile>(() =>
            {
                string newPath = global::System.IO.Path.Combine(destinationFolder.Path, desiredNewName);
                global::System.IO.File.Copy(Path, newPath);
                return new Storage.StorageFile(newPath);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Deletes the current file.
        /// </summary>
        /// <returns></returns>
        public Task DeleteAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _file.DeleteAsync().AsTask();
#else
            return DeleteAsync(StorageDeleteOption.Default);
#endif
        }

        /// <summary>
        /// Deletes the current file, optionally deleting the item permanently.
        /// </summary>
        /// <returns></returns>
        public Task DeleteAsync(StorageDeleteOption option)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _file.DeleteAsync((Windows.Storage.StorageDeleteOption)((int)option)).AsTask();
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run(() =>
            {
                global::System.IO.File.Delete(Path);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the basic properties of the current file.
        /// </summary>
        /// <returns></returns>
        public Task<BasicProperties> GetBasicPropertiesAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<BasicProperties>(async () =>
            {
                return await _file.GetBasicPropertiesAsync();
            });
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.FromResult<BasicProperties>(new BasicProperties(this));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public Task<StorageItemThumbnail> GetThumbnailAsync(ThumbnailMode mode)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageItemThumbnail>(async () =>
            {
                return await _file.GetThumbnailAsync((Windows.Storage.FileProperties.ThumbnailMode)mode);
            });

#else
            if (ContentType.StartsWith("video"))
            {
                return StorageItemThumbnail.CreateVideoThumbnailAsync(this);
            }

            else if(ContentType.StartsWith("image"))
            {
                return StorageItemThumbnail.CreatePhotoThumbnailAsync(this);
            }

            return Task.FromResult<StorageItemThumbnail>(null);
#endif
        }

        /// <summary>
        /// Gets the parent folder of the current file.
        /// </summary>
        /// <returns></returns>
        public Task<StorageFolder> GetParentAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP
            return Task.Run<StorageFolder>(async () =>
            {
                var parent = await _file.GetParentAsync();
                return parent == null ? null : parent;
            });
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run<StorageFolder>(() =>
            {
                var parent = global::System.IO.Directory.GetParent(Path);
                return parent == null ? null : new StorageFolder(parent.FullName);
            });
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFolder>(async () =>
            {
                var parentPath = global::System.IO.Path.GetPathRoot(Path.TrimEnd('\\'));
                Windows.Storage.StorageFolder parent = null;
                if (!string.IsNullOrEmpty(parentPath))
                {
                    parent = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(parentPath);
                }
                return parent == null ? null : parent;
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the file is moved.</param>
        /// <returns></returns>
        public Task MoveAsync(IStorageFolder destinationFolder)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _file.MoveAsync((Windows.Storage.StorageFolder)((StorageFolder)destinationFolder)).AsTask();
#else
            return MoveAsync(destinationFolder, global::System.IO.Path.GetFileName(Path));
#endif
        }

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the file is moved.</param>
        /// <param name="desiredNewName">The desired name of the file after it is moved.</param>
        /// <returns></returns>
        public Task MoveAsync(IStorageFolder destinationFolder, string desiredNewName)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _file.MoveAsync((Windows.Storage.StorageFolder)((StorageFolder)destinationFolder), desiredNewName).AsTask();
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run(() =>
            {
                string newPath = global::System.IO.Path.Combine(destinationFolder.Path, desiredNewName);
                global::System.IO.File.Move(Path, newPath);
                _path = newPath;
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Moves the current file to the location of the specified file and replaces the specified file in that location.
        /// </summary>
        /// <param name="fileToReplace">The file to replace.</param>
        /// <returns>No object or value is returned by this method.</returns>
        public Task MoveAndReplaceAsync(IStorageFile fileToReplace)
        {
            if(fileToReplace == null)
            {
                throw new ArgumentNullException("fileToReplace");
            }
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _file.MoveAndReplaceAsync((Windows.Storage.StorageFile)((StorageFile)fileToReplace)).AsTask();
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run(async () =>
            {
                string fileName = fileToReplace.Name;
                string folder = global::System.IO.Path.GetDirectoryName(fileToReplace.Path);
                await fileToReplace.DeleteAsync();
                await MoveAsync(await StorageFolder.GetFolderFromPathAsync(folder), fileName);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Renames the current file.
        /// </summary>
        /// <param name="desiredName">The desired, new name of the current item.</param>
        /// <returns>No object or value is returned by this method when it completes.</returns>
        public Task RenameAsync(string desiredName)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _file.RenameAsync(desiredName).AsTask();
#else
            return RenameAsync(desiredName, NameCollisionOption.FailIfExists);
#endif
        }

        /// <summary>
        /// Renames the current file.
        /// This method also specifies what to do if an existing item in the current file's location has the same name.
        /// </summary>
        /// <param name="desiredName">The desired, new name of the current file.
        /// <para>If there is an existing item in the current file's location that already has the specified desiredName, the specified <see cref="NameCollisionOption"/>  determines how the system responds to the conflict.</para></param>
        /// <param name="option">The enum value that determines how the system responds if the desiredName is the same as the name of an existing item in the current file's location.</param>
        /// <returns>No object or value is returned by this method when it completes.</returns>
        public Task RenameAsync(string desiredName, NameCollisionOption option)
        {
            if(string.IsNullOrEmpty(desiredName))
            {
                throw new ArgumentNullException("desiredName");
            }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _file.RenameAsync(desiredName, (Windows.Storage.NameCollisionOption)((int)option)).AsTask();
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run(() =>
            {
                string folder = global::System.IO.Path.GetDirectoryName(Path);
                string newPath = global::System.IO.Path.Combine(folder, desiredName);
                switch(option)
                {

                    case NameCollisionOption.GenerateUniqueName:
                        string generatedPath = newPath;
                        int num = 2;
                        while(File.Exists(generatedPath))
                        {
                            generatedPath = global::System.IO.Path.Combine(folder, global::System.IO.Path.GetFileNameWithoutExtension(desiredName), string.Format("({0})", num), global::System.IO.Path.GetExtension(desiredName));
                            num++;
                        }
                        newPath = generatedPath;
                        break;

                    case NameCollisionOption.ReplaceExisting:

                        if(File.Exists(newPath))
                        {
                            File.Delete(newPath);
                        }
                        break;

                    default:
                        break;
                }

                File.Move(Path, global::System.IO.Path.Combine(global::System.IO.Path.GetDirectoryName(Path), desiredName));
                _path = newPath;
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the attributes of a file.
        /// </summary>
        public FileAttributes Attributes
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return (FileAttributes)((int)_file.Attributes);
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _file.DateCreated;
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
                var utc = global::System.IO.File.GetCreationTimeUtc(Path);
                var local = global::System.IO.File.GetCreationTime(Path);
                var offset = local - utc;
                return new DateTimeOffset(local, offset);
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the MIME type of the contents of the file.
        /// </summary>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
        /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// </list>
        /// </remarks>
        public string ContentType
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _file.ContentType;
#elif __UNIFIED__
                string mime = string.Empty;

                string utref = MobileCoreServices.UTType.CreatePreferredIdentifier(MobileCoreServices.UTType.TagClassFilenameExtension, FileType.Substring(1).ToLower(), null);
                if (!string.IsNullOrEmpty(utref))
                {
                    mime = MobileCoreServices.UTType.GetPreferredTag(utref, MobileCoreServices.UTType.TagClassMIMEType);
                }

                return mime;

#elif __ANDROID__
                if(Path.StartsWith("content:"))
                {
                    return Android.App.Application.Context.ContentResolver.GetType(Android.Net.Uri.Parse(Path));
                }
                else
                {
                    return MimeTypeMap.Singleton.GetMimeTypeFromExtension(FileType.Substring(1).ToLower());
                }

#elif TIZEN
                return Tizen.Content.MimeType.MimeUtil.GetMimeType(FileType);

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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _file.FileType;

#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _file.Name;

#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _file.Path;
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
                return _path;
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Indicates whether the current file is equal to the specified file.
        /// </summary>
        /// <param name="item">The <see cref="IStorageItem"/>  object that represents a file to compare against.</param>
        /// <returns>Returns true if the current file is equal to the specified file; otherwise false.</returns>
        public bool IsEqual(IStorageItem item)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            if (item.IsOfType(StorageItemTypes.File))
            {
                return _file == ((StorageFile)item)._file;
            }

            return false;
#else
            return Path == item.Path;
#endif
        }

        /// <summary>
        /// Determines whether the current <see cref="StorageFile"/> matches the specified <see cref="StorageItemTypes"/> value.
        /// </summary>
        /// <param name="type">The value to match against.</param>
        /// <returns>True if the <see cref="StorageFile"/> matches the specified value; otherwise false.</returns>
        /// <seealso cref="StorageItemTypes"/>
        public bool IsOfType(StorageItemTypes type)
        {
            return type == StorageItemTypes.File;
        }
    }
}
//#endif