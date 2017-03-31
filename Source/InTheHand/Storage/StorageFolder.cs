//-----------------------------------------------------------------------
// <copyright file="StorageFolder.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using System.Threading.Tasks;
using InTheHand.Storage.FileProperties;
#if WINDOWS_PHONE
using System.IO.IsolatedStorage;
#endif

namespace InTheHand.Storage
{
    /// <summary>
    /// Manages folders and their contents and provides information about them.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public sealed class StorageFolder : IStorageFolder, IStorageFolder2, IStorageItem, IStorageItem2
    {
        /// <summary>
        /// Gets a StorageFile object to represent the file at the specified path.
        /// </summary>
        /// <param name="path">The path of the file to get a StorageFile to represent.
        /// If your path uses slashes, make sure you use backslashes(\).
        /// Forward slashes(/) are not accepted by this method.</param>
        /// <returns>When this method completes, it returns the file as a StorageFile.</returns>
        public static Task<StorageFolder> GetFolderFromPathAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFolder>(async () =>
            {
                var f = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(path);

                return f == null ? null : new StorageFolder(f);
            });
#else
            return Task.FromResult<StorageFolder>(new StorageFolder(path));
#endif
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Storage.StorageFolder _folder;

        private StorageFolder(Windows.Storage.StorageFolder folder)
        {
            _folder = folder;
        }

        public static implicit operator StorageFolder(Windows.Storage.StorageFolder f)
        {
            return new Storage.StorageFolder(f);
        }

        public static implicit operator Windows.Storage.StorageFolder(StorageFolder f)
        {
            return f._folder;
        }
#else
        private string _path;

        internal StorageFolder(string path)
        {
            _path = path;
        }
#endif

        /// <summary>
        /// Creates a new file with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFile that represents the new file.</returns>
        public Task<StorageFile> CreateFileAsync(string desiredName)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFile>(async () =>
            {
                var f = await _folder.CreateFileAsync(desiredName);
                return f == null ? null : f;
            });

#else
            return CreateFileAsync(desiredName, CreationCollisionOption.FailIfExists);
#endif
        }

        /// <summary>
        /// Creates a new file with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <param name="options">One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFile that represents the new file.</returns>
        public Task<StorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFile>(async () =>
            {
                var f = await _folder.CreateFileAsync(desiredName, (Windows.Storage.CreationCollisionOption)((int)options));
                return f == null ? null : f;
            });

#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run<StorageFile>(() =>
            {
                string filepath = global::System.IO.Path.Combine(Path, desiredName);

                if (global::System.IO.File.Exists(filepath))
                {
                    switch (options)
                    {
                        case CreationCollisionOption.OpenIfExists:
                            return new Storage.StorageFile(filepath);

                        case CreationCollisionOption.ReplaceExisting:
                            File.Delete(filepath);
                            break;

                        case CreationCollisionOption.GenerateUniqueName:
                            for (int i = 1; i < 100; i++)
                            {
                                string newPath = string.Format(filepath.Substring(0, filepath.LastIndexOf('.')) + " ({0})" + filepath.Substring(filepath.LastIndexOf('.')), i);
                                if (!File.Exists(newPath))
                                {
                                    filepath = newPath;
                                    break;
                                }
                            }
                            break;

                        default:
                            throw new IOException();
                    }
                }

#if TIZEN
                File.Create(filepath).Dispose();
#else
                File.Create(filepath).Close();
#endif

                return new StorageFile(filepath);

            });
#else
            throw new PlatformNotSupportedException();
#endif
            }

        /// <summary>
        /// Creates a new subfolder with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new subfolder to create in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFolder that represents the new subfolder.</returns>
        public Task<StorageFolder> CreateFolderAsync(string desiredName)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFolder>(async () =>
            {
                var f = await _folder.CreateFolderAsync(desiredName);
                return f == null ? null : new StorageFolder(f);
            });
#else
            return CreateFolderAsync(desiredName);
#endif
        }

        /// <summary>
        /// Creates a new subfolder with the specified name in the current folder.
        /// This method also specifies what to do if a subfolder with the same name already exists in the current folder. 
        /// </summary>
        /// <param name="desiredName">The name of the new subfolder to create in the current folder.</param>
        /// <param name="options">One of the enumeration values that determines how to handle the collision if a subfolder with the specified desiredName already exists in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFolder that represents the new subfolder.</returns>
        public Task<StorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFolder>(async () =>
            {
                var f = await _folder.CreateFolderAsync(desiredName, (Windows.Storage.CreationCollisionOption)((int)options));
                return f == null ? null : new StorageFolder(f);
            });
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run(() =>
            {
                string newpath = global::System.IO.Path.Combine(Path, desiredName);

                if (global::System.IO.Directory.Exists(newpath))
                {
                    switch (options)
                    {
                        case CreationCollisionOption.OpenIfExists:
                            return new Storage.StorageFolder(newpath);

                        case CreationCollisionOption.ReplaceExisting:
                            Directory.Delete(newpath);
                            break;

                        case CreationCollisionOption.GenerateUniqueName:
                            for (int i = 1; i < 100; i++)
                            {
                                string uniquePath = string.Format(newpath.Substring(0, newpath.LastIndexOf('.')) + " ({0})" + newpath.Substring(newpath.LastIndexOf('.')), i);
                                if (!File.Exists(uniquePath))
                                {
                                    newpath = uniquePath;
                                    break;
                                }
                            }
                            break;

                        default:
                            throw new IOException();
                    }
                }

                Directory.CreateDirectory(newpath);

                return new Storage.StorageFolder(newpath);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Deletes the current folder.
        /// </summary>
        /// <returns></returns>
        public Task DeleteAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _folder.DeleteAsync().AsTask();
#else
            return DeleteAsync(StorageDeleteOption.Default);
#endif
        }

        /// <summary>
        /// Deletes the current folder.
        /// This method also specifies whether to delete the folder permanently.
        /// </summary>
        /// <returns></returns>
        public Task DeleteAsync(StorageDeleteOption option)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _folder.DeleteAsync((Windows.Storage.StorageDeleteOption)((int)option)).AsTask();
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            if (!Directory.Exists(Path))
            {
                throw new FileNotFoundException();
            }
            return Task.Run(() =>
            {
                global::System.IO.Directory.Delete(Path, true);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the basic properties of the current folder.
        /// </summary>
        /// <returns></returns>
        public Task<BasicProperties> GetBasicPropertiesAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<BasicProperties>(async () =>
            {
                return await _folder.GetBasicPropertiesAsync();
            });

#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.FromResult<BasicProperties>(new BasicProperties(this));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the file with the specified name from the current folder. 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Task<StorageFile> GetFileAsync(string filename)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFile>(async () =>
            {
                var f = await _folder.GetFileAsync(filename);
                return f == null ? null : f;
            });
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run<StorageFile>(() =>
            {
                string filepath = global::System.IO.Path.Combine(Path, filename);

                if (!global::System.IO.File.Exists(filepath))
                {
                    throw new FileNotFoundException();
                }

                return new StorageFile(filepath);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <returns></returns>
        public Task<IReadOnlyList<StorageFile>> GetFilesAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<IReadOnlyList<StorageFile>>(async () =>
            {
                List<StorageFile> files = new List<StorageFile>();
                var found = await _folder.GetFilesAsync();
                foreach(var file in found)
                {
                    files.Add(file);
                }

                return files.AsReadOnly();
            });

#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run<IReadOnlyList<StorageFile>>(() =>
            {
                List<StorageFile> files = new List<StorageFile>();

                foreach (string filename in global::System.IO.Directory.GetFiles(Path))
                {
                    files.Add(new StorageFile(global::System.IO.Path.Combine(Path, filename)));
                }

                return files;
            });

#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the specified folder from the current folder. 
        /// </summary>
        /// <param name="name">The name of the child folder to retrieve.</param>
        /// <returns>When this method completes successfully, it returns a StorageFolder that represents the child folder.</returns>
        public Task<StorageFolder> GetFolderAsync(string name)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFolder>(async () =>
            {
                var f = await _folder.GetFolderAsync(name);
                return f == null ? null : new StorageFolder(f);
            });
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run<StorageFolder>(() =>
            {
                string folderpath = global::System.IO.Path.Combine(Path, name);

                if (!global::System.IO.Directory.Exists(folderpath))
                {
                    throw new FileNotFoundException();
                }

                return new StorageFolder(folderpath);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the folders in the current folder.
        /// </summary>
        /// <returns></returns>
        public Task<IReadOnlyList<StorageFolder>> GetFoldersAsync()
        {
            List<StorageFolder> folders = new List<StorageFolder>();
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<IReadOnlyList<StorageFolder>>(async () =>
            {
                var found = await _folder.GetFoldersAsync();
                foreach (var folder in found)
                {
                    folders.Add(new StorageFolder(folder));
                }

                return folders.AsReadOnly();
            });
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run<IReadOnlyList<StorageFolder>>(() =>
            {
                foreach (string foldername in global::System.IO.Directory.GetDirectories(Path))
                {
                    folders.Add(new StorageFolder(global::System.IO.Path.Combine(Path, foldername)));
                }

                return folders.AsReadOnly();
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the items in the current folder.
        /// </summary>
        /// <returns></returns>
        public Task<IReadOnlyList<IStorageItem>> GetItemsAsync()
        {
            List<IStorageItem> items = new List<IStorageItem>();
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<IReadOnlyList<IStorageItem>>(async () =>
            {
                var found = await _folder.GetItemsAsync();
                foreach (var item in found)
                {
                    if (item.IsOfType(Windows.Storage.StorageItemTypes.File))
                    {
                        items.Add((StorageFile)((Windows.Storage.StorageFile)item));
                    }
                    else
                    {
                        items.Add((StorageFolder)((Windows.Storage.StorageFolder)item));
                    }
                }

                return items.AsReadOnly();
            });
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run<IReadOnlyList<IStorageItem>>(() =>
            {
                foreach (string foldername in global::System.IO.Directory.GetDirectories(Path))
                {
                    items.Add(new StorageFolder(global::System.IO.Path.Combine(Path, foldername)));
                }

                foreach (string filename in global::System.IO.Directory.GetFiles(Path))
                {
                    items.Add(new StorageFolder(global::System.IO.Path.Combine(Path, filename)));
                }

                return items.AsReadOnly();
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the parent folder of the current folder.
        /// </summary>
        /// <returns>When this method completes, it returns the parent folder as a StorageFolder.</returns>
        public Task<StorageFolder> GetParentAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP
            return Task.Run<StorageFolder>(async () =>
            {
                var f = await _folder.GetParentAsync();
                return f == null ? null : new StorageFolder(f);
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

                return parent == null ? null : new StorageFolder(parent);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Tries to get the file or folder with the specified name from the current folder.
        /// Returns null instead of raising a FileNotFoundException if the specified file or folder is not found.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the file or folder to get.</param>
        /// <returns>When this method completes successfully, it returns an IStorageItem that represents the specified file or folder.
        /// If the specified file or folder is not found, this method returns null instead of raising an exception.</returns>
        public Task<IStorageItem> TryGetItemAsync(string name)
        {
#if WINDOWS_UWP || WINDOWS_APP
            return Task.Run<IStorageItem>(async () =>
            {
                var i = await _folder.TryGetItemAsync(name);
                if (i == null)
                    return null;
                if(i.IsOfType(Windows.Storage.StorageItemTypes.File))
                {
                    return (StorageFile)((Windows.Storage.StorageFile)i);
                }
                else
                {
                    return (StorageFolder)((Windows.Storage.StorageFolder)i);
                }
            });

#elif WINDOWS_PHONE_APP
            return Task.Run<IStorageItem>(async () =>
            {
                if (name.Contains("."))
                {
                    // names containing a . are files so do this faster files-only approach
                    foreach (var file in await _folder.GetFilesAsync())
                    {
                        if (file.Name == name)
                        {
                            return (StorageFile)((Windows.Storage.StorageFile)file);
                        }
                    }
                }
                else
                {
                    // items with no extension could be either file or folder so check all items
                    foreach (Windows.Storage.IStorageItem item in await _folder.GetItemsAsync())
                    {
                        if (item.Name == name)
                        {
                            if (item.IsOfType(Windows.Storage.StorageItemTypes.File))
                            {
                                return (StorageFile)((Windows.Storage.StorageFile)item);
                            }
                            else
                            {
                                return (StorageFolder)((Windows.Storage.StorageFolder)item);
                            }
                        }
                    }
                }

                return null;
            });
#elif WINDOWS_PHONE
            return Task.Run<IStorageItem>(async () =>
            {
                string isoPath = _folder.GetIsoStorePath();
                if (isoPath == null)
                {
                    // On 8.1 defer to try/catch if outside of localstate/isostore
                    try
                    {
                        var i = await _folder.GetItemAsync(name);
                        if(i.IsOfType(Windows.Storage.StorageItemTypes.File))
                        {
                            return (StorageFile)((Windows.Storage.StorageFile)i);
                        }
                        else
                        {
                            return (StorageFolder)((Windows.Storage.StorageFolder)i);
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }

                if (name.Contains("."))
                {
                    // file
                    if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(global::System.IO.Path.Combine(isoPath, name)))
                    {
                        return (StorageFile)((Windows.Storage.StorageFile)await _folder.GetFileAsync(name));
                    }
                }
                else
                {
                    // folder
                    if (IsolatedStorageFile.GetUserStoreForApplication().DirectoryExists(global::System.IO.Path.Combine(isoPath, name)))
                    {
                        return (StorageFolder)((Windows.Storage.StorageFolder)await _folder.GetFolderAsync(name));
                    }
                    else if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(global::System.IO.Path.Combine(isoPath, name)))
                    {
                        // file without extension
                        return (StorageFile)((Windows.Storage.StorageFile)await _folder.GetFileAsync(name));
                    }
                }

                return null;
            });

#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
            return Task.Run<IStorageItem>(() =>
            {
                string itempath = global::System.IO.Path.Combine(Path, name);
                if (File.Exists(itempath))
                {
                    return new StorageFile(itempath);
                }
                else if (Directory.Exists(itempath))
                {
                    return new StorageFolder(itempath);
                }

                return null;
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the attributes of the current folder.
        /// </summary>
        public FileAttributes Attributes
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return (FileAttributes)((int)_folder.Attributes);
#elif __ANDROID__ || __UNIFIED__  || WIN32 || TIZEN
                return FileAttributesHelper.FromIOFileAttributes(global::System.IO.File.GetAttributes(Path));
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the date and time that the current folder was created. 
        /// </summary>
        public DateTimeOffset DateCreated
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _folder.DateCreated;
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
                var utc = global::System.IO.Directory.GetCreationTimeUtc(Path);
                var local = global::System.IO.Directory.GetCreationTime(Path);
                var offset = local - utc;
                return new DateTimeOffset(local, offset);
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the name of the current folder. 
        /// </summary>
        public string Name
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _folder.Name;
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
                return global::System.IO.Path.GetDirectoryName(Path);
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the full path of the current folder in the file system, if the path is available.
        /// </summary>
        public string Path
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _folder.Path;
#elif __ANDROID__ || __UNIFIED__ || WIN32 || TIZEN
                return _path;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Indicates whether the current folder is equal to the specified folder.
        /// </summary>
        /// <param name="item">The <see cref="IStorageItem"/> object that represents the folder to compare against.</param>
        /// <returns>Returns true if the current folder is equal to the specified folder; otherwise false.</returns>
        public bool IsEqual(IStorageItem item)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            if (item.IsOfType(StorageItemTypes.Folder))
            {
                return _folder == ((StorageFolder)item)._folder;
            }

            return false;
#else
            return Path == item.Path;
#endif
        }

        /// <summary>
        /// Indicates whether the current <see cref="StorageFolder"/> matches the specified <see cref="StorageItemTypes"/> value.
        /// </summary>
        /// <param name="type">The enum value that determines the object type to match against.</param>
        /// <returns>True if the <see cref="StorageFolder"/> matches the specified <see cref="StorageItemTypes"/> value; otherwise false.</returns>
        /// <seealso cref="StorageItemTypes"/>
        public bool IsOfType(StorageItemTypes type)
        {
            return type == StorageItemTypes.Folder;
        }
    }
}