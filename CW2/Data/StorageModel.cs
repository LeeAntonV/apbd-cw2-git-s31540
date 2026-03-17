using CW2.Domain;

namespace CW2.Data;

public class StorageModel
{
    public List<User> Users { get; set; } = new();
    public List<Equipment> Equipment { get; set; } = new();
    public List<Rental> Rentals { get; set; } = new();
}