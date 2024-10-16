PROJECT_NAME=Process
DOCKER_COMPOSE=docker-compose
DOCKER=docker

build:
	@echo "Building Docker image and starting containers..."
	$(DOCKER_COMPOSE) up --build

run-migrations:
	@echo "Running migrations..."
	sudo dotnet ef database update --project "Process"

clean:
	@echo "Stopping and cleaning up Docker containers"
	$(DOCKER_COMPOSE) down
	
