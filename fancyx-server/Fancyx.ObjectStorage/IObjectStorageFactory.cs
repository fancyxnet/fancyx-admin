using Fancyx.Core.Interfaces;

namespace Fancyx.Storage
{
    public interface IObjectStorageFactory : ISingletonDependency
    {
        IObjectStorageService GetService(StorageType storageType, StorageOptions? options = null);
    }
}