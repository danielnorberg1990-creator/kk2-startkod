// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    private List<Item> items = new List<Item>();
    private string path;
    private const double MaxTotal = 1000;

    public ShoppingList(string path)
    {
        this.path = path;
    }

    public void Add(Item item)
    {
        items.Add(item);
    }

      // Returns the number of items in the list.
    public int Count()
    {
    return items.Count;
    }

    // Removes the item the user sees as number 1, 2, 3 ...
    public void RemoveAt(int number)
    {
        items.RemoveAt(number - 1);
    }

    // Adds up the price of every item on the list.
    public double Total()
    {
        double sum = 0;

        for (int i = 0; i < items.Count; i++)
        {
            sum += items[i].Price;
        }

        return sum;
    }

    // True if an item with this price can be added without exceeding the budget.
    public bool CanAdd(double price)
    {
        return Total() + price <= MaxTotal;
    }

    // How much is left before the budget is reached.
    public double Remaining()
    {
        return MaxTotal - Total();
    }

    // Looks up an item by its name. Returns null if there is no such item.
    public Item Find(string name)
    {
        foreach (Item item in items)
        {
            if (string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }
        }

        return null;
    }

    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i]}");
        }

        Console.WriteLine($"Totalt: {Total():F2} kr (kvar till maxbeloppet: {Remaining():F2} kr)");
    }

    // Writes one item per line, as "price;name".
    public void Save()
    {
        List<string> lines = new List<string>();

        foreach (Item item in items)
        {
            lines.Add($"{item.Price};{item.Name}");
        }

        try
        {
            File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n");
            Console.WriteLine("Listan är sparad.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Kunde inte spara listan: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Kunde inte spara listan (filen är skrivskyddad): {ex.Message}");
        }
    }

    // Reads the file back into the list.
    public void Load()
    {
                if (!File.Exists(path))
        {
            Console.WriteLine($"Filen {path} hittades inte. Programmet startar med en tom lista.");
            return;
        }
        
        string text = File.ReadAllText(path);
        string[] lines = text.Split('\n', '\r');

        foreach (string line in lines)
        {
                        if (line.Length == 0) // Controls for empty lines.
            {
                continue;
            }

            string[] parts = line.Split(';');

            if (parts.Length != 2) // Make sure that the program expects 2 inputs, item name and price.
            {
                continue;
            }

            if (double.TryParse(parts[0], out double price))
            {
                items.Add(new Item(parts[1], price));
            }
        }
    }
}

