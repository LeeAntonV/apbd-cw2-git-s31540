using System.Text.Json;
using System.Text.Json.Serialization;
using CW2.Domain;

namespace CW2.Data;

public class JsonStorage
{
    private readonly JsonSerializerOptions _options;

    public JsonStorage()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };
    }

    public void Save(string filePath, List<User> users, List<Equipment> equipment, List<Rental> rentals)
    {
        var data = new StorageModel
        {
            Users = users,
            Equipment = equipment,
            Rentals = rentals
        };

        var json = JsonSerializer.Serialize(data, _options);
        File.WriteAllText(filePath, json);
    }

    public StorageModel Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new StorageModel();
        }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<StorageModel>(json, _options) ?? new StorageModel();
    }
}