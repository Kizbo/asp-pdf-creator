using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System.Collections.Generic;

namespace HTML2PDF.Controllers.PdfHelpers.Elements
{

    /// <summary>
    /// Lista z duplikatami, aka c# version of php associate array +/-
    /// </summary>
    public class ListWithDuplicates : List<KeyValuePair<string, string>>
    {
        public void Add(string key, string value)
        {
            var element = new KeyValuePair<string, string>(key, value);
            this.Add(element);
        }
    }
    /// <summary>
    /// Model określający styl tabeli
    /// </summary>
    public class TableBorderModel
    {
        /// <summary>
        /// Nazwa stylu
        /// </summary>
        public string StyleName { get; set; }
        /// <summary>
        /// Szerokość Obramówki w pt
        /// </summary>
        public double BorderWidth { get; set; }
        /// <summary>
        /// Kolor obramówki obiektu Color/XColor
        /// </summary>
        public Color BorderColor { get; set; }
    }
    /// <summary>
    /// Model określający kolumny tabeli
    /// </summary>
    public class TableColumnsModel
    {
        /// <summary>
        /// Słownik zawierający definicje kolumn tabel, gdzie sizing kolumny => align kolumny (lewy, prawy, srodkowy)
        /// Pierwszy atrybut to string przyjmujący wartości w z suffixem cm, px, pt np. 12cm
        /// Jeśli suffix nie jest dodany, to domyślnie zakłada się, że to jednostka point
        /// 
        /// Drugi atrybut przyjmuje wartości: left, right, center oraz justify. Domyślnie jest left
        /// </summary>
        public ListWithDuplicates _columns = new ListWithDuplicates();

        /// <summary>
        /// Dodanie nowych kolumn do definicji, gdzie sizing kolumny => align kolumny (lewy, prawy, srodkowy)
        /// </summary>
        /// <param name="columns">ListWithDuplicates(string, string) do dodania</param>
        public void addColumns(ListWithDuplicates columns)
        {
            foreach (KeyValuePair<string, string> entry in columns)
            {
                _columns.Add(entry.Key, entry.Value);
            }
        }
    }

   

    /// <summary>
    /// Generowanie prostej tabeli danych
    /// </summary>
    public class GenericTable : IPdfElement
    {
        //private XFont _font;
        public XPoint _coords;
        public double X, Y;
        public bool IsFixed;
        public double height, width;
        public double lastx;
        private string CustomWidthAlignment;
        //modele
        TableBorderModel tbm;
        TableColumnsModel tcm;
        private Document doc;
        private Table table;
        /* Bezużyteczne do tabel ale ktoś zmienił interfejs */
        string IPdfElement.Text { get { return null; } }
        XFont IPdfElement.FontStyle { get { return null; } }
        XParagraphAlignment IPdfElement.Align { get { return XParagraphAlignment.Left; } }
        XStringFormat IPdfElement.Format { get { return null; } }
        /* ------- */

        bool IPdfElement.IsFixed { get { return IsFixed; } set { IsFixed = value; } }
        double IPdfElement.X { get { return X; } set { X = value; } }
        double IPdfElement.Y { get { return Y; } set { Y = value; } }

        double IPdfElement.Height { get { return height; } set { height = value; } }
        double IPdfElement.Width { get { return width; } set { width = value; } }

        double IPdfElement.Lastx { get { return lastx; } set { lastx = value; } }

        public GenericTable(TableColumnsModel columns, TableBorderModel border, double Xcoords = 0, double Ycoords = 0, double CustomWidth = 0, string CustomWidthAlignment = null)
        {
            // jeśli podano koordynaty w zmiennych
            if(Xcoords > 0 && Ycoords > 0)
            {
                X = Xcoords; Y = Ycoords;
            }
            //jeśli podano CustomWidth
            if (CustomWidth > 0)
            {
                width = CustomWidth;    
            }

            //jesli podano alignment
            if(CustomWidthAlignment != null)
            {
                this.CustomWidthAlignment = CustomWidthAlignment;
            }
                

            tcm = columns; tbm = border;
            doc = new Document();
            Section sec = doc.AddSection();
            
            this.table = sec.AddTable();
            this.styleTable();
            this.addColumns();
        }

        /// <summary>
        /// Kalkuluje wielkość elementu dla renderu
        /// </summary>
        public void CalculateSize(XGraphics gfx, double lastx, XPoint margins)
        {
            if(width <= 0)
            width = gfx.PageSize.Width - margins.X * 2;


            if (CustomWidthAlignment != null)
            {
                //jeśli określono alignment
                switch (CustomWidthAlignment)
                {
                    case "right":
                        X = gfx.PageSize.Width - margins.X * 2 - width;
                        break;
                }
            }
            //height jest obliczany przy dodawaniu rowów

        }

        /// <summary>
        /// dodaje style z konstruktora do tabeli
        /// </summary>
        private void styleTable()
        {
            table.Style = tbm.StyleName;
            table.Borders.Color = tbm.BorderColor;
            table.Borders.Width = tbm.BorderWidth;
        }


        /// <summary>
        /// Dodaje kolumny do tabeli
        /// </summary>
        private void addColumns()
        {
            Column column;
            foreach(var item in tcm._columns)
            {
                column = table.AddColumn(item.Key);

                switch (item.Value)
                {
                    case "left":
                        column.Format.Alignment = ParagraphAlignment.Left;
                        break;
                    case "right":
                        column.Format.Alignment = ParagraphAlignment.Right;
                        break;
                    case "center":
                        column.Format.Alignment = ParagraphAlignment.Center;
                        break;
                    case "justify":
                        column.Format.Alignment = ParagraphAlignment.Justify;
                        break;
                    default:
                        column.Format.Alignment = ParagraphAlignment.Left;
                        break;
                }

            }
        }
        /// <summary>
        /// Dodaj sekcje nagłówka do tabeli, nagłówków może być nieskończoność
        /// NAGŁÓWKI MOŻNA MERGOWAĆ - należy dopisać func
        /// </summary>
        /// <param name="headerTexts">Array tekstów nagłówkowych</param>
        /// <param name="alignment">Array alignmentów tekstów, domyślnie lewy. Musi odpowiadać indeksowi tekstu</param>
        /// <param name="background">Color Backgroundu dla rowa</param>
        public void addHeader(string[] headerTexts, string[] alignment, Color background)
        {
            Row row = table.AddRow();
            row.HeadingFormat = true;
            for (var i = 0; i < table.Columns.Count; i++)
            {
                row.Cells[i].AddParagraph(headerTexts[i]);
                if(alignment[i] != null)
                    switch (alignment[i])
                    {
                        case "left":
                            row.Cells[i].Format.Alignment = ParagraphAlignment.Left;
                            break;
                        case "right":
                            row.Cells[i].Format.Alignment = ParagraphAlignment.Right;
                            break;
                        case "center":
                            row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                            break;
                        case "justify":
                            row.Cells[i].Format.Alignment = ParagraphAlignment.Justify;
                            break;
                        default:
                            row.Cells[i].Format.Alignment = ParagraphAlignment.Left;
                            break;
                    }
            }
            height += row.Height;
            //Defaultowane stylowanie, można przerobić
            row.Shading.Color = background;
            row.Format.Font = new Font("Verdana", "8");
            row.TopPadding = 2;
            row.BottomPadding = 2;
            height += row.Format.Font.Size.Point + row.TopPadding + row.BottomPadding + 5;
        }
        /// <summary>
        /// Dodaj content do tabeli, poprzez tabelę dwuwymiarową ktora powinna mieć wielkość taką jaka jest ilość kolumn
        /// </summary>
        /// <param name="content">Tablica dwuwymiarowa z contentem</param>
        /// <param name="bgcolors">Kolory dla poszczegolnych rowów</param>
        public void addContent(string[,] content, string[,] fonts , Color[] bgcolors) 
        {
            double tempheight = 0;
            for(int i = 0; i < content.GetLength(0); i++)
            {
                Row row = table.AddRow();
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    row.Cells[j].AddParagraph(content[i, j]);
                    var fontsize = 8;
                    row.Cells[j].Format.Font = new Font(fonts[i, j], fontsize);
                    if(fontsize > tempheight) tempheight = fontsize;
                    //Defaultowane stylowanie, można przerobić
                    row.Cells[j].Format.LeftIndent = 1;
                    row.Cells[j].Format.RightIndent = 1;
                    
                }
                row.Shading.Color = bgcolors[i];
                //Defaultowane stylowanie, można przerobić
                row.TopPadding = 2;
                row.BottomPadding = 2;
                height += tempheight + row.TopPadding + row.BottomPadding + 5;
                
            }
          
        }

        /// <summary>
        /// Funkcja renderująca tabelę
        /// </summary>
        /// <param name="page">Obiekt strony</param>
        /// <param name="gfx">Kontekst graficzny strony</param>
        /// <param name="coords">Koordynaty ustalone przez wyższy poziom</param>
        public void Render(PdfPage page, XGraphics gfx, XPoint? coords)
        {
            //jeśli podano koordynaty w konstruktorze
            if (X > 0 && Y > 0)
                _coords = new XPoint(X, Y);
            else if (X > 0 && Y <= 0)
            {
                _coords = new XPoint(X, coords.Value.Y);
                Y = coords.Value.Y;
            }
            else
            {
                _coords = (XPoint)coords; //lub jeśli nie podano
                X = _coords.X; Y = _coords.Y;
            }

            // HACK²
            gfx.MUH = PdfFontEncoding.Unicode;
            gfx.MFEH = PdfFontEmbedding.Always;
            // HACK²

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);
            pdfRenderer.Document = doc;
            pdfRenderer.RenderDocument();

            MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(doc);
            docRenderer.PrepareDocument();
            docRenderer.RenderObject(gfx, _coords.X, _coords.Y, width, table);
        }
    }
}