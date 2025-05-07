using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManager.Application.Common.Interfaces;
using ProductManager.Application.DTOs;

namespace ProductManager.Application.Features.Products.Queries.SearchProducts
{
    public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SearchProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            var searchTerm = request.SearchTerm?.ToLower() ?? string.Empty;

            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive && 
                          (p.Code.ToLower().Contains(searchTerm) || 
                           p.Name.ToLower().Contains(searchTerm)))
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
