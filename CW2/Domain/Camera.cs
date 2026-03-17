namespace CW2.Domain
{
    public class Camera : Equipment
    {
        public Camera(string name, string description, string brand, double megapixels, bool interchangeableLens)
            : base(name, description)
        {
            Brand = brand;
            Megapixels = megapixels;
            InterchangeableLens = interchangeableLens;
        }

        public string Brand { get; }
        public double Megapixels { get; }
        public bool InterchangeableLens { get; }

        public override string ToString()
        {
            return base.ToString() +
                   $" | Brand: {Brand} | MP: {Megapixels} | Interchangeable lens: {InterchangeableLens}";
        }
    }
}