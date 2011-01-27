using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.Languages;

using System.Windows.Controls;

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

            //string[] orders = new string[] { Properties.Resources.GB,
            //    Properties.Resources.MB, Properties.Resources.KB,
            //    Properties.Resources.Bytes };

            OneLanguage lang = LanguagesManager.GetCurrLanguage();
            string[] orders = new string[] { lang.SFISGB,
                lang.SFISMB, lang.SFISKB, lang.SFISBytes };

            //OneLanguage lang = new OneLanguage();
            //lang.ResourceDictionary = new System.Windows.ResourceDictionary();
            //lang.ResourceDictionary.Source = new Uri("\\Languages\\en.xaml", UriKind.Relative);

            //string[] orders = new string[] { lang.SFISGB,
            //    lang.SFISMB, lang.SFISKB, lang.SFISBytes };

            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 " + lang.SFISBytes;
            //return "0 " + Properties.Resources.Bytes;
        }

        public static StackPanel GetSizeInStrWPF(long bytes)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            TextBlock tbSize = new TextBlock();
            TextBlock tbAttr = new TextBlock();
            tbAttr.Margin = new System.Windows.Thickness(5, 0, 0, 0); 
            sp.Children.Add(tbSize);
            sp.Children.Add(tbAttr);

            const int scale = 1024;

            OneLanguage lang = LanguagesManager.GetCurrLanguage();
            string[] orders = new string[] { "SFISGB",
                "SFISMB", "SFISKB", "SFISBytes" };


            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                {
                    tbSize.Text = string.Format("{0:##.##}", decimal.Divide(bytes, max));
                    //tbAttr.Text = order;

                    tbAttr.SetResourceReference(TextBlock.TextProperty, order);

                    return sp;
                }


                max /= scale;
            }
            tbSize.Text = "0";
            tbAttr.SetResourceReference(TextBlock.TextProperty, "SFISBytes");
            //tbAttr.Text = lang.SFISBytes;


            return sp;

            //return "0 " + lang.SFISBytes;
        }


    }
}
