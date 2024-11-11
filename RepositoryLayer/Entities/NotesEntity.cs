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
    public class NotesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotesId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime? Remainder { get; set; } 
        public String Colour { get; set; } = "";
        public String Image { get; set; } = "";
        public bool IsArchive { get; set; }
        public bool IsPin { get; set; }
        public bool IsTrash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("NotesUser")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual UserEntity NotesUser { get; set; }




    }
}
