using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;
using System.Threading;

namespace ExplorerNet.Languages
{
    /// <summary>
    /// Класс для управления языками приложения
    /// </summary>
    public class LanguageManager
    {
        /// <summary>
        /// Задаёт язык приложения
        /// </summary>
        /// <param name="languag">Язык приложения</param>
        public void ApplyLanguag(string languag)
        {
            CultureInfo ci = new CultureInfo(languag);

            ApplyLanguag(ci);
        }

        /// <summary>
        /// Задаёт язык приложения
        /// </summary>
        /// <param name="culture">Культура приложения</param>
        public void ApplyLanguag(CultureInfo culture)
        {
            Properties.Settings.Default.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
           // Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Возвращает текущую культуру приложения
        /// </summary>
        /// <returns></returns>
        public CultureInfo GetCurrentCulture()
        {
            return Properties.Settings.Default.CurrentCulture;
        }

        /// <summary>
        /// Список доступных культур для приложения
        /// </summary>
        /// <returns></returns>
        public List<CultureInfo> GetAvailableCultures()
        {
            List<CultureInfo> list = new List<CultureInfo>();
            list.Add(new CultureInfo("en"));
            list.Add(new CultureInfo("ru"));

            return list;
        }
    }
}
