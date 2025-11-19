using Dapper;
using MediatR;
using System.Data;

namespace BackendAPI.Features.Schools
{
    public class GetAllSchoolsQuery: IRequest<IEnumerable<School>>
    {

    }
    
    public class GetAllSchoolsHandler : IRequestHandler<GetAllSchoolsQuery, IEnumerable<School>>
    {
        private readonly IDbConnection _db;

        public GetAllSchoolsHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<School>> Handle(GetAllSchoolsQuery request, CancellationToken cancellationToken)
        {
            var sql = @"SELECT SchoolId, Name FROM Schools";
            return await _db.QueryAsync<School>(sql);
        }
    }
}
