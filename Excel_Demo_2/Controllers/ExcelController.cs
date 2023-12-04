using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Excel_Demo_2.Models;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System.Drawing.Imaging;
using OfficeOpenXml.Drawing.Chart;

public class ExcelController : Controller
{
    private readonly DataContext _dbContext;

    public ExcelController(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult UploadExcel()
    {
        try
        {
            var file = HttpContext.Request.Form.Files[0];

            if (file != null && file.Length > 0)
            {
                var data = ImportExcel(file);

                var existingRecords = _dbContext.peerGroups.ToList();
                _dbContext.peerGroups.RemoveRange(existingRecords);

                _dbContext.SaveChanges();

                _dbContext.peerGroups.AddRange(data.PeerGroups);
                _dbContext.SaveChanges();

                ViewBag.WorksheetCount = data.WorksheetCount;

                return View("\\Views\\TableData.cshtml", data.PeerGroups);
            }
            else
            {
                return BadRequest("No file uploadeds.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Error uploading file: {ex.Message}");
        }
    }

    public (List<PeerGroup> PeerGroups, int WorksheetCount) ImportExcel(IFormFile file)
     {
        List<PeerGroup> peerGroups = new List<PeerGroup>();
        int worksheetCount = 0;

        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            stream.Position = 0;

            using (var package = new ExcelPackage(stream))
            {
                worksheetCount = package.Workbook.Worksheets.Count;

                ExcelWorksheet worksheet = package.Workbook.Worksheets[2]; 

                int startRow = 60;
                int endRow = 76;
                int columnIndex = 23;
                int targetColumn = 20;

                
                worksheet.Column(targetColumn).Style.Font.Bold = true;

                for (int row = startRow; row <= endRow; row++)
                {
                    PeerGroup peerGroup = new PeerGroup
                    {

                        Particular = GetCellValue(worksheet, row, targetColumn),
                        Unit = GetCellValue(worksheet, row, 21),
                        Empty = GetCellValue(worksheet, row, 22),
                        IndustryQuartile = GetChartBytes(worksheet, row, 23),
                        PeerGroupAverage = GetCellValue(worksheet, row, 24),
                        PeerGroupMedian = GetCellValue(worksheet, row, 25),
                        PeerGroupMin = GetCellValue(worksheet, row, 26),
                        PeerGroupMax = GetCellValue(worksheet, row, 27)
                    };

                    peerGroups.Add(peerGroup);
                }
       
            }
        }

        return (peerGroups, worksheetCount);
    }

    private string GetCellValue(ExcelWorksheet worksheet, int row, int columnIndex)
    {
        var cell = worksheet.Cells[row, columnIndex];
        return cell?.Text ?? string.Empty;
    }

    private byte[] GetChartBytes(ExcelWorksheet worksheet, int row, int columnIndex)
    {
        if (worksheet.Drawings.Count > 0)
        {
            foreach (var drawing in worksheet.Drawings)
            {
                if (drawing is ExcelPicture excelPicture && excelPicture.Name.Contains("Chart"))
                {
                    using (var stream = new MemoryStream(excelPicture.Image.ImageBytes))
                    {
                        return stream.ToArray();
                    }
                }
            }
        }

        return null;
    }










}
