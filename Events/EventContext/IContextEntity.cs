using Egsp.Core;
using JetBrains.Annotations;

namespace Egsp.Core
{
    public interface IContextEntity
    {
        /// <summary>
        /// Контекст сущности.
        /// </summary>
        [CanBeNull] IContext Context { get; set; }
        
        /// <summary>
        /// Конвеер событий сущности.
        /// </summary>
        [CanBeNull] IEventBus Bus { get; }
    }
}