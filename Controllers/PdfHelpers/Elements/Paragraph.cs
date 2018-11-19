using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;

namespace HTML2PDF.Controllers.PdfHelpers.Elements
{

    /// <summary>
    /// Tworzy paragraf - tekst z możliwością dostosowania czcionki, formatowania i automatycznego ustalania wielkości pola. 
    /// </summary>

    public class Paragraph : IPdfElement
    {

        private XFont _font;
        private XStringFormat _format;
        private XBrush _brush;
        private string _text;
        public XPoint _coords;
        public double X, Y;
        public bool IsFixed;
        public double height, width;
        public double lastx;
        public XParagraphAlignment _align;

        bool IPdfElement.IsFixed { get { return IsFixed; } set { IsFixed = value; } }
        double IPdfElement.X { get { return X; } set { X = value; } }
        double IPdfElement.Y { get { return Y; } set { Y = value; } }

        double IPdfElement.Height { get { return height; } set { height = value; } }
        double IPdfElement.Width { get { return width; } set { width = value; } }

        double IPdfElement.Lastx { get { return lastx; } set { lastx = value; } }
        
        // Kod dla PageHelper'a
        string IPdfElement.Text { get { return _text; } }
        XFont IPdfElement.FontStyle { get { return _font; } }
        XParagraphAlignment IPdfElement.Align { get { return _align; } }
        XStringFormat IPdfElement.Format { get { return _format;  } }

        //overloady konstruktora

        /// <summary>
        /// Standardowy konstruktor, zawiera wszystkie parametry które można przekazać
        /// </summary>
        /// <param name="text">Tekst</param>
        /// <param name="IsFixed">Czy element ma być absolutnie ustawiony?</param>
        /// <param name="x">Absolutny X</param>
        /// <param name="y">Absolutny Y</param>
        /// <param name="height">Opcjonalna wysokość absolutna</param>
        /// <param name="width">Opcjonalna szerokość absolutna</param>
        /// <param name="font">Obiekt Font dla elementu, domyślnie: Calibri</param>
        /// <param name="size">Wielkość czcionki, domyślnie: 12</param>
        /// <param name="style">Obiekt XFontStyle stylu czcionki</param>
        /// <param name="align">Obiekt XParagraphAlignment dopasowania tekstu do ściany kontenera</param>
        /// <param name="format">Obiekt XStringFormat dopasowania tekstu do ściany dokumentu</param>
        public Paragraph(string text, bool IsFixed = false, double x = 0, double y = 0, double height = 0, double width = 0, string font = null, double size = 12, XFontStyle style = XFontStyle.Regular, XParagraphAlignment align = XParagraphAlignment.Left, XStringFormat format = null)
        {
            ParagraphConstructor(text, IsFixed, x, y, height, width, font, size, style, align, format);

        }

        /* OVERLOADY */
        /// <summary>
        /// Tworzy pusty paragraph, może posłużyć jako blok pustego miejsca do rozdzielenia obiektów
        /// </summary>
        /// <param name="emptySpace">Ilość pustego miejsca w pt na osi OY</param>
        public Paragraph(int emptySpace)
        {
            ParagraphConstructor("  ", false, 0, 0, emptySpace, 0);
        }

        /// <summary>
        /// Prosty konstruktor, podajemy tekst, resztą się zajmuje program
        /// </summary>
        /// <param name="text">Tekst paragrafu</param>
        public Paragraph(string text)
        {
            ParagraphConstructor(text);
        }

        /// <summary>
        /// Konstruktor z obsługą czcionki
        /// </summary>
        /// <param name="text">Tekst</param>
        /// <param name="font">Nazwa czcionki</param>
        /// <param name="fontSize">wielkość czcionki w pointach</param>
        /// <param name="style">Styl czcionki</param>
        public Paragraph(string text, string font, double fontSize, XFontStyle style)
        {
            ParagraphConstructor(text, false, 0, 0, 0, 0, font, fontSize, style);
        }

        /// <summary>
        /// Konstruktor z obsługą statycznego ustawienia
        /// </summary>
        /// <param name="text">Tekst</param>
        /// <param name="x">oś OX</param>
        /// <param name="y">oś OY</param>
        public Paragraph(string text, double x, double y)
        {
            ParagraphConstructor(text, true, x, y);
        }

        /// <summary>
        /// Konstruktor z obsługą formatowania
        /// </summary>
        /// <param name="text">Tekst</param>
        /// <param name="align">alignment tekstu względem kontenera</param>
        public Paragraph(string text, XParagraphAlignment align)
        {
            ParagraphConstructor(text, false, 0, 0, 0, 0, null, 12, XFontStyle.Regular, align, null);

        }

        /// <summary>
        /// Z dlugoscia
        /// </summary>
        /// <param name="text">tekst</param>
        /// <param name="width">dlugosc paragrafu</param>
        public Paragraph(string text, double width)
        {
            ParagraphConstructor(text, false, 0, 0, 0, width);
        }

        /// <summary>
        /// Konstruktor z obsługą wielkości paragrafu w linii oraz czcionki i alignmentu
        /// </summary>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="font"></param>
        /// <param name="fontSize"></param>
        /// <param name="style"></param>
        /// <param name="align"></param>
        public Paragraph(string text, double width, string font, double fontSize, XFontStyle style, XParagraphAlignment align)
        {
            ParagraphConstructor(text, false, 0, 0, 0, width, font, fontSize, style, align, null);
        }


        /// <summary>
        /// Konstruktor z obsługą czcionki i formatowania
        /// </summary>
        /// <param name="text">Tekst</param>
        /// <param name="font">Nazwa czcionki</param>
        /// <param name="fontSize">wielkość czcionki w pointach</param>
        /// <param name="style">Obiekt XFontStyle stylu czcionki</param>
        /// <param name="align">Obiekt XParagraphAlignment dopasowania tekstu do ściany kontenera</param>
        /// <param name="format">Obiekt XStringFormat dopasowania tekstu do ściany dokumentu</param>
        public Paragraph(string text, string font, double fontSize, XFontStyle style, XParagraphAlignment align, XStringFormat format)
        {
            ParagraphConstructor(text, false, 0, 0, 0, 0, font, fontSize, style, align, format);

        }
        /* END OF OVERLOADS */

        //real konstruktor
        private void ParagraphConstructor(string text, bool IsFixed = false, double x = 0, double y = 0, double height = 0, double width = 0, string font = null, double size = 12, XFontStyle style = XFontStyle.Regular, XParagraphAlignment align = XParagraphAlignment.Left, XStringFormat format = null)
        {
            _text = text;

            if (height > 0) { this.height = height; }
            if (width > 0) { this.width = width; }

            if (IsFixed == true) { this.IsFixed = true; }
            _coords = new XPoint(x, y);

            /*if (font == null || size == 0)
            {

                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
                if (font == null) { _font = new XFont("Calibri", size, style, options); }
                else if (size == 0) { _font = new XFont(font, 12, style, options); }
                else { _font = new XFont("Calibri", 12, style, options); }

            }
            else
            {*/

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            _font = new XFont(font, size, style, options);

            //}

            _align = align;

            if (format == null)
            {

                _format = new XStringFormat
                {
                    Alignment = XStringAlignment.Near
                };

            }
            else
            {

                _format = format;

            }
        }

        
        /// <summary>
        /// Filtrowanie stringa pod względem znaku spacji, NIE MOŻE GO BYĆ!
        /// </summary>
        /// <param name="Lines">tekst paragraphu</param>
        /// <returns></returns>
        public string FilterString(string Lines) => Lines.Replace("\n", " ");

        /// <summary>
        /// Kalkuje wielkość obiektu potrzebną do pagehelpera
        /// </summary>
        /// <param name="gfx">Kontekst graficzny page'a</param>
        /// <param name="lastx">X ostatniego wyrenderowanego obiektu</param>
        /// <param name="_margins">Marginesy strony</param>
        public void CalculateSize(XGraphics gfx, double lastx, XPoint _margins)
        {

            if (height <= 0 || width <= 0)
            {

                if (width <= 0){

                    width = (gfx.PageSize.Width) - (_margins.X * 2);

                }

                if (height <= 0)
                {

                    double wordWidth = 0, lineWidth = 0;
                    string lineWord = "", tempWord = "", wholeWord = "";

                    for (int i = 0; i <= _text.Length; i++)
                    {

                        if (i < _text.Length)
                        {

                            while (true)
                            {

                                if (_text[i] == ' ') { tempWord += _text[i]; break; }
                                else if (i >= (_text.Length - 1)) { if (i == _text.Length) { tempWord += _text[i]; } break; }
                                else { tempWord += _text[i]; i++; }

                            }

                        }

                        wordWidth = gfx.MeasureString(" " + tempWord, _font).Width; // XGraphics nie liczy szerokości spacji po słowie, ale przed nim już tak.
                        lineWidth = gfx.MeasureString(lineWord, _font).Width;

                        if (lineWidth + wordWidth >= width)
                        {

                                height += _font.GetHeight(gfx);
                                wholeWord += lineWord;
                                lineWord = tempWord;
                                tempWord = "";

                        }
                        else
                        {

                            lineWord += tempWord;
                            tempWord = "";

                        }

                    }

                    height += _font.GetHeight(gfx); // Bez tego ostatnia linia nie jest dodawana przez pętlę, bo tekst w niej się mieści.

                }

            }

        }

        /// <summary>
        /// Funkcja interfejsu renderująca obiekt
        /// </summary>
        /// <param name="page">obiekt aktualnej strony</param>
        /// <param name="gfx">kontekst graficzny</param>
        /// <param name="FlexCoords"></param>
        public void Render(PdfPage page, XGraphics gfx, XPoint? FlexCoords = null)
        {

            if (FlexCoords != null && !IsFixed) { _coords = (XPoint)FlexCoords; }

            XRect rect = new XRect(_coords, new XSize(width, height));

            X = rect.X;
            Y = rect.Y;
            XTextFormatter tf = new XTextFormatter(gfx)
            {
                Alignment = _align
            };

            tf.DrawString(_text, _font, (_brush ?? XBrushes.Black), rect, XStringFormats.TopLeft);
            
        }

    }

}