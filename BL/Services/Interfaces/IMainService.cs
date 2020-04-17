using BL.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Services.Interfaces
{
    public interface IMainService
    {
        /// <summary>
        /// Сохранить данные из файла Excel
        /// </summary>
        /// <param name="data">данные</param>
        /// <param name="ct">токен отмены</param>
        /// <returns></returns>
        Task UploadDataAsync(Stream data, CancellationToken ct);

        /// <summary>
        /// Получить записи первого листа
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SheetRecordDataModel>> GetFirstSheetRecordListAsync();

        /// <summary>
        /// Получить записи второго листа
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SheetRecordDataModel>> GetSecondSheetRecordListAsync();

        /// <summary>
        /// Получить запись первого листа
        /// </summary>
        /// <param name="id">идентификатор записи</param>
        /// <returns></returns>
        Task<SheetRecordDataModel> GetFirstSheetRecordAsync(int id);

        /// <summary>
        /// Получить запись второго листа
        /// </summary>
        /// <param name="id">идентификатор записи</param>
        /// <returns></returns>
        Task<SheetRecordDataModel> GetSecondSheetRecordAsync(int id);

        /// <summary>
        /// Редактировать запись первого листа
        /// </summary>
        /// <param name="sheetItem">запись</param>
        /// <returns></returns>
        Task UpdateFirstSheetRecordAsync(SheetRecordDataModel sheetItem);

        /// <summary>
        /// Редактировать запись второго листа
        /// </summary>
        /// <param name="sheetItem">запись</param>
        /// <returns></returns>
        Task UpdateSecondSheetRecordAsync(SheetRecordDataModel sheetItem);

        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="sheetNumber">номер листа</param>
        /// <param name="id">идентификатор записи</param>
        /// <returns></returns>
        Task DeleteRecordAsync(int sheetNumber, int id);
    }
}
