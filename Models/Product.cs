using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Price: ${Price:F2}, Quantity: {Quantity}, Category ID: {CategoryId}";
        }
    }
}
