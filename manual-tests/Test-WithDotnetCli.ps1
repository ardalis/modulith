# Clear the test directory before each run
rm -rf eShop

# Reinstall latest version
dotnet new uninstall ../
dotnet new install ../

# Add solution with a first module
dotnet new modulith -n eShop --with-module Payments

# Add second and third module. Third module is DDD
cd eShop
dotnet new modulith `
    --template basic `
    --module-name Shipments `
    --solution eShop

cd eShop/eShop.Web
dotnet new modulith --add ddd-module --module-name Billing --solution eShop

# Add project references. Needed before .Net 9
dotnet add eShop.Web/eShop.Web.csproj reference Shipments/eShop.Shipments/eShop.Shipments.csproj
dotnet add eShop.Web/eShop.Web.csproj reference Billing/eShop.Billing/eShop.Billing.csproj

# Build and run
cd ../..
dotnet build manual-test/eShop
dotnet run --project manual-test/eShop/eShop.Web/eShop.Web.csproj
