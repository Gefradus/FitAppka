using FitAppka.Models.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;

namespace FitAppka.Reports
{
    public class DaysReport
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DaysReport(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        #region Declaration
        readonly int _maxColumn = 6;
        Document _document;
        Font _fontStyle;
        readonly PdfPTable _pdfTable = new PdfPTable(6);
        PdfPCell _pdfCell;
        readonly MemoryStream _memoryStream = new MemoryStream();
        List<DaySummaryDTO> _days = new List<DaySummaryDTO>();
        #endregion

        public byte[] Report(List<DaySummaryDTO> days)
        {
            _days = days;
            _document = new Document();
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 40f, 20f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            float[] sizes = new float[_maxColumn];
            for(int i = 0; i < _maxColumn; i++) {
                sizes[i] = 100; 
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
                Colspan = 2,
                Border = 0
            };
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(SetPageTitle())
            {
                Colspan = 4,
                Border = 0
            };
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
        }

        private PdfPTable AddLogo()
        {
            int maxColumn = 6;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);
            Image img = Image.GetInstance(Path.Combine(_webHostEnvironment.WebRootPath + "/img", "logo.png"));
            _pdfCell = new PdfPCell(img) 
            {
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
            int maxColumn = 6;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);
            _fontStyle = FontFactory.GetFont("Tahoma",18f,1);
            _pdfCell = new PdfPCell(new Phrase("Raport z prowadzonej diety"))
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
            for(int i = 0; i < count; i++)
            {
                _pdfCell = new PdfPCell(new Phrase(""))
                {
                    Colspan = _maxColumn,
                    Border = 0,
                    ExtraParagraphSpace = 10
                };
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
            }
        }

        private void ReportBody()
        {
            CreateTableHeaderTip();
            CreateTableHeader();
            CreateTableBody();
        }

        private void CreateTableBody() {
            _fontStyle = FontFactory.GetFont("Tahoma", 12f, 0);
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LightGray;

            foreach (var item in _days)
            {
                AddCellPhrase(item.Date.ToString("dd.MM.yyyy") + "r.");
                AddCellPhrase(item.KcalConsumed.ToString());
                AddCellPhrase(item.ProteinsConsumed.ToString());
                AddCellPhrase(item.FatsConsumed.ToString());
                AddCellPhrase(item.CarbohydratesConsumed.ToString());
                AddCellPhrase(item.WaterDrunk.ToString());
                _pdfTable.CompleteRow();
            }
        }

        private void CreateTableHeaderTip()
        {
            _fontStyle = FontFactory.GetFont("Tahoma", BaseFont.CP1250, 12f, 1, BaseColor.White);
            _pdfCell = new PdfPCell { Border = 0 };
            AddCellPhrase(string.Empty);
            _pdfCell = new PdfPCell
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.DarkGray,
                Colspan = 5
            };
            AddCellPhrase("Spożycie / cel");
            _pdfCell.Colspan = 1;
        }

        private void CreateTableHeader()
        {
            AddCellPhrase("Dzień");
            AddCellPhrase("Kalorie [kcal]");
            AddCellPhrase("Białko [g]");
            AddCellPhrase("Tłuszcze [g]");
            AddCellPhrase("Węgl. [g]");
            AddCellPhrase("Woda [ml]");

            _pdfTable.CompleteRow();
        }

        private void AddCellPhrase(string s)
        {
            _pdfCell.Phrase = new Phrase(s, _fontStyle);
            _pdfTable.AddCell(_pdfCell);
        }

    }
}
