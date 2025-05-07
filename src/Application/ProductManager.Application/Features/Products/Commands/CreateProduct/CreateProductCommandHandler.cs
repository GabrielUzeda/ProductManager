using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManager.Application.Common.Interfaces;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Verifica se já existe um produto com o mesmo código
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Code == request.Code, cancellationToken);

            if (existingProduct != null)
            {
                // Atualiza o produto existente
                existingProduct.Update(
                    request.Name ?? throw new ArgumentNullException(nameof(request.Name)),
                    request.Description,
                    request.Price > 0 ? request.Price : throw new ArgumentException("Price must be greater than zero", nameof(request.Price)),
                    true
                );
                
                await _context.SaveChangesAsync(cancellationToken);
                return existingProduct.Id;
            }

            // Cria um novo produto
            var entity = new Product(
                request.Code ?? throw new ArgumentNullException(nameof(request.Code)),
                request.Name ?? throw new ArgumentNullException(nameof(request.Name)),
                request.Description,
                request.Price > 0 ? request.Price : throw new ArgumentException("Price must be greater than zero", nameof(request.Price))
            );

            _context.Products.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);


            return entity.Id;
        }
    }
}
