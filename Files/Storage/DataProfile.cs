using System;

namespace Egsp.Files
{
    [Serializable]
    public sealed class DataProfile
    {
        public DataProfile(string name)
        {
            Name = name;
        }
        
        /// <summary>
        /// Имя профиля.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Проверяет профиль на корректность.
        /// </summary>
        public static bool ValidateProfile(DataProfile dataProfile)
        {
            if (ValName(dataProfile.Name) == false)
                return false;

            return true;
        }
        
        private static bool ValName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return true;
        }
    }
}