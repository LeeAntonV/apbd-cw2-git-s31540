using CW2.Domain;
using CW2.Services;

namespace CW2;

internal class Program
{
    static void Main(string[] args)
    {
        var rentalService = new RentalService();

        var student1 = new Student("John", "Doe");
        var student2 = new Student("John", "Smith");
        var employee1 = new Employee("John", "Smith");

        PrintResult(rentalService.AddUser(student1));
        PrintResult(rentalService.AddUser(student2));
        PrintResult(rentalService.AddUser(employee1));

        Console.WriteLine();

        var laptop1 = new Laptop("Dell Latitude 5440", "Office laptop", "Dell", 16, "Intel i7");
        var laptop2 = new Laptop("Lenovo ThinkPad T14", "Programming laptop", "Lenovo", 32, "Ryzen 7");
        var projector1 = new Projector("Epson EB-FH06", "Lecture hall projector", 3500, "1920x1080", "SuperTech");
        var camera1 = new Camera("Canon EOS 250D", "DSLR camera", "Canon", 24.1, true);

        PrintResult(rentalService.AddEquipment(laptop1));
        PrintResult(rentalService.AddEquipment(laptop2));
        PrintResult(rentalService.AddEquipment(projector1));
        PrintResult(rentalService.AddEquipment(camera1));

        Console.WriteLine();
        Console.WriteLine("=== FULL EQUIPMENT LIST ===");
        PrintEquipment(rentalService.GetAllEquipment());

        Console.WriteLine();
        Console.WriteLine("=== ONLY AVAILABLE EQUIPMENT ===");
        PrintEquipment(rentalService.GetAvailableEquipment());

        Console.WriteLine();
        Console.WriteLine("=== CORRECT RENTAL OPERATION ===");
        var rent1 = rentalService.RentEquipment(student1.Id, laptop1.Id, 5, new DateTime(2026, 3, 1));
        PrintResult(rent1);

        Console.WriteLine();
        Console.WriteLine("=== INVALID OPERATION: RENTING ALREADY RENTED EQUIPMENT ===");
        var invalidRent1 = rentalService.RentEquipment(employee1.Id, laptop1.Id, 3, new DateTime(2026, 3, 1));
        PrintResult(invalidRent1);

        Console.WriteLine();
        Console.WriteLine("=== INVALID OPERATION: EXCEEDING STUDENT LIMIT ===");
        var rent2 = rentalService.RentEquipment(student1.Id, laptop2.Id, 4, new DateTime(2026, 3, 2));
        var rent3 = rentalService.RentEquipment(student1.Id, projector1.Id, 3, new DateTime(2026, 3, 2));

        PrintResult(rent2);
        PrintResult(rent3);

        Console.WriteLine();
        Console.WriteLine("=== MARK EQUIPMENT AS UNAVAILABLE ===");
        PrintResult(rentalService.MarkEquipmentUnavailable(camera1.Id));

        Console.WriteLine();
        Console.WriteLine("=== INVALID OPERATION: RENTING UNAVAILABLE EQUIPMENT ===");
        var invalidRent2 = rentalService.RentEquipment(employee1.Id, camera1.Id, 2, new DateTime(2026, 3, 2));
        PrintResult(invalidRent2);

        Console.WriteLine();
        Console.WriteLine($"=== ACTIVE RENTALS FOR USER {student1.FirstName} {student1.LastName} ===");
        PrintRentals(rentalService.GetActiveRentalsForUser(student1.Id));

        Console.WriteLine();
        Console.WriteLine("=== RETURN COMPLETED ON TIME ===");
        PrintResult(rentalService.ReturnEquipment(1, new DateTime(2026, 3, 6)));

        Console.WriteLine();
        Console.WriteLine("=== DELAYED RETURN WITH PENALTY ===");

        var employeeLaptop = new Laptop("HP ProBook 450", "Employee laptop", "HP", 16, "Intel i5");
        PrintResult(rentalService.AddEquipment(employeeLaptop));

        var lateRental = rentalService.RentEquipment(employee1.Id, employeeLaptop.Id, 3, new DateTime(2026, 3, 1));
        PrintResult(lateRental);

        PrintResult(rentalService.ReturnEquipment(3, new DateTime(2026, 3, 8)));

        Console.WriteLine();
        Console.WriteLine("=== OVERDUE RENTALS ON 2026-03-10 ===");
        PrintRentals(rentalService.GetOverdueRentals(new DateTime(2026, 3, 10)));

        Console.WriteLine();
        Console.WriteLine("=== FINAL SUMMARY REPORT ===");
        Console.WriteLine(rentalService.GenerateSummaryReport(new DateTime(2026, 3, 10)));

        Console.WriteLine();
        Console.WriteLine("=== FINAL EQUIPMENT STATE ===");
        PrintEquipment(rentalService.GetAllEquipment());
    }

    private static void PrintResult(OperationResult result)
    {
        Console.WriteLine(result);
    }

    private static void PrintEquipment(IEnumerable<Equipment> equipmentList)
    {
        foreach (var equipment in equipmentList)
        {
            Console.WriteLine(
                $"Id: {equipment.Id}, " +
                $"Name: {equipment.Name}, " +
                $"Status: {equipment.status}, " +
                $"Added: {equipment.AddedDate:yyyy-MM-dd}");
        }
    }

    private static void PrintRentals(IEnumerable<Rental> rentals)
    {
        bool any = false;

        foreach (var rental in rentals)
        {
            Console.WriteLine(rental);
            any = true;
        }

        if (!any)
        {
            Console.WriteLine("No rentals found.");
        }
    }
}