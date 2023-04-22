# dotnet_api
Demo of API authentication

## Setup build environment

```
dotnet dev-certs https
dotnet dev-certs https --trust (Windows & macOs, not availible for Linux)
```

## Test

### MyApi
```
curl -X 'GET' 'https://localhost:5000/weather/weatherforecast' \
  -H 'accept: application/json'
```

```
curl -X 'GET' \
  'https://localhost:5078/miniget' \
  -H 'accept: application/json' \
  -H 'x-api-key: 60E4C9B7-9CD4-43EB-AB87-684617FB263D'

curl -X 'GET' \
  'https://localhost:5078/WeatherForecast' \
  -H 'accept: text/plain' \
  -H 'x-api-key: 60E4C9B7-9CD4-43EB-AB87-684617FB263D'
```


### misc
```
linux trust cert:
apt-get install libnss3-tools
create or verify $HOME/.pki/nssdb folder:
mkdir /home/vscode/.pki/nssdb
sudo -E dotnet dev-certs https -ep /usr/local/share/ca-certificates/aspnet/https.crt --format PEM
Create a JSON file at /usr/lib/firefox/distribution/policies.json;
cat <<EOF | sudo tee /usr/lib/firefox/distribution/policies.json
{
    "policies": {
        "Certificates": {
            "Install": [
                "/usr/local/share/ca-certificates/aspnet/https.crt"
            ]
        }
    }
}
EOF
```

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


[VS Devcontainers](https://hub.docker.com/_/microsoft-vscode-devcontainers)