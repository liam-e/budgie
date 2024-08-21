# Budget API

## Installation

In the root directory, using the dotnet CLI tool, install the dependencies by running:

```bash
dotnet restore
```

To create a new migration:

```bash
dotnet ef migrations add InitialCreate
```

To apply the migration:

```bash
dotnet ef database update
```

To run the server:

```bash
dotnet run --launch-profile https
```
