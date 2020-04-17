using System.ComponentModel.DataAnnotations;

namespace BL
{
    public enum SheetEnum
    {
        [Display(Name = "Лист 1")]
        First = 1,

        [Display(Name = "Лист 2")]
        Second = 2
    }
}
