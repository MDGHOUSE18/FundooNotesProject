using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Request_Models
{
    public class UpdateNotesModel
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime Remainder { get; set; }
        public String Colour { get; set; } = "";
        public String Image { get; set; } = "";
        public bool IsArchive { get; set; }
        public bool IsPin { get; set; }
        public bool IsTrash { get; set; }

    }
}
