using FitAppka.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FitAppka.Reports
{
    public class DayReport
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DayReport(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        #region Declaration
        readonly int _maxColumn = 3;
        Document _document;
        Font _fontStyle;
        readonly PdfPTable _pdfTable = new PdfPTable(3);
        PdfPCell _pdfCell;
        readonly MemoryStream _memoryStream = new MemoryStream();
        List<Day> _days = new List<Day>();
        #endregion

        public byte[] Report(List<Day> days)
        {
            _days = days;
            _document = new Document();
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(5f, 5f, 20f, 5f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter pdfWriter = PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            float[] sizes = new float[_maxColumn];
            for(int i = 0; i < _maxColumn; i++) 
            {
                if(i == 0) sizes[i] = 20;
                else sizes[i] = 100; 
            }
            _pdfTable.SetWidths(sizes);
            ReportHeader();
            EmptyRow(2);
            ReportBody();
            _pdfTable.HeaderRows = 2;
            _document.Add(_pdfTable);
            _document.Close();
            return _memoryStream.ToArray();
        }

        private void ReportHeader()
        {
            _pdfCell = new PdfPCell(AddLogo())
            {
                Colspan = 1,
                Border = 0
            };
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();

            _pdfCell = new PdfPCell(SetPageTitle())
            {
                Colspan = _maxColumn - 1,
                Border = 0
            };
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
        }

        private PdfPTable AddLogo()
        {
            int maxColumn = 1;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);
            string path = _webHostEnvironment.WebRootPath + "/img";
            string imgCombine = Path.Combine(path, "logo.png");
            Image img = Image.GetInstance(imgCombine);
            _pdfCell = new PdfPCell(img) {
                Colspan = maxColumn,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            pdfPTable.AddCell(_pdfCell);
            pdfPTable.CompleteRow();
            return pdfPTable;
        }

        private PdfPTable SetPageTitle()
        {
            int maxColumn = 3;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);
            _fontStyle = FontFactory.GetFont("Tahoma",18f,1);
            _pdfCell = new PdfPCell(new Phrase("Raport z postępów prowadzenia diety"))
            {
                Colspan = maxColumn,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            pdfPTable.AddCell(_pdfCell);
            pdfPTable.CompleteRow();
            return pdfPTable;
        }

        private void EmptyRow(int count)
        {
            for(int i = 1; i < count; i++)
            {
                _pdfCell = new PdfPCell(new Phrase("Raport z postępów prowadzenia diety"))
                {
                    Colspan = _maxColumn,
                    Border = 0,
                    ExtraParagraphSpace = 0
                };
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
            }
        }

        private void ReportBody()
        {
            var fontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            #region Detail table header
            _pdfCell = new PdfPCell(new Phrase("Date", fontStyleBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.Gray
            };
            _pdfTable.AddCell(_pdfCell);

            _pdfCell.Phrase = new Phrase("Kcal", fontStyleBold);
            _pdfTable.AddCell(_pdfCell);

            _pdfCell.Phrase = new Phrase("Plyny", fontStyleBold);
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();
            #endregion

            #region Detail table body
            foreach (var item in _days) {
                _pdfCell = new PdfPCell(new Phrase(item.Date.Value.ToString("dd-MM-yyyy"), _fontStyle))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.Gray
                };
                _pdfTable.AddCell(_pdfCell);

                _pdfCell.Phrase = new Phrase("0", _fontStyle);
                _pdfTable.AddCell(_pdfCell);

                _pdfCell.Phrase = new Phrase(item.WaterDrunk.ToString(), _fontStyle);
                _pdfTable.AddCell(_pdfCell);

                _pdfTable.CompleteRow();
            }
            #endregion
        }
    }
}
