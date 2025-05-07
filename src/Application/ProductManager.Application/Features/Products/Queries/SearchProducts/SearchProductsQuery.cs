using MediatR;
using System.Collections.Generic;
using ProductManager.Application.DTOs;

namespace ProductManager.Application.Features.Products.Queries.SearchProducts
{
    public class SearchProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
