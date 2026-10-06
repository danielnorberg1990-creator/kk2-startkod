ShoppingList list = new ShoppingList("items.txt");
list.Load();

while (true)
{
    Console.WriteLine();
    list.Print();
    Console.WriteLine();
    Console.WriteLine("1. Lägg till vara");
    Console.WriteLine("2. Ta bort vara");
    Console.WriteLine("3. Spara");
    Console.WriteLine("4. Sök vara");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj: ");

    int choice = int.Parse(Console.ReadLine());

double price = 0;
bool valid = false;


while (choice == 1 && !valid)
{
    Console.Write("Namn: ");
    string name = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Namnet får inte vara tomt.");
        continue;
    }

    if (name.Length < 2)
    {
        Console.WriteLine("Namnet måste vara minst 2 tecken långt.");
        continue;
    }

    Console.Write("Pris: ");
    string input = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Priset får inte vara tomt.");
        continue;
    }

    if (!double.TryParse(input, out price))
    {
        Console.WriteLine("Priset måste vara ett tal.");
        continue;
    }
    if (price < 0)
    {
        Console.WriteLine("Priset får inte vara negativt.");
        continue;
    }

    if (!list.CanAdd(price))
    {
        Console.WriteLine("Du har nått max budget, ta bort någon vara om du vill handla mer.");
        continue;
    }

    list.Add(new Item(name, price));
    valid = true;
}

    if (choice == 2)
{
    if (list.Count() == 0)
    {
        Console.WriteLine("Listan är tom. Inget att ta bort.");
    }
    else
    {
        bool removed = false;

        //Felhantering för att ta bort en vara från listan. Användaren måste ange ett giltigt nummer.
        while (!removed)
        {
            Console.Write("Nummer: ");
            string numberInput = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(numberInput, out int number))
            {
                Console.WriteLine("Numret måste vara en utav siffrorna presenterad bredvid varan.");
                continue;
            }

            if (number < 1 || number > list.Count())
            {
                Console.WriteLine($"Ogiltigt nummer. Välj ett nummer mellan 1 och {list.Count()}.");
                continue;
            }

            list.RemoveAt(number);
            removed = true;
        }
    }
}

    else if (choice == 3)
    {
        list.Save();
    }
    else if (choice == 4)
    {
        Console.Write("Namn att söka efter: ");
        string wanted = Console.ReadLine();
        Item found = list.Find(wanted);

        if (found == null)
        {
            Console.WriteLine("Varan finns inte i listan.");
        }
        else
        {
            Console.WriteLine($"Hittade: {found}");
        }
    }
    else if (choice == 5)
    {
        break;
    }
}
