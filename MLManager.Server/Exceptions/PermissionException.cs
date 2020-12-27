using System;
using MLManager.Database;

namespace MLManager.Services
{
    public class PermissionException : Exception
    {
        public int UserId { get; set; }
        public PermissionType PermissionType { get; set; }
        public PermissionLevel PermissionLevel { get; set; }

        public PermissionException(int userId,PermissionType type,PermissionLevel level,string message)
            : base(message) 
            {
                UserId = userId;
                PermissionType = type;
                PermissionLevel = level;
            }
        public PermissionException(int userId,PermissionType type,PermissionLevel level)
            : base($"User {userId} does not have {level} access to {type}")
        {
                UserId = userId;
                PermissionType = type;
                PermissionLevel = level;
        }
    }
}