# efetest-backendnet7-clean-arq

El proyecto utiliza Entity Framework Code First , por lo que tenemos que correr las migraciones.

La base de datos usado es SQL Server

1. Crear base de datos EfeTest
   
   ```
   CREATE DATABASE EfeTest;
   ```
2. Cambiar el nombre de su servidor SQL Server en el archivo appsettings.Development.json

   **Ubicacion:** EfeTest.Backend.Api / appsettings.json / appsettings.Development.json

   ```json
   "ConnectionStrings": {
        "Database": "data source=NOMBRE_DE_SU_SERVIDOR; Initial Catalog=EfeTest;Integrated Security=True;TrustServerCertificate=True",
        "Storage": ""
   }
   ```
   Reemplazar NOMBRE_DE_SU_SERVIDOR con el nombre de su servidor SQL Server.
   
3. Eliminar la carpeta **Migrations** en EfeTest.Backend.Api
4. Ejecutar migraciones
   ```
     > add-migration InitialMigration
   
     > update-database

   ```
