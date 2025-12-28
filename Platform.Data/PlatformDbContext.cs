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
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<HierarchyLevel> HierarchyLevels { get; set; }

        public DbSet<Hierarchy> Hierarchies { get; set; }

        public DbSet<EmployeeInfo> EmployeesInfo { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<AssetType> AssetTypes { get; set; }

        public DbSet<AssetStatus> AssetStatuses { get; set; }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<EventType> EventTypes { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<PollVote> PollVotes { get; set; }

        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveStatus> LeaveStatuses { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<TimeAttendance> TimeAttendances { get; set; }

        #endregion


        #region HierarchyLevel - CRUD Operations



        #region Announcement - CRUD Operations
        public async Task<List<Announcement>> GetAllAnnouncementsAsync()
        {
            return await Announcements
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<Announcement?> GetAnnouncementByIdAsync(int id)
        {
            return await Announcements.FindAsync(id);
        }

        public async Task<Announcement> AddAnnouncementAsync(Announcement entity)
        {
            var entry = await Announcements.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Announcement?> UpdateAnnouncementAsync(Announcement entity)
        {
            var existing = await Announcements.FindAsync(entity.AnnouncementId);
            if (existing == null) return null;

            existing.Title = entity.Title;
            existing.Description = entity.Description;
            existing.IsActive = entity.IsActive;
            // CreatedDate usually isn't updated, or if it is, include it here. 
            // Based on user SQL, they did update it, so let's allow it or set it to now if needed.
            // But usually created date is fixed. The user SQL example: CreatedDate = GETDATE(). 
            // So we can update it if the entity has a new date, but typically we trust the entity coming in unless we want to enforce server time on update.
            // For now, let's assume the entity passed in has the correct values.
            
            Announcements.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAnnouncementAsync(int id)
        {
            var existing = await Announcements.FindAsync(id);
            if (existing == null) return false;

            Announcements.Remove(existing);
            await SaveChangesAsync();
            return true;
        }
        #endregion

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
            existing.ParentId = entity.ParentId;

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
            existing.FullName = entity.FullName;

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
            existing.NameAr = entity.NameAr;

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
            existing.NameAr = entity.NameAr;

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

        public async Task<List<Asset>> GetAssetsByEmployeeIdAsync(int employeeId)
        {
            return await Assets
                         .AsNoTracking()
                         .Include(a => a.AssetType)
                         .Include(a => a.Status)
                         .Include(a => a.Employee)
                         .Where(a => a.EmployeeId == employeeId)
                         .ToListAsync();
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
            existing.NameAr = entity.NameAr;

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

        public async Task<List<Document>> GetDocumentsByEmployeeIdAsync(int employeeId)
        {
            return await Documents
                         .AsNoTracking()
                         .Include(d => d.Employee)
                         .Include(d => d.DocumentType)
                         .Where(d => d.EmployeeId == employeeId)
                         .ToListAsync();
        }

        #endregion

        #region ApplicationUser - CRUD Operations
        public async Task<List<ApplicationUser>> GetAllApplicationUsersAsync()
        {
            return await Set<ApplicationUser>()
                         .AsNoTracking()
                         .ToListAsync();
        }

        public async Task<ApplicationUser?> GetApplicationUserByIdAsync(string id)
        {
            return await Set<ApplicationUser>()
                         .AsNoTracking()
                         .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ApplicationUser> AddApplicationUserAsync(ApplicationUser entity)
        {
            var entry = await Set<ApplicationUser>().AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<ApplicationUser?> UpdateApplicationUserAsync(ApplicationUser entity)
        {
            var existing = await Set<ApplicationUser>().FindAsync(entity.Id);
            if (existing == null) return null;
            // copy scalar properties
            existing.UserName = entity.UserName;
            existing.Email = entity.Email;
            existing.PhoneNumber = entity.PhoneNumber;
            Set<ApplicationUser>().Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteApplicationUserAsync(string id)
        {
            var existing = await Set<ApplicationUser>().FindAsync(id);
            if (existing == null) return false;
            Set<ApplicationUser>().Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion

        #region EventType - CRUD Operations

        public async Task<List<EventType>> GetAllEventTypesAsync()
        {
            return await EventTypes
                         .AsNoTracking()
                         .ToListAsync();
        }

        public async Task<EventType?> GetEventTypeByIdAsync(int id)
        {
            return await EventTypes.FindAsync(id);
        }

        public async Task<EventType> AddEventTypeAsync(EventType entity)
        {
            var entry = await EventTypes.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<EventType?> UpdateEventTypeAsync(EventType entity)
        {
            var existing = await EventTypes.FindAsync(entity.Id);
            if (existing == null) return null;

            existing.Name = entity.Name;
            existing.NameAr = entity.NameAr;

            EventTypes.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteEventTypeAsync(int id)
        {
            var existing = await EventTypes.FindAsync(id);
            if (existing == null) return false;

            EventTypes.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion

        #region Event - CRUD Operations

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await Events
                         .AsNoTracking()
                         .Include(e => e.EventType)
                         .ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await Events
                         .Include(e => e.EventType)
                         .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Event>> GetEventsByDateRangeAsync(DateTime start, DateTime end)
        {
            return await Events
                         .AsNoTracking()
                         .Include(e => e.EventType)
                         .Where(e => e.StartDate >= start && e.EndDate <= end)
                         .ToListAsync();
        }

        public async Task<Event> AddEventAsync(Event entity)
        {
            var entry = await Events.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Event?> UpdateEventAsync(Event entity)
        {
            var existing = await Events.FindAsync(entity.Id);
            if (existing == null) return null;

            existing.Name = entity.Name;
            existing.NameAr = entity.NameAr;
            existing.Location = entity.Location;
            existing.LocationAr = entity.LocationAr;
            existing.EventTypeId = entity.EventTypeId;
            existing.StartDate = entity.StartDate;
            existing.EndDate = entity.EndDate;
            existing.Description = entity.Description;
            existing.DescriptionAr = entity.DescriptionAr;

            Events.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var existing = await Events.FindAsync(id);
            if (existing == null) return false;

            Events.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion

        #region Poll - CRUD Operations

        public async Task<List<Poll>> GetAllPollsAsync()
        {
            return await Polls
                .AsNoTracking()
                .Include(p => p.Options)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        public async Task<Poll?> GetPollByIdAsync(int id)
        {
            return await Polls
                .Include(p => p.Options)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Poll> AddPollAsync(Poll entity)
        {
            var entry = await Polls.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Poll?> UpdatePollAsync(Poll entity)
        {
            var existing = await Polls
                .Include(p => p.Options)
                .FirstOrDefaultAsync(p => p.Id == entity.Id);

            if (existing == null) return null;

            existing.Question = entity.Question;
            existing.QuestionAr = entity.QuestionAr;
            existing.IsActive = entity.IsActive;

            // Handle Poll Options Update
            if (entity.Options != null)
            {
                // Identify options to remove
                var existingOptionIds = existing.Options.Select(o => o.Id).ToList();
                var newOptionIds = entity.Options.Select(o => o.Id).ToList();
                var optionsToRemove = existing.Options.Where(o => !newOptionIds.Contains(o.Id)).ToList();

                foreach (var option in optionsToRemove)
                {
                    // Assuming Cascade Delete is handled by DB FK or EF Core tracking
                    // We explicitly remove from context
                    existing.Options.Remove(option);
                    // Or PollOptions.Remove(option) if attached
                }

                // Add or Update
                foreach (var option in entity.Options)
                {
                    var existingOption = existing.Options.FirstOrDefault(o => o.Id == option.Id);
                    if (existingOption != null)
                    {
                        existingOption.OptionText = option.OptionText;
                        existingOption.OptionTextAr = option.OptionTextAr;
                    }
                    else
                    {
                        // New option, ensure PollId is set if needed or add to collection
                        existing.Options.Add(option);
                    }
                }
            }

            Polls.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeletePollAsync(int id)
        {
            var existing = await Polls.FindAsync(id);
            if (existing == null) return false;

            Polls.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        public async Task<PollVote> AddPollVoteAsync(PollVote entity)
        {
            var entry = await PollVotes.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<bool> HasEmployeeVotedAsync(int pollId, int employeeId)
        {
            return await PollVotes
                .AnyAsync(pv => pv.PollId == pollId && pv.EmployeeId == employeeId);
        }

        public async Task<Dictionary<int, int>> GetPollResultsAsync(int pollId)
        {
            var results = await PollVotes
                .Where(pv => pv.PollId == pollId)
                .GroupBy(pv => pv.PollOptionId)
                .Select(g => new { OptionId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.OptionId, x => x.Count);

            return results;
        }

        #endregion

        

        #region TimeAttendance - Operations
              public async Task<List<TimeAttendance>> GetTimeAttendanceByEmployeeIdAsync(int employeeId)
        {
            return await TimeAttendances
                .AsNoTracking()
                .Where(ta => ta.EmployeeId == employeeId)
                .OrderByDescending(ta => ta.TransactionTime)
                .ToListAsync();
        }

        public async Task<List<TimeAttendance>> GetTimeAttendanceReportAsync(DateTime startDate, DateTime endDate, int? employeeId = null)
        {
            var query = TimeAttendances
                .AsNoTracking()
                .Where(ta => ta.TransactionTime >= startDate && ta.TransactionTime <= endDate);

            if (employeeId.HasValue)
            {
                query = query.Where(ta => ta.EmployeeId == employeeId.Value);
            }

            return await query
                .OrderBy(ta => ta.TransactionTime)
                .ToListAsync();
        }
        public async Task<TimeAttendance> AddTimeAttendanceAsync(TimeAttendance entity)
        {
            var entry = await TimeAttendances.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }
        #endregion

        #region LeaveType - CRUD Operations

        public async Task<List<LeaveType>> GetAllLeaveTypesAsync()
        {
            return await LeaveTypes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LeaveType?> GetLeaveTypeByIdAsync(int id)
        {
            return await LeaveTypes.FindAsync(id);
        }

        public async Task<LeaveType> AddLeaveTypeAsync(LeaveType entity)
        {
            var entry = await LeaveTypes.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<LeaveType?> UpdateLeaveTypeAsync(LeaveType entity)
        {
            var existing = await LeaveTypes.FindAsync(entity.Id);
            if (existing == null) return null;

            existing.NameAr = entity.NameAr;
            existing.Name = entity.Name;
            existing.TypeBalance = entity.TypeBalance;

            LeaveTypes.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteLeaveTypeAsync(int id)
        {
            var existing = await LeaveTypes.FindAsync(id);
            if (existing == null) return false;

            LeaveTypes.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion

        #region LeaveStatus - CRUD Operations

        public async Task<List<LeaveStatus>> GetAllLeaveStatusesAsync()
        {
            return await LeaveStatuses
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LeaveStatus?> GetLeaveStatusByIdAsync(int id)
        {
            return await LeaveStatuses.FindAsync(id);
        }

        public async Task<LeaveStatus> AddLeaveStatusAsync(LeaveStatus entity)
        {
            var entry = await LeaveStatuses.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<LeaveStatus?> UpdateLeaveStatusAsync(LeaveStatus entity)
        {
            var existing = await LeaveStatuses.FindAsync(entity.Id);
            if (existing == null) return null;

            existing.NameAr = entity.NameAr;

            LeaveStatuses.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteLeaveStatusAsync(int id)
        {
            var existing = await LeaveStatuses.FindAsync(id);
            if (existing == null) return false;

            LeaveStatuses.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        #endregion

        #region LeaveBalance - CRUD Operations

        public async Task<List<LeaveBalance>> GetLeaveBalancesByEmployeeIdAsync(int employeeId)
        {
            return await LeaveBalances
                .AsNoTracking()
                .Include(lb => lb.LeaveType)
                .Where(lb => lb.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<LeaveBalance?> GetLeaveBalanceByIdAsync(int id)
        {
            return await LeaveBalances
               .Include(lb => lb.LeaveType)
               .Include(lb => lb.Employee)
               .FirstOrDefaultAsync(lb => lb.Id == id);
        }

        public async Task<LeaveBalance> AddLeaveBalanceAsync(LeaveBalance entity)
        {
            var entry = await LeaveBalances.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<LeaveBalance?> UpdateLeaveBalanceAsync(LeaveBalance entity)
        {
            var existing = await LeaveBalances.FindAsync(entity.Id);
            if (existing == null) return null;

            existing.Balance = entity.Balance;
            // Usually we don't update employee or type for an existing balance record, but can if needed.

            LeaveBalances.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        // Helper to check if enough balance exists
        public async Task<bool> HasEnoughBalanceAsync(int employeeId, int leaveTypeId, int daysRequested)
        {
            var balance = await LeaveBalances
                .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId && lb.LeaveTypeId == leaveTypeId);

            if (balance == null) return false; // Or true if infinite? Assuming explicit balance needed.

            return (balance.Balance ?? 0) >= daysRequested;
        }

        #endregion

        #region Leave - CRUD Operations

        public async Task<List<Leave>> GetLeavesByEmployeeIdAsync(int employeeId)
        {
            return await Leaves
                .AsNoTracking()
                .Include(l => l.LeaveType)
                .Include(l => l.LeaveStatus)
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.Id)
                .ToListAsync();
        }

        public async Task<List<Leave>> GetPendingLeavesAsync()
        {
            // Assuming we check for specific status ID or just return all? 
            // Better to filter by status if we know the ID.
            // But currently flexible. returning all with eager loading.
            return await Leaves
                .Include(l => l.LeaveType)
                .Include(l => l.LeaveStatus)
                .Include(l => l.Employee)
                .OrderByDescending(l => l.Id)
                .ToListAsync();
        }

        public async Task<Leave?> GetLeaveByIdAsync(int id)
        {
            return await Leaves
                .Include(l => l.LeaveType)
                .Include(l => l.LeaveStatus)
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Leave> AddLeaveAsync(Leave entity)
        {
            var entry = await Leaves.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Leave?> UpdateLeaveAsync(Leave entity)
        {
            var existing = await Leaves.FindAsync(entity.Id);
            if (existing == null) return null;

            existing.LeaveStatusId = entity.LeaveStatusId;
            existing.StartDate = entity.StartDate;
            existing.EndDate = entity.EndDate;
            existing.Details = entity.Details;
            // Add other fields if necessary

            Leaves.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

        public async Task<Leave?> UpdateLeaveStatusAsync(int leaveId, int newStatusId)
        {
            var existing = await Leaves.FindAsync(leaveId);
            if (existing == null) return null;

            existing.LeaveStatusId = newStatusId;

            Leaves.Update(existing);
            await SaveChangesAsync();
            return existing;
        }

      

        #endregion
    }
}
