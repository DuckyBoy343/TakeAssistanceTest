using Dapper;
using MediatR;
using System.Data;

namespace BackendAPI.Features.Students
{
    public class GetAllStudentsQuery : IRequest<IEnumerable<Student>>
    {
    }

    public class GetAllStudentsHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<Student>>
    {
        private readonly IDbConnection _db;

        public GetAllStudentsHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Student>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            var sql = @"SELECT StudentId, FullName, GradeId FROM Students";

            return await _db.QueryAsync<Student>(sql);
        }
    }
}
