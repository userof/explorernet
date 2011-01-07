using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

///
///Пространство имён для работы с последними открытыми шаблонами приложения
namespace ExplorerNet.Tools.RecentTemplateNS
{
    /// <summary>
    /// Представление последнего открытого шаблона
    /// </summary>
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

        /// <summary>
        /// Сравнение последних открытых шаблонов
        /// </summary>
        /// <param name="recentTemplate"></param>
        /// <returns></returns>
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
