using CW2.Enum;

namespace CW2.Domain;

public class Student : User
{
    public Student(string firstName, string lastName)
        : base(firstName, lastName, UserType.Student)
    {
    }
}