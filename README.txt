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
//Lagt till hantering att namnet på produkten behöver vara minst 3 tecken långt.
//Lagt till funktion så att decimaler kan skrivas och även rundas av till 2 decimaler.
   
--------------------------------
   3. Problem när man ska ta bort produkt från menyval 2, felkod ifall man väljer siffra utanför det som finns angivet.
   
   Unhandled exception. System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')
   at System.Collections.Generic.List`1.RemoveAt(Int32 index)
   at ShoppingList.RemoveAt(Int32 number) in C:\Gitrepos\kk2-startkod\ShoppingList.cs:line 20
   at Program.<Main>$(String[] args) in C:\Gitrepos\kk2-startkod\Program.cs:line 61

      
--------------------------------