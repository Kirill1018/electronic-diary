namespace ElectronicDiary
{
    internal class Lesson
    {
        int Id { get; set; }
        int SubjId { get; set; }
        public string? Date { get; set; }
        public string? Theme { get; set; }
        public string? Teacher { get; set; }
        public int? Mark { get; set; }
        public Lesson(int id, int subjId,
            string? date, string? theme,
            string? teacher, int? mark)
        {
            Id = id;
            SubjId = subjId;
            Date = date;
            Theme = theme;
            Teacher = teacher;
            Mark = mark;
        }
        public int GetId() => this.Id;
        public int GetSubjId() => this.SubjId;
    }
}