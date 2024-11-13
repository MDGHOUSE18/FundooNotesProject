using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class LabelEntityNotesEntity
    {
        [Key]
        [ForeignKey("LabelId")]
        public int Label_Id { get; set; }
        [ForeignKey("Notes_Id")]
        public int NotesId { get; set; }

        
        public LabelEntity LabelId { get; set; }
        public NotesEntity Notes_Id { get; set; }
    }

}
