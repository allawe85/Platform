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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("details")]
        [StringLength(500)]
        public string? Details { get; set; }

        [Column("employee_id")]
        public int? EmployeeId { get; set; }

        [Required]
        [Column("asset_type_id")]
        public int AssetTypeId { get; set; }

        [Required]
        [Column("status_id")]
        public int StatusId { get; set; }

        [Required]
        [Column("receive_date", TypeName = "date")]
        public DateTime ReceiveDate { get; set; }

        [Column("return_date", TypeName = "date")]
        public DateTime? ReturnDate { get; set; }

        // Navigation properties
        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [ForeignKey("AssetTypeId")]
        public AssetType AssetType { get; set; }

        [ForeignKey("StatusId")]
        public AssetStatus Status { get; set; }
    }
}