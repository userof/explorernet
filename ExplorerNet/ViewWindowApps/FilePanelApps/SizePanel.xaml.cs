using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ExplorerNet.Tools;
using ExplorerNet.Languages;

using System.IO;

using System.Threading;
using System.Windows.Threading;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    /// <summary>
    /// Логика взаимодействия для SizePanel.xaml
    /// </summary>
    public partial class SizePanel : UserControl
    {
        private CustomFileSystemCover data = null;

        
        public static readonly DependencyProperty CalculiatingStateProperty =
            DependencyProperty.Register("CalculiatingState", typeof(CalculiatingStateKind),
            typeof(SizePanel));

        public CalculiatingStateKind CalculiatingState
        {
            get
            {
                return (CalculiatingStateKind)GetValue(CalculiatingStateProperty); 
            }
            set
            {
                SetValue(CalculiatingStateProperty, value);
            }
        }

        public static readonly DependencyProperty InvalidAccessProperty =
            DependencyProperty.Register("InvalidAccess", typeof(bool),
            typeof(SizePanel));

        public bool InvalidAccess
        {
            get
            {
                return (bool)GetValue(InvalidAccessProperty);
            }
            set
            {
                SetValue(InvalidAccessProperty, value);
            }
        }


        public SizePanel()
        {
            InitializeComponent();
            this.CalculiatingState = CalculiatingStateKind.BeforeStart;
            this.InvalidAccess = false;
            //btnSumCalculate.ToolTip = Properties.Resources.CalculateTheDirectorySize;
            //btnSumCalculate.ToolTip = 
            //    LanguagesManager.GetCurrLanguage().SPCalculateTheDirectorySize;

            btnSumCalculate.ToolTip = 
                (ToolTip)this.Resources["ttCalculateTheDirectorySize"];
        }

        public CustomFileSystemCover Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                SetData(value);
            }
        }

        public void SetData(CustomFileSystemCover fsCover)
        {
            if (fsCover.GetType() == typeof(DirectoryCover))
            {
                
                //txtSize.Text = LanguagesManager.GetCurrLanguage().SPDir;
                ccSize.SetResourceReference(ContentControl.ContentProperty, "SPDir");

                btnSumCalculate.MinWidth = 20;
                btnSumCalculate.BorderThickness = new Thickness(4);
                btnSumCalculate.Visibility = System.Windows.Visibility.Visible;
            }
            else if (fsCover.GetType() == typeof(FileCover))
            {
                FileInfo fi = (FileInfo)fsCover.FileSystemElement;

                //txtSize.Text = SizeFileInString.GetSizeInStr(fi.Length);
                ccSize.Content = SizeFileInString.GetSizeInStrWPF(fi.Length);

                btnSumCalculate.BorderThickness = new Thickness(0);
                btnSumCalculate.MinWidth = 0;
                btnSumCalculate.Width = 0;
                btnSumCalculate.Visibility = System.Windows.Visibility.Hidden;
            }

            
        }

        private void btnSumCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (data.GetType() == typeof(DirectoryCover))
            {
                if (CalculiatingState != CalculiatingStateKind.Working)
                {
                    DirectoryCover dCover = (DirectoryCover)data;
                    DirectoryInfo di = dCover.DirectoryElement;

                    //txtSize.Background = Brushes.White;

                    CalculiatorSize cSize = new CalculiatorSize();

                    cSize.UpdateFieleSum += new CalculiatorSize.UpdateFieleSumEventHandler(cSize_UpdateFieleSum);
                    cSize.InvalidAccess += new CalculiatorSize.InvalidAccessEventHandler(cSize_InvalidAccess);
                    cSize.ChangeCalculiatingState += new CalculiatorSize.ChangeCalculiatingStateEventHandler(cSize_ChangeCalculiatingState);

                    Thread thread = new Thread(cSize.Calculiate);
                    //thread.Priority = ThreadPriority.
                    thread.IsBackground = true;
                    thread.Start(di);
                }

                
                //long sum = CalculateSize(di);
                //txtSize.Text = SizeFileInString.GetSizeInStr(sum);
            }

            
        }

        private void cSize_ChangeCalculiatingState(CalculiatingStateKind state)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
            {
                switch (state)
                {
                    case CalculiatingStateKind.BeforeStart:
                        btnSumCalculate.BorderBrush = Brushes.Green;
                        //btnSumCalculate.ToolTip = Properties.Resources.CalculateTheDirectorySize;
                        btnSumCalculate.ToolTip = 
                            //LanguagesManager.GetCurrLanguage().SPCalculateTheDirectorySize;
                            (ToolTip)this.Resources["ttCalculateTheDirectorySize"];
                        break;
                    case CalculiatingStateKind.Working:
                        //btnSumCalculate.ToolTip = Properties.Resources.CalculatingTheDirectorySize;
                        btnSumCalculate.ToolTip = 
                            //LanguagesManager.GetCurrLanguage().SPCalculatingTheDirectorySize;
                            (ToolTip)this.Resources["ttCalculatingTheDirectorySize"];
                        if (InvalidAccess)
                        {
                            btnSumCalculate.BorderBrush = Brushes.Red;
                        }
                        else
                        {
                            btnSumCalculate.BorderBrush = Brushes.Blue;
                        }
                        //btnSumCalculate.BorderThickness = new Thickness(4);
                        break;
                    case CalculiatingStateKind.End:
                        if (InvalidAccess)
                        {
                            btnSumCalculate.BorderBrush = Brushes.Red;
                            //btnSumCalculate.ToolTip = Properties.Resources.CalculatedTheDirectorySizeCompletedIsNot;
                            btnSumCalculate.ToolTip =
                                //LanguagesManager.GetCurrLanguage().SPCalculatedTheDirectorySizeCompletedIsNot;
                                (ToolTip)this.Resources["ttCalculatedTheDirectorySizeCompletedIsNot"];
                        }
                        else
                        {
                            btnSumCalculate.BorderBrush = Brushes.Green;
                            //btnSumCalculate.ToolTip = Properties.Resources.CalculatedTheDirectorySizeCompleted;
                            btnSumCalculate.ToolTip =
                                //LanguagesManager.GetCurrLanguage().SPCalculatedTheDirectorySizeCompleted;
                                (ToolTip)this.Resources["ttCalculatedTheDirectorySizeCompleted"];
                        }
                        //MessageBox.Show("ok");
                        break;
                    default:
                        break;
                }

                btnSumCalculate.UpdateLayout();
                
                //if (state == CalculiatingStateKind.Working)
                //{
                //    btnSumCalculate.Background = Brushes.Red;
                //}

                CalculiatingState = state;
            });
        }

        private void cSize_InvalidAccess(bool isNotAccess)
        {

                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
                {
                    this.InvalidAccess = isNotAccess;
                    //if (isNotAccess)
                    //{
                    //    txtSize.Background = Brushes.Red;
                    //}
                });
        }


        private void cSize_UpdateFieleSum(long sum)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate()
            {
                //txtSize.Text = SizeFileInString.GetSizeInStr(sum);
                ccSize.Content = SizeFileInString.GetSizeInStrWPF(sum);
            }
            );
        }

        //private class CalculateData
        //{
        //    private long sum = 0;

        //    private DirectoryInfo sourceDir = null;

        //    public CalculateData(DirectoryInfo sourceDir, long sum)
        //    {
        //        this.sum = sum;
        //        this.sourceDir = sourceDir;
        //    }

        //    public long Sum
        //    {
        //        get { return sum; }
        //        set { sum = value; }
        //    }

        //    public DirectoryInfo SourceDir
        //    {
        //        get { return sourceDir; }
        //        set { sourceDir = value; }
        //    }
        //}

        private class CalculiatorSize
        {
            public delegate void UpdateFieleSumEventHandler(long sum);

            public event UpdateFieleSumEventHandler UpdateFieleSum;

            public delegate void InvalidAccessEventHandler(bool isNotAccess);

            public event InvalidAccessEventHandler InvalidAccess;

            public delegate void ChangeCalculiatingStateEventHandler(CalculiatingStateKind state);

            public event ChangeCalculiatingStateEventHandler ChangeCalculiatingState;

            public void Calculiate(object data)
            {
                if (data.GetType() == typeof(DirectoryInfo))
                {
                    DirectoryInfo sourceDir = (DirectoryInfo)data;

                    CalculateSize(sourceDir);
                }
            }

            private long CalculateSize(DirectoryInfo sourceDir)
            {
                ChangeCalculiatingState(CalculiatingStateKind.Working);

                long sum = 0;
                GetAllSize(sourceDir, ref sum);

                ChangeCalculiatingState(CalculiatingStateKind.End);

                return sum;
            }

            private void GetAllSize(DirectoryInfo sourceDir, ref long sum)
            {
                try
                {
                    FileInfo[] files = sourceDir.GetFiles();
                    foreach (var f in files)
                    {
                        sum += f.Length;
                    }
                }
                catch(Exception)
                {
                    InvalidAccess(true);
                }

                UpdateFieleSum(sum);
                //UpdateSizeData(sum);

                try
                {
                    DirectoryInfo[] dirs = sourceDir.GetDirectories();
                    foreach (var d in dirs)
                    {
                        GetAllSize(d, ref sum);
                    }
                }
                catch (Exception)
                {
                    InvalidAccess(true);
                }
            }
        }

        public enum CalculiatingStateKind
        {
            BeforeStart,
            Working,
            End
        }


    }
}
