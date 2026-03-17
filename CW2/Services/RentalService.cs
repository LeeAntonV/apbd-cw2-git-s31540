using CW2.Domain;
using CW2.Enum;

namespace CW2.Services;

public class RentalService
{
    private readonly List<User> _users = new();
    private readonly List<Equipment> _equipments = new();
    private readonly List<Rental> _rentals = new();

    private const int StudentLimit = 2;
    private const int EmployeeLimit = 5;
    private const decimal DailyPenalty = 10m;

    public OperationResult AddUser(User user)
    {
        _users.Add(user);
        return OperationResult.Ok($"User added: {user}");
    }

    public OperationResult AddEquipment(Equipment equipment)
    {
        _equipments.Add(equipment);
        return OperationResult.Ok($"Equipment added: {equipment.Name} (Id: {equipment.Id})");
    }

    public IEnumerable<Equipment> GetAllEquipment()
    {
        return _equipments;
    }

    public IEnumerable<Equipment> GetAvailableEquipment()
    {
        return _equipments.Where(e => e.status == Status.Available);
    }

    public IEnumerable<Rental> GetActiveRentalsForUser(int userId)
    {
        return _rentals.Where(r => r.User.Id == userId && r.IsActive);
    }

    public IEnumerable<Rental> GetOverdueRentals(DateTime currentDate)
    {
        return _rentals.Where(r => r.IsOverdue(currentDate));
    }

    public OperationResult MarkEquipmentUnavailable(int equipmentId)
    {
        var equipment = _equipments.FirstOrDefault(e => e.Id == equipmentId);

        if (equipment == null)
            return OperationResult.Fail("Equipment not found.");

        if (equipment.status == Status.Rented)
            return OperationResult.Fail("Cannot mark rented equipment as unavailable.");

        equipment.status = Status.Unavailable;
        return OperationResult.Ok($"Equipment {equipment.Name} marked as unavailable.");
    }

    public OperationResult RentEquipment(int userId, int equipmentId, int rentalDays, DateTime rentalDate)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return OperationResult.Fail("User not found.");

        var equipment = _equipments.FirstOrDefault(e => e.Id == equipmentId);
        if (equipment == null)
            return OperationResult.Fail("Equipment not found.");

        if (equipment.status != Status.Available)
            return OperationResult.Fail("Equipment is not available.");

        int activeRentals = _rentals.Count(r => r.User.Id == userId && r.IsActive);
        int limit = GetUserLimit(user);

        if (activeRentals >= limit)
            return OperationResult.Fail($"User exceeded rental limit ({limit}).");

        var rental = new Rental(user, equipment, rentalDate, rentalDays);

        _rentals.Add(rental);
        equipment.status = Status.Rented;

        return OperationResult.Ok($"Rental created (Id: {rental.Id})");
    }

    public OperationResult ReturnEquipment(int rentalId, DateTime returnDate)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId);

        if (rental == null)
            return OperationResult.Fail("Rental not found.");

        if (!rental.IsActive)
            return OperationResult.Fail("Rental already returned.");

        decimal penalty = CalculatePenalty(rental.DueDate, returnDate);

        rental.ReturnEquipment(returnDate, penalty);
        rental.Equipment.status = Status.Available;

        return OperationResult.Ok($"Returned. Penalty: {penalty}");
    }

    public string GenerateSummaryReport(DateTime currentDate)
    {
        return
            "=== SUMMARY ===\n" +
            $"Users: {_users.Count}\n" +
            $"Equipment: {_equipments.Count}\n" +
            $"Available: {_equipments.Count(e => e.status == Status.Available)}\n" +
            $"Rented: {_equipments.Count(e => e.status == Status.Rented)}\n" +
            $"Unavailable: {_equipments.Count(e => e.status == Status.Unavailable)}\n" +
            $"Active rentals: {_rentals.Count(r => r.IsActive)}\n" +
            $"Overdue: {_rentals.Count(r => r.IsOverdue(currentDate))}\n" +
            $"Penalties: {_rentals.Sum(r => r.Penalty)}";
    }

    private int GetUserLimit(User user)
    {
        return user.UserType switch
        {
            UserType.Student => StudentLimit,
            UserType.Employee => EmployeeLimit,
            _ => 0
        };
    }

    private decimal CalculatePenalty(DateTime dueDate, DateTime returnDate)
    {
        int lateDays = (returnDate.Date - dueDate.Date).Days;

        if (lateDays <= 0)
            return 0;

        return lateDays * DailyPenalty;
    }

    // JSON support
    public List<User> GetUsers() => _users;
    public List<Equipment> GetEquipment() => _equipments;
    public List<Rental> GetRentals() => _rentals;

    public void LoadData(List<User> users, List<Equipment> equipment, List<Rental> rentals)
    {
        _users.Clear();
        _users.AddRange(users);

        _equipments.Clear();
        _equipments.AddRange(equipment);

        _rentals.Clear();
        _rentals.AddRange(rentals);
    }
}