using JetBrains.Annotations;

namespace Egsp.Core
{
    public interface IPhysicsEntity
    {
        /// <summary>
        /// Приложение силы к физической сущности.
        /// </summary>
        /// <param name="force">Прикладываемая сила.</param>
        /// <param name="actor">Отправитель.</param>
        void ApplyForce(Force force, [CanBeNull] IPhysicsEntity actor = null);
    }
}