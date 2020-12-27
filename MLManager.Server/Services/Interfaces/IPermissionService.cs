using System;
using MLManager.Database;
using System.Threading.Tasks;

namespace MLManager.Services
{
    public interface IPermissionService
    {
        Task<PermissionLevel> GetPermissionLevel(int userId,int accountId,PermissionType type);
        Task<bool> HasPermission(int userId,int accountId,PermissionType type,PermissionLevel level);
        Task<bool> HasDatasetSchemaAccess(int userId,Guid datasetId,PermissionLevel level);
        Task<bool> HasDatasetAccess(int userId,Guid datasetId,PermissionLevel level);

    }
}