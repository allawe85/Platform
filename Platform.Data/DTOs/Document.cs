using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("document")]
    public class Document
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Column("document_type_id")]
        public int DocumentTypeId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("details")]
        public string? Details { get; set; }

        [Column("expiry_date", TypeName = "date")]
        public DateTime ExpiryDate { get; set; }

        // Foriegn-Primary relationship navigation properties
        public Employee? Employee { get; set; }

        public DocumentType? DocumentType { get; set; }
    }
}