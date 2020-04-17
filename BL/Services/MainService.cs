using AutoMapper;
using BL.Models;
using BL.Services.Interfaces;
using ClosedXML.Excel;
using DAL;
using DAL.DbObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Services
{
    public class MainService : IMainService
    {
        private readonly DCAContext _context;
        private readonly IMapper _mapper;

        public MainService(DCAContext dCAContext, IMapper mapper)
        {
            _context = dCAContext;
            _mapper = mapper;
        }

        #region IMainInterface
        public async Task UploadDataAsync(Stream data, CancellationToken ct)
        {
            XLWorkbook workbook = new XLWorkbook(data);

            if (workbook.Worksheets is null)
                return;

            foreach (IXLWorksheet worksheet in workbook.Worksheets)
            {
                List<IXLRow> rowList = worksheet.RowsUsed().ToList();

                if (rowList.Count == 0)
                    continue;

                bool haveHeader = rowList.First().Cells().Where(c => !c.IsEmpty()).Take(20).All(c => c.Value.ToString().ToLower().StartsWith("col"));

                IXLRow header = haveHeader ? rowList.First() : null;

                if (haveHeader)
                    rowList = rowList.Skip(1).ToList();

                //в задании указано парсить по 1 строке с каждого листа, поэтому заменяем список строк на первую строку
                rowList = rowList.Take(1).ToList();

                List<SheetRecordDataModel> itemList = new List<SheetRecordDataModel>();

                foreach (IXLRow row in rowList)
                {
                    List<IXLCell> cellInRowList = row.Cells().Where(c => {
                        bool result = !c.IsMerged() || c.MergedRange().FirstCell().Address.Equals(c.Address);
                        return result;
                    }).Take(20).ToList();

                    if (cellInRowList.All(c => c.IsEmpty()))
                        continue;

                    SheetRecordDataModel item = new SheetRecordDataModel();

                    int cellNumber = 0;

                    foreach (IXLCell cell in cellInRowList)
                    {
                        if (ct.IsCancellationRequested)
                            throw new OperationCanceledException("Загрузка отменена пользователем.");

                        cellNumber++;

                        if (cell.IsEmpty())
                            continue;

                        PropertyInfo field;

                        //если есть шапка с названиями колонок, то заполняем модель по ней, если нет - по порядку
                        if (haveHeader)
                        {
                            var columnName = header.Cell(cellNumber.ToString()).Value.ToString();

                            field = item.GetType().GetProperties().FirstOrDefault(f => f.Name.ToLower().Equals(columnName));
                        }
                        else
                            field = item.GetType().GetProperties().FirstOrDefault(f => f.Name.ToLower().Contains(cellNumber.ToString()));

                        if (field != null)
                            field.SetValue(item, cell.Value.ToString());
                    }

                    itemList.Add(item);
                }

                switch (worksheet.Position)
                {
                    case 1:
                        await AddFirstSheetItemsAsync(itemList);
                        break;
                    case 2:
                        await AddSecondSheetItemsAsync(itemList);
                        break;
                }
            }
        }

        public async Task<IEnumerable<SheetRecordDataModel>> GetFirstSheetRecordListAsync()
        {
            List<SheetRecordDataModel> recordList = await _context.FirstSheetItems.Where(i => !i.DtDelete.HasValue).AsNoTracking()
                .Select(i => _mapper.Map<SheetRecordDataModel>(i)).ToListAsync();

            return recordList;
        }

        public async Task<IEnumerable<SheetRecordDataModel>> GetSecondSheetRecordListAsync()
        {
            List<SheetRecordDataModel> recordList = await _context.SecondSheetItems.Where(i => !i.DtDelete.HasValue).AsNoTracking()
                .Select(i => _mapper.Map<SheetRecordDataModel>(i)).ToListAsync();

            return recordList;
        }

        public async Task<SheetRecordDataModel> GetFirstSheetRecordAsync(int id)
        {
            FirstSheetItem entity = await _context.FirstSheetItems.Where(i => !i.DtDelete.HasValue).AsNoTracking()
                .FirstOrDefaultAsync(x => x.ID == id);

            if (entity is null)
                return null;

            return _mapper.Map<SheetRecordDataModel>(entity);
        }

        public async Task<SheetRecordDataModel> GetSecondSheetRecordAsync(int id)
        {
            SecondSheetItem entity = await _context.SecondSheetItems.Where(i => !i.DtDelete.HasValue).AsNoTracking()
                .FirstOrDefaultAsync(x => x.ID == id);

            if (entity is null)
                return null;

            return _mapper.Map<SheetRecordDataModel>(entity);
        }

        public async Task UpdateFirstSheetRecordAsync(SheetRecordDataModel sheetItem)
        {
            FirstSheetItem entityFromDb = await _context.FirstSheetItems.Where(x => !x.DtDelete.HasValue).AsNoTracking()
                .FirstOrDefaultAsync(i => i.ID == sheetItem.ID);

            if (entityFromDb is null || entityFromDb.DtDelete.HasValue)
                throw new Exception("Запись не найдена.");

            FirstSheetItem entity = _mapper.Map<FirstSheetItem>(sheetItem);

            entity.DtAdd = entityFromDb.DtAdd;
            entity.DtEdit = DateTime.UtcNow;

            try
            {
                _context.Entry(entity).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateSecondSheetRecordAsync(SheetRecordDataModel sheetItem)
        {
            SecondSheetItem entityFromDb = await _context.SecondSheetItems.Where(x => !x.DtDelete.HasValue).AsNoTracking()
                .FirstOrDefaultAsync(i => i.ID == sheetItem.ID);

            if (entityFromDb is null || entityFromDb.DtDelete.HasValue)
                throw new Exception("Запись не найдена.");

            SecondSheetItem entity = _mapper.Map<SecondSheetItem>(sheetItem);

            entity.DtAdd = entityFromDb.DtAdd;
            entity.DtEdit = DateTime.UtcNow;

            try
            {
                _context.Entry(entity).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteRecordAsync(int sheetNumber, int id)
        {
            switch (sheetNumber)
            {
                case 1:
                    await DeleteFirstSheetItemAsync(id);
                    break;
                case 2:
                    await DeleteSecondSheetItemAsync(id);
                    break;
            }
        }
        #endregion

        private async Task AddFirstSheetItemsAsync(List<SheetRecordDataModel> itemList)
        {
            List<FirstSheetItem> entityList = itemList.Select(i =>
            {
                FirstSheetItem entity = _mapper.Map<FirstSheetItem>(i);
                entity.ID = 0;
                return entity;
            }).ToList();

            try
            {
                _context.FirstSheetItems.AddRange(entityList);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task AddSecondSheetItemsAsync(List<SheetRecordDataModel> itemList)
        {
            List<SecondSheetItem> entityList = itemList.Select(i =>
            {
                SecondSheetItem entity = _mapper.Map<SecondSheetItem>(i);
                entity.ID = 0;
                return entity;
            }).ToList();

            try
            {
                _context.SecondSheetItems.AddRange(entityList);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task DeleteFirstSheetItemAsync(int id)
        {
            FirstSheetItem entity = await _context.FirstSheetItems.Where(x => !x.DtDelete.HasValue)
                .FirstOrDefaultAsync(i => i.ID == id);

            if (entity is null || entity.DtDelete.HasValue)
                throw new Exception("Запись не найдена.");

            entity.DtDelete = DateTime.UtcNow;

            try
            {
                _context.Entry(entity).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task DeleteSecondSheetItemAsync(int id)
        {
            SecondSheetItem entity = await _context.SecondSheetItems.Where(x => !x.DtDelete.HasValue)
                .FirstOrDefaultAsync(i => i.ID == id);

            if (entity is null || entity.DtDelete.HasValue)
                throw new Exception("Запись не найдена.");

            try
            {
                _context.SecondSheetItems.Remove(entity);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
