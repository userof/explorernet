using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;


using ExplorerNet.MVVM.ViewModel;
using ExplorerNet.MVVM.Helper;

namespace ExplorerNet.MVVM.ViewModel
{
    internal class NoteWindowViewModel : BaseViewModel
    {
        public ICommand InsertDateCommand { get { return new BaseCommand(InsertDate); } }

        private void InsertDate()
        {
            Description += " " + DateTime.Now.ToLongDateString();
        }

        public ICommand InsertFromClipboardCommand { get { 
            return new BaseCommand(InsertFromClipboard, CanInsertFromClipboard); } }

        private void InsertFromClipboard()
        {
            Description += " " + System.Windows.Clipboard.GetText();
        }

        private bool CanInsertFromClipboard()
        {
            return System.Windows.Clipboard.ContainsText();
        }

        public ICommand OkCommand { get { return new BaseCommand(Ok); } }

        private void Ok()
        {
            DialogAnswer = true;
            CloseWindow();
        }

        public ICommand CancelCommand { get { return new BaseCommand(Cancel); } }

        private void Cancel()
        {
            DialogAnswer = false;
            CloseWindow(); 
        }

        public NoteWindowViewModel()
        {
            if (this.IsInDesignMode)
            {
                Description = "Test text";
            }
        }


        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(() => Description);
            }
        }

        private bool? dialogAnswer = null;

        public bool? DialogAnswer
        {
            get { return dialogAnswer; }
            set
            {
                dialogAnswer = value;
                OnPropertyChanged(() => DialogAnswer);
            }
        }

        public event Action RequestClose;

        public void CloseWindow()
        {
            if (RequestClose != null)
            {
                RequestClose();
            }
        }


    }
}
