﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.Tools
{
    public static class SizeFileInString
    {
        public static string GetSizeInStr(long bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);


            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";

        }
    }
}
