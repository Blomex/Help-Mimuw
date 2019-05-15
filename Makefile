DBNAME=archive_db
SQL_DROP=scripts/DropTables.sql
SQL_SAMPLE=scripts/SampleData.sql
TESTDB=archive_test_db

.PHONY: clean_test
clean_test:
	psql -U postgres -c 'DROP DATABASE IF EXISTS ${TESTDB}'

.PHONY: env
env:
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools"'

.PHONY: run
run:
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools" && cd archive && dotnet run .'

.PHONY: test
test:
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools" && cd archive.Tests && dotnet test .'

.PHONY: test_mock
test_mock: clean_test
	cp -n ./archive/appsettings-local.json ./archive/.appsettings-local.json_tmp
	psql -U postgres -c 'CREATE DATABASE ${TESTDB}'
	awk '{gsub("Database=.*;UserId", "Database=archive_test_db;UserId");print}' ./archive/.appsettings-local.json_tmp > ./archive/appsettings-local.json
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools" && cd archive && dotnet ef database update'
	psql -d ${TESTDB} -U postgres -a -f ${SQL_SAMPLE}
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools" && cd archive.Tests && dotnet test .'
	mv ./archive/.appsettings-local.json_tmp ./archive/appsettings-local.json


.PHONY: migrate
migrate:
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools" && cd archive && dotnet ef migrations add ${NAME}'

.PHONY: update
update:
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools" && cd archive && dotnet ef database update'

.PHONY: insert
insert:
	psql -d ${DBNAME} -U postgres -a -f ${SQL_SAMPLE}
