using Push_License.View;
using System.Windows;
using DeviceLib;

namespace Push_License
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            #region 정식

            Device devlib = new Device();
            // 정해진 유저 체크용 (비활성화)
            //if (devlib.CheckIP())
            //{
                LoginView loginView = new LoginView();
                loginView.Show();
                loginView.IsVisibleChanged += (s, ev) =>
                {
                    if (loginView.IsVisible == false && loginView.IsLoaded)
                    {
                        MainView mainView = new MainView();
                        mainView.Show();
                        loginView.Close();
                    }
                };
            //}

            #endregion

            #region 디버깅용

            //MainView mainView = new MainView();
            //mainView.Show();

            #endregion
        }
    }
}
