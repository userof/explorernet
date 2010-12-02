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

        private string _skin = "";

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

            fs.Dispose();
        }

        /// <summary>
        /// Загрузка шаблона с файла
        /// </summary>
        /// <param name="fileName">Имя файла для загрузки шаблона</param>
        /// <returns>Экземпляр шаблона</returns>
        public static ViewWindowTemplate Load(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ViewWindowTemplate));

            Stream reader = new FileStream(fileName, FileMode.Open);

            ViewWindowTemplate result = (ViewWindowTemplate)serializer.Deserialize(reader);

            reader.Dispose();

            return result;
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

        public string Skin
        {
            get { return _skin; }
            set { _skin = value; }
        }

        /// <summary>
        /// Создаём шаблон по умолчанию.
        /// </summary>
        /// <returns></returns>
        public static ViewWindowTemplate CreateDefoultTemplate()
        {
            ViewWindowTemplate template = new ViewWindowTemplate();
            template.Name = "defoult";


            LevelTemplate level1 = new LevelTemplate();
            level1.Height = Properties.Settings.Default.HeightLevel;
            FilePanelTemplate fp1 = new FilePanelTemplate();
            fp1.FilePanelSettings = new FilePanelApps.FilePanelSettings();
            //fp1.Width = Properties.Settings.Default.WidthFilepanel;
            FilePanelTemplate fp2 = new FilePanelTemplate();
            fp2.FilePanelSettings = new FilePanelApps.FilePanelSettings();
            //fp2.Width = Properties.Settings.Default.WidthFilepanel;
            level1.FilePanels.Add(fp1);
            level1.FilePanels.Add(fp2);

            LevelTemplate level2 = new LevelTemplate();
            level2.Height = Properties.Settings.Default.HeightLevel;
            FilePanelTemplate fp3 = new FilePanelTemplate();
            fp3.FilePanelSettings = new FilePanelApps.FilePanelSettings();
            //fp3.Width = Properties.Settings.Default.WidthFilepanel;
            FilePanelTemplate fp4 = new FilePanelTemplate();
            fp4.FilePanelSettings = new FilePanelApps.FilePanelSettings();
            //fp4.Width = Properties.Settings.Default.WidthFilepanel;
            level2.FilePanels.Add(fp3);
            level2.FilePanels.Add(fp4);

            template.Levels.Add(level1);
            template.Levels.Add(level2);

            return template;
        }
    }
}
