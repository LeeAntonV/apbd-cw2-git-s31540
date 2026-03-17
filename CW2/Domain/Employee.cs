using CW2.Enum;

namespace CW2.Domain;

public class Employee : User
{
    public Employee(string firstName, string lastName)
        : base(firstName, lastName, UserType.Employee)
    {
    }
}