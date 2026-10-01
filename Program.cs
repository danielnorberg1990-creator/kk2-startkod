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

    if (name.Length < 3)
    {
        Console.WriteLine("Namnet måste vara minst 3 tecken långt.");
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

    list.Add(new Item(name, price));
    valid = true;
}

    if (choice == 2)
    {
        Console.Write("Nummer: ");
        int number = int.Parse(Console.ReadLine());
        list.RemoveAt(number);
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
