using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Data
{
    public class DesignTimeDbContextFactory  : IDesignTimeDbContextFactory<PlatformDbContext>
    {
        public PlatformDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost;Database=PlatformDB;Trusted_Connection=True;TrustServerCertificate=True";

            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(connectionString);

            return new PlatformDbContext(optionsBuilder.Options);

            /*
            Reference database from the instructor db server to work in training room


            But if you need to work on your local machine you have to create database on your db server with the name PlatformDB 
            before running below commands

            To enable ef migrations run: 
            dotnet tool install dotnet-ef --version 9.0.11


            First terminal command after inherting identity db context with identity user:
                        
            dotnet ef migrations add InitialCreate --project Platform.Data

            Then run the update database command:
            dotnet ef database update --project Platform.Data
            */
        }
    }
}
