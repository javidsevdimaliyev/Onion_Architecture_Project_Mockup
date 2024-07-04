using MediatR;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using SolutionName.Domain.Entities;

namespace SolutionName.Application.Features.Queries.Products.GetAll
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQueryRequest, GetProductQueryResponse>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IReadRepository<ProductEntity, int> _productRepository;
        public GetProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productRepository = unitOfWork.ReadRepository<ProductEntity, int>();
        }


        public async Task<GetProductQueryResponse> Handle(GetProductQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.FindAllAsync(cancellationToken);
            return new() { Products = result };
        }
    }
}
