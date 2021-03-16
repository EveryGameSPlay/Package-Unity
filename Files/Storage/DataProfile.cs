using System;

namespace Egsp.Files
{
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