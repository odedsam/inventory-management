using System;
using InventoryManagement.UI; // import ui namespace

namespace InventoryManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var ui = new InventoryConsoleUI();
                ui.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
