using MediatR;
using System.Collections.Generic;
using ProductManager.Application.DTOs;

namespace ProductManager.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
    }
}
