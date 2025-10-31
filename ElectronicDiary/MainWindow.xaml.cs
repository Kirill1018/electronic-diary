using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Navigation;

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
            SqlConnection sqlConnection = Header.SqlConnection;
            sqlConnection.Close();
            sqlConnection.Open();
            string sql = "select * from users";
            IDbCommand iDbCommand = new SqlCommand(sql, sqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            int? id = null;
            bool isEnt = false;
            while (iDataReader.Read()) if (iDataReader.GetString(1) == user
                    .Text && iDataReader.GetString(2) == parole.Text
                    && iDataReader.GetBoolean(3))
                {
                    id = iDataReader.GetInt32(0);
                    isEnt = true;
                }
            iDataReader.Close();
            if (isEnt)
            {
                NavigationWindow navigationWindow = new NavigationWindow();
                navigationWindow.Content = new Diary(id);
                navigationWindow.Show();
                Close();
            }
        }
    }
}