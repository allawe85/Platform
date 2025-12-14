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
        // No Singltone implementation because we will use DI (Dependency Injection)
        public PlatformDbContext(DbContextOptions options) : base(options)
        {
        }

        // Business DbSets go here
        public DbSet<HierarchyLevel> HierarchyLevels { get; set; }

        public DbSet<Hierarchy> Hierarchies { get; set; }

        public DbSet<EmployeeInfo> EmployeesInfo { get; set; }

        public DbSet<Employee> Employees { get; set; }

        // Business Logic goes here

        #region HierarchyLevel - CRUD Operations
        public async Task<List<HierarchyLevel>> GetAllHierarchyLevelsAsync()
        {
            return await HierarchyLevels
                         .AsNoTracking()
                         .ToListAsync();
        }

        public async Task<HierarchyLevel?> GetHierarchyLevelByIdAsync(int id)
        {
            return await HierarchyLevels.FindAsync(id);
        }

        public async Task<HierarchyLevel> AddHierarchyLevelAsync(HierarchyLevel entity)
        {
            var entry = await HierarchyLevels.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<HierarchyLevel?> UpdateHierarchyLevelAsync(HierarchyLevel entity)
        {
            var existing = await HierarchyLevels.FindAsync(entity.Id);
            if (existing == null) return null;

            // copy scalar properties
            existing.Name = entity.Name;
            existing.NameAr = entity.NameAr;

            HierarchyLevels.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteHierarchyLevelAsync(int id)
        {
            var existing = await HierarchyLevels.FindAsync(id);
            if (existing == null) return false;

            HierarchyLevels.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion

        #region Hierarchy - CRUD Operations
        public async Task<List<Hierarchy>> GetAllHierarchiesAsync()
        {
            return await Hierarchies
                         .AsNoTracking()
                         .Include(h => h.HierarchyLevel)
                         .ToListAsync();
        }

        public async Task<Hierarchy?> GetHierarchyByIdAsync(int id)
        {
            return await Hierarchies
                         .Include(h => h.HierarchyLevel)
                         .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<Hierarchy> AddHierarchyAsync(Hierarchy entity)
        {
            var entry = await Hierarchies.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Hierarchy?> UpdateHierarchyAsync(Hierarchy entity)
        {
            var existing = await Hierarchies.FindAsync(entity.Id);
            if (existing == null) return null;

            existing.Name = entity.Name;
            existing.NameAr = entity.NameAr;
            existing.HierarchyLevelId = entity.HierarchyLevelId;

            Hierarchies.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteHierarchyAsync(int id)
        {
            var existing = await Hierarchies.FindAsync(id);
            if (existing == null) return false;

            Hierarchies.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion

        #region EmployeeInfo - Read Operations
        public async Task<List<EmployeeInfo>> GetAllEmployeesAsync()
        {
            return await EmployeesInfo
                         .AsNoTracking()
                         .ToListAsync();
        }

        public async Task<EmployeeInfo?> GetEmployeeByIdAsync(int id)
        {
            return await EmployeesInfo
                         .AsNoTracking()
                         .FirstOrDefaultAsync(e => e.Id == id);
        }

        #endregion

        #region Employee - CRUD Operations
        public async Task<List<Employee>> GetAllEmployeeRecordsAsync()
        {
            return await Employees
                         .AsNoTracking()
                         .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeRecordByIdAsync(int id)
        {
            return await Employees.FindAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee entity)
        {
            var entry = await Employees.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Employee?> UpdateEmployeeAsync(Employee entity)
        {
            var existing = await Employees.FindAsync(entity.Id);
            if (existing == null) return null;

            // copy scalar properties explicitly
            existing.CivilId = entity.CivilId;
            existing.EmployeeId = entity.EmployeeId;
            existing.HierarchyId = entity.HierarchyId;
            existing.AspnetusersId = entity.AspnetusersId;

            Employees.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var existing = await Employees.FindAsync(id);
            if (existing == null) return false;

            Employees.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion
    }
}
