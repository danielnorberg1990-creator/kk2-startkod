**Felsökning och felhantering:**
================================

1. Första felmeddelandet när man försöker starta programmet är följande:

"Unhandled exception. System.IndexOutOfRangeException: Index was outside the bounds of the array.
   at ShoppingList.Load() in C:\Gitrepos\kk2-startkod\ShoppingList.cs:line 90
   at Program.<Main>$(String[] args) in C:\Gitrepos\kk2-startkod\Program.cs:line 2"

   Det kan härledas till Load funktionen i ShoppingList.cs. Fick lägga till \r då filerna sparar med \n & \r.
   "string[] lines = text.Split('\n', '\r');". Detta lagar problemet med att man inte kunde se item name utan endast priset, tidigare.