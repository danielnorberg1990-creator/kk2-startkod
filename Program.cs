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

// Create the shopping list and load the items saved in items.txt (if the file exists).
ShoppingList list = new ShoppingList("items.txt");
list.Load();

// Main loop: show the list and menu, read a choice, and do what the user choose to input.
while (true)
{
    // Show the current list of products and its total.
    Console.WriteLine();
    list.Print();
    Console.WriteLine();
    // Menu choices.
    Console.WriteLine("1. Lägg till vara");
    Console.WriteLine("2. Ta bort vara");
    Console.WriteLine("3. Spara");
    Console.WriteLine("4. Sök vara");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj: ");
    // Read the user's input choice.
    string choiceInput = ReadLineOrExit();
    int choice;

    // Keep asking until the choice is a number between 1 and 5, else will throw error message that tells the user to choose a number between 1 and 5.
    while (!int.TryParse(choiceInput, out choice) || choice < 1 || choice > 5)
    {
        Console.WriteLine("Ogiltigt val. Välj ett nummer mellan 1 och 5.");
        choiceInput = ReadLineOrExit();
    }

// Set the price to 0 and valid to false, so that the program can check if the user has entered a valid item.
double price = 0;
bool valid = false;


// Choice 1: add an item. Ask again until a valid item has been added by correct price and name standards are met.
while (choice == 1 && !valid)
{
    Console.Write("Namn: ");
    // Asks the user what the name of the item is.
    string name = ReadLineOrExit();

    // Error handling for the variable name.
    if (string.IsNullOrWhiteSpace(name)) // Handles null and white spaces.
    {
        Console.WriteLine("Namnet får inte vara tomt.");
        continue;
    }

    // The user has to input a item name that contains at least 2 characters.
    if (name.Length < 2)
    {
        Console.WriteLine("Namnet måste vara minst 2 tecken långt.");
        continue;
    }

    Console.Write("Pris: ");
    // Ask the user what price the item should have.
    string input = ReadLineOrExit();

    // Error handling null and white spaces.
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Priset får inte vara tomt.");
        continue;
    }

    // Rule that sets the price has to be a number and not a string.
    if (!double.TryParse(input, out price))
    {
        Console.WriteLine("Priset måste vara ett tal.");
        continue;
    }
    // Rule that sets price cannot be negative.
    if (price < 0)
    {
        Console.WriteLine("Priset får inte vara negativt.");
        continue;
    }

    // Rule that sets price must be a real finite number (rejects inputs like NaN or infinity).
    if (!double.IsFinite(price))
    {
        Console.WriteLine("Priset måste vara ett tal på ett korrekt format.");
        continue;
    }

    // The new item, created in the try block below.
    Item newItem;

    // The Item constructor can throw if the values are invalid. Handle it so the program never crashes.
    try
    {
        newItem = new Item(name, price); // Create a new item.
    }
    catch (ArgumentOutOfRangeException ex) // This exception is thrown if the price is negative or infinite.
    {
        Console.WriteLine($"{ex.Message}"); 
        continue;
    }
    catch (ArgumentException ex) // This exception is thrown if the name is empty.
    {
        Console.WriteLine($"{ex.Message}");
        continue;
    }

    // Add returns false if the item would push the total cost over the budget and then the user cannot add more items.
    if (!list.Add(newItem))
    {
        Console.WriteLine("Du har nått max budget, ta bort någon vara om du vill handla mer.");
        continue;
    }

    valid = true;
}

    // Choice 2: remove an item.
    if (choice == 2)
{
    // An empty list has nothing to remove.
    if (list.Count() == 0)
    {
        Console.WriteLine("Listan är tom. Inget att ta bort.");
    }
    else
    {
        // Loops until a valid input has been given by the user. 
        bool removed = false;

        // Error handling when removing an item: the user must enter a valid number.
        while (!removed)
        {
            Console.Write("Nummer: ");
            string numberInput = ReadLineOrExit();

            // The input has to be a number presented next to the item that you want to remove.
            if (!int.TryParse(numberInput, out int number))
            {
                Console.WriteLine("Numret måste vara en utav siffrorna presenterad bredvid varan.");
                continue;
            }

            // Error handling when removing an item: the user must enter a number that exists in the list.
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

    // Choice 3: save the list to items.txt.
    else if (choice == 3)
    {
        list.Save();
    }
    // Choice 4: look an item up by name.
    else if (choice == 4)
    {
        Console.Write("Namn att söka efter: "); // Input what you want to search for.
        string wanted = ReadLineOrExit(); // Read the input and store it in the variable wanted.
        Item found = list.Find(wanted); // Search for the item in the list and store it in the variable found.

        // Tells the user whether the item was found or not.
        if (found == null)
        {
            Console.WriteLine("Varan finns inte i listan.");
        }
        else
        {
            Console.WriteLine($"Hittade: {found}");
        }
    }
    // Choice 5: quit the program.
    else if (choice == 5)
    {
        break;
    }
}
