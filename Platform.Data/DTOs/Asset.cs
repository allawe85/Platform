using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("asset")]
    public class Asset
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("details")]
        public string? Details { get; set; }

        [Column("employee_id")]
        public int? EmployeeId { get; set; }

        [Column("asset_type_id")]
        public int AssetTypeId { get; set; }

        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("receive_date", TypeName = "date")]
        public DateTime ReceiveDate { get; set; }

        [Column("return_date", TypeName = "date")]
        public DateTime? ReturnDate { get; set; }

        // Navigation properties
        public Employee? Employee { get; set; }

        public AssetType? AssetType { get; set; }

        public AssetStatus? Status { get; set; }
    }
}