	// Singleton: will create and share a single instance of the serivce through the application life
	// Scoped: will create and share an instance of the service per request
	// Transient: will create and share an instance of the service every time

	dotnet run --launch-profile https

	dotnet publish -c Release -o ./publish