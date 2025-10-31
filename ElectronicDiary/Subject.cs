namespace ElectronicDiary
{
    internal class Subject
    {
        int Id { get; set; }
        public string? Name { get; set; }
        public Subject(int id, string? name)
        {
            Id = id;
            Name = name;
        }
        public int GetId() => this.Id;
    }
}