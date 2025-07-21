using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using InventoryManagement.Models; // import namespace models

namespace InventoryManagement.Logic
{
    public class InventoryManager
    {
        private readonly string _dataFilePath = "inventory_data.json";
        private List<Product> _products;
        private List<Category> _categories;
        private int _nextProductId = 1;
        private int _nextCategoryId = 1;

        public InventoryManager()
        {
            _products = new List<Product>();
            _categories = new List<Category>();
            LoadData();
        }

        // category CRUD Operations
        public void AddCategory(string name, string description)
        {
            var category = new Category
            {
                Id = _nextCategoryId++,
                Name = name,
                Description = description
            };
            _categories.Add(category);
            SaveData();
        }

        public List<Category> GetAllCategories()
        {
            return _categories.ToList();
        }

        public Category? GetCategoryById(int id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        public bool UpdateCategory(int id, string name, string description)
        {
            var category = GetCategoryById(id);
            if (category != null)
            {
                category.Name = name;
                category.Description = description;
                SaveData();
                return true;
            }
            return false;
        }

        public bool DeleteCategory(int id)
        {
            var category = GetCategoryById(id);
            if (category != null)
            {
                // check if any products use this category
                if (_products.Any(p => p.CategoryId == id))
                {
                    return false; // cannot delete category with products
                }
                _categories.Remove(category);
                SaveData();
                return true;
            }
            return false;
        }

        // product CRUD Operations
        public void AddProduct(string name, string description, decimal price, int quantity, int categoryId)
        {
            var product = new Product
            {
                Id = _nextProductId++,
                Name = name,
                Description = description,
                Price = price,
                Quantity = quantity,
                CategoryId = categoryId
            };
            _products.Add(product);
            SaveData();
        }

        public List<Product> GetAllProducts()
        {
            return _products.ToList();
        }

        public Product? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public bool UpdateProduct(int id, string name, string description, decimal price, int categoryId)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                product.Name = name;
                product.Description = description;
                product.Price = price;
                product.CategoryId = categoryId;
                SaveData();
                return true;
            }
            return false;
        }

        public bool DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                _products.Remove(product);
                SaveData();
                return true;
            }
            return false;
        }

        // inventory Operations
        public bool StockIn(int productId, int quantity)
        {
            var product = GetProductById(productId);
            if (product != null && quantity > 0)
            {
                product.Quantity += quantity;
                SaveData();
                return true;
            }
            return false;
        }

        public bool SellProduct(int productId, int quantity)
        {
            var product = GetProductById(productId);
            if (product != null && quantity > 0 && product.Quantity >= quantity)
            {
                product.Quantity -= quantity;
                SaveData();
                return true;
            }
            return false;
        }

        public List<Product> GetLowStockProducts(int threshold = 5)
        {
            return _products.Where(p => p.Quantity <= threshold).ToList();
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _products.Where(p => p.CategoryId == categoryId).ToList();
        }

        // data Persistence
        private void SaveData()
        {
            try
            {
                var data = new InventoryData
                {
                    Products = _products,
                    Categories = _categories
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(_dataFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    string jsonString = File.ReadAllText(_dataFilePath);
                    var data = JsonSerializer.Deserialize<InventoryData>(jsonString);

                    if (data != null)
                    {
                        _products = data.Products ?? new List<Product>();
                        _categories = data.Categories ?? new List<Category>();

                        // Update next IDs
                        _nextProductId = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
                        _nextCategoryId = _categories.Count > 0 ? _categories.Max(c => c.Id) + 1 : 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                _products = new List<Product>();
                _categories = new List<Category>();
            }
        }
    }
}
