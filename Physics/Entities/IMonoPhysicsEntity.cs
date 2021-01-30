using UnityEngine;

namespace Egsp.Core
{
    public interface IMonoPhysicsEntity : IPhysicsEntity
    {
        GameObject GameObject { get; }
    }
}