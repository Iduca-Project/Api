 param (
    [Parameter(Mandatory=$true)][string]$ProjectName
 )

#mkdir $ProjectName
#cd "$ProjectName"

#================API=======================
dotnet new webapi -n "$ProjectName.Api"
cd "$ProjectName.Api"
	mkdir Controllers
	mkdir Enums
	mkdir Extensions
cd ..

#==============DOMAIN=======================
dotnet new classlib -n "$ProjectName.Domain"
cd "$ProjectName.Domain"
	mkdir Common
		mkdir Common/Enums
		mkdir Common/Messages
	mkdir Models
		#_____________entity
cd ..

#==============APPLICATION===================
dotnet new classlib -n "$ProjectName.Application"
cd "$ProjectName.Application"
	mkdir Common
		mkdir Common/Behaviors
		mkdir Common/Exceptions
	mkdir Config
	mkdir Features
		#_____________entity
	mkdir Repository
		#_____________entity
cd ..

#============PERSISTENCE====================
dotnet new classlib -n "$ProjectName.Persistence"
cd "$ProjectName.Persistence"
	mkdir Context
		#____________default
		#____________default
	mkdir Migrations
	mkdir Repositories
		#____________entity
	mkdir Tables
		#____________default
		#____________entity
cd ..


#=======================REFERENCES=======================
dotnet add ./$ProjectName.Api reference ./$ProjectName.Application/$ProjectName.Application.csproj ./$ProjectName.Persistence/$ProjectName.Persistence.csproj
dotnet add ./$ProjectName.Application reference ./$ProjectName.Domain/$ProjectName.Domain.csproj
#dotnet add ./$ProjectName.Persistence reference ./$ProjectName.Application/$ProjectName.Application.csproj ./$ProjectName.Domain/$ProjectName.Domain.csproj



#cd ..