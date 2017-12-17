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
using System.Windows.Shapes;

namespace MyTcpClient
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public bool IsOk{get; set;}
        public string ResultString { get; set; }
        public Registration()
        {
            InitializeComponent();
            IsOk = false;
        }

        private void ButtonRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (TBuserNameReg.Text == "")
            {
                LabelResultReg.Content = "Please, enter user name";
                return;
            }
            if (TBpasswordReg.Text == "")
            {
                LabelResultReg.Content = "Please, enter password";
                return;
            }
            if (TBsurnameReg.Text == "")
            {
                LabelResultReg.Content = "Please, enter surname";
                return;
            }
            if (TBpassportNameReg.Text == "")
            {
                LabelResultReg.Content = "Please, enter your real name";
                return;
            }
            if (TBemailReg.Text == "")
            {
                LabelResultReg.Content = "Please, enter email";
                return;
            }
            if (TBcityReg.Text == "")
            {
                LabelResultReg.Content = "Please, enter city";
                return;
            }
            string resString = "reg:" + 
                "user:" + TBuserNameReg.Text +
                "pswd:" + TBpasswordReg + 
                "srnm:" + TBsurnameReg.Text+
                "pasp:" + TBpassportNameReg.Text +
                "emal:" + TBemailReg.Text +
                "city:" + TBcityReg.Text;
            ResultString = resString;
            IsOk = true;
        }
    }
}
