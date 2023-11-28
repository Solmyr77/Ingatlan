namespace Ingatlan
{
    internal class Program
    {
        static string GetInput(string text = "", bool hasToBeNumber = false)
        {
            string temp = "";

            do
            {
                try
                {
                    Console.Write(text);

                    temp = Console.ReadLine().Trim();

                    if (temp == "")
                    {
                        throw new Exception("A mező nem lehet üres!");
                    }

                    bool isNumber = int.TryParse(temp, out _);

                    if (hasToBeNumber && isNumber && int.Parse(temp) < 1)
                    {
                        temp = "";
                        throw new Exception("A mező nem lehet nulla vagy negatív szám!");
                    }

                    else if (hasToBeNumber && !isNumber)
                    {
                        temp = "";
                        throw new Exception("A mező csak számot tartalmazhat!");
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            } while (temp == "");

            return temp;
        }

        static List<string> ReadFromFile(string path)
        {
            List<string> tempLines = new List<string>();

            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    tempLines.Add(line);
                }
            }

            return tempLines;
        }

        static async Task WriteToFileAsync(string path, string lines)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                await sw.WriteLineAsync(lines);
            }
        }

        static void ShowMainMenu()
        {
            Console.Clear();

            Console.WriteLine("(1) Új ügyfél felvitele");
            Console.WriteLine("(2) Új ingatlan felvitele");
            Console.WriteLine("(3) Ingatlanok listázása");
            Console.WriteLine("(4) Ügyfelek listázása");
            Console.WriteLine("(5) Ajánlat készítése");
            Console.WriteLine("(6) Kilépés");

            bool isOptionSelected = false;
            do
            {
                Console.WriteLine();
                string option = GetInput("Opció: ", hasToBeNumber: true);

                switch (option)
                {
                    case "1":
                        isOptionSelected = true;
                        break;

                    case "2":
                        isOptionSelected = true;
                        break;

                    case "3":
                        isOptionSelected = true;
                        break;

                    case "4":
                        isOptionSelected = true;
                        break;

                    case "5":
                        isOptionSelected = true;
                        break;

                    case "6":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("\nHelytelen opció!");
                        break;
                }
            }
            while (!isOptionSelected);
        }

        static void Main()
        {
            List<string> ingatlanok = ReadFromFile("ingatlanok.txt");


        }
    }
}