using System;

namespace MLManager.Database
{
    [Flags]    
    public enum PermissionLevel
    {
        READ = 1,
        WRITE = 2,
        CREATE = 4
    }
}