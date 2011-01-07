using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.Tools
{
    /// <summary>
    /// Предоставляет размер в единицах, зависящих от размера
    /// </summary>
    public static class SizeFileInString
    {
        /// <summary>
        /// Предоставляет размер в единицах, зависящих от размера
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetSizeInStr(long bytes)
        {

            const int scale = 1024;
            string[] orders = new string[] { Properties.Resources.GB,
                Properties.Resources.MB, Properties.Resources.KB,
                Properties.Resources.Bytes };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 " + Properties.Resources.Bytes;

        }
    }
}
