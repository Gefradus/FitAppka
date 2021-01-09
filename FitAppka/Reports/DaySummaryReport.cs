using FitnessApp.Models.DTO.DaySummaryDTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace FitnessApp.Reports
{
    public class DaySummaryReport
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DaySummaryReport(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        #region Declaration
        readonly int _maxColumn = 4;
        Document _document;
        Font _fontStyle;
        readonly PdfPTable _pdfTable = new PdfPTable(4);
        PdfPCell _pdfCell;
        readonly MemoryStream _memoryStream = new MemoryStream();
        DaySummaryWithDetailsDTO _daySummary = new DaySummaryWithDetailsDTO();
        #endregion

        public byte[] Report(DaySummaryWithDetailsDTO daySummary)
        {
            _daySummary = daySummary;
            _document = new Document();
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 40f, 20f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            float[] sizes = new float[_maxColumn];
            for (int i = 0; i < _maxColumn; i++)
            {
                sizes[i] = 100;
            }
            _pdfTable.SetWidths(sizes);

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
                Colspan = 2,
                Border = 0,
                VerticalAlignment = Element.ALIGN_BOTTOM,
            };
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
        }

        private PdfPTable AddLogo()
        {
            int maxColumn = 4;
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
            int maxColumn = 4;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);
            _fontStyle = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 14f, 0);
            _pdfCell = new PdfPCell(new Phrase("DZIENNY RAPORT PROWADZONEJ DIETY", _fontStyle))
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
            for (int i = 0; i < count; i++)
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
            ReportHeader();
            EmptyRow(6);
            CreateDayRow();
            CreateTableHeader();
            CreateTableBody();
        }

        private void CreateDayRow()
        {
            EmptyRow(5);
            _fontStyle = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 18f, 0);
            AddCellPhrase("Dzień: " + _daySummary.DaySummaryDTO.Date.ToString("dd.MM.yyyy")+"r.");
        }
       
        private void CreateTableBody()
        {
            var s = _daySummary.DaySummaryDTO;
            var d = _daySummary.DetailsDTO;

            AddFirstColumnCellPhrase("Kalorie [kcal]: ");
            AddBasicCellPhrase(RoundDouble(s.KcalConsumed));
            AddBasicCellPhrase(RoundDouble(s.KcalGoal));
            AddBasicCellPhrase(RoundDouble(s.KcalConsumed - s.KcalGoal));
            _pdfTable.CompleteRow();

            AddFirstColumnCellPhrase("Białko [g]: ");
            AddBasicCellPhrase(RoundDouble(s.ProteinsConsumed));
            AddBasicCellPhrase(RoundDouble(s.ProteinsGoal));
            AddBasicCellPhrase(RoundDouble(s.ProteinsConsumed - s.ProteinsGoal));

            AddFirstColumnCellPhrase("Tłuszcze [g]: ");
            AddBasicCellPhrase(RoundDouble(s.FatsConsumed));
            AddBasicCellPhrase(RoundDouble(s.FatsGoal));
            AddBasicCellPhrase(RoundDouble(s.FatsConsumed - s.FatsGoal));

            AddFirstColumnCellPhrase("Węglowodany [g]: ");
            AddBasicCellPhrase(RoundDouble(s.CarbohydratesConsumed));
            AddBasicCellPhrase(RoundDouble(s.CarbohydratesGoal));
            AddBasicCellPhrase(RoundDouble(s.CarbohydratesConsumed - s.CarbohydratesGoal));

            AddFirstColumnCellPhrase("Woda [ml]: ");
            AddBasicCellPhrase(s.WaterDrunk.ToString());
            AddBasicCellPhrase(RoundDouble(s.KcalGoal));
            AddBasicCellPhrase(RoundDouble(s.WaterDrunk - s.KcalGoal));

            AddFirstColumnCellPhrase("Wit. A [µg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminA_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminA_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminA_Consumed - d.VitaminA_Goal));

            AddFirstColumnCellPhrase("Wit. C [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminC_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminC_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminC_Consumed - d.VitaminC_Goal));

            AddFirstColumnCellPhrase("Wit. D [µg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminD_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminD_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminD_Consumed - d.VitaminD_Goal));

            AddFirstColumnCellPhrase("Wit. K [µg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminK_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminK_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminK_Consumed - d.VitaminK_Goal));

            AddFirstColumnCellPhrase("Wit. E [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminE_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminE_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminE_Consumed - d.VitaminE_Goal));

            AddFirstColumnCellPhrase("Wit. B1 [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB1_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB1_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB1_Consumed - d.VitaminB1_Goal));

            AddFirstColumnCellPhrase("Wit. B2 [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB2_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB2_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB2_Consumed - d.VitaminB2_Goal));

            AddFirstColumnCellPhrase("Wit. B5 [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB5_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB5_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB5_Consumed - d.VitaminB5_Goal));

            AddFirstColumnCellPhrase("Wit. B6 [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB6_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB6_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB6_Consumed - d.VitaminB6_Goal));

            AddFirstColumnCellPhrase("Kwas foliowy [µg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.FolicAcid_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.FolicAcid_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.FolicAcid_Consumed - d.FolicAcid_Goal));

            AddFirstColumnCellPhrase("Wit. B12 [µg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB12_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB12_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminB12_Consumed - d.VitaminB12_Goal));

            AddFirstColumnCellPhrase("Wit. PP [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminPp_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminPp_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.VitaminPp_Consumed - d.VitaminPp_Goal));

            AddFirstColumnCellPhrase("Biotyna [µg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Biotin_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Biotin_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Biotin_Consumed - d.Biotin_Goal));

            AddFirstColumnCellPhrase("Cynk [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Zinc_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Zinc_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Zinc_Consumed - d.Zinc_Goal));

            AddFirstColumnCellPhrase("Fosfor [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Phosphorus_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Phosphorus_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Phosphorus_Consumed - d.Phosphorus_Goal));

            AddFirstColumnCellPhrase("Jod [µg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Iodine_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Iodine_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Iodine_Consumed - d.Iodine_Goal));

            AddFirstColumnCellPhrase("Magnez [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Magnesium_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Magnesium_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Magnesium_Consumed - d.Magnesium_Goal));

            AddFirstColumnCellPhrase("Miedź [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Copper_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Copper_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Copper_Consumed - d.Copper_Goal));

            AddFirstColumnCellPhrase("Potas [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Potassium_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Potassium_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Potassium_Consumed - d.Potassium_Goal));

            AddFirstColumnCellPhrase("Selen [µg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Selenium_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Selenium_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Selenium_Consumed - d.Selenium_Goal));

            AddFirstColumnCellPhrase("Sód [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Sodium_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Sodium_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Sodium_Consumed - d.Sodium_Goal));

            AddFirstColumnCellPhrase("Wapń [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Calcium_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Calcium_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Calcium_Consumed - d.Calcium_Goal));

            AddFirstColumnCellPhrase("Żelazo [mg]: ");
            AddBasicCellPhrase(RoundDoubleMicro(d.Iron_Consumed));
            AddBasicCellPhrase(RoundDoubleMicro(d.Iron_Goal));
            AddBasicCellPhrase(RoundDoubleMicro(d.Iron_Consumed - d.Iron_Goal));
        }

        private string RoundDouble(double? number)
        {
            return ((double)Math.Round((decimal)number, 1, MidpointRounding.AwayFromZero)).ToString();
        }

        private string RoundDoubleMicro(double? number)
        {
            return ((double)Math.Round((decimal)number, 2, MidpointRounding.AwayFromZero)).ToString();
        }


        private void CreateBasicCell()
        {
            _fontStyle = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 12f);
            _pdfCell = new PdfPCell
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.White
            };
        }

        private void CreateFirstColumnCell()
        {
            _fontStyle = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 12f, 1, BaseColor.White);
            _pdfCell = new PdfPCell
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.DarkGray,
                Colspan = 1,
                ExtraParagraphSpace = 4
            };
        }

        private void CreateTableHeader()
        {
            CreateFirstColumnCell();
            AddCellPhrase("Składnik");
            AddCellPhrase("Spożyto");
            AddCellPhrase("Cel");
            AddCellPhrase("Różnica");
            _pdfTable.CompleteRow();
        }

        private void AddFirstColumnCellPhrase(string s)
        {
            CreateFirstColumnCell();
            AddCellPhrase(s);
        }

        private void AddBasicCellPhrase(string s)
        {
            CreateBasicCell();
            AddCellPhrase(s);
        }

        private void AddCellPhrase(string s)
        {
            _pdfCell.Phrase = new Phrase(s, _fontStyle);
            _pdfTable.AddCell(_pdfCell);
        }


    }
}
