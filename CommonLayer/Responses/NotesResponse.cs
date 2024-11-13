using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Responses
{
    public class NotesResponse
    {
        public int NotesId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Colour { get; set; } = "";
        public String Image { get; set; } = "";
        public bool IsArchive { get; set; }
        public bool IsPin { get; set; }
        public bool IsTrash { get; set; }
        public List<string> Labels { get; set; } = new List<string>();

    }
}
