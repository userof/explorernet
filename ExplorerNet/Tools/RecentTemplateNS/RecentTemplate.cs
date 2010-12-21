using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.Tools.RecentTemplateNS
{
    [Serializable]
    public class RecentTemplate : IEquatable<RecentTemplate>
    {
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string path = "";

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public bool Equals(RecentTemplate recentTemplate)
        {
            //RecentTemplate recentTemplate = (RecentTemplate)obj;

            if (this.Path == recentTemplate.Path)
            {
                return true;
            }
            else
            {
                return false;
            }


            //return base.Equals(obj);
        }
    }
}
