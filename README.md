This is a simple application for teaching the programming Merit badge to Boy Scouts

You will need to install dotnet for your OS. https://docs.microsoft.com/en-us/dotnet/core/install/

You will need to set permissions on the folder so that the OS has access.  For my dev environment, I set Everyone to have read access.

Obviously, you will also need to install docker and docker-compose.  Instructions can be found here: https://docs.docker.com/compose/install/

For linux, I did have to make one change while I research how to implement certs.  Change the environment settings to this:

environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:80
It removes SSL support.
