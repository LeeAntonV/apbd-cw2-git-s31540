namespace CW2.Domain
{
    public class Projector : Equipment
    {
        public Projector(string name,string description, int brightnessLumens, string resolution, string technology)
            : base(name, description)
        {
            BrightnessLumens = brightnessLumens;
            Resolution = resolution;
            Technology = technology;
        }

        public int BrightnessLumens { get; }
        public string Resolution { get; }
        public string Technology { get; }

        public override string ToString()
        {
            return base.ToString() +
                   $" | Brightness: {BrightnessLumens} lm | Resolution: {Resolution} | Technology: {Technology}";
        }
    }
}