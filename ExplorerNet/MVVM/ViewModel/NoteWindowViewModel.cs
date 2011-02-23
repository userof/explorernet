using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.MVVM.ViewModel;

namespace ExplorerNet.MVVM.ViewModel
{
    internal class NoteWindowViewModel : BaseViewModel
    {
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
    }
}
