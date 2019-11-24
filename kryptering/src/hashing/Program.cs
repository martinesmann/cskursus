using System;

namespace hashing
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "Her er en tekst som jeg gerne sikre jeg kan validere ikke er blevet ændret";

            var nonKeyedHash1 = NonKeyHash.Compute(text);
            var nonKeyedHash2 = NonKeyHash.Compute(text);

            var keyedHash1 = KeyHash.Compute(text);
            var keyedHash2 = KeyHash.Compute(text);

            var keyedHash3 = KeyHash.Compute(text, "P@ssw0rd");
            var keyedHash4 = KeyHash.Compute(text, "P@ssw0rd");


            Console.WriteLine($"Non-Keyed: {text} => {nonKeyedHash1} | validate: {nonKeyedHash1 == nonKeyedHash2}");

            // data keyed hashing bruger en nu nøgle hver gang må disse to ikke være ens!
            Console.WriteLine($"    Keyed: {text} => {keyedHash1} | validate: {keyedHash1 != keyedHash2}");

            Console.WriteLine($"    Keyed: {text} => {keyedHash3} | validate: {keyedHash3 == keyedHash4}");

        }
    }
}
