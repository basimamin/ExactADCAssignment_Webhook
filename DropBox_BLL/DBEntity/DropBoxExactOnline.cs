//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DropBox_BLL.DBEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class DropBoxExactOnline
    {
        public int Id { get; set; }
        public string DropBoxPath { get; set; }
        public string ExactOnlineGUID { get; set; }
        public byte isFile { get; set; }
        public System.DateTime DropBoxFileModifiedDate { get; set; }
        public byte FileStillAlive { get; set; }
    }
}