# dotnet_api
Demo of API authentication

## Setup build environment

### Add the Microsoft package repository
```
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
```

### Install the SDK (dotnet-sdk-7.0, dotnet-sdk-6.0 etc.)
```
sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0
```

### Install the runtime (aspnetcore-runtime-7.0, aspnetcore-runtime-6.0, dotnet-runtime-7.0, dotnet-runtime-6.0 etc.)
```
sudo apt-get update && sudo apt-get install -y aspnetcore-runtime-7.0
```