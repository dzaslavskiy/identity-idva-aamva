# identity-idva-aamva

Prototype of microservice to access AAMVA verification api.

dotnet run --project .\aamva\aamva.csproj

 cf push --vars-file .\vars.yaml

 dotnet build

 dotnet test


 cf ssh -N -T -L localhost:8000:<soap_host> aamva
 cf ssh -N -T -L localhost:8000:verificationservices2-cert.aamva.org:18449 sk-api




 Generating service from wsdl
 

user secrets for local


 cf create-user-provided-service aamva -p aamva/secrets.json
 {
  "cert_data": "",
  "cert_password": ""
}


 dev -> local
 prod -> cf



cat ./input.json | dotnet user-secrets set
type .\input.json | dotnet user-secrets set