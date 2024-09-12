# Clear the test directory before each run
#rm -rf test/**

# Reinstall latest version
dotnet new uninstall .
dotnet new install .

# Add solution with a first module
cd test/
dotnet new modulith -n eShop --with-module Payments

# Add second and third module. Third module is DDD
cd eShop
dotnet new modulith --add basic-module --module-name Shipments --solution eShop
dotnet new modulith --add ddd-module --module-name Billing --solution eShop

# Add project references. Needed before .Net 9
dotnet add eShop.Web/eShop.Web.csproj reference Shipments/eShop.Shipments/eShop.Shipments.csproj
dotnet add eShop.Web/eShop.Web.csproj reference Billing/eShop.Billing/eShop.Billing.csproj

# Build and run
cd ../..
dotnet build test/eShop
dotnet run --project test/eShop/eShop.Web/eShop.Web.csproj
