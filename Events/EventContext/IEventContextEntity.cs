using Egsp.Core;
using JetBrains.Annotations;

namespace Egsp.Core
{
    public interface IEventContextEntity
    {
        void SetEventBus([NotNull]EventBus eventBus);

        /// <summary>
        /// Вызывается контекстом после определения всех сущностей.
        /// </summary>
        void AfterContextSetup();
    }
}