using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
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
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://116.254.101.228:8080/APISAKURAJWT/")
            };
            client.DefaultRequestHeaders.Add("Authorization", "bootcamp eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJBc3NldCBNYW5hZ2VtZW50IiwiaWF0IjoxNTcxMjEwMzQ0fQ.egQGVL6fHVvPnann4tvJlDR-4N7Pg8J-KC9hhbqa0w90ulWKya2sQUpIVQyqghy4iwBAmQu1fkVopr3eFPk34A");
            var responseTask = client.GetAsync(EmailTxt.Text);
            responseTask.Wait();
            var resultTask = responseTask.Result;
            if (resultTask.IsSuccessStatusCode)
            {
                JToken stuff1 = JObject.Parse(JsonConvert.DeserializeObject(resultTask.Content.ReadAsStringAsync().Result).ToString());
                var get = stuff1.SelectToken("data[0].Password").ToString();
                var resultCheck = BCrypt.Net.BCrypt.Verify(myPassword, get);
                if (resultCheck)
                {
                    MessageBox.Show("Good");
                }
                else
                {
                    MessageBox.Show("Bad");
                }
            }
            else
            {
                // try to find something
            }
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
