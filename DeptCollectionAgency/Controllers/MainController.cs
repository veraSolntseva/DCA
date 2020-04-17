using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BL.Models;
using BL.Services.Interfaces;
using DeptCollectionAgency.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeptCollectionAgency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IMainService _mainService;
        private readonly IMapper _mapper;

        public MainController(IMainService mainService, IMapper mapper)
        {
            _mainService = mainService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetAllRecordsAsync(DateTime? startDt, DateTime? endDt)
        {
            List<SheetRecordViewModel> recordList = new List<SheetRecordViewModel>();

            try
            {
                IEnumerable<SheetRecordDataModel> recordFirstEnumeration = await _mainService.GetFirstSheetRecordListAsync();

                IEnumerable<SheetRecordDataModel> recordSecondEnumeration = await _mainService.GetSecondSheetRecordListAsync();

                recordList = recordFirstEnumeration.Union(recordSecondEnumeration).Select(i => _mapper.Map<SheetRecordViewModel>(i)).ToList();

                if (startDt.HasValue)
                    recordList = recordList.Where(i => i.DtAdd >= startDt.Value).ToList();

                if (endDt.HasValue)
                    recordList = recordList.Where(i => i.DtAdd <= endDt.Value).ToList();
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

            return new JsonResult(recordList.OrderBy(r => r.DtAdd));
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetFirstSheetRecordsAsync(DateTime? startDt, DateTime? endDt)
        {
            List<SheetRecordViewModel> recordList = new List<SheetRecordViewModel>();

            try
            {
                IEnumerable<SheetRecordDataModel> recordFirstEnumeration = await _mainService.GetFirstSheetRecordListAsync();

                recordList = recordFirstEnumeration.Select(i => _mapper.Map<SheetRecordViewModel>(i)).ToList();

                if (startDt.HasValue)
                    recordList = recordList.Where(i => i.DtAdd >= startDt.Value).ToList();

                if (endDt.HasValue)
                    recordList = recordList.Where(i => i.DtAdd <= endDt.Value).ToList();
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

            return new JsonResult(recordList.OrderBy(r => r.DtAdd));
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetSecondSheetRecordsAsync(DateTime? startDt, DateTime? endDt)
        {
            List<SheetRecordViewModel> recordList = new List<SheetRecordViewModel>();

            try
            {
                IEnumerable<SheetRecordDataModel> recordSecondEnumeration = await _mainService.GetSecondSheetRecordListAsync();

                recordList = recordSecondEnumeration.Select(i => _mapper.Map<SheetRecordViewModel>(i)).ToList();

                if (startDt.HasValue)
                    recordList = recordList.Where(i => i.DtAdd >= startDt.Value).ToList();

                if (endDt.HasValue)
                    recordList = recordList.Where(i => i.DtAdd <= endDt.Value).ToList();
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

            return new JsonResult(recordList.OrderBy(r => r.DtAdd));
        }

        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetFirstSheetRecordAsync(int id)
        {
            if (id <= 0)
                return new JsonResult("Некорректный идентификатор.");

            SheetRecordDataModel record = await _mainService.GetFirstSheetRecordAsync(id);

            if (record is null)
                return new JsonResult(new { error = "Запись не найдена." });

            SheetRecordViewModel model = _mapper.Map<SheetRecordViewModel>(record);

            return new JsonResult(model);
        }

        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetSecondSheetRecordAsync(int id)
        {
            if (id <= 0)
                return new JsonResult("Некорректный идентификатор.");

            SheetRecordDataModel record = await _mainService.GetSecondSheetRecordAsync(id);

            if (record is null)
                return new JsonResult(new { error = "Запись не найдена." });

            SheetRecordViewModel model = _mapper.Map<SheetRecordViewModel>(record);

            return new JsonResult(model);
        }

        [HttpGet("[action]/{sheet}/{id}")]
        public async Task<JsonResult> GetRecordAsync(int sheet, int id)
        {
            if (id <= 0)
                return new JsonResult("Некорректный идентификатор записи.");

            if (sheet <= 0)
                return new JsonResult("Некорректный идентификатор листа.");

            SheetRecordDataModel record = default;

            switch (sheet)
            {
                case 1: 
                    record = await _mainService.GetFirstSheetRecordAsync(id);
                    break;
                case 2:
                    record = await _mainService.GetSecondSheetRecordAsync(id);
                    break;
            }

            if (record is null || record is (SheetRecordDataModel)default)
                return new JsonResult(new { error = "Запись не найдена." });

            return new JsonResult(_mapper.Map<SheetRecordViewModel>(record));
        }

        [HttpPost("file/upload")]
        public async Task<IActionResult> UploadDataFromFileAsync(IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран или пустой.");

            string fileExtension = Path.GetExtension(file.FileName);

            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                return BadRequest("Некорректный формат файла.");

            try
            {
                using (Stream data = file.OpenReadStream())
                {
                    await _mainService.UploadDataAsync(data, ct);
                }
            }
            catch (OperationCanceledException ex)
            {
                return Ok(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Данные из файла сохранены.");
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> EditFirstSheetRecordAsync([FromBody] SheetRecordViewModel record)
        {
            if (!ModelState.IsValid || record.ID < 1)
                return BadRequest("Некорректные данные.");

            SheetRecordDataModel recordDataModel = _mapper.Map<SheetRecordDataModel>(record);

            try
            {
                await _mainService.UpdateFirstSheetRecordAsync(recordDataModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Запись обновлена.");
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> EditSecondSheetRecordAsync([FromBody] SheetRecordViewModel record)
        {
            if (!ModelState.IsValid || record.ID < 1)
                return BadRequest("Некорректные данные.");

            SheetRecordDataModel recordDataModel = _mapper.Map<SheetRecordDataModel>(record);

            try
            {
                await _mainService.UpdateSecondSheetRecordAsync(recordDataModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Запись обновлена.");
        }

        [HttpDelete("[action]/{sheet}/{id}")]
        public async Task<IActionResult> DeleteRecordAsync(int sheet, int id)
        {
            if (id <= 0)
                return BadRequest("Некорректный идентификатор записи.");

            if (sheet <= 0)
                return BadRequest("Некорректный идентификатор листа.");

            try
            {
                await _mainService.DeleteRecordAsync(sheet, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Запись удалена.");
        }
    }
}
