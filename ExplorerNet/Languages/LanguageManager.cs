using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;
using System.Threading;

namespace ExplorerNet.Languages
{
    public class LanguageManager
    {
        public void ApplyLanguag(string languag)
        {
            CultureInfo ci = new CultureInfo(languag);

            ApplyLanguag(ci);
        }

        public void ApplyLanguag(CultureInfo culture)
        {
            Properties.Settings.Default.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
           // Properties.Settings.Default.Save();
        }

        public CultureInfo GetCurrentCulture()
        {
            return Properties.Settings.Default.CurrentCulture;
        }

        public List<CultureInfo> GetAvailableCultures()
        {
            List<CultureInfo> list = new List<CultureInfo>();
            list.Add(new CultureInfo("en"));
            list.Add(new CultureInfo("ru"));

            return list;
        }
    }
}
