using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ExplorerNet.ViewWindowApps.Templates
{
    /// <summary>
    /// Шаблон для окна ViewWindow. Хранит настройки и расположения уровней и файловых панелей.
    /// </summary>
    [Serializable]
    public class ViewWindowTemplate
    {
        /// <summary>
        /// Коллекция уровней
        /// </summary>
        private List<LevelTemplate> _levels = null;

        /// <summary>
        /// Внутренне имя шаблона
        /// </summary>
        private string _name = "";

        public ViewWindowTemplate()
        {
            _levels = new List<LevelTemplate>();
        }

        /// <summary>
        /// Метод, для сохранения (serialize) данного шаблона в файл (XML)
        /// </summary>
        /// <param name="fileName">Путь файла для сохранения шаблона</param>
        public void Save(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            FileStream fs = new FileStream(fileName, FileMode.Create);
            serializer.Serialize(fs, this);
        }

        /// <summary>
        /// Внутренне имя шаблона
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Коллекция уровней
        /// </summary>
        public List<LevelTemplate> Levels
        {
            get { return _levels; }
        }
    }
}
