namespace ElectronicDiary
{
    internal class Homework
    {
        int Id { get; set; }
        string? Task { get; set; }
        int? LessId { get; set; }
        DateTime? Deadline { get; set; } = new DateTime();
        int? SubjId { get; set; }
        public string? DateTimeOfLess { get; set; }
        public string? Theme { get; set; }
        int? UserId { get; set; }
        public string? Teacher { get; set; }
        public int? Mark { get; set; }
        public string? Comment { get; set; }
        public Homework(int id, string? task,
            int? lessId, DateTime? deadline,
            int? subjId, string? dateTimeOfLess,
            string? theme, int? userId,
            string? teacher, int? mark,
            string? comment)
        {
            Id = id;
            Task = task;
            LessId = lessId;
            Deadline = deadline;
            SubjId = subjId;
            DateTimeOfLess = dateTimeOfLess;
            Theme = theme;
            UserId = userId;
            Teacher = teacher;
            Mark = mark;
            Comment = comment;
        }
        public int GetId() => this.Id;
        public string? GetTask() => this.Task;
        public void SetTask(string? task) => this.Task = task;
        public int? GetLessId() => this.LessId;
        public void SetLessId(int? lessId) => this.LessId = lessId;
        public DateTime? GetDeadl() => this.Deadline;
        public int? GetSubjId() => this.SubjId;
        public void SetSubjId(int subjId) => this.SubjId = subjId;
        public int? GetUserId() => this.UserId;
        public void SetUserId(int? userId) => this.UserId = userId;
    }
}