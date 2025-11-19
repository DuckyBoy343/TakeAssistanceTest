using BackendAPI.Features.Schools;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController : Controller
    {
        private readonly IMediator _mediator;
        public SchoolController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchools()
        {
            var query = new GetAllSchoolsQuery();
            var schools = await _mediator.Send(query);
            return Ok(schools);
        }
    
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSchoolById(int id)
        {
            var query = new GetSchoolByIdQuery { Id = id };
            var school = await _mediator.Send(query);
            return school != null ? Ok(school) : NotFound();
        }
    }
}