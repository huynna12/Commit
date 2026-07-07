namespace Commit.Api.Services
{
    public class NameGeneratorService
    {
        private static readonly Random random = new Random();
        private readonly string[] _characteristics = new string[]
        {
            "Sweet","Brave", "Clever","Mighty","Wise","Fierce","Noble","Bold","Fearless",
            "Loyal", "Wild", "Scary", "Gentle", "Smart", "Strong", "TopG", "Lowkey", "Nonchalant", "Chalant",
            "Chill", "Cool", "Epic", "Legendary", "Savage", "Radical", "Daring", "Adventurous", "Courageous",
        };

        private readonly string[] _animals = new string[] { 
            "Lion", "Tiger", "Bear", "Wolf", "Eagle", "Shark", "Panther", "Falcon", "Cheetah",
            "Dragon", "Phoenix", "Griffin", "Unicorn", "Pegasus", "Kraken", "Leopard", "Jaguar",
            "Hawk", "Cobra", "Raven", "Fox", "Otter", "Dolphin", "Whale", "Elephant", "Gorilla",
            "Rhino", "Buffalo", "Crocodile", "Alligator", "Turtle", "Tortoise", "Octopus", "Squid",
        };

        public string GenerateName()
        {
            string characteristic = _characteristics[random.Next(_characteristics.Length)];
            string animal = _animals[random.Next(_animals.Length)];
            return $"{characteristic} {animal}";
        }
    }
}
