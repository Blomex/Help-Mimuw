.PHONY: run
run:
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools" && cd archive && dotnet run .'

.PHONE: test
test:
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools" && cd archive.Tests && dotnet test .'

.PHONY: migrate
migrate:
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools" && cd archive && dotnet ef migrations add ${NAME}'

.PHONY: update
update:
	bash -c 'export PATH="$$PATH:/home/${USER}/.dotnet/tools" && cd archive && dotnet ef database update'


DBNAME=archive_db
FILE=misc/SampleData.sql

.PHONY: insert
insert:
	psql -d ${DBNAME} -U postgres -a -f ${FILE}
