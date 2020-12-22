This is a simple application for teaching the programming Merit badge to Boy Scouts

You will need to install dotnet for your OS. https://docs.microsoft.com/en-us/dotnet/core/install/

You will need to set permissions on the folder so that the OS has access.  For my dev environment, I set Everyone to have read access.

Obviously, you will also need to install docker and docker-compose.  Instructions can be found here: https://docs.docker.com/compose/install/

Linux Instructions:
run this from the command line:
> dotnet dev-certs https --clean
> dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p {create a password here}


Make these changes to the docker-compose.yml file, to add the certificate for SSL:
environment:

      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=https://+:443;http://+:80
      - ASPNETCORE_Kestrel__Certificates__Default__Password={Password defined above}
      - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
volumes:

      - ~/.aspnet/https:/https:ro
      
