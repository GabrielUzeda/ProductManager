using MediatR;
using ProductManager.Application.DTOs;

namespace ProductManager.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public int Id { get; set; }
    }
}
