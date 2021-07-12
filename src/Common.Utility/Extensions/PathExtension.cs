using System.IO;
using System.Runtime.InteropServices;

namespace Common.Utility.Extensions
{
    /// <summary>
    /// 不同操作系统下文件路径处理扩展类
    /// </summary>
    public static class PathExtension
    {
        /// <summary>
        /// 处理当前操作系统下的文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRuntimePath(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }
            //ForLinux
            if (IsLinuxRuntime())
                return GetLinuxPath(path);
            //ForWindows
            if (IsWindowRuntime())
                return GetWindowPath(path);
            return path;
        }

        /// <summary>
        /// OSPlatform.Windows监测运行环境
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowRuntime()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        /// <summary>
        /// OSPlatform.Linux运行环境
        /// </summary>
        /// <returns></returns>
        public static bool IsLinuxRuntime()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        /// <summary>
        /// Linux环境下路径处理
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetLinuxPath(string path)
        {
            string pathTemp = Path.Combine(path);
            return pathTemp.Replace("\\", "/");
        }

        /// <summary>
        /// Windows环境下路径处理
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetWindowPath(string path)
        {
            string pathTemp = Path.Combine(path);
            return pathTemp.Replace("/", "\\");
        }
    }
}
