using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Windows.Controls;

namespace ElectronicDiary
{
    /// <summary>
    /// Логика взаимодействия для Diary.xaml
    /// </summary>
    public partial class Diary : Page
    {
        public int? Identifier { get; set; }
        public Diary(int? identifier)
        {
            InitializeComponent();
            this.Identifier = identifier;
            Header.Load(this);
        }

        private void OnClickChosTask(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Homework homework = (Homework)homNeedToBeSubm.SelectedItem;
                Task task = new Task(homework.GetTask()!);
                task.Show();
            }
            catch (NullReferenceException) { }
        }

        private void OnClickUplChosHomForCheck(object sender, System.Windows.RoutedEventArgs e)
        {
            Homework? homework = (Homework?)homNeedToBeSubm.SelectedItem;
            if (homework is null) return;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SqlConnection sqlConnection = Header.SqlConnection;
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                string sql = "insert into checking(userId, homId, "
                    + $"binFile, lodgeName) values({this.Identifier}, {homework.GetId()}, "
                    + $"@file, '{openFileDialog.SafeFileName}')";
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection,
                    transaction);
                sqlCommand.Parameters.Add("@file", SqlDbType
                    .VarBinary).Value = File.ReadAllBytes(openFileDialog
                    .FileName);
                sqlCommand.ExecuteNonQuery();
                transaction.Commit();
                Header.Load(this);
            }
        }

        private void OnClickComplChosHom(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Homework homework = (Homework)homNeedToBeSubm.SelectedItem;
                Work work = new Work(homework.GetId(), this);
                work.Show();
            }
            catch (NullReferenceException) { }
        }
    }
}