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
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollaboratorId { get; set; }
        [ForeignKey("NotesCollaborator")]
        public int NotesId { get; set; }
        [ForeignKey("UserCollaborator")]
        public int UserId { get; set; }
        public string Email {  get; set; }
        public DateTime AddedAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public virtual NotesEntity NotesCollaborator {  get; set; }
        [JsonIgnore]
        public virtual UserEntity UserCollaborator {  get; set; }

    }
}
