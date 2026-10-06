// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    // The items that are currently on the list.
    private List<Item> items = new List<Item>();
    // The file saves and loads to and from.
    private string path;
    // The budget cap: the total of the list may not exceed this amount or greeted with a message.
    private const double MaxTotal = 1000;

    // Keeps track of which file the list is saved to and loaded from.
    public ShoppingList(string path)
    {
        this.path = path;
    }

    // Adds the item to the list unless it would push the total cost past the budget cap.
    // Returns true if the item was added, false if the budget would be exceeded.
    public bool Add(Item item)
    {
        if (Total() + item.Price > MaxTotal) // If the total cost would exceed the cap, don't add it.
        {
            return false;
        }

        items.Add(item);
        return true;
    }

      // Returns the numbers of the items according to the list.
    public int Count()
    {
    return items.Count;
    }

    // Removes the item the user sees as number 1, 2, 3 and so on.
    public void RemoveAt(int number)
    {
        items.RemoveAt(number - 1);
    }

    // Totals the total cost of the items on the list.
    public double Total()
    {
        double sum = 0;

        for (int i = 0; i < items.Count; i++) // Loops through the list and adds the price of each item to the sum.
        {
            sum += items[i].Price; // Add the price of the item at index i to the sum.
        }

        return sum;
    }

    // Counts how much room is left in the budget before the cap is reached.
    public double Remaining()
    {
        return MaxTotal - Total();
    }

    // Looks up an item by its name. Returns null if there is no such item.
    public Item Find(string name)
    {
        foreach (Item item in items)
        {
            if (string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase)) // Case-insensitive comparison of the item name and the search name.
            {
                return item;
            }
        }

        return null;
    }

    // Prints every item with its belonging number, followed by the total and the remaining budget.
    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i]}"); // Prints the item at index i with its number (i + 1) and the item itself (which calls ToString() on the item).
        }

        Console.WriteLine($"Totalt: {Total():F2} kr (kvar till maxbeloppet: {Remaining():F2} kr)"); // Prints the total cost and the remaining budget, formatted to 2 decimals.
    }

    // Writes one item per line, as accordingly "price;name".
    public void Save()
    {
        // Build one line per item in the "price;name" format.
        List<string> lines = new List<string>();

        foreach (Item item in items)
        {
            lines.Add($"{item.Price};{item.Name}"); // Add the price and name of the item to the list of lines.
        }

        // Write the file, handling the errors that can occur during writing.
        try
        {
            File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n"); // Write the lines to the file, separated by newlines, and add a newline at the end.
            Console.WriteLine("Listan är sparad."); // Confirmation that the file saved successfully.
        }
        catch (IOException ex) // This exception is thrown if the file is locked or the disk is full.
        {
            Console.WriteLine($"Kunde inte spara listan: {ex.Message}"); 
        }
        catch (UnauthorizedAccessException ex) // This exception is thrown if the file is read-only or the user does not have permission to write to it.
        {
            Console.WriteLine($"Kunde inte spara listan (filen är skrivskyddad): {ex.Message}");
        }
    }

    // Reads the file back into the list at program startup, if it exists.
    public void Load()
    {
                if (!File.Exists(path)) // checks for the file items.txt, if it does not exist, the program starts with an empty list.
        {
            Console.WriteLine($"Filen {path} hittades inte. Programmet startar med en tom lista.");
            return;
        }
        
        // Read the whole file and split it into lines.
        string text = File.ReadAllText(path); // Read the entire contents of the file into a string.
        string[] lines = text.Split('\n', '\r'); // Split the string into lines, using both newline and carriage return as separators.

        foreach (string line in lines)
        {
                        if (line.Length == 0) // Controls for empty lines in the file.
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
            if (!double.TryParse(parts[0], out double price)) // Check if the price is a number.
            {
                continue;
            }

            // Skip lines that would create an invalid item (negative, NaN or empty name).
            if (price < 0 || !double.IsFinite(price) || parts[1].Length == 0) // Check for negative price, NaN, or empty name
            {
                continue;
            }

            // If the line is valid, the item adds to the list.
            items.Add(new Item(parts[1], price));
        }
    }
}

