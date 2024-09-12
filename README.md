[![CI](https://github.com/david-acm/modulith/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/david-acm/modulith/actions/workflows/ci.yml)
[![NuGet Version](https://img.shields.io/nuget/vpre/Ardalis.Modulith)](https://www.nuget.org/packages/Ardalis.Modulith)

* _⚠️This project is a work in progress and will likely receive many API changes before v2.0.0. Please keep this in mind when using, as there will be breaking changes often._

* _⚠️ Automatic project references for new modules are only supported in .Net SDK 9.0.100-preview.7.* and newer. Please make a [manual project reference](#add-a-reference-to-the-new-module) when using earlier versions._

**🆕 Try UI Module generation with Blazor. Jump to [Modules with UI](#modules-with-ui)**

(originally hosted at **david-acm/modulith** - thanks David for the contribution!)

Modulith is a `dotnet new template` suite for [Modular Monoliths](https://dometrain.com/bundle/from-zero-to-hero-modular-monoliths-in-dotnet/). It streamlines the creation of new .Net solutions and the addition of modules to existing ones.

But, what is a Modular Monolith? Glad you asked. It is a software architecture style to build maintainable applications as a single unit, but in nicely separated modules (Modu-lith, pun intended 🙃). 
 More [about Modular Monoliths](#-about-modular-monoliths).

# 🚀 Quickstart

#### Install the tool running:

```pwsh
dotnet new install Ardalis.Modulith
```

#### Create a new solution:

``` pwsh
dotnet new modulith -n eShop --with-module Payments 
```

`eShop` is your solution name, and `Payments` is the first module name (defaults to `FirstModule` if not specified).

#### Create a new module


``` pwsh
cd eShop
dotnet new modulith --add basic-module --module-name Shipments --solution eShop
```

*⚠️ `cd` into the solution folder to add the module inside the solution.*

`Shipments` is the name of your new module. This will create a new module folder with the same three projects as in `Payments/`. 

#### Add a reference to the new module

Run:

``` pwsh
dotnet add eShop.Web/eShop.Web.csproj reference Shipments/eShop.Shipments/eShop.Shipments.csproj
```

That's it, no need to register the new service -- [but you can](#direct-service-registration). The template scans you assemblies and registers services from your modules.

Happy coding!

Running the solution should show both modules with their default endpoint:

<img src="https://github.com/david-acm/modulith/assets/71415563/94bf15ff-4a4e-4a6c-a3d2-3755926579c5" width="600" />

#### Direct service registration

However, if you prefer more control and less magic, or you want to modify registration class, you can remove the `builder.DiscoverAndRegisterModules();`  in `program.cs` and add the service registration for each module:

```cs
using eShop.Shipments
...
PaymentsModuleServiceRegistrar.ConfigureServices(builder);
```

# 🏛️ Solution directory structure

The previous command creates the following project structure:

- eShop
  - `Users/` 👈 Your first module
  - `eShop.Web/` 👈 Your entry point

Inside `Payments`, the second module added, you will find the project folders:

- `eShop.Payments/` 👈 Your project code goes here
- `eShop.Payments.Contracts/` 👈 Public contracts other modules can depend on
- `eShop.Payments.Tests/` 👈 Your module tests

## Project dependencies

Since this is a Modular Monolith, there are a few rules that are enforced to guarantee the modularity:

- Every type in `eShop.Payments/` is internal
- This is enforced by an [ArchUnit](https://github.com/TNG/ArchUnitNET) test in `eShop.Payments.Tests/`
- The only exception to the last two rules is the static class that configures the services for the module: `PaymentsModuleServiceRegistrar.cs`
- `.Contracts/` and `.Tests/` projects depend on `eShop.Payments/`. The opposite is not possible. This is by design.

\* *You can always change these rules after you have created the solution to suit your needs. But be mindful of why you are changing the rules. For example, it is ok to add an additional public extensions class to configure the application pipeline, while adding a public contract to `eShop.Payments/` is not. We have a project for those.*

## Adding a reference automatically to new modules

This is only supported on .Net 9 preview 6. If you are running an earlier version you will need to run [these commands manually](#add-a-reference-to-the-new-module).

⚠️ `cd` into the solution folder. I.e. `eShop/`, then run:

``` pwsh
dotnet new modulith-proj --ModuleName Shipments --existingProject eShop.Web/eShop.Web.csproj
```

Here `Shipments` is the name of your new module, and `eShop.Web/eShop.Web.csproj` is the path to your web entry project. If you change this, make sure you update it to the new path and that is relative to the solution folder.

# Modules with UI

You can generate a solution with a Blazor UI by using the ```--with-ui```:

``` pwsh
dotnet new modulith -n eShop --with-module Payments --with-ui
```

Running the application will show the following blazor app:

![Screenshot of Blazor app with Payments module](<assets/with-ui.png>)

The app uses [MudBlazor](https://www.mudblazor.com/) as the component library. The template includes a menu item and page for the newly created module with UI whose components are defined in the ```eShop.Payments.UI``` project. We include a link to the Swagger UI page in the _API_ menu item.

The previous command will create a solution with a few additional projects.

- **eShop.UI:** Is the client project that will be compiled to WebAssembly and executed from the browser. This contains the layout and routes components; but most importantly the ```program.cs``` to register the services for the client side application.
- **eShop.Payments.UI:** Is a razor class library where you can define the components specific to that UI module.
- **eShop.Payments.HttpModels** Contains the DTOs used to send requests from the Blazor client project (```eShop.UI```) to the WebApi endpoints in ```eShop.Shipments```.

## Adding new modules with UI

New modules with UI can be added running:

```pwsh
cd eShop
dotnet new modulith --add basic-module --module-name Shipments --solution eShop --with-ui
```

⚠️ New modules with UI can only be added to solutions that were instantiated using the ```-WithUI``` parameter.

However, to allow routing to the newly created module component for the ```Shipments``` module, you need to register the new assembly.

In blazor WebAssembly, the routeable components that are not present in the executing assembly need to be passed as arguments to the ```Router``` component. In this template, this is done using the ```BlazorAssemblyDiscoveryService```. Simply add the following to the ```GetAssemblies``` array:

```cs
typeof(ShipmentsComponent).Assembly
```

After the modification the class should look like this:

```cs
public class BlazorAssemblyDiscoveryService : IBlazorAssemblyDiscoveryService
{
  public IEnumerable<Assembly> GetAssemblies() => [typeof(PaymentsComponent).Assembly, typeof(ShipmentsComponent).Assembly];
}
```

For each additional module you create you will need to add a new assembly to this array.

More about this in [Blazor's documentation page: Lazy Load Assemblies with WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-lazy-load-assemblies?view=aspnetcore-8.0#assemblies-that-include-routable-components)

# 📊 About Modular Monoliths

A Modular Monolithic app benefits from the simple deployment of a monolith and the separation of concerns that microservices offer. While avoiding the complexities and maintainability issues they can introduce. When you are ready and *if* you need it, you can split a module as a microservice. Best of both worlds 🌎

This is not a new concept. Martin Fowler [explains it here](https://martinfowler.com/bliki/MonolithFirst.html), and Ardalis teaches it [here](https://ardalis.com/introducing-modular-monoliths-goldilocks-architecture/#:~:text=A%20Modular%20Monolith%20is%20a%20software%20architecture%20that,that%20they%20are%20loosely%20coupled%20and%20highly%20cohesive.).

The templates in this project follow the solution structure as taught by [Ardalis](https://github.com/ardalis) in [his course *Modular Monoliths in DotNet*](https://dometrain.com/bundle/from-zero-to-hero-modular-monoliths-in-dotnet/).

# 🛃 Custom templates

No template fits all needs. If you want to customize the template you can [clone this repository as a template](https://docs.github.com/en/repositories/creating-and-managing-repositories/creating-a-repository-from-a-template).

Once you have cloned the repo locally you can make your custom changes in the `working/content` directory. This directory contains the project templates used for every instatiation. Then you can install the template locally running:

*⚠️ Make sure to uninstall the original template*
```pwsh
dotnet new install .
```

You can find more information about building ```dotnet new``` templates, including how to add commands and parameters, at Microsoft docs page: [Custom templates for dotnet new](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)

# 🗑️ Uninstall Modulith

```pwsh
dotnet new uninstall Ardalis.Modulith
```
