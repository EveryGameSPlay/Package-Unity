using JetBrains.Annotations;

namespace Egsp.Core
{
    /// <summary>
    /// Сущность, которая может присоединиться к контексту.
    /// Этой сущности должен быть передан поток событий самого контекста.
    /// </summary>
    public interface IContextEntity
    {
        /// <summary>
        /// Контекст сущности.
        /// </summary>
        [CanBeNull] IContext Context { get; set; }
    }
}