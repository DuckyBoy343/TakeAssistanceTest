using Dapper;
using MediatR;
using System.Data;

namespace BackendAPI.Features.Schools
{
    public class GetSchoolByIdQuery : IRequest<School?>
    {
        public int Id { get; set; }
    }

    public class GetSchoolByIdHandler : IRequestHandler<GetSchoolByIdQuery, School>
    {
        private readonly IDbConnection _db;
        //escribir ctor para generar el constructor
        public GetSchoolByIdHandler(IDbConnection db)
        {
            _db = db;
        }
        public async Task<School> Handle(GetSchoolByIdQuery Request, CancellationToken cancelationToken)
        {
            var sql = "SELECT * FROM Schools WHERE schoolid = @Id";

            return await _db.QueryFirstOrDefaultAsync<School>(sql, new { Request.Id });

        }
    }
    
}
