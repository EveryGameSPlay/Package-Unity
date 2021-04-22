using System.IO;

namespace Egsp.Extensions
{
    public static class FileSystemExtensions
    {
        /// <summary>
        /// Приводит слеши к единому формату.
        /// </summary>
        public static string NormalizePath(string path)
        {
            return path
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static bool ComparePaths(string path1, string path2)
        {
            if (NormalizePath(path1) == NormalizePath(path2))
            {
                return true;
            }

            return false;
        }

        public static string GetNameOnly(this FileInfo info)
        {
            return Path.GetFileNameWithoutExtension(info.Name);
        }
    }
}