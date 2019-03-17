# IO

Building app on heroku:
buildpack: https://github.com/jincod/dotnetcore-buildpack
app-url: http://help-mimuw.herokuapp.com/
dashboard: https://dashboard.heroku.com/apps/help-mimuw


Building app on localhost:
```make run```

Generating migrations:
```make NAME=name_of_migration_here migrate```

Applying migrations:
```make update```

Inserting sample data:
```make insert```

comnecting to heroku postgreSQL remotely
```heroku pg:psql postgresql-symmetrical-49981 --app help-mimuw```
