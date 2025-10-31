using Microsoft.Data.SqlClient;
using System.Windows;

namespace ElectronicDiary
{
    /// <summary>
    /// Логика взаимодействия для Work.xaml
    /// </summary>
    public partial class Work : Window
    {
        int HomId { get; set; }
        Diary Diary { get; set; }
        public Work(int homId, Diary diary)
        {
            InitializeComponent();
            this.HomId = homId;
            this.Diary = diary;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string? maintText = maintenance.Text;
            string sql = "insert into checking(userId, homId, "
                + $"content) values({this.Diary.Identifier}, {this.HomId}, "
                + $"'{maintText}')";
            if (maintText == string.Empty) return;
            SqlCommand sqlCommand = new SqlCommand(sql, Header.SqlConnection);
            sqlCommand.ExecuteNonQuery();
            IDataSourc.Load(this.Diary);
            Close();
        }

        private void maintenance_KeyDown(object sender, System.Windows.Input
            .KeyEventArgs e) { if (e.Key == System.Windows
                .Input.Key.Enter)
            {
                int index = maintenance.SelectionStart;
                string text = maintenance.Text;
                maintenance.Text = text.Substring(0, index) + Environment
                    .NewLine + text.Substring(index);
                maintenance.SelectionStart = text.Length + 1;
            }
        }
    }
}