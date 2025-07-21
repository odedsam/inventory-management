using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManagement.Logic; // import logics namespace
using InventoryManagement.Models; // import models namespace

namespace InventoryManagement.UI
{
    public class InventoryConsoleUI
    {
        private readonly InventoryManager _inventoryManager;

        public InventoryConsoleUI()
        {
            _inventoryManager = new InventoryManager();
        }

        public void Run()
        {
            Console.WriteLine("Welcome to Inventory Management System!");
            Console.WriteLine("=====================================");

            while (true)
            {
                ShowMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageCategories();
                        break;
                    case "2":
                        ManageProducts();
                        break;
                    case "3":
                        InventoryOperations();
                        break;
                    case "4":
                        ShowReports();
                        break;
                    case "5":
                        Console.WriteLine("Thank you for using Inventory Management System!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("\n=== MAIN MENU ===");
            Console.WriteLine("1. Manage Categories");
            Console.WriteLine("2. Manage Products");
            Console.WriteLine("3. Inventory Operations");
            Console.WriteLine("4. Reports");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
        }

        private void ManageCategories()
        {
            Console.Clear();
            Console.WriteLine("=== CATEGORY MANAGEMENT ===");
            Console.WriteLine("1. Add Category");
            Console.WriteLine("2. View All Categories");
            Console.WriteLine("3. Update Category");
            Console.WriteLine("4. Delete Category");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddCategory();
                    break;
                case "2":
                    ViewAllCategories();
                    break;
                case "3":
                    UpdateCategory();
                    break;
                case "4":
                    DeleteCategory();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        private void AddCategory()
        {
            Console.WriteLine("\n--- Add New Category ---");
            Console.Write("Enter category name: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Enter category description: ");
            var description = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(name))
            {
                _inventoryManager.AddCategory(name, description);
                Console.WriteLine("Category added successfully!");
            }
            else
            {
                Console.WriteLine("Category name cannot be empty.");
            }
        }

        private void ViewAllCategories()
        {
            Console.WriteLine("\n--- All Categories ---");
            var categories = _inventoryManager.GetAllCategories();

            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
                return;
            }

            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }
        }

        private void UpdateCategory()
        {
            Console.WriteLine("\n--- Update Category ---");
            ViewAllCategories();
            Console.Write("Enter category ID to update: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var category = _inventoryManager.GetCategoryById(id);
                if (category != null)
                {
                    Console.WriteLine($"Current: {category}");
                    Console.Write("Enter new name (or press Enter to keep current): ");
                    var name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name)) name = category.Name;

                    Console.Write("Enter new description (or press Enter to keep current): ");
                    var description = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(description)) description = category.Description;

                    if (_inventoryManager.UpdateCategory(id, name, description))
                    {
                        Console.WriteLine("Category updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update category.");
                    }
                }
                else
                {
                    Console.WriteLine("Category not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        private void DeleteCategory()
        {
            Console.WriteLine("\n--- Delete Category ---");
            ViewAllCategories();
            Console.Write("Enter category ID to delete: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (_inventoryManager.DeleteCategory(id))
                {
                    Console.WriteLine("Category deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to delete category. It may have products assigned to it.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        private void ManageProducts()
        {
            Console.Clear();
            Console.WriteLine("=== PRODUCT MANAGEMENT ===");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View All Products");
            Console.WriteLine("3. Update Product");
            Console.WriteLine("4. Delete Product");
            Console.WriteLine("5. View Products by Category");
            Console.WriteLine("6. Back to Main Menu");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddProduct();
                    break;
                case "2":
                    ViewAllProducts();
                    break;
                case "3":
                    UpdateProduct();
                    break;
                case "4":
                    DeleteProduct();
                    break;
                case "5":
                    ViewProductsByCategory();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        private void AddProduct()
        {
            Console.WriteLine("\n--- Add New Product ---");

            // show categories first
            var categories = _inventoryManager.GetAllCategories();
            if (categories.Count == 0)
            {
                Console.WriteLine("No categories available. Please create a category first.");
                return;
            }

            Console.WriteLine("Available Categories:");
            foreach (var category in categories)
            {
                Console.WriteLine($"  {category.Id}. {category.Name}");
            }

            Console.Write("Enter product name: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Enter product description: ");
            var description = Console.ReadLine() ?? "";
            Console.Write("Enter price: ");

            if (decimal.TryParse(Console.ReadLine(), out decimal price) && price >= 0)
            {
                Console.Write("Enter initial quantity: ");
                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity >= 0)
                {
                    Console.Write("Enter category ID: ");
                    if (int.TryParse(Console.ReadLine(), out int categoryId))
                    {
                        var category = _inventoryManager.GetCategoryById(categoryId);
                        if (category != null && !string.IsNullOrWhiteSpace(name))
                        {
                            _inventoryManager.AddProduct(name, description, price, quantity, categoryId);
                            Console.WriteLine("Product added successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Invalid category ID or product name cannot be empty.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid category ID.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid quantity.");
                }
            }
            else
            {
                Console.WriteLine("Invalid price.");
            }
        }

        private void ViewAllProducts()
        {
            Console.WriteLine("\n--- All Products ---");
            var products = _inventoryManager.GetAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            foreach (var product in products)
            {
                var category = _inventoryManager.GetCategoryById(product.CategoryId);
                Console.WriteLine($"{product} | Category: {category?.Name ?? "Unknown"}");
            }
        }

        private void UpdateProduct()
        {
            Console.WriteLine("\n--- Update Product ---");
            ViewAllProducts();
            Console.Write("Enter product ID to update: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var product = _inventoryManager.GetProductById(id);
                if (product != null)
                {
                    Console.WriteLine($"Current: {product}");

                    Console.Write("Enter new name (or press Enter to keep current): ");
                    var name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name)) name = product.Name;

                    Console.Write("Enter new description (or press Enter to keep current): ");
                    var description = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(description)) description = product.Description;

                    Console.Write("Enter new price (or press Enter to keep current): ");
                    var priceInput = Console.ReadLine();
                    decimal price = product.Price;
                    if (!string.IsNullOrWhiteSpace(priceInput) && !decimal.TryParse(priceInput, out price))
                    {
                        Console.WriteLine("Invalid price. Keeping current value.");
                        price = product.Price;
                    }

                    Console.WriteLine("Available Categories:");
                    var categories = _inventoryManager.GetAllCategories();
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"  {category.Id}. {category.Name}");
                    }

                    Console.Write("Enter new category ID (or press Enter to keep current): ");
                    var categoryInput = Console.ReadLine();
                    int categoryId = product.CategoryId;
                    if (!string.IsNullOrWhiteSpace(categoryInput) && !int.TryParse(categoryInput, out categoryId))
                    {
                        Console.WriteLine("Invalid category ID. Keeping current value.");
                        categoryId = product.CategoryId;
                    }

                    if (_inventoryManager.UpdateProduct(id, name, description, price, categoryId))
                    {
                        Console.WriteLine("Product updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update product.");
                    }
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        private void DeleteProduct()
        {
            Console.WriteLine("\n--- Delete Product ---");
            ViewAllProducts();
            Console.Write("Enter product ID to delete: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (_inventoryManager.DeleteProduct(id))
                {
                    Console.WriteLine("Product deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to delete product.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        private void ViewProductsByCategory()
        {
            Console.WriteLine("\n--- Products by Category ---");
            var categories = _inventoryManager.GetAllCategories();

            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
                return;
            }

            Console.WriteLine("Available Categories:");
            foreach (var category in categories)
            {
                Console.WriteLine($"  {category.Id}. {category.Name}");
            }

            Console.Write("Enter category ID: ");
            if (int.TryParse(Console.ReadLine(), out int categoryId))
            {
                var products = _inventoryManager.GetProductsByCategory(categoryId);
                var category = _inventoryManager.GetCategoryById(categoryId);

                Console.WriteLine($"\nProducts in category '{category?.Name ?? "Unknown"}':");
                if (products.Count == 0)
                {
                    Console.WriteLine("No products found in this category.");
                }
                else
                {
                    foreach (var product in products)
                    {
                        Console.WriteLine(product);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid category ID.");
            }
        }

        private void InventoryOperations()
        {
            Console.Clear();
            Console.WriteLine("=== INVENTORY OPERATIONS ===");
            Console.WriteLine("1. Stock In");
            Console.WriteLine("2. Sell Product");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StockIn();
                    break;
                case "2":
                    SellProduct();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        private void StockIn()
        {
            Console.WriteLine("\n--- Stock In ---");
            ViewAllProducts();
            Console.Write("Enter product ID to stock in: ");

            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                var product = _inventoryManager.GetProductById(productId);
                if (product != null)
                {
                    Console.WriteLine($"Current stock: {product.Quantity}");
                    Console.Write("Enter quantity to add: ");

                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        if (_inventoryManager.StockIn(productId, quantity))
                        {
                            Console.WriteLine($"Successfully added {quantity} items. New stock: {product.Quantity}");
                        }
                        else
                        {
                            Console.WriteLine("Failed to stock in.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity.");
                    }
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid product ID.");
            }
        }

        private void SellProduct()
        {
            Console.WriteLine("\n--- Sell Product ---");
            ViewAllProducts();
            Console.Write("Enter product ID to sell: ");

            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                var product = _inventoryManager.GetProductById(productId);
                if (product != null)
                {
                    Console.WriteLine($"Available stock: {product.Quantity}");
                    Console.Write("Enter quantity to sell: ");

                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        if (_inventoryManager.SellProduct(productId, quantity))
                        {
                            decimal totalPrice = product.Price * quantity;
                            Console.WriteLine($"Successfully sold {quantity} items for ${totalPrice:F2}. Remaining stock: {product.Quantity}");
                        }
                        else
                        {
                            Console.WriteLine("Failed to sell. Check if sufficient stock is available.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity.");
                    }
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid product ID.");
            }
        }

        private void ShowReports()
        {
            Console.Clear();
            Console.WriteLine("=== REPORTS ===");
            Console.WriteLine("1. Low Stock Report");
            Console.WriteLine("2. Inventory Summary");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowLowStockReport();
                    break;
                case "2":
                    ShowInventorySummary();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        private void ShowLowStockReport()
        {
            Console.WriteLine("\n--- Low Stock Report ---");
            Console.Write("Enter stock threshold (default 5): ");
            var input = Console.ReadLine();
            int threshold = 5;

            if (!string.IsNullOrWhiteSpace(input))
            {
                int.TryParse(input, out threshold);
            }

            var lowStockProducts = _inventoryManager.GetLowStockProducts(threshold);

            if (lowStockProducts.Count == 0)
            {
                Console.WriteLine($"No products with stock <= {threshold} found.");
            }
            else
            {
                Console.WriteLine($"Products with stock <= {threshold}:");
                foreach (var product in lowStockProducts)
                {
                    var category = _inventoryManager.GetCategoryById(product.CategoryId);
                    Console.WriteLine($"{product} | Category: {category?.Name ?? "Unknown"}");
                }
            }
        }

        private void ShowInventorySummary()
        {
            Console.WriteLine("\n--- Inventory Summary ---");
            var products = _inventoryManager.GetAllProducts();
            var categories = _inventoryManager.GetAllCategories();

            Console.WriteLine($"Total Categories: {categories.Count}");
            Console.WriteLine($"Total Products: {products.Count}");
            Console.WriteLine($"Total Items in Stock: {products.Sum(p => p.Quantity)}");
            Console.WriteLine($"Total Inventory Value: ${products.Sum(p => p.Price * p.Quantity):F2}");

            if (categories.Count > 0)
            {
                Console.WriteLine("\nProducts per Category:");
                foreach (var category in categories)
                {
                    var categoryProducts = _inventoryManager.GetProductsByCategory(category.Id);
                    Console.WriteLine($"  {category.Name}: {categoryProducts.Count} products, {categoryProducts.Sum(p => p.Quantity)} items");
                }
            }
        }
    }
}
