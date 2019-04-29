using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace archive.Data.Enums
{
    public class UserRoles
    {
        public const string NEW_USRER = "NEW_USER";
        public const string REGULAR_USER = "REGULAR_USER";
        public const string ARCHUSER = "ARCHUSER";

        public static readonly string[] AllRoles = { NEW_USRER, REGULAR_USER, ARCHUSER };
    }
}
