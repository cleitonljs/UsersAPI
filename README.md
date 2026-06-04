iniciar container do MySql:
	docker run -d -e MYSQL_ROOT_PASSWORD=SenhaForte123 -p 3306:3306 mysql:latest

parar container:
	docker stop <nome> ou <ID>

excluir container:
	docker rm <nome> ou <ID>
	
para conectar no mysql via dbbeaver:
	na janela de conexão: driver properties, marcar allowPublicKeyRetrieval = true
	
Iniciar container do rabbit no docker:
	docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management

Abrir painel do rabbit:
	http://localhost:15672

Criar migration do EF:
	dotnet ef migrations add CriacaoBanco  --project .\Infrastructure\Infrastructure.csproj --startup-project .\UsersAPI\UsersAPI.csproj

Executar a migration do EF:
	dotnet ef database update --project .\Infrastructure\Infrastructure.csproj --startup-project .\UsersAPI\UsersAPI.csproj