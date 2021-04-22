using System.Numerics;

namespace Egsp.Core.Pathfinding.NonAlloc
{
    public class PathFuncs
    {
        /// <summary>
        /// Оценивает дистанцию между положением и текущей точкой.
        /// </summary>
        public static bool Vector3Continuation(Vector3 origin, Vector3 current)
        {
            var distance = (origin - current).LengthSquared();

            if (distance <= float.Epsilon)
            {
                return true;
            }

            return false;
        }
    }
}