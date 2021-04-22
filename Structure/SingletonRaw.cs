using System;
using JetBrains.Annotations;

namespace Egsp.Core
{
    /// <summary>
    /// Базовый класс для всех объектов-одиночек. Существует вне рамок Unity.
    /// </summary>
    public abstract class SingletonRaw<TSingleton> : IDisposable
        where TSingleton : SingletonRaw<TSingleton>, new()
    {
        [NotNull]
        public static TSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (!AllowLazyInstance)
                        throw new LazyInstanceException(typeof(TSingleton));

                    CreateInstanceSafely();
                }

                return _instance ?? throw new NullReferenceException();
            }
            protected set => _instance = value;
        }

        /// <summary>
        /// Разрешена ли инициализация при обращении к экземпляру.
        /// </summary>
        protected static bool AllowLazyInstance
        {
            get
            {
                var lazyAttribute =
                    (LazyInstanceAttribute) Attribute.GetCustomAttribute(typeof(TSingleton),
                        typeof(LazyInstanceAttribute));

                if (lazyAttribute == null)
                    return true;

                return lazyAttribute.AllowLazyInstance;
            }
        }
        
        private static TSingleton _instance;

        public static void DestroyIfExist()
        {
            if (_instance != null)
            {
                _instance.Dispose();
                _instance = null;
            }
        }

        public static TSingleton CreateInstance()
        {
            DestroyIfExist();
            
            CreateInstanceSafely();

            return _instance;
        }
        
        private static void CreateInstanceSafely()
        {
            _instance = new TSingleton();
        }
        
        
        public virtual void Dispose()
        {
        }
    }
}