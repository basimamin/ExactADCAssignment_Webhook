using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExactAssignment.BLL
{
    public class ExactOnlineFolder
    {
        public string FolderId { get; set; }
        public string FolderName { get; set; }                
        public DateTime ModificationDate { get; set; }
        public string ParentFolder { get; set; }        

    }
}
