using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace archive.Data.Enums
{
    public class UserRoles
    {
        /// <summary>
        /// Zweryfikowany użytkownik, który może dodawać zadania i rozwiązania
        /// </summary>
        public const string TRUSTED_USER = "TRUSTED_USER";

        /// <summary>
        /// Moderator, który może dodawać przedmioty, egzaminy i edytować nieswoje rozwiązania
        /// </summary>
        public const string MODERATOR = "MODERATOR";

        /// <summary>
        /// Imperator, który dodatkowo może mianować moderatorów
        /// </summary>
        public const string IMPERATOR = "IMPERATOR";

        public static readonly string[] AllRoles = { TRUSTED_USER, MODERATOR, IMPERATOR };
    }
}
