using System;
using Dapper;
using System.Linq;
using MLManager.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MLManager.Services
{
    //Should rename this the user service and make it able to handle permissions also
    public class PermissionService : IPermissionService
    {
        private readonly MLManagerContext _ctx;

        public PermissionService(MLManagerContext ctx)
        {
            _ctx = ctx;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return _ctx.DisposeAsync();
        } 

        public async Task<bool> HasDatasetAccess(int userId,Guid datasetId,PermissionLevel level)
        {
            return (await _ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<bool>(@"
                
            ",new
            {
                userId,
                datasetId,
                level
            }));
        }

        public async Task<bool> HasDatasetSchemaAccess(int userId,Guid datasetId,PermissionLevel level)
        {
            return (await _ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<bool>(@"
                
            ",new
            {
                userId,
                datasetId,
                level
            }));
        }

        //Might should rename this to 'global' permission
        public async Task<bool> HasPermission(int userId,int accountId,PermissionType type,PermissionLevel level)
        {
            return level == (level & await GetPermissionLevel(userId,accountId,type));
        }

        public async Task<PermissionLevel> GetPermissionLevel(int userId,int accountId,PermissionType type)
        {
             Permission permission = await _ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<Permission>(@"
                    SELECT * FROM public.permissions
                    WHERE user_id = @userId AND account_id = @accountId AND permission_type = @type;
                ",new
                {
                    userId,
                    accountId,
                    type
                });   

            if(permission == null)
            {
                return PermissionLevel.NONE;
            }

            return permission.PermissionLevel;
        }
    }
}