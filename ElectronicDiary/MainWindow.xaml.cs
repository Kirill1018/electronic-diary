using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media
    .Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicDiary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = Header.Conn;
            conn.Close();
            conn.Open();
            string sql = "select * from users";
            IDbCommand command = new SqlCommand(sql, conn);
            IDataReader reader = command.ExecuteReader();
            bool isEnt = false;
            while (reader.Read())
            {
                string customer = reader.GetString(1), passcode = reader.GetString(2);
                bool isAppr = reader.GetBoolean(3);
                if (customer == user.Text && passcode == parole.Text && isAppr) isEnt = true;
            }
            reader.Close();
            NavigationWindow win = new NavigationWindow();
            win.Content = new Diary();
            if (isEnt)
            {
                win.Show();
                Close();
            }
        }
    }
}