using UnityEngine;

namespace Egsp.Core
{
    public static class CameraExtensions
    {
        /// <param name="worldPosition">Используется в том случае, если камера в режиме перспективы.</param>
        public static Vector2 SizeInWorld(this Camera camera, Vector3 worldPosition = default(Vector3))
        {
            // ?????????????????????????????? why switch, ok
            switch (camera.orthographic)
            {
                case true : return SizeInWorldOrtho(camera);
                case false : return SizeInWorldPerspective(camera, worldPosition);
            }
        }

        public static Vector2 SizeInWorldOrtho(this Camera camera)
        {
            var height = camera.orthographicSize * 2f;
            var width = height * camera.aspect;

            return new Vector2(width, height);
        }

        /// <summary>
        /// Если мировая позиция будет равна нулевому вектору, то будет взято среднее расстояние между near и far clips
        /// </summary>
        public static Vector3 SizeInWorldPerspective(this Camera camera, Vector3 worldPosition)
        {
            if (worldPosition == Vector3.zero)
            {
                worldPosition = new Vector3(
                    Mathf.Lerp(camera.nearClipPlane, camera.farClipPlane, 0.5f), 0);
            }
            
            return SizeInWorldPerspective(camera, (worldPosition - camera.transform.position).magnitude);
        }
        
        public static Vector3 SizeInWorldPerspective(this Camera camera, float distanceFromCamera)
        {
            var height = 2.0f * distanceFromCamera * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var width = height * camera.aspect;

            return new Vector3(width, height);
        }
    }
}