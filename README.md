# Budget API

## Installation

In the root directory, using the dotnet CLI tool, install the dependencies by running:

```bash
dotnet restore
```

To create a new migration:

```bash
dotnet ef migrations add InitialCreate --project Budgie.API/Budgie.API.csproj
```

To apply the migration:

```bash
dotnet ef database update --project Budgie.API/Budgie.API.csproj
```

To run the server:

```bash
ASPNETCORE_ENVIRONMENT=Development dotnet watch run --launch-profile https --project Budgie.API/Budgie.API.csproj
```

To test:

```bash
ASPNETCORE_ENVIRONMENT=Test dotnet test
```

```bash
rm Budgie.API/Migrations/*
dotnet ef migrations add InitialCreate --project Budgie.API/Budgie.API.csproj
dotnet ef database update --project Budgie.API/Budgie.API.csproj
ASPNETCORE_ENVIRONMENT=Development dotnet watch run --launch-profile https --project Budgie.API/Budgie.API.csproj
```

## To do

- [ ] Chart
- [ ] Categorise transactions page
- [ ] Improve seed data
- [ ] Demo account
- Messages
  - [ ] Signed up
  - [ ] Logged in
  - [ ] Logged out
  - [ ] Transaction created
  - [ ] Budget limit created
- Add data feed
  - [ ] Select source
  - [ ] Make functional
- Upload CSV
  - [ ] Testing
- Summary
  - [ ] Previous periods
- Transaction
  - [ ] Full info
  - [ ] Page
  - [ ] Edit
  - [ ] Delete
- Category
  - [ ] Page
  - [ ] Clean up data/icons
- Clean up design
  - [ ] Figma mockups - establish design language
  - [ ] Reusable layout components
