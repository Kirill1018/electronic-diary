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
            List<users> users = Header.Db.users
                .ToList<users>();
            users? client = users.Find(customer => (customer.username == user
            .Text && customer.password == parole.Text
            && customer.isStud));
            if (client is null) return;
            NavigationWindow navigationWindow = new NavigationWindow();
            navigationWindow.Content = new Diary(client.Id);
            navigationWindow.Show();
            Close();
        }
    }
}