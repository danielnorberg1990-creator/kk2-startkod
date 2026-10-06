**Felsökning och felhantering:**
================================

1. Första felmeddelandet när man försöker starta programmet är följande:

"Unhandled exception. System.IndexOutOfRangeException: Index was outside the bounds of the array.
   at ShoppingList.Load() in C:\Gitrepos\kk2-startkod\ShoppingList.cs:line 90
   at Program.<Main>$(String[] args) in C:\Gitrepos\kk2-startkod\Program.cs:line 2"

// Det kan härledas till Load funktionen i ShoppingList.cs. Fick lägga till \r då filerna sparar med \n & \r.
//"string[] lines = text.Split('\n', '\r');". Detta lagar problemet med att man inte kunde se item name utan endast priset, tidigare.
   
--------------------------------

   2. Problem när man ska lägga till ny vara (menyval 1). Man kan lämna tom sträng i textfältet eller bokstäver på priset, 
   vilket man inte ska kunna.

   Får följande felmeddelande:
   "Unhandled exception. System.FormatException: The input string '' was not in a correct format.
   at System.Number.ThrowFormatException[TChar](ReadOnlySpan`1 value)
   at System.Int32.Parse(String s)
   at Program.<Main>$(String[] args) in C:\Gitrepos\kk2-startkod\Program.cs:line 23"

//Lagt till felhantering gällande pris och namn, fältet får inte vara tomt eller felaktig (bokstav där siffra förväntas).
//Lagt till hantering att namnet på produkten behöver vara minst 2 tecken långt.
//Lagt till funktion så att decimaler kan skrivas och även rundas av till 2 decimaler.
   
--------------------------------
   3. Problem när man ska ta bort produkt från menyval 2, felkod ifall man väljer siffra utanför det som finns angivet.
   
   Unhandled exception. System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')
   at System.Collections.Generic.List`1.RemoveAt(Int32 index)
   at ShoppingList.RemoveAt(Int32 number) in C:\Gitrepos\kk2-startkod\ShoppingList.cs:line 20
   at Program.<Main>$(String[] args) in C:\Gitrepos\kk2-startkod\Program.cs:line 61

//Lagt till hantering för att man inte ska kunna skriva tal utanför listan när man tar bort varat från menyval 2.
//Man kan inte skriva bokstäver där siffra förväntas.      
--------------------------------

    4. Problem i huvudmenyn. Programmet kraschar om man skriver bokstäver eller ett tal utanför 1-5 i menyvalet.

    Får följande felmeddelande:
    "Unhandled exception. System.FormatException: The input string 'abc' was not in a correct format.
    at System.Number.ThrowFormatException[TChar](ReadOnlySpan`1 value)
    at System.Int32.Parse(String s)
    at Program.<Main>$(String[] args) in C:\Gitrepos\kk2-startkod\Program.cs:line 16"

    //Lagt till int.TryParse istället för int.Parse i huvudmenyn. Om inmatningen inte är ett tal mellan 1 och 5 visas felmeddelandet "Ogiltigt val. Välj ett nummer mellan 1 och 5." och programmet frågar igen i stället för att krascha.

--------------------------------

    5. Programmet kraschar vid start om items.txt inte finns i mappen.

    Får följande felmeddelande:
    "Unhandled exception. System.IO.FileNotFoundException: Could not find file 'C:\Gitrepos\kk2-startkod\items.txt'.
    File name: 'C:\Gitrepos\kk2-startkod\items.txt'
    at System.IO.File.ReadAllText(String path, Encoding encoding)
    at ShoppingList.Load() in C:\Gitrepos\kk2-startkod\ShoppingList.cs:line 84
    at Program.<Main>$(String[] args) in C:\Gitrepos\kk2-startkod\Program.cs:line 2"

    //Lagt till en kontroll med File.Exists i Load innan filen läses. Om filen inte finns startar programmet med en tom lista i stället för att krascha.

--------------------------------

    6. Menyval 4 (sök vara) hittade inte varan om bokstavstorleken inte stämde exakt. Sökte man på "ost" fick man meddelandet "Varan finns inte i listan." trots att "Ost" fanns i listan.

    //Ändrat jämförelsen i Find från == till string.Equals med StringComparison.OrdinalIgnoreCase. Sökningen blir nu oberoende av små och stora bokstäver, "ost", "Ost" och "OST" hittar alla varan "Ost".

--------------------------------

    7. I Save fanns en tom catch som fångade alla fel utan att visa något, och "Listan är sparad." skrevs ut oavsett om sparandet lyckades. Testades genom att göra items.txt skrivskyddad: programmet sa "Listan är sparad." men ingen data skrevs till filen.

    //"Listan är sparad." flyttades in i try-blocket så att den endast visas när sparandet lyckats. Den tomma catch ersattes med specifika catchar för IOException och UnauthorizedAccessException som skriver ut ett felmeddelande när sparandet misslyckas.
    //I .NET 10 är UnauthorizedAccessException inte längre en underklass till IOException (undantaget har flyttats till System-namespace), därför krävs två separata catchar för att fånga båda fallen.

--------------------------------

**Designval:**
==================

1. Budgettaket — hur Add säger nej.

   Add returnerar bool i stället för att kasta ett undantag. Den returnerar true om varan lades
   till och false om varan skulle göra att totalbeloppet blir högre än taket (MaxTotal = 1000 kr).
   I det senare fallet läggs alltså varan inte till.

   //public bool Add(Item item)
   //{
   //    if (Total() + item.Price > MaxTotal) return false;
   //    items.Add(item);
   //    return true;
   //}

   Varför returvärdet i stället för undantag: att stöta på budgettaket är en förväntad situation.
   Användaren kan helt enkelt vilja köpa något som tillsammans med resten av listan blir för dyrt —
   det är inget fel i programmet och inget onormalt tillstånd. Undantag passar bäst för oväntade
   tillstånd; för en förväntad situation räcker ett returvärde. Det följer exemplet från kursen
   där ett bankuttag returnerar false när saldot inte räcker i stället för att kasta undantag.

   Vad Program.cs gör med svaret: det kontrollerar returvärdet. Om Add returnerar false skrivs
   "Du har nått max budget, ta bort någon vara om du vill handla mer." och programmet frågar
   användaren igen i stället för att krascha. Eftersom taket sitter i ShoppingList — inte bara i
   menyn — kan ingen kod som kallar Add lägga till en vara som spränger taket.

2. Item skyddar sig själv.

   Konstruktorn vägrar skapa ett trasigt objekt och kastar i stället:
   - ArgumentException om namnet är tomt eller bara mellanslag.
   - ArgumentOutOfRangeException om priset är negativt.

   Egenskaperna Name och Price är skrivskyddade (endast get), så ett Item inte kan ändras till
   ett ogiltigt tillstånd efter skapandet.

   Program.cs fångar undantagen runt konstruktionen (catch för ArgumentOutOfRangeException före
   catch för ArgumentException, eftersom ArgumentOutOfRangeException ärver från ArgumentException)
   och visar meddelandet för användaren, så programmet kraschar inte. Menyn förvaliderar dessutom
   namn och pris redan när användaren skriver in dem, så undantagen i Item fungerar som ett
   skyddsnät för fall där ett ogiltigt Item ändå försöker skapas.

--------------------------------

**Klassdiagram:**
==================

+------------------+                      +---------------------------+                      +----------------------+
| Program          | -> anropar --------->| ShoppingList              | -> innehåller ------>| Item                 |
| - Main()         |                      | - items: List<Item>       |                      | - Name: string       |
+------------------+                      | - path: string            |                      | - Price: double      |
                                          | - MaxTotal: double        |                      | + Item(name, price)  |
                                          | + Add(item): bool         |                      | + ToString(): string |
                                          | + RemoveAt(number)        |                      +----------------------+
                                          | + Find(name): Item        |
                                          | + Total(): double         |
                                          | + Remaining(): double     |
                                          | + Print()                 |
                                          | + Save()                  |
                                          | + Load()                  |
                                          +---------------------------+
