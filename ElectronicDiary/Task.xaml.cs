using System.Windows;

namespace ElectronicDiary
{
    /// <summary>
    /// Логика взаимодействия для Task.xaml
    /// </summary>
    public partial class Task : Window
    {
        public Task(string duty)
        {
            InitializeComponent();
            assignment.Text = duty;
        }
    }
}