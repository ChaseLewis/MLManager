using System;

namespace MLManager.Database
{
    [Flags]    
    public enum PermissionLevel
    {
        NONE = 0,
        READ = 1,
        WRITE = 2,
        READWRITE = 3,
        CREATE = 4,
        DELETE = 8
    }
}