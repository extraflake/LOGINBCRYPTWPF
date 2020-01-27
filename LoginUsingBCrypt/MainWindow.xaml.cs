using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace LoginUsingBCrypt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string connection = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
        SqlConnection sqlConnection = new SqlConnection(connection);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string myPassword = PasswordTxt.Text;
            string myHash = BCrypt.Net.BCrypt.HashPassword(myPassword);
            var getPassword = sqlConnection.Query<UserLogin>("select * from Account where Email = @Email", new { Email = EmailTxt.Text }).SingleOrDefault();
            var result = BCrypt.Net.BCrypt.Verify(myPassword, getPassword.Password);
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            string myPassword = PasswordTxt.Text;
            string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
            string myHash = BCrypt.Net.BCrypt.HashPassword(myPassword, mySalt);
            var affectedRows = sqlConnection.Execute("INSERT INTO ACCOUNT (Email, Password) VALUES (@Email, @Password)", new { Email = EmailTxt.Text, Password = myHash });
            if (affectedRows < 0)
            {
                MessageBox.Show("Failed to Register");
            }
            else
            {
                MessageBox.Show("Success to Register");
            }
        }
    }
}
