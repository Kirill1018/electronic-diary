using Microsoft.Data.SqlClient;
using System.Data;

namespace ElectronicDiary
{
    internal class Header
    {
        public static SqlConnection SqlConnection { get; } = new SqlConnection("Data Source=desktop-neuqaj1\\sqlexpress;Initial Catalog=\"electronic diary\";Integrated Security=True;Trust Server Certificate=True");
        public static void Load(Diary diary)
        {
            string sql = "select * from passSubj where Id = (select Id from passSubj where "
                + $"groupId = ({IDataSourc.SelGroupIdFromUs(diary.Identifier)}) and isArch = 'false')";
            IDbCommand iDbCommand = new SqlCommand(sql, SqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            List<Subject> subjects1 = new List<Subject>(), subjects2 = new List<Subject>();
            List<string> subjNam1 = new List<string>(), subjNam2 = new List<string>();
            while (iDataReader.Read()) IDataSourc.Add(subjects1, subjNam1,
                iDataReader);
            iDataReader.Close();
            diary.endSubj.ItemsSource = subjNam1;
            sql = "select * from currSubj where Id = (select Id from currSubj where "
                + $"groupId = ({IDataSourc.SelGroupIdFromUs(diary.Identifier)}))";
            iDbCommand = new SqlCommand(sql, SqlConnection);
            iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read()) IDataSourc.Add(subjects2, subjNam2,
                iDataReader);
            iDataReader.Close();
            diary.actSubj.ItemsSource = subjNam2;
            List<Subject>[] subjects = { subjects1, subjects2 };
            List<Lesson> tutList = new List<Lesson>();
            foreach (List<Subject> items in subjects) foreach (Subject subject in items)
                {
                    sql = $"select * from lessons where isPass = 'true' and subjId = {subject.GetId()}";
                    iDbCommand = new SqlCommand(sql, SqlConnection);
                    iDataReader = iDbCommand.ExecuteReader();
                    while (iDataReader.Read())
                    {
                        DateTime dateTime = iDataReader.GetDateTime(3);
                        Lesson lesson = new Lesson(iDataReader.GetInt32(0), iDataReader.GetInt32(2),
                            $"{dateTime.Day}.{dateTime.Month}.{dateTime
                            .Year}", iDataReader.GetString(4),
                            null, null);
                        tutList.Add(lesson);
                    }
                    iDataReader.Close();
                }
            string[] tables = { "passSubj", "currSubj" };
            foreach (Lesson lesson in tutList) foreach (string table in tables)
                {
                    sql = $"select * from users where Id = (select userId from {table} where Id = {lesson.GetSubjId()})";
                    iDbCommand = new SqlCommand(sql, SqlConnection);
                    iDataReader = iDbCommand.ExecuteReader();
                    while (iDataReader.Read()) lesson.Teacher = iDataReader
                            .GetString(1);
                    iDataReader.Close();
                }
            foreach (Lesson lesson in tutList)
            {
                sql = $"select * from marks where Id = (select Id from marks where lessId = {lesson.GetId()} and userId = {diary.Identifier})";
                iDbCommand = new SqlCommand(sql, SqlConnection);
                iDataReader = iDbCommand.ExecuteReader();
                while (iDataReader.Read()) lesson.Mark = iDataReader
                        .GetInt32(1);
                iDataReader.Close();
            }
            diary.tutorials.ItemsSource = tutList;
            sql = $"select * from checking where userId = {diary.Identifier}";
            iDbCommand = new SqlCommand(sql, SqlConnection);
            iDataReader = iDbCommand.ExecuteReader();
            List<Homework> works = new List<Homework>(), worksWithPosMark = new List<Homework>(),
                worksWithBadMark = new List<Homework>(), worksNeedToBeSubm = new List<Homework>();
            while (iDataReader.Read())
            {
                DBNull dBNull = DBNull.Value;
                byte[]? binFile = (iDataReader["binFile"] == dBNull) ? null : (byte[]?)iDataReader["binFile"];
                string? content = (iDataReader["content"] == dBNull) ? null : iDataReader.GetString(5), comment = (iDataReader["comment"] == dBNull) ? null : iDataReader.GetString(6);
                int? mark = (iDataReader["mark"] == dBNull) ? null : iDataReader.GetInt32(7);
                Homework homework = new Homework(iDataReader.GetInt32(2), binFile,
                    content, null,
                    null, null,
                    null, null,
                    null, null,
                    null, mark,
                    comment);
                works.Add(homework);
            }
            iDataReader.Close();
            foreach (Homework homework in works)
            {
                sql = $"select * from homeworks where Id = {homework.GetId()}";
                iDbCommand = new SqlCommand(sql, SqlConnection);
                iDataReader = iDbCommand.ExecuteReader();
                while (iDataReader.Read())
                {
                    homework.SetTask(iDataReader.GetString(1));
                    homework.SetLessId(iDataReader.GetInt32(2));
                }
                iDataReader.Close();
            }
            foreach (Homework homework in works) IDataSourc.SelFromLess(homework);
            foreach (string table in tables) foreach (Homework homework in works) IDataSourc.SelFromSubj(table, homework);
            foreach (string table in tables) foreach (Homework homework in works) IDataSourc.SelFromUs(homework.GetUserId(), homework);
            foreach (Homework homework in works)
            {
                int? mark = homework.Mark;
                if (mark >= 4) worksWithPosMark.Add(homework);
                else if (mark < 4) worksWithBadMark.Add(homework);
            }
            diary.homWithPosMark.ItemsSource = worksWithPosMark;
            diary.homWithBadMark.ItemsSource = worksWithBadMark;
            foreach (Lesson lesson in tutList)
            {
                int lessId = lesson.GetId();
                sql = $"select * from homeworks where lessId = {lessId}";
                iDbCommand = new SqlCommand(sql, SqlConnection);
                iDataReader = iDbCommand.ExecuteReader();
                while (iDataReader.Read())
                {
                    Homework homework = new Homework(iDataReader.GetInt32(0), null,
                        null, iDataReader.GetString(1),
                        lessId, iDataReader.GetDateTime(3),
                        null, null,
                        null, null,
                        null, null,
                        null);
                    worksNeedToBeSubm.Add(homework);
                }
                iDataReader.Close();
            }
            foreach (Homework homework in works) for (int i = 0; i < worksNeedToBeSubm.Count; i++) if (homework.GetId() == worksNeedToBeSubm[i]
                        .GetId()) worksNeedToBeSubm.RemoveAt(i);
            foreach (Homework homework in worksNeedToBeSubm) IDataSourc.SelFromLess(homework);
            foreach (string table in tables) foreach (Homework homework in worksNeedToBeSubm) IDataSourc.SelFromSubj(table, homework);
            foreach (string table in tables) foreach (Homework homework in worksNeedToBeSubm) IDataSourc.SelFromUs(homework.GetUserId(), homework);
            int count = worksNeedToBeSubm.Count;
            for (int i = 0; i < count; i++) if (worksNeedToBeSubm[i].GetDeadl() < DateTime.Now) worksNeedToBeSubm
                        .RemoveAt(i);
            for (int i = 0; i < count; i++)
            {
                sql = $"select * from checking";
                iDbCommand = new SqlCommand(sql, SqlConnection);
                iDataReader = iDbCommand.ExecuteReader();
                while (iDataReader.Read()) if (iDataReader.GetInt32(1) == diary
                        .Identifier && iDataReader.GetInt32(2) == worksNeedToBeSubm[i].GetId()) worksNeedToBeSubm
                            .RemoveAt(i);
                iDataReader.Close();
            }
            diary.homNeedToBeSubm.ItemsSource = worksNeedToBeSubm;
        }
    }
}