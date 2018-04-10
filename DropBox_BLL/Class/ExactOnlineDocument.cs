using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExactAssignment.BLL
{
    public class ExactOnlineDocument
    {
        public string DocumentId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }        
        public ulong FileSizeInBytes { get; set; }
        public DateTime ModificationDate { get; set; }        
        public string FileURL { get; set; }
    }
}
