using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExactAssignment.BLL
{
    public class ExactOnlineFile
    {
        public string ID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool isFolder { get; set; }
        public ulong FileSizeInBytes { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool isDeleted { get; set; }
        public string FileURL { get; set; }

    }
}
