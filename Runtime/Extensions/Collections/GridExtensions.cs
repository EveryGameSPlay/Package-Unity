using UnityEngine;

namespace Egsp.Core
{
    public static class GridExtensions
    {
        /// <summary>
        /// Получение позиции в мировых координатах.
        /// </summary>
        public static Vector3 GetPositionRaw(this Grid grid, int x,int y)
        {
            return new Vector3(x * grid.CellParamsValue.Width, y * grid.CellParamsValue.Height) + grid.SceneParamsValue.Offset;
        }

        /// <summary>
        /// Получение позиции центра ячейки в мировых координатах.
        /// </summary>
        public static  Vector3 GetPosition(this Grid grid, int x,int y)
        {
            return new Vector3(x * grid.CellParamsValue.Width, y * grid.CellParamsValue.Height) 
                   + new Vector3(grid.CellParamsValue.Width, grid.CellParamsValue.Height) * 0.5f + grid.SceneParamsValue.Offset;
        }

        /// <summary>
        /// Возвращает мировую позицию нижнего левого угла.
        /// </summary>
        public static Vector3 BottomLeft(this Grid grid)
            => grid.SceneParamsValue.Offset;

        /// <summary>
        /// Возвращает мировую позицию верхнего правого угла.
        /// </summary>
        public static Vector3 TopRight(this Grid grid)
            => grid.SceneParamsValue.Offset + new Vector3(grid.CellParamsValue.Width * grid.Width, grid.CellParamsValue.Height * grid.Height);
        
        /// <summary>
        /// Возвращает позицию в зависимости от размерности ячеек.
        /// </summary>
        /// <param name="position">Позиция в мировом пространстве</param>
        public static Vector3 SnapRaw(this Grid grid, Vector3 position)
        {
            position.x = Mathf.Floor(position.x / grid.CellParamsValue.Width)
                         *  grid.CellParamsValue.Width;
            
            position.y = Mathf.Floor(position.y /  grid.CellParamsValue.Height)
                         * grid.CellParamsValue.Height;

            return position;
        }
        
        /// <summary>
        /// Возвращает позицию в зависимости от размерности ячеек, но центрированную.
        /// </summary>
        public static Vector3 Snap(this Grid grid,Vector3 position)
        {
            position.x = Mathf.Floor(position.x / grid.CellParamsValue.Width)
                * grid.CellParamsValue.Width+grid.CellParamsValue.Width*0.5f;
            
            position.y = Mathf.Floor(position.y /  grid.CellParamsValue.Height)
                *  grid.CellParamsValue.Height+ grid.CellParamsValue.Height*0.5f;

            return position;
        }
        
        /// <summary>
        /// Возвращает позицию ближайшей ячейки
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 GetNearest(this Grid grid, Vector3 position)
        {
            position = Snap(grid, position);
            
            int x, y;
            grid.WorldToIndex(position,out x,out y);
            
            if (grid.InBounds(x,y))
                return position;

            x = Mathf.Clamp(x, 0, grid.Width-1);
            y = Mathf.Clamp(y, 0, grid.Height-1);

            return GetPosition(grid, x, y);

        }
        
        /// <summary>
        /// Дополнительно возвращает номер индекса в сетке.
        /// </summary>
        public static Vector3 GetNearest(this Grid grid, Vector3 position, out int x,out int y)
        {
            position = Snap(grid, position);
            
            grid.WorldToIndex(position,out x,out y);
            
            if (grid.InBounds(x,y))
                return position;

            x = Mathf.Clamp(x, 0, grid.Width-1);
            y = Mathf.Clamp(y, 0, grid.Height-1);

            return GetPosition(grid, x, y);
        }
    }
}