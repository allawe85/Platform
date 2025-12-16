using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("asset_status")]
    public class AssetStatus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("name_ar")]
        public string NameAr { get; set; }

        // Navigation
        public ICollection<Asset>? Assets { get; set; } = new List<Asset>();
    }
}