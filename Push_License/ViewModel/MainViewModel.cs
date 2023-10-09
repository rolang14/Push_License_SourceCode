using Push_License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Push_License.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }

            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        private InitialViewModel InitialViewModel { get; }
        public LicenseViewModel LicenseViewModel { get; private set; }
        public SearchViewModel SearchViewModel { get; private set; }
        public ICommand ShowLicenseViewCommand { get; }
        public ICommand ShowSearchViewCommand { get; }

        public MainViewModel()
        {
            InitialViewModel = new InitialViewModel();
            LicenseViewModel = new LicenseViewModel();
            SearchViewModel = new SearchViewModel();
            ShowLicenseViewCommand = new ButtonCommand(ExecuteShowLicenseViewCommand);
            ShowSearchViewCommand = new ButtonCommand(ExecuteShowSearchViewCommand);
            ExecuteShowInitialViewCommand(null);
        }

        private void ExecuteShowInitialViewCommand(object obj)
        {
            CurrentChildView = InitialViewModel;
        }

        private void ExecuteShowLicenseViewCommand(object obj)
        {
            CurrentChildView = LicenseViewModel;
        }

        private void ExecuteShowSearchViewCommand(object obj)
        {
            CurrentChildView = SearchViewModel;
        }
    }
}
