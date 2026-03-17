using CW2.Enum;

namespace CW2.Domain;

public abstract class User
{
    public static int _nextId = 1;
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public UserType UserType { get; }

    protected User(string id, string firstName, string lastName, UserType userType)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        UserType = userType;
    }
}