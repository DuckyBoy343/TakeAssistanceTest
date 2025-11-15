using Microsoft.AspNetCore.Mvc;
using MediatR;
using BackendAPI.Features.Students;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var query = new GetStudentByIdQuery { Id = id };

            var student = await _mediator.Send(query);

            return student != null ? Ok(student) : NotFound();
        }
    }
}
