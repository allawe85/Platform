using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Platform.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Data
{
    public class PlatformDbContext : IdentityDbContext<ApplicationUser>
    {
        public PlatformDbContext(DbContextOptions options) : base(options)
        {
        }

        // Business DbSets go here




        // Business Logic goes here
    }
}
