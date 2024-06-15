cd /Users/davidchaparro/RiderProjects/modulith/test/
rm -rf /Users/davidchaparro/RiderProjects/modulith/test/**
dotnet new uninstall /Users/davidchaparro/RiderProjects/modulith/working/content/modulith
dotnet new install /Users/davidchaparro/RiderProjects/modulith/working/content/modulith

dotnet new modulith -n eShop --with-module Payments --WithUi

cd eShop
dotnet new modulith --add basic-module --with-name Shipments --to eShop --WithUi
dotnet new modulith --add ddd-module --with-name Billing --to eShop

dotnet build

dotnet add eShop.Web/eShop.Web.csproj reference Shipments/eShop.Shipments/eShop.Shipments.csproj
dotnet add eShop.Web/eShop.Web.csproj reference Billing/eShop.Billing/eShop.Billing.csproj

dotnet build

# dotnet run --project ./eShop.Web/eShop.Web.csproj
