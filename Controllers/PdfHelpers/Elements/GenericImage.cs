using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System.Web;

namespace HTML2PDF.Controllers.PdfHelpers.Elements
{

    public class GenericImage : IPdfElement
    {

        /// <summary>
        /// Tworzy obrazek z pliku z serwera, jeżeli nie jest podany to wybiera "~/App_Data/uploads/logo.png".
        /// </summary>

        private XImage image;
        private XStringFormat _format;
        private string _url;
        public XPoint _coords;
        public double X, Y;
        public bool IsFixed;
        public double height, width;
        public double lastx;

        bool IPdfElement.IsFixed { get { return IsFixed; } set { IsFixed = value; } }
        double IPdfElement.X { get { return X; } set { X = value; } }
        double IPdfElement.Y { get { return Y; } set { Y = value; } }

        double IPdfElement.Height { get { return height; } set { height = value; } }
        double IPdfElement.Width { get { return width; } set { width = value; } }

        double IPdfElement.Lastx { get { return lastx; } set { lastx = value; } }

        // Kod dla PageHelper'a
        string IPdfElement.Text { get { return _url; } }
        XFont IPdfElement.FontStyle { get { return null; } }
        XParagraphAlignment IPdfElement.Align { get { return XParagraphAlignment.Left; } }
        XStringFormat IPdfElement.Format { get { return _format; } }

        public GenericImage(string url = null, bool IsFixed = false, double x = 0, double y = 0, double height = 0, double width = 0, XStringFormat format = null)
        {

            if (url == null)
            {

                _url = "~/App_Data/uploads/logo.png";

            }
            else
            {

                _url = url;

            }

            try
            {

                image = XImage.FromFile(HttpContext.Current.Server.MapPath(_url));

            }
            catch
            {

                throw new System.ApplicationException("Nie udało się załadować obrazka. Czy adres jest prawidłowy? Jeżeli go nie podano to czy w folderze ~/App_Data/uploads znajduje się logo.png?");

            }

            if (height > 0 && width > 0)
            {

                this.height = height;
                this.width = width;

            }
            else
            {

                this.height = image.PixelHeight;
                this.width = image.PixelWidth;

            }

            if (IsFixed == true) { this.IsFixed = true; }
            _coords = new XPoint(x, y);

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
        /// Kalkuluje wielkość obiektu dla renderu
        /// </summary
        public void CalculateSize(XGraphics gfx, double lastx, XPoint _margins)
        {

            if (height <= 0 && width <= 0)
            {

                height = image.PixelHeight;
                width = image.PixelWidth;

            }

            if(this.height > gfx.PageSize.Height - _margins.Y || this.width > gfx.PageSize.Width - _margins.X)
            {

                throw new System.ApplicationException("Plik z obrazkiem jest większy niż cała strona! Określ rozmiary w parametrach lub podaj mniejszy obraz.");

            }

        }

        /// <summary>
        /// Renderuje obiekt
        /// </summary>
        public void Render(PdfPage page, XGraphics gfx, XPoint? FlexCoords = null)
        {

            if (FlexCoords != null && !IsFixed) { _coords = (XPoint)FlexCoords; }

            XRect rect;

            if (height > 0 && width > 0)
            {

                rect = new XRect(_coords, new XSize(width, height));

            }
            else
            {

                throw new System.ApplicationException("Obraz musi mieć jakiś rozmiar, a ten jest zerowy lub ujemny.");

            }

            X = rect.X;
            Y = rect.Y;

            gfx.DrawImage(image, _coords.X, _coords.Y, width, height);

        }

    }

}