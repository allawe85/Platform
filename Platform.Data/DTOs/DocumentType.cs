using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("document_type")]
    public class DocumentType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("name_ar")]
        public string NameAr { get; set; }
        // Adding a list from the Document table while document type id is foriegn to support getting documents by document type id
        public ICollection<Document>? Documents { get; set; } = new List<Document>();
    }
}