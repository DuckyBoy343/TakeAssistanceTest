using Dapper;
using MediatR;
using System.Data;

namespace BackendAPI.Features.Students
{
    public class GetStudentByIdQuery : IRequest<Student?>
    {
        public int Id { get; set; }
    }

    public class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, Student?>
    {
        private readonly IDbConnection _db;

        public GetStudentByIdHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Student> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var sql = "SELECT * FROM Students WHERE StudentId = @Id";

            return await _db.QueryFirstOrDefaultAsync<Student>(sql, new { request.Id });
        }

    }
}
