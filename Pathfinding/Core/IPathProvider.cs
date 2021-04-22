namespace Egsp.Core.Pathfinding
{
    /// <summary>
    /// Объект данного типа предоставляет путь между точками по запросу. 
    /// </summary>
    public interface IPathProvider<TPoint>
    {
        IPathResponse<TPoint> RequestPath(TPoint entryPoint, TPoint endPoint);
    }
}