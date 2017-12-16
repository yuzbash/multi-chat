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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client(10000);//change constructor later
            
            if (client.Connect()) // and can login in future
            {
                MainWindow window = new MainWindow(client);
                window.Show();
                this.Close();
            }
            else
            {
                LabelLoginResult.Content = "Error with connection";                
            }
        }
    }
}
