using System.Windows;

namespace ElectronicDiary
{
    /// <summary>
    /// Логика взаимодействия для Work.xaml
    /// </summary>
    public partial class Work : Window
    {
        Diary Diary { get; set; }
        int HomId { get; set; }
        public Work(int homId, Diary diary)
        {
            InitializeComponent();
            this.Diary = diary;
            this.HomId = homId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string? maintText = maintenance.Text;
            if (maintText == string.Empty) return;
            checking checking = new checking();
            checking.userId = this.Diary.Identifier;
            checking.homId = this.HomId;
            checking.content = maintText;
            Header.Db.checking
                .InsertOnSubmit(checking);
            Header.Db.SubmitChanges();
            Header.Load(this.Diary);
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