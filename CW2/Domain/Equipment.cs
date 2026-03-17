using CW2.Enum;

namespace CW2.Domain;

public abstract class Equipment
{
    private static int _nextId = 1;
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime AddedDate { get; set; }
    public Status status { get; set; } = Status.Available;

    public Equipment(string name, string description = "")
    {
        Id = _nextId++;
        Name = name;
        Description = description;
        AddedDate = DateTime.Now;
        status = Status.Available;
    }
}