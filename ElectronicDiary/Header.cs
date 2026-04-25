namespace ElectronicDiary
{
    internal class Header
    {
        public static DataClassesDataContext Db { get; } = new DataClassesDataContext();
        public static void Load(Diary diary)
        {
            List<users> users = Db.users.ToList<users>();
            int? identifier = diary.Identifier;
            users? user = users.Find(customer => customer.Id == identifier);
            if (user is null) return;
            int? groupId = user.groupId;
            List<passSubj> passSubj = Db.passSubj.ToList<passSubj>(), passSubjByGroup = passSubj
                .FindAll(subjects => (subjects.groupId == groupId && subjects.isArch == false));
            List<string> namOfPassSubj = new List<string>(), namOfCurrSubj = new List<string>();
            foreach (passSubj subject in passSubjByGroup) namOfPassSubj.Add(subject.name);
            diary.endSubj.ItemsSource = namOfPassSubj;
            List<currSubj> currSubj = Db.currSubj.ToList<currSubj>(), currSubjByGroup = currSubj
                .FindAll(subjects => subjects.groupId == groupId);
            foreach (currSubj subject in currSubjByGroup) namOfCurrSubj.Add(subject.name);
            diary.actSubj.ItemsSource = namOfCurrSubj;
            List<lessons> lessons = Db.lessons.ToList<lessons>();
            List<Lesson> tutList = new List<Lesson>();
            foreach (passSubj subject in passSubjByGroup)
            {
                List<lessons> lessonsBySubj = lessons.FindAll(tutorial => (tutorial.isPass && tutorial
                .subjId == subject.Id));
                IDataSourc.Add(lessonsBySubj, tutList);
            }
            foreach (currSubj subject in currSubjByGroup)
            {
                List<lessons> lessonsBySubj = lessons.FindAll(tutorial => (tutorial.isPass && tutorial
                .subjId == subject.Id));
                IDataSourc.Add(lessonsBySubj, tutList);
            }
            foreach (Lesson lesson in tutList)
            {
                int subjId = lesson.GetSubjId();
                passSubj? endSubj = passSubj.Find(subject => subject.Id == subjId);
                if (endSubj is null)
                {
                    currSubj? actSubj = currSubj.Find(subject => subject.Id == subjId);
                    users? client = actSubj is null ? null : users.Find(customer => customer.Id == actSubj
                    .userId);
                    lesson.Teacher = client is null ? null : client.username;
                }
                else
                {
                    users? client = users.Find(customer => customer.Id == endSubj
                    .userId);
                    lesson.Teacher = client is null ? null : client.username;
                }
            }
            List<marks> marks = Db.marks.ToList<marks>();
            foreach (Lesson lesson in tutList)
            {
                marks? mark = marks.Find(rating => (rating.lessId == lesson
                .GetId() && rating.userId == identifier));
                lesson.Mark = mark is null ? null : mark.number;
            }
            diary.tutorials.ItemsSource = tutList;
            List<checking> checkings = Db.checking.ToList<checking>(), checkByUs = checkings
                .FindAll(homework => homework.userId == identifier);
            List<Homework> works = new List<Homework>(), worksWithPosMark = new List<Homework>(),
                worksWithBadMark = new List<Homework>(), worksNeedToBeSubm = new List<Homework>();
            foreach (checking checking in checkings)
            {
                Homework homework = new Homework(checking.homId, checking.binFile,
                    checking.content, null,
                    null, null,
                    null, null,
                    null, null,
                    null, checking.mark,
                    checking.comment);
                works.Add(homework);
            }
            List<homeworks> homeworks = Db.homeworks.ToList<homeworks>();
            foreach (Homework homework in works)
            {
                homeworks? work = homeworks.Find(task => task.Id == homework
                .GetId());
                homework.SetTask(work is null ? null : work.task);
                homework.SetLessId(work is null ? null : work.lessId);
            }
            foreach (Homework homework in works)
            {
                try { IDataSourc.SelFromLess(lessons, homework); }
                catch (NullReferenceException) { }
            }
            foreach (Homework homework in works) IDataSourc.SelFromSubj(homework, passSubj,
                currSubj);
            foreach (Homework homework in works) IDataSourc.SelFromUs(users, homework);
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
                List<homeworks> tasks = homeworks.FindAll(homework => homework.lessId == lesson
                .GetId());
                foreach (homeworks task in tasks)
                {
                    Homework homework = new Homework(task.Id, null,
                        null, task.task,
                        lesson.GetId(), task.deadline,
                        null, null,
                        null, null,
                        null, null,
                        null);
                    worksNeedToBeSubm.Add(homework);
                }
            }
            foreach (Homework homework in works) for (int i = 0; i < worksNeedToBeSubm.Count; i++) if (homework.GetId() == worksNeedToBeSubm[i]
                        .GetId())
                    {
                        worksNeedToBeSubm.RemoveAt(i);
                        i--;
                    }
            foreach (Homework homework in worksNeedToBeSubm)
            {
                try { IDataSourc.SelFromLess(lessons, homework); }
                catch (NullReferenceException) { }
            }
            foreach (Homework homework in worksNeedToBeSubm) IDataSourc.SelFromSubj(homework, passSubj,
                currSubj);
            foreach (Homework homework in worksNeedToBeSubm) IDataSourc.SelFromUs(users, homework);
            for (int i = 0; i < worksNeedToBeSubm.Count; i++) if (worksNeedToBeSubm[i].GetDeadl() < DateTime
                    .Now)
                {
                    worksNeedToBeSubm.RemoveAt(i);
                    i--;
                }
            for (int i = 0; i < worksNeedToBeSubm.Count; i++) foreach (checking checking in checkings) if (checking.userId == identifier && checking
                        .homId == worksNeedToBeSubm[i].GetId())
                    {
                        worksNeedToBeSubm.RemoveAt(i);
                        i--;
                    }
            diary.homNeedToBeSubm.ItemsSource = worksNeedToBeSubm;
        }
    }
}