using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archive.Data.Entities;
using Task = System.Threading.Tasks.Task;

namespace archive.Services.Users
{
    public interface IAchievementsService
    {
        /// <summary>
        /// Dodaje do bazy danych achievement, jeżeli nie istnieje.
        /// </summary>
        /// <param name="achievement">Acheievement do zapisania. Numeryczne ID jest ignorowane,
        /// sprawdzane jest tylko <c>NormalizedName</c></param>
        /// <returns><c>true</c> jeżeli achievement został dodany do bazy; <c>false</c> jeżeli nie został dodany,
        /// bo achievement o tym identyfikatorze już istniał</returns>
        Task<bool> DeclareAchievement(Achievement achievement);

        /// <summary>
        /// Przyznaje użytkownikowi achievement o podanym identyfikatorze.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="achievementIdentifier"></param>
        Task GrantAchievement(ApplicationUser user, string achievementIdentifier);

        /// <summary>
        /// Sprawdza, czy użytkownik ma achievement o podanym identyfikatorze.
        /// </summary>
        /// <param name="user">Użytkownik, u którego szukamy achievementa</param>
        /// <param name="achievementNormalizedName">Znormalizowana nazwa achievementa</param>
        /// <returns>Czy <c>user</c> ma achievement identyfikowany przez <c>achievementNormalizedName</c></returns>
        Task<bool> HasAchievement(ApplicationUser user, string achievementNormalizedName);

        /// <summary>
        /// Zwraca wszyskie achievementy użytkownika.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Wszystkie achievementy, które zostały przyznane użytkownikowi <c>user</c></returns>
        Task<ICollection<Achievement>> UsersAchievements(ApplicationUser user);
    }
}
