## Baza danych

### Many to many

Niestety Entity Framework na razie nie potrafi w relacje many-to-many i trzeba mu pomóc [tak jak tu opisali](https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration).

### Natural vs Artificial Primary Keys

Problem jest taki: w bazie mamy achievementy identyfikowane niby najlepiej przez jakiś napis, żeby w kodzie łatwo było się odwoływac do konkretnego, tak jak role w ASP. W związku z tym *naturalnym* kluczem wydaje się być właśnie ten napis... z drugiej strony to oznaczałoby trzymanie go w wielu miejscach (foreign key... many to many...), więc było może trochę słabe. Korci żeby dołożyć numeryczne ID...

Okazuje się, że jest to dylemat stary jak świat --- [artykuł](https://sqlstudies.com/2016/08/29/natural-vs-artificial-primary-keys/).

Jako, że tu mamy relację many-to-many, więc klucz na pewno będzie się powtarzał w wielu miejscach, to został użyty sztuczny klucz. BTW wspomniane wyżej role użytkowników używane przez ASP też są tak zrobione.
