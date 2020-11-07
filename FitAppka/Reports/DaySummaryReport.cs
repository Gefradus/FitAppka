using FitAppka.Models.DTO.DaySummaryDTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FitAppka.Reports
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
            _fontStyle = FontFactory.GetFont(BaseFont.COURIER, 22f, 1);
            _pdfCell = new PdfPCell(new Phrase("RAPORT PROWADZONEJ DIETY"))
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
            CreateTableHeader();
            CreateTableBody();
        }

       
        private void CreateTableBody()
        {
            var s = _daySummary.DaySummaryDTO;
            var d = _daySummary.DetailsDTO;

            AddFirstColumnCellPhrase("Kalorie [kcal]: ");
            AddBasicCellPhrase(s.KcalConsumed.ToString());
            AddBasicCellPhrase(s.KcalGoal.ToString());
            AddBasicCellPhrase((s.KcalConsumed - s.KcalGoal).ToString());
            _pdfTable.CompleteRow();

            AddFirstColumnCellPhrase("Białko [g]: ");
            AddBasicCellPhrase(s.ProteinsConsumed.ToString());
            AddBasicCellPhrase(s.ProteinsGoal.ToString());
            AddBasicCellPhrase((s.ProteinsConsumed - s.ProteinsGoal).ToString());

            AddFirstColumnCellPhrase("Tłuszcze [g]: ");
            AddBasicCellPhrase(s.FatsConsumed.ToString());
            AddBasicCellPhrase(s.FatsGoal.ToString());
            AddBasicCellPhrase((s.FatsConsumed - s.FatsGoal).ToString());

            AddFirstColumnCellPhrase("Węgl. [g]: ");
            AddBasicCellPhrase(s.CarbohydratesConsumed.ToString());
            AddBasicCellPhrase(s.CarbohydratesGoal.ToString());
            AddBasicCellPhrase((s.CarbohydratesConsumed - s.CarbohydratesGoal).ToString());

            AddFirstColumnCellPhrase("Woda [ml]: ");
            AddBasicCellPhrase(s.WaterDrunk.ToString());
            AddBasicCellPhrase(s.KcalGoal.ToString());
            AddBasicCellPhrase((s.WaterDrunk - s.KcalGoal).ToString());

            AddFirstColumnCellPhrase("Wit. A [µg]: ");
            AddBasicCellPhrase(d.VitaminA_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminA_Goal.ToString());
            AddBasicCellPhrase((d.VitaminA_Consumed - d.VitaminA_Goal).ToString());

            AddFirstColumnCellPhrase("Wit. C [mg]: ");
            AddBasicCellPhrase(d.VitaminC_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminC_Goal.ToString());
            AddBasicCellPhrase((d.VitaminC_Consumed - d.VitaminC_Goal).ToString());

            AddFirstColumnCellPhrase("Wit. D [µg]: ");
            AddBasicCellPhrase(d.VitaminD_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminD_Goal.ToString());
            AddBasicCellPhrase((d.VitaminD_Consumed - d.VitaminD_Goal).ToString());

            AddFirstColumnCellPhrase("Wit. K [µg]: ");
            AddBasicCellPhrase(d.VitaminK_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminK_Goal.ToString());
            AddBasicCellPhrase((d.VitaminK_Consumed - d.VitaminK_Goal).ToString());

            AddFirstColumnCellPhrase("Wit. E [mg]: ");
            AddBasicCellPhrase(d.VitaminE_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminE_Goal.ToString());
            AddBasicCellPhrase((d.VitaminE_Consumed - d.VitaminE_Goal).ToString());

            AddFirstColumnCellPhrase("Wit. B1 [mg]: ");
            AddBasicCellPhrase(d.VitaminB1_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminB1_Goal.ToString());
            AddBasicCellPhrase((d.VitaminB1_Consumed - d.VitaminB1_Goal).ToString());

            AddFirstColumnCellPhrase("Wit. B2 [mg]: ");
            AddBasicCellPhrase(d.VitaminB2_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminB2_Goal.ToString());
            AddBasicCellPhrase((d.VitaminB2_Consumed - d.VitaminB2_Goal).ToString());

            AddFirstColumnCellPhrase("Wit. B5 [mg]: ");
            AddBasicCellPhrase(d.VitaminB5_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminB5_Goal.ToString());
            AddBasicCellPhrase((d.VitaminB5_Consumed - d.VitaminB5_Goal).ToString());

            AddFirstColumnCellPhrase("Wit. B6 [mg]: ");
            AddBasicCellPhrase(d.VitaminB6_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminB6_Goal.ToString());
            AddBasicCellPhrase((d.VitaminB6_Consumed - d.VitaminB6_Goal).ToString());

            AddFirstColumnCellPhrase("Kwas foliowy [µg]: ");
            AddBasicCellPhrase(d.FolicAcid_Consumed.ToString());
            AddBasicCellPhrase(d.FolicAcid_Goal.ToString());
            AddBasicCellPhrase((d.FolicAcid_Consumed - d.FolicAcid_Goal).ToString());

            AddFirstColumnCellPhrase("Wit. B12 [µg]: ");
            AddBasicCellPhrase(d.VitaminB12_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminB12_Goal.ToString());
            AddBasicCellPhrase((d.VitaminB12_Consumed - d.VitaminB12_Goal).ToString());

            AddFirstColumnCellPhrase("Wit. PP [mg]: ");
            AddBasicCellPhrase(d.VitaminPp_Consumed.ToString());
            AddBasicCellPhrase(d.VitaminPp_Goal.ToString());
            AddBasicCellPhrase((d.VitaminPp_Consumed - d.VitaminPp_Goal).ToString());

            AddFirstColumnCellPhrase("Biotyna [µg]: ");
            AddBasicCellPhrase(d.Biotin_Consumed.ToString());
            AddBasicCellPhrase(d.Biotin_Goal.ToString());
            AddBasicCellPhrase((d.Biotin_Consumed - d.Biotin_Goal).ToString());

            AddFirstColumnCellPhrase("Cynk [mg]: ");
            AddBasicCellPhrase(d.Zinc_Consumed.ToString());
            AddBasicCellPhrase(d.Zinc_Goal.ToString());
            AddBasicCellPhrase((d.Zinc_Consumed - d.Zinc_Goal).ToString());

            AddFirstColumnCellPhrase("Fosfor [mg]: ");
            AddBasicCellPhrase(d.Phosphorus_Consumed.ToString());
            AddBasicCellPhrase(d.Phosphorus_Goal.ToString());
            AddBasicCellPhrase((d.Phosphorus_Consumed - d.Phosphorus_Goal).ToString());

            AddFirstColumnCellPhrase("Jod [µg]: ");
            AddBasicCellPhrase(d.Iodine_Consumed.ToString());
            AddBasicCellPhrase(d.Iodine_Goal.ToString());
            AddBasicCellPhrase((d.Iodine_Consumed - d.Iodine_Goal).ToString());

            AddFirstColumnCellPhrase("Magnez [mg]: ");
            AddBasicCellPhrase(d.Magnesium_Consumed.ToString());
            AddBasicCellPhrase(d.Magnesium_Goal.ToString());
            AddBasicCellPhrase((d.Magnesium_Consumed - d.Magnesium_Goal).ToString());

            AddFirstColumnCellPhrase("Miedź [mg]: ");
            AddBasicCellPhrase(d.Copper_Consumed.ToString());
            AddBasicCellPhrase(d.Copper_Goal.ToString());
            AddBasicCellPhrase((d.Copper_Consumed - d.Copper_Goal).ToString());

            AddFirstColumnCellPhrase("Potas [mg]: ");
            AddBasicCellPhrase(d.Potassium_Consumed.ToString());
            AddBasicCellPhrase(d.Potassium_Goal.ToString());
            AddBasicCellPhrase((d.Potassium_Consumed - d.Potassium_Goal).ToString());

            AddFirstColumnCellPhrase("Selen [µg]: ");
            AddBasicCellPhrase(d.Selenium_Consumed.ToString());
            AddBasicCellPhrase(d.Selenium_Goal.ToString());
            AddBasicCellPhrase((d.Selenium_Consumed - d.Selenium_Goal).ToString());

            AddFirstColumnCellPhrase("Sód [mg]: ");
            AddBasicCellPhrase(d.Sodium_Consumed.ToString());
            AddBasicCellPhrase(d.Sodium_Goal.ToString());
            AddBasicCellPhrase((d.Sodium_Consumed - d.Sodium_Goal).ToString());

            AddFirstColumnCellPhrase("Wapń [mg]: ");
            AddBasicCellPhrase(d.Calcium_Consumed.ToString());
            AddBasicCellPhrase(d.Calcium_Goal.ToString());
            AddBasicCellPhrase((d.Calcium_Consumed - d.Calcium_Goal).ToString());

            AddFirstColumnCellPhrase("Żelazo [mg]: ");
            AddBasicCellPhrase(d.Iron_Consumed.ToString());
            AddBasicCellPhrase(d.Iron_Goal.ToString());
            AddBasicCellPhrase((d.Iron_Consumed - d.Iron_Goal).ToString());
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
