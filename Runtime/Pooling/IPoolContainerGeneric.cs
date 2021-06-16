namespace Egsp.Core
{
    /// <summary>
    /// Пул объектов универсального типа
    /// </summary>
    interface IPoolContainer<T>: IPoolContainer where T : class, IPoolObject
    {
        void Add(T poolObject);

        T Take();
    }
}
