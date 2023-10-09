using System.Security;
using System.Windows.Input;
using Push_License.Core;
using ConnectLib;
using System.Windows;

namespace Push_License.ViewModel
{
    internal class LoginViewModel : ViewModelBase
    {
        private string _id;
        private SecureString _pw;
        private bool _isLoginEnabled;
        private bool _isViewVisible = true;

        public ICommand LoginCommand { get; }

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                UpdateLoginButtonEnabled();
                OnPropertyChanged(nameof(ID));
            }
        }

        public SecureString PW
        {
            get { return _pw; }
            set
            {
                _pw = value;
                UpdateLoginButtonEnabled();
                OnPropertyChanged(nameof(PW));
            }
        }

        public bool IsLoginEnabled
        {
            get { return _isLoginEnabled; }
            set
            {
                _isLoginEnabled = value;
                OnPropertyChanged(nameof(IsLoginEnabled));
            }
        }

        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set
            {
                if (_isViewVisible != value)
                {
                    _isViewVisible = value;
                    OnPropertyChanged(nameof(IsViewVisible));
                }

            }
        }

        public LoginViewModel()
        {
            LoginCommand = new ButtonCommand(ExecuteLoginCommand);
        }

        private void UpdateLoginButtonEnabled()
        {
            IsLoginEnabled = !string.IsNullOrEmpty(ID) && !SecureStringLib.IsSecureStringEmpty(PW);
        }



        private void ExecuteLoginCommand(object obj)
        {
            if (IsLoginEnabled)
                Login();
        }

        private async void Login()
        {
            // Login Pass (비활성화)
            //string res = await Connect.Instance._wsLoginSend(ID, SecureStringLib.SecureStringToString(PW));
            //try
            //{
            //    IsViewVisible = !bool.Parse(res);
            //    if (IsViewVisible)
            //    {
            //        MessageBox.Show("로그인 정보가 잘못되었습니다.");
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show(res);
            //}

            IsViewVisible = false;
        }
    }
}
