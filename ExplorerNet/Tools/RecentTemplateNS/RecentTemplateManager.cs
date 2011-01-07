using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.ViewWindowApps.Templates;

namespace ExplorerNet.Tools.RecentTemplateNS
{
    /// <summary>
    /// Класс для управления последними открытыми шаблонами приложения
    /// </summary>
    public static class RecentTemplateManager
    {
        /// <summary>
        /// Возвращает последние открытые шаблоны приложения
        /// </summary>
        /// <param name="templatesCount">Количество возвращаемых шаблонов</param>
        /// <returns></returns>
        public static RecentTemplates GetRecentTemplates(int templatesCount)
        {
            RecentTemplates result = new RecentTemplates();

            if (Properties.Settings.Default.RecentTemplates != null)
            {
                int i = 0;
                foreach (var rt in Properties.Settings.Default.RecentTemplates)
                {
                    if (System.IO.File.Exists(rt.Path))
                    {

                        if (i < templatesCount)
                        {
                            result.Add(rt);
                        }
                        i++;
                    }
                    else
                    {
                        Properties.Settings.Default.RecentTemplates.Remove(rt);
                        Properties.Settings.Default.Save();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Добавляет новый шаблон в список последних открытых шаблонов
        /// </summary>
        /// <param name="recentTemplate">Обложка для шаблона</param>
        public static void AddNewRecentTemplate(RecentTemplate recentTemplate)
        {
            if (Properties.Settings.Default.RecentTemplates == null)
            {
                Properties.Settings.Default.RecentTemplates = new RecentTemplates();
            }

            var rts = Properties.Settings.Default.RecentTemplates;

            if (rts.Contains(recentTemplate))
            {
                rts.Remove(recentTemplate);
            }

            rts.Insert(0, recentTemplate);

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Добавляет новый шаблон в список последних открытых шаблонов
        /// </summary>
        /// <param name="recentTemplateName">Имя шаблона</param>
        /// <param name="recentTemplatePath">Путь к шаблону</param>
        public static void AddNewRecentTemplate(string recentTemplateName, string recentTemplatePath)
        {
            RecentTemplate rTemplate = new RecentTemplate();
            rTemplate.Name = recentTemplateName;
            rTemplate.Path = recentTemplatePath;

            AddNewRecentTemplate(rTemplate);
        }
    }
}
