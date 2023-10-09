using Push_License.ViewModel;
using System.Windows;
using Push_License.View;

namespace Push_License
{
    internal class ViewNavigator : INavigationService
    {
        public void NavigateToViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            Window window = null;
            if (typeof(TViewModel) == typeof(MainViewModel))
            {
                window = new MainView();
            }
            //else if (typeof(TViewModel) == typeof(MainViewModel))
            //{
            //    window = new MainView();
            //}

            // Add logic if you need
            window.Show();
        }
    }
}
