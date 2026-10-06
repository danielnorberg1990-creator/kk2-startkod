// Reads a line of input. If the input stream has ended (for example Ctrl+Z in a console),
// there is nothing left to read, so the program exits cleanly instead of looping forever.
string ReadLineOrExit()
{
    string line = Console.ReadLine();

    if (line == null)
    {
        Environment.Exit(0);
    }

    return line;
}

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
    string choiceInput = ReadLineOrExit();
    int choice;

    while (!int.TryParse(choiceInput, out choice) || choice < 1 || choice > 5)
    {
        Console.WriteLine("Ogiltigt val. Välj ett nummer mellan 1 och 5.");
        choiceInput = ReadLineOrExit();
    }

double price = 0;
bool valid = false;


while (choice == 1 && !valid)
{
    Console.Write("Namn: ");
    string name = ReadLineOrExit();

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
    string input = ReadLineOrExit();

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

    if (!double.IsFinite(price))
    {
        Console.WriteLine("Priset måste vara ett tal på ett korrekt format.");
        continue;
    }

    Item newItem;

    // The Item constructor can throw if the values are invalid. Handle it so the program never crashes.
    try
    {
        newItem = new Item(name, price);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"{ex.Message}");
        continue;
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"{ex.Message}");
        continue;
    }

    if (!list.Add(newItem))
    {
        Console.WriteLine("Du har nått max budget, ta bort någon vara om du vill handla mer.");
        continue;
    }

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
            string numberInput = ReadLineOrExit();

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
        string wanted = ReadLineOrExit();
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
