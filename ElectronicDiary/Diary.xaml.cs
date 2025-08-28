using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicDiary
{
    /// <summary>
    /// Логика взаимодействия для Diary.xaml
    /// </summary>
    public partial class Diary : Page
    {
        public Diary()
        {
            InitializeComponent();
            SqlConnection conn = Header.Conn;
            string sql = "select * from passSubj";
            IDbCommand iDbCommand = new SqlCommand(sql, conn);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            List<Subject> subjects1 = new List<Subject>(), subjects2 = new List<Subject>();
            while (iDataReader.Read()) AddSubj(iDataReader, subjects1);
            iDataReader.Close();
            endSubj.ItemsSource = subjects1;
            sql = "select * from currSubj";
            iDbCommand = new SqlCommand(sql, conn);
            iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read()) AddSubj(iDataReader, subjects2);
            iDataReader.Close();
            actSubj.ItemsSource = subjects2;
            sql = "select * from homeworks, statuses where homeworks.status_id = statuses.Id and statuses.name = 'домашка сдана на положительную оценку'";
            iDbCommand = new SqlCommand(sql, conn);
            iDataReader = iDbCommand.ExecuteReader();
            List<Homework> homeworks1 = new List<Homework>(), homeworks2 = new List<Homework>(),
                homeworks3 = new List<Homework>();
            while (iDataReader.Read()) AddHom(iDataReader, homeworks1);
            iDataReader.Close();
            homWithPosMark.ItemsSource = homeworks1;
            sql = "select * from homeworks, statuses where homeworks.status_id = statuses.Id and statuses.name = 'домашка сдана на плохую оценку, требуется пересдать'";
            iDbCommand = new SqlCommand(sql, conn);
            iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read()) AddHom(iDataReader, homeworks2);
            iDataReader.Close();
            homWithBadMark.ItemsSource = homeworks2;
            sql = "select * from homeworks, statuses where homeworks.status_id = statuses.Id and statuses.name = 'домашка не сдана, но срок ещё не подошёл'";
            iDbCommand = new SqlCommand(sql, conn);
            iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read()) AddHom(iDataReader, homeworks3);
            iDataReader.Close();
            homWhichMustBeSubm.ItemsSource = homeworks3;
        }
        static void AddSubj(IDataReader dataReader, List<Subject> list)
        {
            DateTime dateTime = dataReader.GetDateTime(2);
            Subject subject = new Subject(dataReader.GetString(1), $"{dateTime.Month}.{dateTime.Day}.{dateTime.Year}",
                dataReader.GetString(3), dataReader.GetString(4),
                dataReader.GetInt32(5));
            list.Add(subject);
        }
        static void AddHom(IDataReader dataReader, List<Homework> list)
        {
            DateTime dateTime = dataReader.GetDateTime(1);
            DBNull dBNull = DBNull.Value;
            int? assessment = (dataReader["mark"] == dBNull) ? null : (int?)dataReader["mark"];
            string? comment = (dataReader["comment"] == dBNull) ? null : (string?)dataReader["comment"];
            Homework homework = new Homework($"{dateTime.Month}.{dateTime.Day}.{dateTime.Year}", dataReader.GetString(2),
                dataReader.GetString(3), dataReader.GetString(4),
                assessment, comment);
            list.Add(homework);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Homework homework = (Homework)homWhichMustBeSubm.SelectedItem;
                Work work = new Work(homework);
                work.Show();
            }
            catch (NullReferenceException) { }
        }
    }
}