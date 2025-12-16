using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("asset_type")]
    public class AssetType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        // Navigation
        public ICollection<Asset>? Assets { get; set; } = new List<Asset>();
    }
}