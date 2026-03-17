namespace CW2.Domain;

public class Rental
{
    private static int _nextId = 1;

    public int Id { get; set; }
    public User User { get; set; } = null!;
    public Equipment Equipment { get; set; } = null!;
    public DateTime RentalDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public decimal Penalty { get; set; }

    public bool IsActive => ActualReturnDate == null;
    public bool IsReturned => ActualReturnDate != null;
    public bool WasReturnedOnTime => ActualReturnDate != null && ActualReturnDate.Value.Date <= DueDate.Date;

    public Rental()
    {
    }

    public Rental(User user, Equipment equipment, DateTime rentalDate, int rentalDays)
    {
        Id = _nextId++;
        User = user;
        Equipment = equipment;
        RentalDate = rentalDate;
        DueDate = rentalDate.AddDays(rentalDays);
        ActualReturnDate = null;
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

        ActualReturnDate = returnDate;
        Penalty = penalty;
    }

    public static void SetNextId(int nextId)
    {
        if (nextId > _nextId)
        {
            _nextId = nextId;
        }
    }

    public override string ToString()
    {
        string returnText = ActualReturnDate.HasValue
            ? ActualReturnDate.Value.ToString("yyyy-MM-dd")
            : "Not returned";

        return $"Rental Id: {Id}, User: {User.FirstName} {User.LastName}, Equipment: {Equipment.Name}, " +
               $"Rental Date: {RentalDate:yyyy-MM-dd}, Due Date: {DueDate:yyyy-MM-dd}, " +
               $"Return Date: {returnText}, Penalty: {Penalty}";
    }
}