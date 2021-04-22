﻿using Egsp.Core;

namespace Egsp.Files.Serializers
{
    // Сериализаторы не поддерживают форматы друг друга. 
    // TODO: при сериализации и десериализации нужно в файле помечать от какого сериализатора данные.
    // Например на первой строчке может быть тип сриализатора.
    // Это не необходимо, но упростит использование разных сериализаторов в одном проекте.
    
    /// <summary>
    /// Интерфейс для сериализаторов.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Сериализует объект.
        /// </summary>
        byte[] Serialize<T>(T obj);

        /// <summary>
        /// Десериализует объект.
        /// При неудаче возвращается default(T).
        /// </summary>
        Option<T> Deserialize<T>(byte[] serializedData);
    }
}