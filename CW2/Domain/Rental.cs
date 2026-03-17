namespace CW2.Domain;

public class Rental
{
    private static int _nextId = 1;

    public int Id { get; }
    public User User { get; }
    public Equipment Equipment { get; }
    public DateTime RentalDate { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnDate { get; private set; }
    public decimal Penalty { get; private set; }

    public bool IsActive => ReturnDate == null;
    public bool IsReturned => ReturnDate != null;
    public bool WasReturnedOnTime => ReturnDate != null && ReturnDate.Value.Date <= DueDate.Date;

    public Rental(User user, Equipment equipment, DateTime rentalDate, int rentalDays)
    {
        Id = _nextId++;
        User = user;
        Equipment = equipment;
        RentalDate = rentalDate;
        DueDate = rentalDate.AddDays(rentalDays);
        ReturnDate = null;
        Penalty = 0;
    }

    public bool IsOverdue(DateTime currentDate)
    {
        return IsActive && currentDate.Date > DueDate.Date;
    }

    public void ReturnEquipment(DateTime returnDate, decimal penalty)
    {
        if (IsReturned)
        {
            throw new InvalidOperationException("This rental has already been returned.");
        }

        ReturnDate = returnDate;
        Penalty = penalty;
    }

    public override string ToString()
    {
        string returnText = ReturnDate.HasValue
            ? ReturnDate.Value.ToString("yyyy-MM-dd")
            : "Not returned";

        return $"Rental Id: {Id}, User: {User.FirstName} {User.LastName}, Equipment: {Equipment.Name}, " +
               $"Rental Date: {RentalDate:yyyy-MM-dd}, Due Date: {DueDate:yyyy-MM-dd}, " +
               $"Return Date: {returnText}, Penalty: {Penalty}";
    }
}