using UnityEngine;

namespace Egsp.Core
{
    public static class InputExtensions
    {
        /// <summary>
        /// Получение координат мыши в мировом пространстве с Z
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            var worldPos = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPos;
        }

        /// <summary>
        /// Получение позиции мыши в мировых координатах без учета Z
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPosition()
        {
            var mouseWorldPosition = GetMouseWorldPositionWithZ();
            mouseWorldPosition.z = 0f;

            return mouseWorldPosition;
        }
    }
}