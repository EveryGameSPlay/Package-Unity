using JetBrains.Annotations;

namespace Egsp.Core
{
    /// <summary>
    /// Объект данного типа при обнаружении новых IContextEntity должен уведомить IContext с помощью специального
    /// потока событий.
    /// </summary>
    public interface IContextEntityDetector
    {
        [CanBeNull] IEventBus DetectBus { set; }
    }
}