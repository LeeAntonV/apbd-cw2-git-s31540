using CW2.Enum;

namespace CW2.Domain;

public abstract class User
{
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserType UserType { get; }

    protected User(string firstName, string lastName, UserType userType)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        UserType = userType;
    }

    public override string ToString()
    {
        return $"{UserType} | Id: {Id} | {FirstName} {LastName}";
    }
}