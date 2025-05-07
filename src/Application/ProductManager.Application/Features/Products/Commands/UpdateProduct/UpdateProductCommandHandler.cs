using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManager.Application.Common.Interfaces;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }


            // Verifica se já existe outro produto com o mesmo código
            if (entity.Code != request.Code)
            {
                var existingProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.Code == request.Code && p.Id != request.Id, cancellationToken);

                if (existingProduct != null)
                {
                    throw new InvalidOperationException($"A product with code '{request.Code}' already exists.");
                }
            }


            entity.Update(
                request.Name ?? throw new ArgumentNullException(nameof(request.Name)),
                request.Description,
                request.Price > 0 ? request.Price : throw new ArgumentException("Price must be greater than zero", nameof(request.Price)),
                request.IsActive
            );

            await _context.SaveChangesAsync(cancellationToken);

            return;
        }
    }
}
