using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolutionName.Application.Features.Queries.Products.GetAll;

namespace SolutionName.WebAPI.Controllers.Business
{   
    //[ClaimAccess(Claim = ClaimKeyConsts.Admin)]
    public class ProductController : BaseController
    {
        readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get(GetProductQueryRequest request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
