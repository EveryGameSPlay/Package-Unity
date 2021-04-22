namespace Egsp.Core.Pathfinding.NonAlloc
{
    public interface IPathProvider<TPoint>
    {
        PathResponse<TPoint> RequestPath(TPoint entryPoint, TPoint endPoint);
    }
}