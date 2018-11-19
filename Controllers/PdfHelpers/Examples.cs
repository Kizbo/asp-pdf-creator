using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTML2PDF.Controllers.PdfHelpers
{
    public class Examples
    {
        public void urlop()
        {
            PageHelper ph = new PageHelper();

           /*
            * 
             ph.Add(new PdfHelpers.Elements.Paragraph("Data wystawienia wniosku: 0000-00-00", XParagraphAlignment.Right));
            ph.Add(new PdfHelpers.Elements.Paragraph(25));
            ph.Add(new PdfHelpers.Elements.Paragraph("Imię i Nazwisko Pracownika: Jan Kowalski"));
            ph.Add(new PdfHelpers.Elements.Paragraph("Stanowisko: Operator Mopa"));
            ph.Add(new PdfHelpers.Elements.Paragraph(50));
            ph.Add(new PdfHelpers.Elements.Paragraph("WNIOSEK URLOPOWY", "Verdana", 12, XFontStyle.Bold, XParagraphAlignment.Center, XStringFormats.Default));
            ph.Add(new PdfHelpers.Elements.Paragraph("Proszę o udzielenie {typ urlopu} w okresie od dnia {data-start} do dnia {data-end} włącznie tj. {ilosc-dni} dni roboczych za rok {rok}"));
            ph.Add(new PdfHelpers.Elements.Paragraph(25));
            ph.Add(new PdfHelpers.Elements.Paragraph("Zastępstwo w czasie mojej nieobecności będzie pełnić: {zastępca}"));
            ph.Add(new PdfHelpers.Elements.Paragraph(25));
            ph.Add(new PdfHelpers.Elements.Paragraph("Wniosek został zaakceptowany w systemie przez: {akceptator}", "Verdana", 8, XFontStyle.Regular));
            ph.Add(new PdfHelpers.Elements.Paragraph(75));
            ph.Add(new PdfHelpers.Elements.Paragraph(".........................................", 270));
            ph.Add(new PdfHelpers.Elements.Paragraph(".........................................", false, 0, 0, 0, 270, null, 12, XFontStyle.Regular, XParagraphAlignment.Right));
            ph.Add(new PdfHelpers.Elements.Paragraph("Podpis wnioskodawcy", 270, "Verdana", 10, XFontStyle.Regular, XParagraphAlignment.Default));
            ph.Add(new PdfHelpers.Elements.Paragraph("Podpis zatwierdzającego", 270, "Verdana", 10, XFontStyle.Regular, XParagraphAlignment.Right));
            ph.Add(new PdfHelpers.Elements.Paragraph(100));
            ph.Add(new PdfHelpers.Elements.Paragraph("Wniosek wygenerowany z systemu NewHR24", XParagraphAlignment.Right));

            */

        }


        public void faktura()
        {

            /*
            // Kod do generowania pliku PDF

            PageHelper ph = new PageHelper(20, 20, "Faktura PRO-FORMA");

            ph.Add(new GenericImage()); // Logo
            ph.Add(new PdfHelpers.Elements.Paragraph("Bydgoszcz, 26.04.2018 r.", false, 0, 0, 0, 394, null, 11, XFontStyle.Regular, XParagraphAlignment.Right));

            ph.Add(new PdfHelpers.Elements.Paragraph(" ", false, 0, 0, 15, 0)); // Dla odstępów

            // Nagłówek
            ph.Add(new PdfHelpers.Elements.Paragraph("FAKTURA PRO FORMA NR T/FPF/04/2018/1", false, 0, 0, 0, 0, null, 20, XFontStyle.Regular, XParagraphAlignment.Center));
            ph.Add(new PdfHelpers.Elements.Paragraph("ORYGINAŁ", false, 0, 0, 0, 0, null, 12, XFontStyle.Regular, XParagraphAlignment.Center));
            // /Nagłówek

            ph.Add(new PdfHelpers.Elements.Paragraph(" ", false, 0, 0, 50, 0)); // Dla odstępów

            // Nabywca i kupujący
            ph.Add(new PdfHelpers.Elements.Paragraph("Sprzedawca:", false, 0, 0, 0, 276, null, 10, XFontStyle.Underline));
            ph.Add(new PdfHelpers.Elements.Paragraph("Kupujący:", false, 0, 0, 0, 276, null, 10, XFontStyle.Underline));
            ph.Add(new PdfHelpers.Elements.Paragraph("KDR Solutions Sp. z o.o.", false, 0, 0, 0, 276, null, 10));
            ph.Add(new PdfHelpers.Elements.Paragraph("Contosos S.A.", false, 0, 0, 0, 276, null, 10));
            ph.Add(new PdfHelpers.Elements.Paragraph("ul. Kozietulskiego 4a", false, 0, 0, 0, 276, null, 10));
            ph.Add(new PdfHelpers.Elements.Paragraph("ul. Konstruktorska 4", false, 0, 0, 0, 276, null, 10));
            ph.Add(new PdfHelpers.Elements.Paragraph("85-657 Bydgoszcz", false, 0, 0, 0, 276, null, 10));
            ph.Add(new PdfHelpers.Elements.Paragraph("02-673 Warszawa", false, 0, 0, 0, 276, null, 10));
            // /Nabywca i kupujący

            ph.Add(new PdfHelpers.Elements.Paragraph(" ", false, 0, 0, 20, 0)); // Dla odstępów

            // Kod tabeli
            // Obramowanie
            TableBorderModel tbm = new TableBorderModel
            {
                StyleName = "Test",
                BorderColor = MigraDoc.DocumentObjectModel.Colors.Black,
                BorderWidth = 0.2
            };

            // Definicja kolumn
            TableColumnsModel tcm = new TableColumnsModel();
            var columns = new ListWithDuplicates
            {
                { "50", "center" },
                { "200", "center" },
                { "65", "center" },
                { "55", "center" },
                { "100", "center" },
                { "65", "center" }
            };
            tcm.addColumns(columns);
            // /Definicja kolumn

            // Lista produktów
            var table = new GenericTable(tcm, tbm);
            // Nagłówki
            table.addHeader(
                new string[] { "Lp.", "Nazwa towaru/usługi", "Netto", "VAT", "Wartość VAT", "Brutto" },
                new string[] { "center", "center", "center", "center", "center", "center" }
            , Colors.LightSkyBlue);
            // /Nagłówki
            // Produkty
            table.addContent(
                new string[,] {
                    { "1", "Zasilenie stanu konta w serwisie", "30.00 PLN", "23%", "6.90 PLN", "36.90 PLN" }
                },
                new string[,]
                {
                    { "Verdana", "Verdana", "Verdana", "Verdana", "Verdana", "Verdana",}
                },
                new Color[] { Colors.AliceBlue }
                );
            // /Produkty
            ph.Add(table);
            // /Lista produktów
            // /Tabela

            // Tabela
            // Kolumny
            tcm = new TableColumnsModel();
            columns = new ListWithDuplicates
            {
                { "90", "center" },
                { "65", "center" },
                { "65", "center" },
                { "70", "center" }
            };
            tcm.addColumns(columns);
            // /Kolumny

            table = new GenericTable(tcm, tbm, 0, 0, 290, "right");

            // Nagłówki
            table.addHeader(
                new string[] { "Według stawki VAT", "Netto", "Wartość", "Podsumowanie" },
                new string[] { "center", "center", "center", "center" }
            , Colors.LightSkyBlue);
            // /Nagłówki
            // Produkty
            table.addContent(
                new string[,] {
                    { "23%", "30.00 PLN", "6.90 PLN", "36.90 PLN" },
                    { "Do zapłaty", "30.00 PLN", "6.90 PLN", "36.90 PLN" }
                },
                new string[,]
                {
                    { "Verdana", "Verdana", "Verdana", "Verdana"},
                    { "Verdana", "Verdana", "Verdana", "Verdana"}
                },
                new Color[] { Colors.AliceBlue, Colors.AliceBlue }
                );
            // /Produkty
            ph.Add(table);
            // /Tabela

            ph.Add(new PdfHelpers.Elements.Paragraph(" ", false, 0, 0, 35, 0)); // Dla odstępów

            ph.Add(new PdfHelpers.Elements.Paragraph("Sposób płatności: Przelew", false, 0, 0, 0, 276, null, 10));
            ph.Add(new PdfHelpers.Elements.Paragraph("DO ZAPŁATY: 36,90 PLN", false, 0, 0, 0, 260, null, 11, XFontStyle.Bold, XParagraphAlignment.Right));
            ph.Add(new PdfHelpers.Elements.Paragraph("Termin płatności: 26-04-2018", false, 0, 0, 0, 276, null, 10));
            ph.Add(new PdfHelpers.Elements.Paragraph("Zapłacono: 0,00 PLN", false, 0, 0, 0, 259, null, 10, XFontStyle.Regular, XParagraphAlignment.Right));

            ph.Add(new PdfHelpers.Elements.Paragraph(" ", false, 0, 0, 35, 0)); // Dla odstępów

            ph.Add(new PdfHelpers.Elements.Paragraph("————————————————————————————————————————————————————", false, 0, 0, 0, 0, null, 11));
            ph.Add(new PdfHelpers.Elements.Paragraph("Dane do przelewu", false, 0, 0, 0, 0, null, 11, XFontStyle.Italic));
            ph.Add(new PdfHelpers.Elements.Paragraph("Numer konta: 57 1020 0003 8345 3823 6983 9192", false, 0, 0, 0, 0, null, 10));
            ph.Add(new PdfHelpers.Elements.Paragraph("PKO BP S.A.", false, 0, 0, 0, 0, null, 10));
            ph.Add(new PdfHelpers.Elements.Paragraph("Tytułem: T/FPF/04/2018/1", false, 0, 0, 0, 0, null, 10));
            ph.Add(new PdfHelpers.Elements.Paragraph("————————————————————————————————————————————————————", false, 0, 0, 0, 0, null, 11));

            //ph.Add(new PdfHelpers.Elements.Paragraph(900)); // Dla odstępów
            */
        }
    }
}
 