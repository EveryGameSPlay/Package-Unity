using JetBrains.Annotations;

namespace Egsp.Core
{
    public interface IEventContext
    {
        [NotNull] EventBus Bus { get; }

        void SetupContextToEntities();
    }
}