# Clear the test directory before each run
#rm -rf test/**

# Reinstall latest version
dotnet new uninstall .
dotnet new install .


# Add an UI enabled solution with a first module
cd test/
dotnet new modulith -n eShop --module-name Payments --with-ui

# Add second and third module. Third module is DDD
cd eShop
dotnet new modulith --solution eShop --with-ui --add basic-module --module-name Shipments
dotnet new modulith --solution eShop --with-ui --add crud --module-name Shipments --endpoint-name Student

dotnet new modulith --add ddd-module --module-name Billing --solution eShop

# Add project references. Needed before .Net 9
dotnet add eShop.Web/eShop.Web.csproj reference Shipments/eShop.Shipments/eShop.Shipments.csproj
dotnet add eShop.Web/eShop.Web.csproj reference Billing/eShop.Billing/eShop.Billing.csproj

# Build and run
cd ../..
dotnet build test/eShop
dotnet run --project test/eShop/eShop.Web/eShop.Web.csproj
