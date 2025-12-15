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


        #region DbSets - Tables Representation
        public DbSet<HierarchyLevel> HierarchyLevels { get; set; }

        public DbSet<Hierarchy> Hierarchies { get; set; }

        public DbSet<EmployeeInfo> EmployeesInfo { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<AssetType> AssetTypes { get; set; }

        public DbSet<AssetStatus> AssetStatuses { get; set; }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Document> Documents { get; set; }

        #endregion


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


        #region Asset Types - CRUD Operations

        public async Task<List<AssetType>> GetAllAssetTypesAsync()
        {
            return await AssetTypes
                         .AsNoTracking()
                         .ToListAsync();
        }

        public async Task<AssetType?> GetAssetTypeByIdAsync(int id)
        {
            return await AssetTypes.FindAsync(id);
        }

        public async Task<AssetType> AddAssetTypeAsync(AssetType entity)
        {
            var entry = await AssetTypes.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<AssetType?> UpdateAssetTypeAsync(AssetType entity)
        {
            var existing = await AssetTypes.FindAsync(entity.Id);
            if (existing == null) return null;
            // copy scalar properties
            existing.Name = entity.Name;
            AssetTypes.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAssetTypeAsync(int id)
        {
            var existing = await AssetTypes.FindAsync(id);
            if (existing == null) return false;
            AssetTypes.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion

        #region Asset Statuses - CRUD Operations

        public async Task<List<AssetStatus>> GetAllAssetStatusesAsync()
        {
            return await AssetStatuses
                         .AsNoTracking()
                         .ToListAsync();
        }

        public async Task<AssetStatus?> GetAssetStatusByIdAsync(int id)
        {
            return await AssetStatuses.FindAsync(id);
        }

        public async Task<AssetStatus> AddAssetStatusAsync(AssetStatus entity)
        {
            var entry = await AssetStatuses.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<AssetStatus?> UpdateAssetStatusAsync(AssetStatus entity)
        {
            var existing = await AssetStatuses.FindAsync(entity.Id);
            if (existing == null) return null;
            // copy scalar properties
            existing.Name = entity.Name;
            AssetStatuses.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAssetStatusAsync(int id)
        {
            var existing = await AssetStatuses.FindAsync(id);
            if (existing == null) return false;
            AssetStatuses.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion

        #region Asset - CRUD Operations

        public async Task<List<Asset>> GetAllAssetsAsync()
        {
            return await Assets
                         .AsNoTracking()
                         .Include(a => a.AssetType)
                         .Include(a => a.Status)
                         .Include(a => a.Employee)
                         .ToListAsync();
        }

        public async Task<Asset?> GetAssetByIdAsync(int id)
        {
            return await Assets
                         .Include(a => a.AssetType)
                         .Include(a => a.Status)
                         .Include(a => a.Employee)
                         .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Asset> AddAssetAsync(Asset entity)
        {
            var entry = await Assets.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Asset?> UpdateAssetAsync(Asset entity)
        {
            var existing = await Assets.FindAsync(entity.Id);
            if (existing == null) return null;
            // copy scalar properties
            existing.Details = entity.Details;
            existing.EmployeeId = entity.EmployeeId;
            existing.AssetTypeId = entity.AssetTypeId;
            existing.StatusId = entity.StatusId;
            existing.ReceiveDate = entity.ReceiveDate;
            existing.ReturnDate = entity.ReturnDate;
            Assets.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAssetAsync(int id)
        {
            var existing = await Assets.FindAsync(id);
            if (existing == null) return false;
            Assets.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion


        #region Document Types - CRUD Operations

        public async Task<List<DocumentType>> GetAllDocumentTypesAsync()
        {
            return await DocumentTypes
                         .AsNoTracking()
                         .ToListAsync();
        }

        public async Task<DocumentType?> GetDocumentTypeByIdAsync(int id)
        {
            return await DocumentTypes.FindAsync(id);
        }

        public async Task<DocumentType> AddDocumentTypeAsync(DocumentType entity)
        {
            var entry = await DocumentTypes.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<DocumentType?> UpdateDocumentTypeAsync(DocumentType entity)
        {
            var existing = await DocumentTypes.FindAsync(entity.Id);
            if (existing == null) return null;
            // copy scalar properties
            existing.Name = entity.Name;
            DocumentTypes.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteDocumentTypeAsync(int id)
        {
            var existing = await DocumentTypes.FindAsync(id);
            if (existing == null) return false;
            DocumentTypes.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion

        #region Document - CRUD Operations

        public async Task<List<Document>> GetAllDocumentsAsync()
        {
            return await Documents
                         .AsNoTracking()
                         .Include(d => d.Employee)
                         .Include(d => d.DocumentType)
                         .ToListAsync();
        }

        public async Task<Document?> GetDocumentByIdAsync(int id)
        {
            return await Documents
                         .Include(d => d.Employee)
                         .Include(d => d.DocumentType)
                         .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Document> AddDocumentAsync(Document entity)
        {
            var entry = await Documents.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Document?> UpdateDocumentAsync(Document entity)
        {
            var existing = await Documents.FindAsync(entity.Id);
            if (existing == null) return null;
            // copy scalar properties
            existing.EmployeeId = entity.EmployeeId;
            existing.DocumentTypeId = entity.DocumentTypeId;
            existing.Title = entity.Title;
            existing.Details = entity.Details;
            existing.ExpiryDate = entity.ExpiryDate;
            Documents.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteDocumentAsync(int id)
        {
            var existing = await Documents.FindAsync(id);
            if (existing == null) return false;
            Documents.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        public async Task GetAssetController(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
