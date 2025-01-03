### Required to install
* <a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0" target="_blank">ASP.NET Core Runtime 8.0.11</a>
* <a href="https://www.microsoft.com/en-us/sql-server/sql-server-downloads" target="_blank"> Microsoft SQL Server 2022</a>+

### Clone
  Clone this repo to your local machine using:
  ```
https://github.com/Michael-Kolpakov/filmio-api.git
  ```
  Or if your have an associated SSH key:
  ```
git@github.com:Michael-Kolpakov/filmio-api.git
  ```

### Setup
  **1. Change connection string**  
   * Go to `appsettings.Develop.json` in **Filmio.WebApi** project and write your local database connection string in following format:
    
     ```
      Data Source={local_server_name};Database=FilmioDb;User Id={username};Password={password};MultipleActiveResultSets=true;TrustServerCertificate=true;Trusted_Connection=True;
     ```

  **2. Add database seeding**
   - Go to your `Microsoft SQL Server Management Studio` and create manually database with `FilmioDb` name:

     ```sql
     CREATE DATABASE FilmioDb;
     ```
     
  **3. Run project and seed local database**  
   * Run project and make sure that database was filled with data

### How to run local 
 Run the Streetcode project than open your browser and enter https://localhost:5001/swagger/index.html url. If you had this page already opened, just reload it.

### How to connect to db locally
1. launch SQL Server management Studio
2. In the pop-up window:
    - enter **"localhost"** as the server name;
    - select **"windows authentication"** as authentication mechanism;
3. After the connection has been established, right-click on the server (the first line with the icon), on the left-hand side of the UI
4. In the the appeared window find and click on **"properties"**
5. In the properties section, select **"security"** page
6. Make sure that **"Server authentication"** radio-button is set to **"SQL Server and Windows Authentication mode"**
7. Click "Ok"
8. Then again, on the left-hand side of the UI find folder entitled **"Security"**, and expand it
9. In unrolled list of options find folder "Logins", and expand it
10. At this point, you should have **"sa"** as the last option.
    If for some reason you do not see it, please refer to https://stackoverflow.com/questions/35753254/why-login-without-rights-can-see-sa-login
11. Right-click on the "sa" item, select "properties"
12. Change password to the default system one - **"Admin@1234"**. Don't forget to confirm it afterwards
13. On the left-hand side select **"Status"** page, and set **"Login"** radio-button to **"Enabled"**
14. Click "Ok"
15. Right click on **"localhost"** server on the left-hand side of the UI and click **"Restart"**

Now you can connect to your localhost instance with login (sa) and password (Admin@1234)!
