//-----------------------------------------------------------------------
// <copyright file="StorageFile.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Storage.StorageFile))]
#else

using System;
using System.IO;

using System.Threading.Tasks;
using Windows.Foundation;

namespace Windows.Storage
{
    /// <summary>
    /// Represents a file.
    /// Provides information about the file and its content, and ways to manipulate them.
    /// </summary>
    public sealed class StorageFile : IStorageFile, IStorageItem, IStorageItem2
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
        /// Replaces the specified file with a copy of the current file.
        /// </summary>
        /// <param name="fileToReplace"></param>
        /// <returns></returns>
        public IAsyncAction CopyAndReplaceAsync(IStorageFile fileToReplace)
        {
#if __ANDROID__ || __IOS__
            return Task.Run(() =>
            {
                File.Replace(this.Path, fileToReplace.Path, null);
            }).AsAsyncAction();
#else
            throw new PlatformNotSupportedException();
#endif
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
            return DeleteAsync(StorageDeleteOption.Default);
        }

        /// <summary>
        /// Deletes the current file, optionally deleting the item permanently.
        /// </summary>
        /// <returns></returns>
        public IAsyncAction DeleteAsync(StorageDeleteOption option)
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
                this._path = newPath;
            }).AsAsyncAction();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Moves the current file to the location of the specified file and replaces the specified file in that location.
        /// </summary>
        /// <param name="fileToReplace">The file to replace.</param>
        /// <returns>No object or value is returned by this method.</returns>
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
        /// Renames the current file.
        /// </summary>
        /// <param name="desiredName">The desired, new name of the current item.</param>
        /// <returns>No object or value is returned by this method when it completes.</returns>
        public IAsyncAction RenameAsync(string desiredName)
        {
            return RenameAsync(desiredName, NameCollisionOption.FailIfExists);
        }

        /// <summary>
        /// Renames the current file.
        /// This method also specifies what to do if an existing item in the current file's location has the same name.
        /// </summary>
        /// <param name="desiredName">The desired, new name of the current file.
        /// <para>If there is an existing item in the current file's location that already has the specified desiredName, the specified <see cref="NameCollisionOption"/>  determines how the system responds to the conflict.</para></param>
        /// <param name="option">The enum value that determines how the system responds if the desiredName is the same as the name of an existing item in the current file's location.</param>
        /// <returns>No object or value is returned by this method when it completes.</returns>
        public IAsyncAction RenameAsync(string desiredName, NameCollisionOption option)
        {
            if(string.IsNullOrEmpty(desiredName))
            {
                throw new ArgumentNullException("desiredName");
            }

#if __ANDROID__ || __IOS__
            return Task.Run(() =>
            {
                string folder = global::System.IO.Path.GetDirectoryName(this.Path);
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
        /// Indicates whether the current file is equal to the specified file.
        /// </summary>
        /// <param name="item">The <see cref="IStorageItem"/>  object that represents a file to compare against.</param>
        /// <returns>Returns true if the current file is equal to the specified file; otherwise false.</returns>
        public bool IsEqual(IStorageItem item)
        {
            return this.Path == item.Path;
        }

        /// <summary>
        /// Determines whether the current <see cref="StorageFile"/> matches the specified <see cref="StorageItemTypes"/> value.
        /// </summary>
        /// <param name="type">The value to match against.</param>
        /// <returns>True if the <see cref="StorageFile"/> matches the specified value; otherwise false.</returns>
        public bool IsOfType(StorageItemTypes type)
        {
            return type == StorageItemTypes.File;
        }
    }
}
#endif