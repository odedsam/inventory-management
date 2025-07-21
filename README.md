# Inventory Management System

A simple C# console application for managing inventory with persistent data storage.

## Features

- **Category Management**: Create, view, update, and delete product categories
- **Product Management**: Full CRUD operations for products with pricing and stock tracking
- **Inventory Operations**: Stock in products and process sales
- **Reports**: Low stock alerts and inventory summaries
- **Data Persistence**: Automatic save/load using JSON file storage

## Quick Start

1. **Setup Project:**
   ```bash
   dotnet new console -n InventorySystem
   cd InventorySystem
   ```

2. **Replace Program.cs** with the provided code

3. **Run Application:**
   ```bash
   dotnet run
   ```

## Usage Flow

1. **First Run**: Create categories (e.g., Electronics, Food, Clothing)
2. **Add Products**: Assign products to categories with prices and initial stock
3. **Manage Inventory**: Use stock in/sell operations to track inventory changes
4. **Monitor**: Check low stock reports and inventory summaries

## Data Storage

- All data is automatically saved to `inventory_data.json`
- Data persists between application sessions
- No database setup required

## Menu Structure

```
Main Menu
├── Manage Categories
├── Manage Products
├── Inventory Operations
├── Reports
└── Exit
```

## Requirements

- .NET 6.0 or later
- System.Text.Json (included in .NET)

## Example Usage

Create a category → Add products → Stock in items → Process sales → View reports
