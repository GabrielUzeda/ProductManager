using System.Collections.Generic;
using System.Threading.Tasks;
using ProductManager.Domain.Entities;

namespace ProductManager.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetByCodeAsync(string code);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<bool> SaveChangesAsync();
    }
}
