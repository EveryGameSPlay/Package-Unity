using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// <inheritdoc cref="IPhysicsEntity"/>
    /// </summary>
    public interface IMonoPhysicsEntity : IPhysicsEntity
    {
        GameObject GameObject { get; }
    }
}