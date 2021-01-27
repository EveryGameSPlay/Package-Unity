using JetBrains.Annotations;

namespace Egsp.Core
{
    public interface IContextEntityDetector
    {
        [CanBeNull] IEventBus DetectBus { set; }
    }
}