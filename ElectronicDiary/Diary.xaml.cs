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
        public int Identifier { get; set; }
        public Diary(int identifier)
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
                checking checking = new checking();
                checking.userId = this.Identifier;
                checking.homId = homework.GetId();
                checking.binFile = File.ReadAllBytes(openFileDialog
                    .FileName);
                checking.lodgeName = openFileDialog.SafeFileName;
                Header.Db.checking
                    .InsertOnSubmit(checking);
                Header.Db.SubmitChanges();
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