using CW2.Domain;
using CW2.Enum;
using CW2.Services;

namespace CW2.Domain;

public class RentalService
{
    private readonly List<User> _users = new();
    private readonly List<Equipment> _equipments = new();
    private readonly List<Rental> _rentals = new();

    private const int StudentLimit = 2;
    private const int EmployeeLimit = 5;
    private const decimal DailyPenalty = 10m;

    public IReadOnlyList<User> Users => _users;
    public IReadOnlyList<Equipment> Equipments => _equipments;
    public IReadOnlyList<Rental> Rentals => _rentals;

    public OperationResult AddUser(User user)
    {
        _users.Add(user);
        return OperationResult.Ok($"User added: {user.FirstName} {user.LastName} (Id: {user.Id})");
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
        {
            return OperationResult.Fail($"Equipment with id {equipmentId} not found.");
        }

        if (equipment.status == Status.Rented)
        {
            return OperationResult.Fail("Cannot mark rented equipment as unavailable.");
        }

        equipment.status = Status.Unavailable;
        return OperationResult.Ok($"Equipment {equipment.Name} marked as unavailable.");
    }

    public OperationResult RentEquipment(int userId, int equipmentId, int rentalDays, DateTime rentalDate)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return OperationResult.Fail($"User with id {userId} not found.");
        }

        var equipment = _equipments.FirstOrDefault(e => e.Id == equipmentId);
        if (equipment == null)
        {
            return OperationResult.Fail($"Equipment with id {equipmentId} not found.");
        }

        if (equipment.status != Status.Available)
        {
            return OperationResult.Fail($"Equipment {equipment.Name} is not available.");
        }

        int activeRentals = _rentals.Count(r => r.User.Id == userId && r.IsActive);
        int limit = GetUserLimit(user);

        if (activeRentals >= limit)
        {
            return OperationResult.Fail(
                $"User {user.FirstName} {user.LastName} exceeded rental limit ({limit}).");
        }

        var rental = new Rental(user, equipment, rentalDate, rentalDays);
        _rentals.Add(rental);
        equipment.status = Status.Rented;

        return OperationResult.Ok(
            $"Rental created. Rental Id: {rental.Id}, User: {user.FirstName} {user.LastName}, Equipment: {equipment.Name}");
    }

    public OperationResult ReturnEquipment(int rentalId, DateTime returnDate)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId);

        if (rental == null)
        {
            return OperationResult.Fail($"Rental with id {rentalId} not found.");
        }

        if (!rental.IsActive)
        {
            return OperationResult.Fail("This rental is already closed.");
        }

        decimal penalty = CalculatePenalty(rental.DueDate, returnDate);
        rental.ReturnEquipment(returnDate, penalty);
        rental.Equipment.status = Status.Available;

        return OperationResult.Ok(
            $"Equipment returned. Rental Id: {rental.Id}, Penalty: {penalty}");
    }

    public string GenerateSummaryReport(DateTime currentDate)
    {
        int totalEquipment = _equipments.Count;
        int availableEquipment = _equipments.Count(e => e.status == Status.Available);
        int rentedEquipment = _equipments.Count(e => e.status == Status.Rented);
        int unavailableEquipment = _equipments.Count(e => e.status == Status.Unavailable);

        int totalUsers = _users.Count;
        int totalRentals = _rentals.Count;
        int activeRentals = _rentals.Count(r => r.IsActive);
        int overdueRentals = _rentals.Count(r => r.IsOverdue(currentDate));
        decimal totalPenalties = _rentals.Sum(r => r.Penalty);

        return
            "=== SUMMARY REPORT ===\n" +
            $"Users: {totalUsers}\n" +
            $"Equipment total: {totalEquipment}\n" +
            $"Available: {availableEquipment}\n" +
            $"Rented: {rentedEquipment}\n" +
            $"Unavailable: {unavailableEquipment}\n" +
            $"Rentals total: {totalRentals}\n" +
            $"Active rentals: {activeRentals}\n" +
            $"Overdue rentals: {overdueRentals}\n" +
            $"Collected penalties: {totalPenalties}";
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
        {
            return 0;
        }

        return lateDays * DailyPenalty;
    }
}