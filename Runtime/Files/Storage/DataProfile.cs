using System;

namespace Egsp.Core
{
    /// <summary>
    /// Данные профиля хранения данных.
    /// Ничего серьезного в себе не несет, только имя корневого каталога.
    /// </summary>
    [Serializable]
    public struct DataProfile
    {
        /// <summary>
        /// Имя профиля.
        /// </summary>
        public readonly string Name;
        
        public DataProfile(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Проверяет профиль на корректность.
        /// </summary>
        public static bool ValidateProfile(DataProfile dataProfile)
        {
            if (ValidateName(dataProfile.Name) == false)
                return false;

            return true;
        }
        
        public static bool ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return true;
        }
    }
}