# Autoryzacja w ASP

_**UWAGA:** Tyle mi się udalo na razie zrozumieć. Mogą być jakieś błędy niestety, używacie na własną odpowiedzialnoć. BUHAHAHA!_

## Ogólny wstęp

O ile rozumiem, są dwa terminy:
- *Authentication* oznacza logowanie użytkowników 
- *Authorization* oznacza przypisywanie użytkownikom określonych uprawnień (ten może to, a tamten tamto, ale tego już nie). O tym właśnie jest ten Memdump.

W ASP jest możliwych kilka podejść do Autoryzacji; poniżej co ciekawsze (które w miarę ogarnąłem):
- *Role-based* najprostrza i na razie ta stosowana przez nas. Urzytkownikom systemu przypisuje się konkretne role i z nich wynikają uprawnienia. Na przykład zbieracz jabłek powinien móc wejść do sadu, a sprzątaczka do kanciapy z mopem. Ktoś może zbierać jabłka i być sprzątaczką jednocześnie.
- *Claim-based* tutaj użytkwnikom przypisujemy pary klucz-wartość i z nimi coś robimy (na przykład `wiek -> N` i wymagamy pełnoletniości).
- [*Resource-based*](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/resourcebased?view=aspnetcore-2.1) służy to tego, żeby użytkownik mógł edytować __swoje__ posty.
- *Policy-based* taki kombajn, gdzie opisujemy bardziej złożone reguły dostępu (na przykład rozwiązanie może edytować *moderator lub jego właściciel* -- mamy jednocześnie role-based i resource-based).


## Role-Based Authorization

Informacje o rolach sa przechowywane w tabeli `AspNetRoles`. Domyślne role to głupi string razem z kluczem -- tylko przypisujemy użytkownikom role (napisy), do których należą.

Zeby móc przypisać jakąś rolę, tudzież pewnie jakkolwiek inaczej z niej skorzystać, musi się ona znaleźć w bazie danych (zob. metoda `archive.Startup.CreateUserRoles()`).

Informacje o tym, który użytkownik ma jaką rolę, są trzymane w tabeli `AspNetUserRoles`.

Ponieważ jak zasygnalizowałem we wstępie, rola tylko daje pewne uprawnienia, to w szczególności nie ma tam struktury hierarchicznej (np. moderator może wszystko, co użytkownik). Stąd trzeba dodawać użytkownikowi rolę, żeby dać większe uprawnienia, ale nie usuwać z tych, które już ma (jeżeli nie chcemy mu tych uprawnień odebrać). Z grubsza chyba coś jak grupa użytkownika na Linuxie.

Żeby wymagać od uzytkownika posiadania pewnej roli do wykoania akcji kntrolera, oznaczamy ją przez `[Authorize(Roles = "NAZWA ROLI")]` (u nas nazwy ról są wpisane w stałe). Można też podać kilka możliwych ról (wtedy wystarczy mieć którąkolwiek), bądź oznaczyć całą klasę kontrolera atrybutem `[Authenticated(...)]`, żeby dodać domyślne wymagania na wszystkie akcje. Polecam obejrzeć przykłady rozpisane w [dokumentacji](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-2.2#adding-role-checks).

### Ciekawostki

1. Po dodaniu użytkownikowi roli nie zobaczymy tego od razu. Trzeba się wylogować i zalogować na nowo, żeby ASP się ogarnął.

## Resuorce/Policy-based Authorization

To pewnie dopiszę jak będę robił edytowanie rozwiązań, bo będzie mi tam potrzebne, ale jeszcze tego nie ogarnąłem. Stay tuned.

## Linki

1. [Tu](https://social.technet.microsoft.com/wiki/contents/articles/51333.asp-net-core-2-0-getting-started-with-identity-and-role-management.aspx) jest napisane, jak skonfigurować role (w szczególności jak wstawić odpowiednie wpisy do bazy danych, co jest najmniej trywialną częścią).
2. Żeby w kodzie dodać użytkownikowi rolę, używamy klasy `UserManager`, która jest dostępna jako service (zobacz pierwszy commit w tym branchu i `HomeController`).
3. Główna dokumentacja jest [tu i w kolejnych sekcjach](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/introduction?view=aspnetcore-2.2).