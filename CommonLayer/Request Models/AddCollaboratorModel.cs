using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Request_Models
{
    public class AddCollaboratorModel
    {
        public int NotesId { get; set; }
        public string Email { get; set; }
    }
}
