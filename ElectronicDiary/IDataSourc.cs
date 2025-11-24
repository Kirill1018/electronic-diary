using Microsoft.Data.SqlClient;
using System.Data;

namespace ElectronicDiary
{
    internal interface IDataSourc
    {
        public static string SelGroupIdFromUs(int? identifier) => $"select groupId from users where Id = {identifier}";
        public static void Add(List<Subject> subjects, List<string> names,
            IDataReader dataReader)
        {
            Subject subject = new Subject(dataReader.GetInt32(0), dataReader.GetString(1));
            subjects.Add(subject);
            names.Add(subject.Name!);
        }
        public static void SelFromLess(Homework work)
        {
            string query = $"select * from lessons where Id = {work.GetLessId()}";
            IDbCommand iDbCommand = new SqlCommand(query, Header.SqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read())
            {
                work.SetSubjId(iDataReader.GetInt32(2));
                DateTime dateTime = iDataReader.GetDateTime(3);
                work.DateTimeOfLess = $"{dateTime.Day}.{dateTime
                    .Month}.{dateTime.Year}";
                work.Theme = iDataReader.GetString(4);
            }
            iDataReader.Close();
        }
        public static void SelFromSubj(string schedule, Homework work)
        {
            string query = $"select * from {schedule} where Id = {work.GetSubjId()}";
            IDbCommand iDbCommand = new SqlCommand(query, Header.SqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read()) work.SetUserId((int)iDataReader["userId"]);
            iDataReader.Close();
        }
        public static void SelFromUs(int? identifier, Homework work)
        {
            string query = $"select * from users where Id = {identifier}";
            IDbCommand iDbCommand = new SqlCommand(query, Header.SqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read()) work.Teacher = iDataReader
                    .GetString(1);
            iDataReader.Close();
        }
    }
}