![modulith cover](https://github.com/david-acm/modulith/blob/main/modulith-cover.png)

Modulith is a `dotnet new template` suite for Modular Monoliths. It streamlines the creation of new .Net solutions and the addition of modules to existing ones.

But, what is a Modular Monolith? Glad you asked. It is a software architecture style to build maintainable applications as a single unit, but in nicely separated modules (Modu-lith, pun intended 🙃). This way, an app benefits from the separation of concerns that microservices offer without the complexities they bring. When you are ready and *if* you need it, you can split a module as a microservice. Best of both worlds 🌎

This is not a new concept. Martin Fowler [explains it here](https://martinfowler.com/bliki/MonolithFirst.html), and Ardalis teaches it [here](https://ardalis.com/introducing-modular-monoliths-goldilocks-architecture/#:~:text=A%20Modular%20Monolith%20is%20a%20software%20architecture%20that,that%20they%20are%20loosely%20coupled%20and%20highly%20cohesive.).

The templates in this project follow the solution structure as taught by [Ardalis](https://github.com/ardalis) in [his course *Modular Monoliths in DotNet*](https://dometrain.com/bundle/from-zero-to-hero-modular-monoliths-in-dotnet/).

# Installing

You can install both templates by running:

TODO: publish it to nuget
```pwsh
nuget install 
```

Or, by cloning the repo and installing it locally running:

``` pwsh
dotnet new install .
```

# Creating new modular solutions

Just run: 

``` pwsh
dotnet new modulith -n eShop --with-module Payments 
```

Here `eShop` is your solution name, and `Payments` is the first module name (defaults to `FirstModule` if not specified).

## Solution directory structure

The previous command creates the following project structure:

- eShop
  - `Users/` 👈 Your first module
  - `eShop.Web/` 👈 Your entry point

Inside `First Module`, you will find the project folders:

- `eShop.Payments/` 👈 Your project code goes here
- `eShop.Payments.Contracts/` 👈 Public contracts other modules can depend on
- `eShop.Payments.Tests/` 👈 Your module tests

## Project dependencies

Since this is a Modular Monolith, there are a few rules that are enforced to guarantee the modularity:

- Every type in `eShop.Payments/` is internal
- This 👆 is enforced by an archUnit test in `eShop.Payments.Tests/`
- The only exception to the last two rules is the static class that configures the services for the module: `UsersModule...Extensions.cs`
- `.Contracts/` and `.Tets/` projects depend on `eShop.Payments/`. The opposite is not possible. This is by design.

\* *You can always change these rules after you have created the solution to suit your needs. But be mindful of why you are changing the rules. For example, it is ok to add an additional public extensions class to configure the application pipeline, while adding a public contract to `eShop.Payments/` is not. We have a project for those.*

# Creating a new module 

⚠️ `cd` into the solution folder. I.e. `eShop/`, then run:

``` pwsh
dotnet new modulith-proj --add-module Shipments --to eShop
```

Here, `Shipments` is the name of your new module. This will create a new module folder with the same three projects as in `Users/`. 

Manually add the reference from the newly created `Shipment.csproj` to the web entry project from your IDE or by running:

``` pwsh
dotnet add eShop.Web/eShop.Web.csproj reference Shipment/eShop.Shipment/eShop.Shipment.csproj
```

# Adding a reference automatically to new modules

We support this, but the .Net SDK does not yet. There is an active PR at [dotnet/sdk #40133](https://github.com/dotnet/sdk/pull/40133). Give it a vote if you'd like this feature:

⚠️ `cd` into the solution folder. I.e. `eShop/`, then run:

``` pwsh
dotnet new modulith-proj --ModuleName Shipments --existingProject eShop.Web/eShop.Web.csproj
```

Here `Shipments` is the name of your new module, and `eShop.Web/eShop.Web.csproj` is the path to your web entry project. If you changed this, make sure you update it to the new path and that is relative to the solution folder.

# Contributing

TODO: New templates should have both solution and project in the same folder

Contributing is very easy. To add your own templates just copy one of the folders in the repository root and change it to fit your needs
