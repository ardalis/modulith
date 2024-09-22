cls
# Clear the test directory before each run
rm -rf manual-test/**

# Reinstall latest version
dotnet new uninstall .
dotnet new install .


# Add an UI enabled solution with a first module
cd manual-test/
dotnet new modulith `
    --name eShop `
    --module-name Payments `
    --with-ui

# Add second and third module. Third module is DDD
cd ./eShop/eShop.Web
dotnet new modulith `
    --template basic `
    --module-name Shipments `
    --with-ui #`
    #--project eShop/eShop.Web/eShop.Web.csproj `
    #--output eShop/eShop.Web/eShop.Web.csproj

cd ../Shipments/eShop.Shipments
dotnet new modulith-basic-endpoint `
    --with-ui `
    --endpoint-name Provider `
    -d
cd ../../
dotnet build

dotnet new modulith --add ddd-module --module-name Billing

# Add project references. Needed before .Net 9
# dotnet add eShop.Web/eShop.Web.csproj reference Shipments/eShop.Shipments/eShop.Shipments.csproj
# dotnet add eShop.Web/eShop.Web.csproj reference Billing/eShop.Billing/eShop.Billing.csproj

# Build and run
cd ../..

# dotnet build manual-test/eShop
# dotnet run --project manual-test/eShop/eShop.Web/eShop.Web.csproj
