# Site Machine Tracker
Practice on using Web API and JavaScript Fetch API

## Running
1. Run `site-machine-tracker.sln` in Debug or Release mode. 
1. Open the following URL in a browser: https://localhost:7055/index.html
   * URL can be changed in `Properties/launchSettings.json`

## Front-End Client
1. Select a User from the dropdown list
   * The site name which the User belongs to will be shown.
   * The machines on this site will be marked on the map.
1. Filter the machines by name and or type to limit what is shown
1. Changing the selected user or the filters will clear the map

Uses [OpenStreetMap](https://www.openstreetmap.org/) and [Leaflet](https://leafletjs.com/)

## Back-End API

### `api/Machine/Users`
Lists down all users (including invalid ones)

### `api/Machine/Types`
Lists down all machine types

### `api/Machine/{userId:int}?machineName={name}&machineType={type}`
Lists down all machines in the Site which the given User belongs to.
If the machine name is specified, it filters the results with the given name. If the machine type is specified, it filters the results based on the specified type.

### `api/Machine/{machineId:int}/{latitude:double}/{longitude:double}`
Updates the location of the given machine with the new latitude and longitude

## Test Data
1. Sites (Number of users, Number of machines)
   1. Stockholm (2, 5)
   1. Göteborg (1, 5)
   1. Malmö (1, 0)
1. Users (Site where they belong)
   1. William (Stockholm)
   1. Noah (Stockholm)
   1. Alice (Göteborg)
   1. Carl (Malmö)
   1. Olivia (*non-existent site*)
   1. Astrid (*non-existent site*)
1. Machines (Site where they belong - Machine Type)
   1. Björn (Stockholm - Excavator)
   1. Erik (Stockholm - Dozer)
   1. Astrid (Stockholm - Excavator)
   1. Ragnar (Stockholm - Dozer)
   1. Ulf (Stockholm - Excavator)
   1. Sigrid (Göteborg - Dozer)
   1. Ivar (Göteborg - Excavator)
   1. Harald (Göteborg - Dozer)
   1. Freya (Göteborg - Excavator)
   1. Thorsten (Göteborg - Dozer)
   1. Gunnar (*non-existent site* - Excavator)
   1. Leif (*non-existent site* - Dozer)

*Machines and Users without an existent site are examples of invalid data*
