using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using ExplorerNet.Tools;
using ExplorerNet.Languages;
using ExplorerNet.Tools.Wallpapers;
using ExplorerNet.MVVM.Helper;

namespace ExplorerNet.MVVM.ViewModel
{
    internal class SettingWindowViewModel : BaseViewModel
    {

        private WallpaperSetting wallpaperSetting = null;

        private Color wallpaperColor;

        public Color WallpaperColor
        {
            get { return wallpaperColor; }
            set
            {
                wallpaperColor = value;
                WallpaperColorBrush = new SolidColorBrush(wallpaperColor);
                OnPropertyChanged(() => wallpaperColor);
            }
        }

        private SolidColorBrush wallpaperColorBrush = null;

        public SolidColorBrush WallpaperColorBrush
        {
            get { return wallpaperColorBrush; }
            set
            {
                wallpaperColorBrush = value;
                OnPropertyChanged(() => wallpaperColorBrush);
            }
        }


        private WallpaperKind wallpaperSettingKind;

        private string wallpaperPath = "";

        private Stretch wallpaperStreach = Stretch.None;

        public WallpaperKind WallpaperSettingKind
        {
            get { return wallpaperSettingKind; }
            set 
            {

                wallpaperSettingKind = value;
                wallpaperSetting.Kind = value;
                OnPropertyChanged(() => wallpaperSettingKind);
            }
        }

        public Stretch WallpaperStreach
        {
            get { return wallpaperStreach; }
            set 
            { 
                wallpaperStreach = value;
                OnPropertyChanged(() => wallpaperStreach);
            }
        }

        /// <summary>
        /// Команда открытия диалога выбора Wallpaper
        /// </summary>
        public ICommand SetPictureWallpaperCommand { get { return new BaseCommand(SetPictureWallpaper, CanSetPictureWallpaper); } }

        /// <summary>
        /// 
        /// </summary>
        public ICommand SetFonWallpaperCommand { get { return new BaseCommand(SetFonWallpaper, CanSetFonWallpaper); } }

        /// <summary>
        /// 
        /// </summary>
        public ICommand WallpaperOpenDialogCommand { get { return new BaseCommand(WallpaperOpenDialog); } }

        public ICommand ColorOpenDialogCommand { get { return new BaseCommand(ColorOpenDialog); } }

        private void ColorOpenDialog()
        {
            ColorDialog cd = new ColorDialog();

            System.Drawing.Color cc = System.Drawing.Color.FromArgb(WallpaperColor.A, WallpaperColor.R,
                    WallpaperColor.G, WallpaperColor.B);
            cd.Color = cc;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                Color c = Color.FromArgb(cd.Color.A, cd.Color.R, 
                    cd.Color.G, cd.Color.B);

                WallpaperColor = c;
                SaveWallpaperSetting();
                WallpaperManager wm = new WallpaperManager();
                wm.ShowWindowFon();
            }
        }


        #region Streach wallpapers commands

        public ICommand SetNoneStreachWallpaperCommand
        {
            get
            {
                return new BaseCommand(SetNoneStreachWallpaper,
                    CanSetNoneStreachWallpaper);
            }
        }

        private void SetNoneStreachWallpaper()
        {
            WallpaperStreach = Stretch.None;
            SaveWallpaperSetting();
            WallpaperManager wm = new WallpaperManager();
            wm.Init();
        }

        private bool CanSetNoneStreachWallpaper()
        {
            return (WallpaperStreach != Stretch.None);
        }

        public ICommand SetFillStreachWallpaperCommand
        {
            get
            {
                return new BaseCommand(SetFillStreachWallpaper,
                    CanSetFileStreachWallpaper);
            }
        }

        private void SetFillStreachWallpaper()
        {
            WallpaperStreach = Stretch.Fill;
            SaveWallpaperSetting();
            WallpaperManager wm = new WallpaperManager();
            wm.Init();
        }

        private bool CanSetFileStreachWallpaper()
        {
            return (WallpaperStreach != Stretch.Fill);
        }

        public ICommand SetUniformStreachWallpaperCommand
        {
            get
            {
                return new BaseCommand(SetUniformStreachWallpaper,
                    CanSetUniformStreachWallpaper);
            }
        }

        private void SetUniformStreachWallpaper()
        {
            WallpaperStreach = Stretch.Uniform;
            SaveWallpaperSetting();
            WallpaperManager wm = new WallpaperManager();
            wm.Init();
        }

        private bool CanSetUniformStreachWallpaper()
        {
            return (WallpaperStreach != Stretch.Uniform);
        }

        public ICommand SetUniformToFillStreachWallpaperCommand
        {
            get
            {
                return new BaseCommand(SetUniformToFillStreachWallpaper,
                    CanSetUniformToFillStreachWallpaper);
            }
        }

        private void SetUniformToFillStreachWallpaper()
        {
            WallpaperStreach = Stretch.UniformToFill;
            SaveWallpaperSetting();
            WallpaperManager wm = new WallpaperManager();
            wm.Init();
        }

        private bool CanSetUniformToFillStreachWallpaper()
        {
            return (WallpaperStreach != Stretch.UniformToFill);
        }

        #endregion



        public SettingWindowViewModel()
        {

            //Если мы не в режиме дизайнера обновляем список скинов
            if (!App.IsDesignTime)
            {
                RefreshSkins();

                RefreshLanguages();
            }
            RefreshWallpapers();

        }

        /// <summary>
        /// Обновляет состояние логики Wallpaper
        /// </summary>
        private void RefreshWallpapers()
        {
            wallpaperSetting = WallpaperSetting.Load();
            WallpaperSettingKind = wallpaperSetting.Kind;
            WallpaperPath = wallpaperSetting.Path;
            WallpaperStreach = wallpaperSetting.Stretch;

            WallpaperColor = wallpaperSetting.Color;


            WallpaperGridsInit();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetPictureWallpaper()
        {
            WallpaperSettingKind = WallpaperKind.Picture;
            WallpaperGridsInit();

            WallpaperManager wm = new WallpaperManager();
            wm.Init();
        }

        private bool CanSetPictureWallpaper()
        {
            return WallpaperSettingKind != WallpaperKind.Picture;
        }

        private void SetFonWallpaper()
        {
            WallpaperSettingKind = WallpaperKind.Fon;
            WallpaperGridsInit();

            WallpaperManager wm = new WallpaperManager();
            wm.Init();
        }

        /// <summary>
        /// Панели вкладки Wallpapers становятся активными или нет 
        /// в зависимости от состояния
        /// </summary>
        private void WallpaperGridsInit()
        {
            GridPictureWallpaperShow = (WallpaperSettingKind == WallpaperKind.Picture);
            GridFonWallpaperShow = (WallpaperSettingKind == WallpaperKind.Fon);
        }

        private bool CanSetFonWallpaper()
        {
            return WallpaperSettingKind != WallpaperKind.Fon;
        }

        private bool gridPictureWallpaperShow = false;
        private bool gridFonWallpaperShow = false;

        /// <summary>
        /// 
        /// </summary>
        public bool GridPictureWallpaperShow
        {
            get
            {
                return gridPictureWallpaperShow;// WallpaperSettingKind == WallpaperKind.Picture;
            }
            set 
            { 
                gridPictureWallpaperShow = value;
                OnPropertyChanged(() => gridPictureWallpaperShow);
            }
        }

        public bool GridFonWallpaperShow
        {
            get
            {
                return gridFonWallpaperShow;// WallpaperSettingKind == WallpaperKind.Picture;
            }
            set
            {
                gridFonWallpaperShow = value;
                OnPropertyChanged(() => gridFonWallpaperShow);
            }
        }

        /// <summary>
        /// Открытия диалога выбора Wallpaper
        /// </summary>
        private void WallpaperOpenDialog()
        {
            System.Windows.Forms.OpenFileDialog dlg =
                new System.Windows.Forms.OpenFileDialog();
            dlg.FileName = WallpaperPath; // "Template"; // Default file name
            //dlg.DefaultExt = ext; // Default file extension
            dlg.Filter = "Image Files|*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                WallpaperPath = dlg.FileName;

                SaveWallpaperSetting();

                WallpaperManager wallpaperManager = new WallpaperManager();
                wallpaperManager.RefreshPicture();
                wallpaperManager.Init();

                //_SaveTemplate(dlg.FileName);
            }

        }

        private void SaveWallpaperSetting()
        {
            wallpaperSetting.Kind = wallpaperSettingKind;
            wallpaperSetting.Path = WallpaperPath;
            wallpaperSetting.Stretch = WallpaperStreach;
            wallpaperSetting.ColorA = WallpaperColor.A;
            wallpaperSetting.ColorR = WallpaperColor.R;
            wallpaperSetting.ColorG = WallpaperColor.G;
            wallpaperSetting.ColorB = WallpaperColor.B;

            wallpaperSetting.Save();
        }

        public string WallpaperPath
        {
            get { return wallpaperPath; }
            set
            {
                wallpaperPath = value;
                OnPropertyChanged(()=>wallpaperPath);
            }
        }

        #region languages logic

        /// <summary>
        /// Список доступных языков
        /// </summary>
        private List<OneLanguage> languages = null;

        /// <summary>
        /// Текущий язык
        /// </summary>
        private OneLanguage selectedLanguage = null;

        /// <summary>
        /// Команда открытия директории с языками приложения
        /// </summary>
        public ICommand OpenLangDirCommand { get { return new BaseCommand(OpenLangDir); } }

        /// <summary>
        /// Обновить список языков
        /// </summary>
        private void RefreshLanguages()
        {
            LanguagesManager lm = new LanguagesManager();
            languages = lm.GetAllOneLanguages();

            SelectedLanguage = lm.CurrLanguage;
        }

        /// <summary>
        /// Открыть директория файлов с языками приложения
        /// </summary>
        private void OpenLangDir()
        {
            Languages.LanguagesManager lm = new LanguagesManager();
            System.Diagnostics.Process.Start(lm.LanguagesDirPath);
        }

        /// <summary>
        /// Отображения кнопки выбора языка
        /// </summary>
        public bool LanguageSelectorVisible
        {
            get
            {
                return Properties.Settings.Default.LanguageSelectorVisible;
            }
            set
            {
                Properties.Settings.Default.LanguageSelectorVisible = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged(() => value);
            }
        }

        /// <summary>
        /// Меняем язык приложения
        /// </summary>
        public OneLanguage SelectedLanguage
        {
            get
            {
                return selectedLanguage;
            }
            set
            {
                selectedLanguage = value;
                ChangeLanguage(selectedLanguage);
                OnPropertyChanged(() => selectedLanguage);
            }
        }

        /// <summary>
        /// Получаем или задаём язык приложения
        /// </summary>
        /// <param name="lang"></param>
        private void ChangeLanguage(OneLanguage lang)
        {
            LanguagesManager lm = new LanguagesManager();
            lm.ChangeOneLanguage(lang);
        }

        /// <summary>
        /// Список доступных языков
        /// </summary>
        public List<OneLanguage> Languages
        {
            get { return languages; }
        }

        #endregion

        #region skins logic

        /// <summary>
        /// Список скинов
        /// </summary>
        private List<string> skins = null;

        /// <summary>
        /// Выделенный скин
        /// </summary>
        private string selectedSkin = "";

        /// <summary>
        /// Команда перезагрузки приложения
        /// </summary>
        public ICommand ResetAppCommand { get { return new BaseCommand(ResetApp); } }

        /// <summary>
        /// Команда открытия директории скинов
        /// </summary>
        public ICommand OpenSkinDirectoryCommand { get { return new BaseCommand(OpenSkinDirectory); } }

        /// <summary>
        /// Обновляем список скинов
        /// </summary>
        public void RefreshSkins()
        {
            SkinManager sm = new SkinManager();

            skins = sm.GetSkins().ToList();

            if (skins.Count > 0)
            {
                SelectedSkin = sm.GetCurrentSkin();
            }

        }

        /// <summary>
        /// Выделенный скин
        /// </summary>
        public string SelectedSkin
        {
            get
            {
                return selectedSkin;
            }
            set
            {
                selectedSkin = value;
                ChangeSkin(selectedSkin);
                OnPropertyChanged(() => selectedSkin);
            }
        }

        /// <summary>
        /// Изменяем скин
        /// </summary>
        /// <param name="skinName"></param>
        public void ChangeSkin(string skinName)
        {
            SkinManager sm = new SkinManager();
            sm.ApplySkin(skinName);
        }

        /// <summary>
        /// Список скинов
        /// </summary>
        public List<string> Skins
        {
            get
            {
                return skins;
            }
        }

        /// <summary>
        /// Перегрузить приложения
        /// </summary>
        private void ResetApp()
        {
            App.Current.Shutdown();
            FileStarter.Start(ExplorerNet.Sys.AppInfo.AppPath);
        }

        /// <summary>
        /// Открыть директорию скинов
        /// </summary>
        private void OpenSkinDirectory()
        {
            SkinManager sm = new SkinManager();
            System.Diagnostics.Process.Start(sm.SkinDirPath);
        }

        #endregion

        
    }
}
