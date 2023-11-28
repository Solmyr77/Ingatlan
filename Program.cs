using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace Ingatlan
{
    class Person
    {
        private int id;
        private string name;
        private string phoneNumber;

        public Person(int id, string name, string phoneNumber)
        {
            this.Id = id;
            this.Name = name;
            this.PhoneNumber = phoneNumber;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }

        public override string ToString()
        {
            return $"{this.Id}\t{this.Name}\t{this.PhoneNumber}";
        }
    }

    class Property
    {
        private int id;
        private string address;
        private int area;
        private int price;

        public Property(int id, string address, int area, int price)
        {
            this.Id = id;
            this.Address = address;
            this.Area = area;
            this.Price = price;
        }

        public string Address { get => address; set => address = value; }
        public int Area { get => area; set => area = value; }
        public int Price { get => price; set => price = value; }
        public int Id { get => id; set => id = value; }

        public override string ToString()
        {
            return $"{this.Id}\t{this.Address}\t\t\t\t{this.Area}\t{this.Price}";
        }
    }

    internal class Program
    {
        static void ShowReturnMessage()
        {
            Console.WriteLine("Nyomjon meg egy gombot a főmenübe lépéshez . . .");
            Console.ReadKey();

            ShowMainMenu();
        }
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
        static async Task WriteToFileAsync(string path, string line)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    await sw.WriteAsync(line);
                }

                Console.WriteLine("Sikeres mentés!");
                Console.ReadKey();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fájl írási hiba történt: {ex.Message}");

                ShowReturnMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt: {ex.Message}");

                ShowReturnMessage();
            }
        }

        static List<string> ReadFromFile(string path)
        {
            List<string> tempLines = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        tempLines.Add(line);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fájl olvasási hiba történt: {ex.Message}");

                ShowReturnMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt: {ex.Message}");

                ShowReturnMessage();
            }

            return tempLines;
        }

        static List<Property> GetPropertiesFromFile()
        {
            List<string> rawProperties = ReadFromFile("ingatlanok.txt");

            List<Property> properties = new List<Property>();

            foreach (string line in rawProperties)
            {
                string[] splitLine = line.Split("\t");

                properties.Add(new Property(Convert.ToInt32(splitLine[0]), splitLine[1], Convert.ToInt32(splitLine[2]), Convert.ToInt32(splitLine[3])));
            }

            return properties;
        }

        static List<Person> GetPeopleFromFile()
        {
            List<string> rawPeople = ReadFromFile("ugyfelek.txt");

            List<Person> people = new List<Person>();

            foreach (string line in rawPeople)
            {
                string[] splitLine = line.Split("\t");

                people.Add(new Person(Convert.ToInt32(splitLine[0]), splitLine[1], splitLine[2]));
            }

            return people;
        }

        static async void AddNewPerson()
        {
            Console.Clear();

            List<Person> savedPeople = GetPeopleFromFile();

            int id = savedPeople.Last().Id;
            string name = GetInput("Adja meg az ügyfél nevét: ");
            string phoneNumber = GetInput("Adja meg az ügyfél telefonszámát: ");

            savedPeople.Add(new Person(id + 1, name, phoneNumber));

            StringBuilder sb = new StringBuilder();

            foreach (Person person in savedPeople)
            {
                sb.Append(person.ToString() + "\n");
            }

            await WriteToFileAsync("ugyfelek.txt", sb.ToString());

            ShowMainMenu();
        }

        static async void AddNewProperty()
        {
            Console.Clear();

            List<Property> savedProperties = GetPropertiesFromFile();

            int id = savedProperties.Last().Id;

            string address = GetInput("Adja meg az ingtalan címét: ");
            int area = Convert.ToInt32(GetInput("Adja meg az ingatlan alapterületét: ", hasToBeNumber: true));
            int price = Convert.ToInt32(GetInput("Adja meg az ingatlan árát: ", hasToBeNumber: true));

            savedProperties.Add(new Property(id + 1, address, area, price));

            StringBuilder sb = new StringBuilder();

            foreach (Property property in savedProperties)
            {
                sb.Append(property.ToString() + "\n");
            }

            await WriteToFileAsync("ingatlanok.txt", sb.ToString());

            ShowMainMenu();
        }
        static void ListPeople()
        {
            Console.Clear();

            List<Person> savedPeople = GetPeopleFromFile();

            foreach (Person person in savedPeople)
            {
                Console.WriteLine(person.ToString());
            }

            Console.ReadKey();
            ShowMainMenu();
        }

        static async void DeleteProperty()
        {
            Console.Clear();

            List<Property> savedProperties = GetPropertiesFromFile();

            Console.WriteLine("ID\tCím\t\t\t\t\t\tTer.\tÁr");
            for (int i = 0; i < savedProperties.Count; i++)
            {
                Property property = savedProperties[i];

                Console.WriteLine($"({i + 1}) - {property.ToString()}");
            }

            int propertyIndex = Convert.ToInt32(GetInput("Válassza ki a törlendő ingatlant: "));

            savedProperties.RemoveAt(propertyIndex - 1);

            StringBuilder sb = new StringBuilder();

            foreach (Property property in savedProperties)
            {
                sb.Append(property.ToString());
            }

            await WriteToFileAsync("ingatlanok.txt", sb.ToString());

            Console.WriteLine("Sikeres törlés!");
            Console.ReadKey();
        }

        static void ListProperties()
        {
            Console.Clear();

            List<Property> savedProperties = GetPropertiesFromFile();

            Console.WriteLine("ID\tCím\t\t\t\t\t\tTer.\tÁr");

            foreach (Property property in savedProperties)
            {
                Console.WriteLine(property.ToString());
            }

            Console.WriteLine("\n(1) Viszalépés a főmenübe");
            Console.WriteLine("(2) Ingatlan törlése");

            int index = -1;
            do
            {
                int tempIndex = Convert.ToInt32(GetInput("Válasszon egy opciót: ", hasToBeNumber: true));

                if (!(tempIndex == 1 || tempIndex == 2))
                {
                    continue;
                }
                else
                {
                    index = tempIndex;
                }
            }
            while (index == -1);

            if (index == 1)
            {
                ShowMainMenu();
            }
            else
            {
                DeleteProperty();
            }

            ShowMainMenu();
        }

        static async void MakeOffer()
        {
            Console.Clear();

            List<Property> savedProperties = GetPropertiesFromFile();
            List<Person> savedPeople = GetPeopleFromFile();

            for (int i = 0; i < savedPeople.Count; i++)
            {
                Person person = savedPeople[i];

                Console.WriteLine($"({i+1}) - {person.ToString()}");
            }

            int personIndex = Convert.ToInt32(GetInput("Ügyfél: ", hasToBeNumber: true));

            Person selectedPerson = savedPeople[personIndex - 1];

            for (int i = 0; i < savedProperties.Count; i++)
            {
                Property property = savedProperties[i];

                Console.WriteLine($"({i + 1}) - {property.ToString()}");
            }

            int propertyIndex = Convert.ToInt32(GetInput("Ingatlan: ", hasToBeNumber: true));

            Property selectedProperty = savedProperties[propertyIndex - 1];

            StringBuilder sb = new StringBuilder();

            sb.Append($"{selectedPerson.ToString()}\t\t{DateTime.Today.Year}.{DateTime.Today.Month}.{DateTime.Today.Day}\n");
            sb.Append(selectedProperty.ToString());

            await WriteToFileAsync($"{selectedPerson.Id}_{Convert.ToString(DateTime.Today.Year).Substring(2)}{DateTime.Today.Month}{DateTime.Today.Day}.txt", sb.ToString());

            ShowMainMenu();
        }

        static void ShowMainMenu()
        {
            Console.Clear();

            Console.WriteLine("(1) Új ügyfél felvitele");
            Console.WriteLine("(2) Új ingatlan felvitele");
            Console.WriteLine("(3) Ügyfelek listázása");
            Console.WriteLine("(4) Ingatlanok listázása");
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
                        AddNewPerson();
                        isOptionSelected = true;
                        break;

                    case "2":
                        AddNewProperty();
                        isOptionSelected = true;
                        break;

                    case "3":
                        ListPeople();
                        isOptionSelected = true;
                        break;

                    case "4":
                        ListProperties();
                        isOptionSelected = true;
                        break;

                    case "5":
                        MakeOffer();
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
            ShowMainMenu();
        }
    }
}