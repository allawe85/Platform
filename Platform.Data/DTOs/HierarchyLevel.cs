using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Data.DTOs
{
    [Table("hierarchy_level")]
    public class HierarchyLevel
    {
        [Key]
        [Column("id")]

        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("name_ar")]
        public string NameAr { get; set; }
    }
}
