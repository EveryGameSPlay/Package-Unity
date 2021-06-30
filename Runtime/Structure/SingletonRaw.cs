using System;
using JetBrains.Annotations;

namespace Egsp.Core
{
    /// <summary>
    /// Базовый класс для всех объектов-одиночек. Существует вне рамок Unity.
    /// </summary>
    public abstract class SingletonRaw<TSingleton> where TSingleton : SingletonRaw<TSingleton>, new()
    {
        private static TSingleton _instance;
        
        public static WeakEvent<TSingleton> OnInstanceCreated = new WeakEvent<TSingleton>();
        
        /// <summary>
        /// Существует ли экземпляр.
        /// </summary>
        public static bool Exist => _instance != null;
        
        [NotNull]
        public static TSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (!AllowLazyInstance)
                        throw new LazyInstanceException(typeof(TSingleton));

                    CreateInstance();
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
            _instance.OnInstanceCreatedInternal();
        }
        
        protected virtual void OnInstanceCreatedInternal()
        {
            OnInstanceCreated.Raise(_instance);
        }
        
        
        protected virtual void Dispose()
        {
        }
    }
}