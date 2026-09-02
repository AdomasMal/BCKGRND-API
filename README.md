# BCKGRND API
An ASP.NET web API for interfacing android application with MySQL database and ArcGIS feature layer. Requires [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
### Setup
1. Clone the repository:
```
git clone https://github.com/TheFeish/BCKGRND-API.git
```
2. Go to project directory:
```
cd ./BCKGRND-API/BCKGRND
```
3. Install .NET EF core tools and create MySQL database:
```
dotnet tool restore
dotnet ef database update
```
4. Modify **appsettings.json** and **launchSetting.json** files.
### Running the program
* Build and run the program:
```
dotnet run
```
* Endpoint documentation can be found in **\<applicationURL\>/swagger/index.html** (**https<span>://</span>localhost:7040/swagger/index.html** by default).
### Features
* Can store, edit and delete user data.
* Handles registration and logging in.
* Can store features' location, name, tags and images.
* Can add features to ArcGIS feature layer.
* Can retrieve loacation data by its name, tag, id or proximity.
