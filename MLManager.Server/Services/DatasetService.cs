using Dapper;
using System.Linq;
using MLManager.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MLManager.Services
{
    public class DatasetService : IDatasetService
    {
        private readonly MLManagerContext _ctx;
        public DatasetService(MLManagerContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Dataset> CreateDataset(int userId,string datasetName)
        {
            return await _ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<Dataset>(@"
                INSERT INTO public.datasets
                (""UserId"",""DatasetName"")
                VALUES
                (@UserId,@DatasetName)
                RETURNING *;
            ",new
            {
                UserId = userId,
                DatasetName = datasetName
            });
        }

        public async Task<DatasetSchema> GetCurrentSchema(int userId, int datasetId)
        {
            return await _ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<DatasetSchema>(@"
                SELECT * FROM public.dataset_schemas
                WHERE ""UserId"" = @UserId && ""DatasetId"" = @DatasetId ORDER BY ""VersionId"" DESCENDING LIMIT 1;
            ",new
            {
                UserId = userId,
                DatasetId = datasetId
            });
        }

        public async Task<DatasetSchema> CreateNewSchema(int userId,int datasetId,string schemaJson)
        {
            return await _ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<DatasetSchema>(@"
                DECLARE currentVersionId INT;
                SELECT currentVersionId = COALESCE(MAX(VersionId),0) FROM public.dataset_schemas
                WHERE UserId = @UserId && DatasetId = @DatasetId ORDER BY VersionId DESCENDING;

                INSERT INTO public.dataset_schemas 
                (UserId,DatasetId,Schema)
                VALUES
                (@UserId,@DatasetId,@Schema)
                RETURNING *;
            ",new
            {
                UserId = userId,
                DatasetId = datasetId,
                Schema = schemaJson
            });

        }

        // public async Task<IEnumerable<Dataset>> GetDatasets(int accountId, int userId,int offset = 0,int size = 25)
        // {
        //     return await _ctx.Datasets
        //     .Where(x => x.UserId == userId)
        //     .Skip(offset)
        //     .Take(size)
        //     .ToListAsync();
        // }
    }
}