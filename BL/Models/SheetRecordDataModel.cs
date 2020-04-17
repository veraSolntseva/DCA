using DAL.DbObjects;
using System;

namespace BL.Models
{
    public class SheetRecordDataModel : SheetItem
    {
        public SheetEnum Sheet { get; set; }

        public SheetRecordDataModel()
        {
            DtAdd = DateTime.UtcNow;
        }
    }
}
