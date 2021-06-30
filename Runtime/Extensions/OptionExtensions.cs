namespace Egsp.Core
{
    public static class OptionExtensions
    {
        /// <summary>
        /// Возвращает объект типа Option<T>. 
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Option<T> Some<T>(this T value)
        {
            return new Option<T>(value);
        }
    }
}