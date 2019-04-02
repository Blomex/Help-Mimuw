# Archiwum egzaminów
## Konfiguracja projektu ogólnie
Do uruchomienia aplikacji potrzebny jest zainstalowany i uruchomiony serwer `postgresql` oraz środowisko dotnet, wraz `ef` -- entity framework. 
Po założeniu pustej bazy dane do połączenia się z nią należy ustawić w pliku `archive/appsettings-local.json`, 
który przesłoni globalne ustawienia łączenia się z bazą danych. 

## Skrypty
Do generowania testowych danych służy skrypt `scripts/SampleData.sql`.
Do uruchomienia aplikacji można użyć polecenia `dotnet run`, bądź posłużyć się skryptem `Makefile`, 
który udostępnia również następujące opcje (każdą z nich (oprócz ostatniej) można zrealizować za pomocą polecenia `dotnet`):

Zbudowanie aplikacji: ```make run```

Uruchomienie testów: ```make test```

Wygenerowanie nowej migracji na podstawie schematu w kodzie: ```make NAME=nazwa_migracji migrate```

Zaaplikowanie migracji: ```make update```

Wygenerowanie testowych danych w lokalnej bazie: ```make insert```

## Heroku
Aktualnie aplikacja jest automatycznie budowana z jednego z branchy.
Do automatycznego budowania używamy buildpacka: https://github.com/jincod/dotnetcore-buildpack.
Applikacja znajduje się pod adresem: http://help-mimuw.herokuapp.com/.
Dostęp do środowiska uzyskać można poprzez dashboard: https://dashboard.heroku.com/apps/help-mimuw,
bądź poprzez terminal -- programem-klientem `heroku`.

## Przykładowa konfiguracja
Przykładowy projekt na systemie Manjaro GNU/Linux, jądro w wersji 4.4. 
Postępując analogicznie uda się z pewnością postawić aplikację też na innych systemach.
```
[user@host ~]$ uname -a
Linux 4.4.167-1-MANJARO #1 SMP PREEMPT Tue Dec 18 20:22:29 UTC 2018 x86_64 GNU/Linux
```
### Konfiguracja bazy danych
Instalujemy serwer postgresql:
```
[user@host ~]$ sudo pacman -S postgresql
```

Logujemy się jako użytkownik `postgres`:
```
[user@host ~]$ sudo -iu postgres
```

Inicujemy serwer bazy danych:
```
[postgres@host ~]$ initdb -D /var/lib/postgres/data
```

Zmieniamy użytkownika na pierwotnego:
```
[postgres@host ~]$ exit
```

Uruchamiamy serwer bazy danych, jeżeli chcielibyśmy by był on uruchamiany przy starcie systemu -- wystarczy `systemctl enable postgresql`.
```
[user@host ~]$ sudo systemctl start postgresql
```

Logujemy się do bazy danych jako użytkownik `postgres`:
```
[user@host ~]$ psql -U postgres
```

Tworzymy egzemplarz bazy danych, z ktorego będzie korzystać aplikacja, przy czym nazwa `archive_db` jest arbitralna.
```
postgres=# CREATE DATABASE archive_db;
```

### Konfiguracja środowiska dotnet
Instalujemy pakiet sdk środowiska dotnet:
```
[user@host ~]$ sudo pacman -S dotnet-sdk
```

Instalujemy entity framework.
```
[user@host ~]$ dotnet tool install --global dotnet-ef
```

Ustawiamy odpowiednie ścieżki -- mozna zrobić to permanentnie, np. w `.profile`.
```
export PATH="$PATH:/home/$USER/.dotnet/tools"
```

Ustawiamy w aplikacji dane do logowania dla naszej bazy danych:
```
[user@host archive]$ vim appsettings-local.json 
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=archive_db;UserId=postgres;Password=postgres"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Aplikujemy pliki migracji, które powinny być już w repozytorium, wygenerowane wcześniej:
```
[user@host Archiwum-Egzaminow]$ make update
```

Instalujemy wymagane certyfikaty deweloperskie:
```
[user@host Archiwum-Egzaminow]$ dotnet tool install --global dotnet-dev-certs
```

Instalujemy certyfikaty dla naszej aplikacji:
```
[user@host Archiwum-Egzaminow]$ dotnet dev-certs https
```

Uruchamiamy aplikację:
```
[user@host Archiwum-Egzaminow]$ make run
```

Uruchamiany klienta aplikacji:
```
[user@host Archiwum-Egzaminow]$ chromium https://localhost:5001
```
