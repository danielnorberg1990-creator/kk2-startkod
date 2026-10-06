// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    // The items on the list.
    private List<Item> items = new List<Item>();
    // The file the list is saved to and loaded from.
    private string path;
    // The budget cap: the total of the list may not exceed this amount.
    private const double MaxTotal = 1000;

    // Remembers which file the list is saved to and loaded from.
    public ShoppingList(string path)
    {
        this.path = path;
    }

    // Adds the item unless it would push the total past the budget cap.
    // Returns true if the item was added, false if the budget would be exceeded.
    public bool Add(Item item)
    {
        if (Total() + item.Price > MaxTotal)
        {
            return false;
        }

        items.Add(item);
        return true;
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

    // Prints every item with its number, followed by the total and the remaining budget.
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
        // Build one line per item in the "price;name" format.
        List<string> lines = new List<string>();

        foreach (Item item in items)
        {
            lines.Add($"{item.Price};{item.Name}");
        }

        // Write the file, handling the errors that can occur while writing.
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
        
        // Read the whole file and split it into lines.
        string text = File.ReadAllText(path);
        string[] lines = text.Split('\n', '\r');

        foreach (string line in lines)
        {
                        if (line.Length == 0) // Controls for empty lines.
            {
                continue;
            }

            // Split the line into price (before ';') and name (after ';').
            string[] parts = line.Split(';');

            if (parts.Length != 2) // Make sure that the program expects 2 inputs, item name and price.
            {
                continue;
            }

            // Skip lines where the price is not a number.
            if (!double.TryParse(parts[0], out double price))
            {
                continue;
            }

            // Skip lines that would create an invalid item (negative, NaN or empty name).
            if (price < 0 || !double.IsFinite(price) || parts[1].Length == 0)
            {
                continue;
            }

            // The line is valid, so add the item to the list.
            items.Add(new Item(parts[1], price));
        }
    }
}

