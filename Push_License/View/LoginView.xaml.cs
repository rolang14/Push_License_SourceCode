using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Push_License.ViewModel;

namespace Push_License.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        #region Events

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            WindowAPI.CloseWindow();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowAPI.MinimizeWindow(this);
        }

        // DragMove
        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowAPI.DragWindow(this);
        }

        // For Resizing Window adjust each Monitor if it moved to another screen.
        // But we are not going to make it maximize, so this in unnecessary
        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            WindowAPI.setMaxHeight(this);
        }

        private void pnlControlBar_MouseLeave(object sender, MouseEventArgs e)
        {
            WindowAPI.setMaxHeight(this);
        }

        private void txtLoginId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ID = ((TextBox)sender).Text;
            }
        }

        private void txtLoginPW_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).PW = ((PasswordBox)sender).SecurePassword;
            }
        }

        #endregion
    }
}
