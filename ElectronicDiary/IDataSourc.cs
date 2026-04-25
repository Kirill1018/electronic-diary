namespace ElectronicDiary
{
    internal interface IDataSourc
    {
        public static void Add(List<lessons> lessonsBySubj, List<Lesson> lessons) { foreach (lessons lesson in lessonsBySubj)
            {
                DateTime dateTime = lesson.date;
                Lesson tutorial = new Lesson(lesson.Id, lesson.subjId,
                    $"{dateTime.Day}.{dateTime.Month}.{dateTime
                    .Year}", lesson.theme,
                    null, null);
                lessons.Add(tutorial);
            }
        }
        public static void SelFromLess(List<lessons> lessons, Homework homework)
        {
            lessons? lesson = lessons.Find(tutorial => tutorial.Id == homework
                                    .GetLessId());
            homework.SetSubjId(lesson!.subjId);
            DateTime dateTime = lesson.date;
            homework.DateTimeOfLess = $"{dateTime.Day}.{dateTime
                .Month}.{dateTime.Year}";
            homework.Theme = lesson.theme;
        }
        public static void SelFromSubj(Homework homework, List<passSubj> passSubj,
            List<currSubj> currSubj)
        {
            int? subjId = homework.GetSubjId();
            passSubj? endSubj = passSubj.Find(subject => subject.Id == subjId);
            if (endSubj is null)
            {
                currSubj? actSubj = currSubj.Find(subject => subject.Id == subjId);
                homework.SetUserId(actSubj is null ? null : actSubj.userId);
            }
            else homework.SetUserId(endSubj.userId);
        }
        public static void SelFromUs(List<users> users, Homework homework)
        {
            users? client = users.Find(customer => customer.Id == homework
                .GetUserId());
            homework.Teacher = client is null ? null : client.username;
        }
    }
}