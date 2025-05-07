using System;
using ProductManager.Domain.Common;

namespace ProductManager.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        protected Product() { }

        public Product(string code, string name, string description, decimal price)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Price = price > 0 ? price : throw new ArgumentException("Price must be greater than zero", nameof(price));
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string description, decimal price, bool isActive)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Price = price > 0 ? price : throw new ArgumentException("Price must be greater than zero", nameof(price));
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
