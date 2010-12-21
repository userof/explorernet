using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.ViewWindowApps.Templates;

namespace ExplorerNet.Tools.RecentTemplateNS
{
    public static class RecentTemplateManager
    {
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

        public static void AddNewRecentTemplate(string recentTemplateName, string recentTemplatePath)
        {
            RecentTemplate rTemplate = new RecentTemplate();
            rTemplate.Name = recentTemplateName;
            rTemplate.Path = recentTemplatePath;

            AddNewRecentTemplate(rTemplate);
        }
    }
}
