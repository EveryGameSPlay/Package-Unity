using JetBrains.Annotations;

namespace Egsp.Core
{
    /// <summary>
    /// Базовый интерфейс для контекста.
    /// Содержит в себе поток событий.
    /// </summary>
    public interface IContext
    {
        [NotNull] IEventBus Bus { get; }

        void AddEntity([NotNull] IContextEntity contextEntity);
    }
}