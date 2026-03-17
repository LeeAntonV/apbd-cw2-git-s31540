namespace CW2.Domain
{
    public class Laptop : Equipment
    {
        public Laptop(string name, string brand, int ramGb, string cpuModel)
            : base(name, name)
        {
            Brand = brand;
            RamGb = ramGb;
            CpuModel = cpuModel;
        }

        public string Brand { get; }
        public int RamGb { get; }
        public string CpuModel { get; }

        public override string ToString()
        {
            return base.ToString() + $" | Brand: {Brand} | RAM: {RamGb} GB | CPU: {CpuModel}";
        }
    }
}