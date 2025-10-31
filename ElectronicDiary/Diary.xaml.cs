using Microsoft.Data.SqlClient;
using Microsoft.Win32;
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
            IDataSourc.Load(this);
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
                FileStream fstream = File.OpenRead(openFileDialog.FileName);
                byte[] buffer = new byte[fstream.Length];
                fstream.Read(buffer, 0,
                    buffer.Length);
                string sql = "insert into checking(userId, homId, "
                    + $"binFile, lodgeName) values({this.Identifier}, {homework.GetId()}, "
                    + $"convert(varbinary(max), '{buffer}'), '{openFileDialog.SafeFileName}')";
                SqlCommand sqlCommand = new SqlCommand(sql, Header.SqlConnection);
                sqlCommand.ExecuteNonQuery();
                IDataSourc.Load(this);
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