using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace Push_License.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
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

        #endregion

    }
}
