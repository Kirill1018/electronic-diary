using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media
    .Imaging;
using System.Windows.Shapes;

namespace ElectronicDiary
{
    /// <summary>
    /// Логика взаимодействия для Work.xaml
    /// </summary>
    public partial class Work : Window
    {
        Homework Act { get; set; } = new Homework(null, null,
            null, null,
            null, null);
        public Work(Homework act)
        {
            InitializeComponent();
            this.Act = act;
            workName.Text = this.Act
                .Task;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = workCont.Text;
            if (text == String.Empty) return;
            string query = $"insert into checking(name, content) values('{this.Act.Task}', '{text}')";
            SqlCommand cmd = new SqlCommand(query, Header.Conn);
            cmd.ExecuteNonQuery();
            Close();
        }
    }
}