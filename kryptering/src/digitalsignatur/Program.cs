using System;

namespace digitalsignatur
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "Her er en tekst som jeg gerne vil sikre ikke bliver ændret";

            var signatur = DigitalSignatur.Sign(text);
            var validate1 = DigitalSignatur.Validate(signatur);

            signatur.Text += ".";
            var validate2 = DigitalSignatur.Validate(signatur);


            Console.WriteLine($"Signatur: {text} => signed (original) | validate: {validate1}");
            Console.WriteLine($"Signatur: {text} => signed ( ændret ) | validate: {validate2}");
        }
    }
}
