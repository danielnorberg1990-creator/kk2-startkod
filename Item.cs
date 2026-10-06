// One item on the shopping list.
class Item
{
    // Read-only so a created item can never be changed into an invalid state.
    public string Name { get; }
    public double Price { get; }

    // The item protects itself: instead of silently creating a broken object,
    // the constructor throws if the values are invalid.
    public Item(string name, double price)
    {
        if (string.IsNullOrWhiteSpace(name)) // Check for null, empty, or whitespace-only string
        {
            throw new ArgumentException("Namnet får inte vara tomt.", nameof(name));
        }

        if (price < 0) // Check for negative price
        {
            throw new ArgumentOutOfRangeException(nameof(price), price, "Priset får inte vara negativt.");
        }

        Name = name;
        Price = price;
    }

    // How the item is shown: "name - price kr".
    public override string ToString()
    {
        return $"{Name} - {Price} kr";
    }
}
