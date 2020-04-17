using AutoMapper;
using BL.Models;
using DAL.DbObjects;
using DeptCollectionAgency.ViewModel;
using DeptCollectionAgency.Tools;
using BL;

namespace DeptCollectionAgency.Mapper
{
    public class MainMapperProfile : Profile
    {
        public MainMapperProfile()
        {
            CreateMap<SheetRecordDataModel, SheetRecordViewModel>()
                .ForMember(x => x.SheetName, opt => opt.MapFrom(y => y.Sheet.GetDisplayValue()))
                .ForMember(x => x.SheetNumber, opt => opt.MapFrom(e => (int)e.Sheet));

            CreateMap<SheetRecordViewModel, SheetRecordDataModel>()
                .ForMember(x => x.Sheet, opt => opt.MapFrom(y => (SheetEnum)y.SheetNumber));

            CreateMap<FirstSheetItem, SheetRecordDataModel>()
                .ForMember(x => x.DtAdd, opt => opt.MapFrom(y => y.DtAdd.ToLocalTime()))
                .ForMember(x => x.DtEdit, opt => opt.MapFrom(y => y.DtEdit.HasValue ? y.DtEdit.Value.ToLocalTime() : y.DtEdit))
                .ForMember(x => x.DtDelete, opt => opt.MapFrom(y => y.DtDelete.HasValue ? y.DtDelete.Value.ToLocalTime() : y.DtDelete))
                .ForMember(x => x.Sheet, opt => opt.MapFrom(y => SheetEnum.First));

            CreateMap<SecondSheetItem, SheetRecordDataModel>()
                .ForMember(x => x.DtAdd, opt => opt.MapFrom(y => y.DtAdd.ToLocalTime()))
                .ForMember(x => x.DtEdit, opt => opt.MapFrom(y => y.DtEdit.HasValue ? y.DtEdit.Value.ToLocalTime() : y.DtEdit))
                .ForMember(x => x.DtDelete, opt => opt.MapFrom(y => y.DtDelete.HasValue ? y.DtDelete.Value.ToLocalTime() : y.DtDelete))
                .ForMember(x => x.Sheet, opt => opt.MapFrom(y => SheetEnum.Second));

            CreateMap<SheetRecordDataModel, FirstSheetItem>();

            CreateMap<SheetRecordDataModel, SecondSheetItem>();

        }
    }
}
