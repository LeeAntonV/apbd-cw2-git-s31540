using CW2.Data;
using CW2.Domain;
using CW2.Services;

namespace CW2;

internal class Program
{
    static void Main(string[] args)
    {
        var rentalService = new RentalService();
        var storage = new JsonStorage();
        string filePath = "data.json";

        var data = storage.Load(filePath);
        rentalService.LoadData(data.Users, data.Equipment, data.Rentals);

        if (!rentalService.GetUsers().Any() && !rentalService.GetEquipment().Any())
        {
            SeedSampleData(rentalService);
        }

        Console.WriteLine("=== DEMONSTRATION SCENARIO ===");
        RunDemoScenario(rentalService);

        Console.WriteLine();
        Console.WriteLine("=== INTERACTIVE MENU ===");
        RunMenu(rentalService);

        storage.Save(
            filePath,
            rentalService.GetUsers(),
            rentalService.GetEquipment(),
            rentalService.GetRentals()
        );

        Console.WriteLine("Data saved.");
    }

    static void SeedSampleData(RentalService rentalService)
    {
        rentalService.AddUser(new Student("Anton", "Lee"));
        rentalService.AddUser(new Student("Anton", "Lee"));
        rentalService.AddUser(new Employee("Anton", "Lee"));

        rentalService.AddEquipment(new Laptop("Dell Latitude 5440", "Office laptop", "Dell", 16, "Intel i7"));
        rentalService.AddEquipment(new Laptop("Lenovo ThinkPad T14", "Programming laptop", "Lenovo", 32, "Ryzen 7"));
        rentalService.AddEquipment(new Projector("Epson EB-FH06", "Lecture hall projector", 3500, "1920x1080", "LCD"));
        rentalService.AddEquipment(new Camera("Canon EOS 250D", "DSLR camera", "Canon", 24.1, true));
    }

    static void RunDemoScenario(RentalService rentalService)
    {
        if (rentalService.GetRentals().Any())
        {
            Console.WriteLine("Demo scenario skipped because rentals already exist.");
            Console.WriteLine(rentalService.GenerateSummaryReport(DateTime.Today));
            return;
        }

        Console.WriteLine(rentalService.RentEquipment(1, 1, 5, new DateTime(2026, 3, 1)));
        Console.WriteLine(rentalService.RentEquipment(3, 1, 3, new DateTime(2026, 3, 1)));
        Console.WriteLine(rentalService.RentEquipment(1, 2, 4, new DateTime(2026, 3, 2)));
        Console.WriteLine(rentalService.RentEquipment(1, 3, 3, new DateTime(2026, 3, 2)));
        Console.WriteLine(rentalService.MarkEquipmentUnavailable(4));
        Console.WriteLine(rentalService.RentEquipment(3, 4, 2, new DateTime(2026, 3, 2)));
        Console.WriteLine(rentalService.ReturnEquipment(1, new DateTime(2026, 3, 6)));

        rentalService.AddEquipment(new Laptop("HP ProBook 450", "Employee laptop", "HP", 16, "Intel i5"));
        Console.WriteLine(rentalService.RentEquipment(3, 5, 3, new DateTime(2026, 3, 1)));
        Console.WriteLine(rentalService.ReturnEquipment(3, new DateTime(2026, 3, 8)));

        Console.WriteLine();
        Console.WriteLine(rentalService.GenerateSummaryReport(new DateTime(2026, 3, 10)));
    }

    static void RunMenu(RentalService rentalService)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. Add user");
            Console.WriteLine("2. Add equipment");
            Console.WriteLine("3. Show all equipment");
            Console.WriteLine("4. Show available equipment");
            Console.WriteLine("5. Rent equipment");
            Console.WriteLine("6. Return equipment");
            Console.WriteLine("7. Mark equipment unavailable");
            Console.WriteLine("8. Show active rentals for user");
            Console.WriteLine("9. Show overdue rentals");
            Console.WriteLine("10. Show summary report");
            Console.WriteLine("11. Save data now");
            Console.WriteLine("0. Exit");
            Console.Write("Choose option: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    AddUserMenu(rentalService);
                    break;
                case "2":
                    AddEquipmentMenu(rentalService);
                    break;
                case "3":
                    ShowAllEquipment(rentalService);
                    break;
                case "4":
                    ShowAvailableEquipment(rentalService);
                    break;
                case "5":
                    RentEquipmentMenu(rentalService);
                    break;
                case "6":
                    ReturnEquipmentMenu(rentalService);
                    break;
                case "7":
                    MarkEquipmentUnavailableMenu(rentalService);
                    break;
                case "8":
                    ShowActiveRentalsForUserMenu(rentalService);
                    break;
                case "9":
                    ShowOverdueRentals(rentalService);
                    break;
                case "10":
                    Console.WriteLine(rentalService.GenerateSummaryReport(DateTime.Today));
                    break;
                case "11":
                    var storage = new JsonStorage();
                    storage.Save("data.json", rentalService.GetUsers(), rentalService.GetEquipment(), rentalService.GetRentals());
                    Console.WriteLine("Data saved.");
                    break;
                case "0":
                    Console.WriteLine("Exiting program.");
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void AddUserMenu(RentalService rentalService)
    {
        Console.Write("Enter user type (student/employee): ");
        string? type = Console.ReadLine()?.Trim().ToLower();

        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine() ?? "";

        if (type == "student")
        {
            Console.WriteLine(rentalService.AddUser(new Student(firstName, lastName)));
        }
        else if (type == "employee")
        {
            Console.WriteLine(rentalService.AddUser(new Employee(firstName, lastName)));
        }
        else
        {
            Console.WriteLine("Invalid user type.");
        }
    }

    static void AddEquipmentMenu(RentalService rentalService)
    {
        Console.Write("Enter equipment type (laptop/projector/camera): ");
        string? type = Console.ReadLine()?.Trim().ToLower();

        Console.Write("Enter name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter description: ");
        string description = Console.ReadLine() ?? "";

        switch (type)
        {
            case "laptop":
                Console.Write("Enter brand: ");
                string laptopBrand = Console.ReadLine() ?? "";

                Console.Write("Enter RAM (GB): ");
                int ramGb = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Enter CPU: ");
                string cpu = Console.ReadLine() ?? "";

                Console.WriteLine(rentalService.AddEquipment(
                    new Laptop(name, description, laptopBrand, ramGb, cpu)));
                break;

            case "projector":
                Console.Write("Enter brightness (lumens): ");
                int brightness = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Enter resolution: ");
                string resolution = Console.ReadLine() ?? "";

                Console.Write("Enter technology (LCD/DLP/etc): ");
                string technology = Console.ReadLine() ?? "";

                Console.WriteLine(rentalService.AddEquipment(
                    new Projector(name, description, brightness, resolution, technology)));
                break;

            case "camera":
                Console.Write("Enter brand: ");
                string cameraBrand = Console.ReadLine() ?? "";

                Console.Write("Enter megapixels: ");
                double megapixels = double.Parse(Console.ReadLine() ?? "0");

                Console.Write("Interchangeable lens (true/false): ");
                bool interchangeableLens = bool.Parse(Console.ReadLine() ?? "false");

                Console.WriteLine(rentalService.AddEquipment(
                    new Camera(name, description, cameraBrand, megapixels, interchangeableLens)));
                break;

            default:
                Console.WriteLine("Invalid equipment type.");
                break;
        }
    }

    static void ShowAllEquipment(RentalService rentalService)
    {
        foreach (var equipment in rentalService.GetAllEquipment())
        {
            Console.WriteLine($"{equipment.Id} | {equipment.Name} | {equipment.status}");
        }
    }

    static void ShowAvailableEquipment(RentalService rentalService)
    {
        foreach (var equipment in rentalService.GetAvailableEquipment())
        {
            Console.WriteLine($"{equipment.Id} | {equipment.Name} | {equipment.status}");
        }
    }

    static void RentEquipmentMenu(RentalService rentalService)
    {
        Console.Write("Enter user id: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Enter equipment id: ");
        int equipmentId = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Enter rental length in days: ");
        int rentalDays = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine(rentalService.RentEquipment(userId, equipmentId, rentalDays, DateTime.Today));
    }

    static void ReturnEquipmentMenu(RentalService rentalService)
    {
        Console.Write("Enter rental id: ");
        int rentalId = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine(rentalService.ReturnEquipment(rentalId, DateTime.Today));
    }

    static void MarkEquipmentUnavailableMenu(RentalService rentalService)
    {
        Console.Write("Enter equipment id: ");
        int equipmentId = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine(rentalService.MarkEquipmentUnavailable(equipmentId));
    }

    static void ShowActiveRentalsForUserMenu(RentalService rentalService)
    {
        Console.Write("Enter user id: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        var rentals = rentalService.GetActiveRentalsForUser(userId);

        bool any = false;
        foreach (var rental in rentals)
        {
            Console.WriteLine(rental);
            any = true;
        }

        if (!any)
        {
            Console.WriteLine("No active rentals for this user.");
        }
    }

    static void ShowOverdueRentals(RentalService rentalService)
    {
        var rentals = rentalService.GetOverdueRentals(DateTime.Today);

        bool any = false;
        foreach (var rental in rentals)
        {
            Console.WriteLine(rental);
            any = true;
        }

        if (!any)
        {
            Console.WriteLine("No overdue rentals.");
        }
    }
}