using System;
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
        private readonly IPermissionService _permissionService;
        public DatasetService(MLManagerContext ctx,IPermissionService permissionService)
        {
            _ctx = ctx;
            _permissionService = permissionService;
        }

        public async Task<Dataset> CreateDataset(int userId, int accountId,string datasetName)
        {
            //Should check if the user has permissions to create the dataset for the account
            if(!await _permissionService.HasPermission(userId,accountId,PermissionType.Datasets,PermissionLevel.CREATE))
            {
                throw new PermissionException(userId,PermissionType.Datasets,PermissionLevel.CREATE,
                $"User does not have permission to create a dataset for the selected account.");
            }

            return await _ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<Dataset>(@"
                INSERT INTO public.datasets
                (account_id,database_name)
                VALUES
                (@accountId,@datasetName)
                RETURNING *;
            ",new
            {
                accountId,
                datasetName
            });
        }

        public async Task<DatasetSchema> CreateNewSchema(int userId,Guid datasetId,string schema)
        {
            if(!await _permissionService.HasDatasetSchemaAccess(userId,datasetId,PermissionLevel.CREATE))
            {
                throw new PermissionException(userId,PermissionType.DatasetSchema,PermissionLevel.CREATE,
                $"User does not have permission to create a new schema for the selected dataset.");
            }

            return await _ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<DatasetSchema>($@"
                DECLARE currentVersionId INT;
                SELECT currentVersionId = COALESCE(MAX(VersionId),0) FROM public.dataset_schemas
                WHERE dataset_id = @datasetId ORDER BY version_id DESCENDING;

                INSERT INTO public.dataset_schemas
                (dataset_id,version_id,schema)
                VALUES
                (@datasetId,currentVersionId+1,@schema)
                RETURNING *
            ",new
            {
                datasetId,
                schema
            });
        }

        public async Task<DatasetSchema> GetCurrentSchema(Guid datasetId)
        {
            return await _ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<DatasetSchema>(@"
                SELECT * FROM public.dataset_schemas
                WHERE dataset_id = @datasetId ORDER BY version_id DESCENDING LIMIT 1;
            ",new
            {
                datasetId
            });
        }

        public async Task<DatasetSchema> GetCurrentSchema(int userId, Guid datasetId)
        {
            //Should check if the user has permissions to create the dataset for the account
            if(!await _permissionService.HasDatasetSchemaAccess(userId,datasetId,PermissionLevel.READ))
            {
                throw new InvalidOperationException($"User {userId} does not have permission to access schemas for dataset {datasetId}");
            }

            return await GetCurrentSchema(datasetId);
        }

        public async Task<DatasetSchema> GetSchema(int userId,Guid datasetId,int versionId)
        {
            //Should check if the user has permissions to create the dataset for the account
            if(!await _permissionService.HasDatasetSchemaAccess(userId,datasetId,PermissionLevel.READ))
            {
                throw new InvalidOperationException($"User {userId} does not have permission to access schemas for dataset {datasetId}");
            }

            return await _ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<DatasetSchema>(@"
                SELECT * FROM public.dataset_schemas
                WHERE account_id = @accountId AND dataset_id = @datasetId AND version_id = @versionId;
            ",new
            {
                datasetId,
                versionId
            });  
        }
    }
}