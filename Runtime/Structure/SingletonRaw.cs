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

        protected virtual bool CanBeDestroyedOutside => true;

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


        public static bool DestroyIfExist()
        {
            if (_instance != null)
            {
                if (!_instance.CanBeDestroyedOutside)
                    return false;
                
                _instance.Dispose();
                _instance = null;

                return true;
            }

            return true;
        }

        public static TSingleton CreateInstance()
        {
            if (!DestroyIfExist())
                return _instance;
            
            CreateInstanceInternal();

            return _instance;
        }
        
        protected static void CreateInstanceInternal()
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