using BL;
using BL.Models;
using DAL.DbObjects;
using DeptCollectionAgency.Tools;
using System;
using System.ComponentModel.DataAnnotations;

namespace DeptCollectionAgency.ViewModel
{
    public class SheetRecordViewModel : SheetItem
    {
        public string SheetName { get; set; }

        [Required(ErrorMessage = "Выберите номер листа")]
        [Range(1, 2, ErrorMessage = "Номер листа может быть только 1 или 2")]
        public int SheetNumber { get; set; }

    }
}
