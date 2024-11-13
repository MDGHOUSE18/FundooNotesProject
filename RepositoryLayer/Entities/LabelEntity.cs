using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Many-to-many relationship with NotesEntity
        [JsonIgnore]
        public List<NotesEntity> Notes { get; set; } = new List<NotesEntity>();

        [ForeignKey("UserLabel")]
        public int userId { get; set; }
        [JsonIgnore]
        public UserEntity UserLabel { get; set; }
    }
}
