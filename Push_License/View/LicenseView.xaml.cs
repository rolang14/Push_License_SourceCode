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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Push_License.View
{
    /// <summary>
    /// LicenseView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LicenseView : UserControl
    {
        public LicenseView()
        {
            InitializeComponent();
        }

        private void txtCompanyCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).CompanyCode = ((TextBox)sender).Text;
            }
        }

        private void txtLicenseKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).LicenseKey = ((TextBox)sender).Text;
            }
        }

        private void txtLicenseCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).LicenseCount = ((TextBox)sender).Text;
            }
        }
    }
}
