using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManager.Application.Common.Interfaces;

namespace ProductManager.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }

            entity.Update(
                entity.Name,
                entity.Description,
                entity.Price,
                false
            );
            
            await _context.SaveChangesAsync(cancellationToken);

            return;
        }
    }
}
