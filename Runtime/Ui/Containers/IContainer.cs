﻿using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Интерфейс для всех контейнеров пользовательского интерфейса.
    /// </summary>
    public interface IContainer 
    {
        /// <summary>
        /// Создает экземпляр объекта, помещает его в контейнер и возвращает на него ссылку.
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        TObject PutPrefab<TObject>(TObject prefab) where TObject : Component;

        /// <summary>
        /// Помещает уже созданный экземпляр в контейнер и возвращает на него ссылку.
        /// </summary>
        TObject Put<TObject>(TObject instance) where TObject : Component;

        void Clear();

        void DestroyLast();
    }
}