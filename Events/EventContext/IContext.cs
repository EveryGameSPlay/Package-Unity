using JetBrains.Annotations;

namespace Egsp.Core
{
    public interface IContext
    {
        [NotNull] IEventBus Bus { get; }

        void AddEntity([NotNull] IContextEntity contextEntity);
    }
}