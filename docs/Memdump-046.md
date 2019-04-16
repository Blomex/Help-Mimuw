# Memdump z #46 "Kody egzaminów i przedmiotów"

## Baza danych

1. `ApplicationDbContextModelSnapshot.cs` reprezentuje najnowszy stan bazy danych, na podstrawie którego EF tworzy kolejne migracje.

2. [Mapowanie typów w Npgsql](https://www.npgsql.org/doc/types/basic.html)

3. Cofanie nałożonych migracji:
    - Polecenie `Update-Database -Migration RebuiltDatabase` przwracabazę danych do stanu takiego, jaki jest po migracji `RebuiltDatabse` (przydatne, jak chcemy na przykład cofnąć swoją ostatnią migrację)
	- `Remove-Migration` usunie pliki ostatnio wygenerowanej migracji
	- Ciąg poleceń pozwalających przywrócić BD, usunąc ostatnią migrację, wygenerować ją raz jeszcze (może zaktualizowaną) i nałożyć na baze danych:
	```
	Update-Database -Migration RebuiltDatabase
	Remove-Migration
	Add-Migration Shortcuts
	Update-Database
	```


## Kontrolery

1. Jak chcemy w jednym kontrolerze wywołać funkcję innego kontrolera, to w `Startup` robimy:
```CS
services.AddMvc().AddControllersAsServices()
```
I wtedy kontrolery będą dostępne jako zależonści (parametry konstruktora; jak logger i repozytorium).

2. Niemniej zwrócenie widoku z innego kontrolera nie jest takie proste. Żeby zadziałało wymaga podania bezwzględnych ścierzek do plików widoków w tym drugim kontrolerze (zob `HomeController.Shortcut()`)

3. `RedirectToAction` to zwykłe przekierowanie HTTP

## Router

1. Podobno nie można mieć dwóch parametrów opcjonalnych.