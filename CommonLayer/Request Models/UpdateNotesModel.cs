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
        public String Image { get; set; } = "";

    }
}
