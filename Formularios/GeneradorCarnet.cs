using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using QRCoder;
using RegistroBiometricoUMG.Modelos;

namespace RegistroBiometricoUMG.Formularios
{
    public static class GeneradorCarnet
    {
        public static void Generar(Persona persona)
        {
            // ── Font resolver ───────────────────────────────────────────────
            if (GlobalFontSettings.FontResolver == null)
                GlobalFontSettings.FontResolver = new UmgFontResolver();

            // ── Carpeta de salida ───────────────────────────────────────────
            string carpeta = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Carnets");
            Directory.CreateDirectory(carpeta);

            string rutaPDF = Path.Combine(carpeta,
                $"Carnet_{persona.NumeroCarnet.Replace("-", "_")}.pdf");

            // ── QR temporal ─────────────────────────────────────────────────
            string rutaQR = Path.Combine(carpeta, "qr_temp.png");
            string contenidoQR =
                $"CARNET:{persona.NumeroCarnet}|" +
                $"NOMBRE:{persona.Nombre} {persona.Apellido}|" +
                $"CORREO:{persona.Correo}|" +
                $"TIPO:{persona.TipoPersona}";
            GenerarQRComoArchivo(contenidoQR, rutaQR);

            // ── Documento PDF ───────────────────────────────────────────────
            var documento = new PdfDocument();
            documento.Info.Title = $"Carnet UMG - {persona.Nombre} {persona.Apellido}";

            var pagina = documento.AddPage();
            pagina.Width  = XUnit.FromMillimeter(85.6);
            pagina.Height = XUnit.FromMillimeter(54);

            var g = XGraphics.FromPdfPage(pagina);

            // ── Colores ─────────────────────────────────────────────────────
            var colorUMG  = XColor.FromArgb(0, 51, 102);
            var colorGris = XColor.FromArgb(240, 242, 245);
            var brushUMG  = new XSolidBrush(colorUMG);
            var brushGris = new XSolidBrush(colorGris);

            double ancho = pagina.Width.Point;
            double alto  = pagina.Height.Point;
            double mm(double v) => XUnit.FromMillimeter(v).Point;

            // ── Fondo azul ──────────────────────────────────────────────────
            g.DrawRectangle(brushUMG, 0, 0, ancho, alto);

            // Franja gris central
            g.DrawRectangle(brushGris, 0, mm(14), ancho, mm(28));

            // ── Logo UMG ────────────────────────────────────────────────────
            string rutaLogo = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Recursos", "logo_umg.png");

            if (File.Exists(rutaLogo))
            {
                try
                {
                    var logo = XImage.FromFile(rutaLogo);
                    // Logo pequeño en esquina superior izquierda
                    g.DrawImage(logo, mm(1), mm(1), mm(11), mm(11));
                }
                catch { /* si falla el logo, continúa sin él */ }
            }

            // ── Fuentes ─────────────────────────────────────────────────────
            var fTitulo  = new XFont("Verdana", 5.5, XFontStyleEx.Bold);
            var fSub     = new XFont("Verdana", 4.5, XFontStyleEx.Regular);
            var fNombre  = new XFont("Verdana", 6,   XFontStyleEx.Bold);
            var fCarnet  = new XFont("Verdana", 5.5, XFontStyleEx.Bold);
            var fDato    = new XFont("Verdana", 4.5, XFontStyleEx.Regular);
            var fPie     = new XFont("Verdana", 3.8, XFontStyleEx.Italic);

            // ── Encabezado ──────────────────────────────────────────────────
            // Título en una sola línea centrado
            g.DrawString("UNIVERSIDAD MARIANO GÁLVEZ DE GUATEMALA",
                fTitulo, XBrushes.White,
                new XRect(mm(13), mm(2), ancho - mm(14), mm(6)),
                XStringFormats.TopLeft);

            // Subtítulo: año + jornada
            g.DrawString($"{DateTime.Now.Year}  —  Jornada Domingos",
                fSub, XBrushes.White,
                new XRect(mm(13), mm(7.5), ancho - mm(14), mm(5)),
                XStringFormats.TopLeft);

            // ── Foto ────────────────────────────────────────────────────────
            double fotoX = mm(2);
            double fotoY = mm(15);
            double fotoW = mm(18);
            double fotoH = mm(22);

            if (File.Exists(persona.RutaFoto))
            {
                try
                {
                    var img = XImage.FromFile(persona.RutaFoto);
                    g.DrawImage(img, fotoX, fotoY, fotoW, fotoH);
                }
                catch { DibujarPlaceholder(g, fotoX, fotoY, fotoW, fotoH); }
            }
            else
            {
                DibujarPlaceholder(g, fotoX, fotoY, fotoW, fotoH);
            }

            // ── Datos ───────────────────────────────────────────────────────
            double tx = mm(22);
            double ty = mm(15.5);
            double tw = mm(38);

            g.DrawString($"{persona.Nombre} {persona.Apellido}",
                fNombre, brushUMG,
                new XRect(tx, ty, tw, mm(6)), XStringFormats.TopLeft);

            ty += mm(5);
            g.DrawString($"Carnet: {persona.NumeroCarnet}",
                fCarnet, XBrushes.Black,
                new XRect(tx, ty, tw, mm(5)), XStringFormats.TopLeft);

            ty += mm(4.5);
            g.DrawString($"Tipo: {persona.TipoPersona}",
                fDato, XBrushes.Black,
                new XRect(tx, ty, tw, mm(5)), XStringFormats.TopLeft);

            ty += mm(4);
            g.DrawString($"Carrera: {persona.Carrera}",
                fDato, XBrushes.Black,
                new XRect(tx, ty, tw, mm(5)), XStringFormats.TopLeft);

            ty += mm(4);
            g.DrawString($"Sección: {persona.Seccion}",
                fDato, XBrushes.Black,
                new XRect(tx, ty, tw, mm(5)), XStringFormats.TopLeft);

            ty += mm(4);
            g.DrawString(persona.Correo,
                fDato, XBrushes.Black,
                new XRect(tx, ty, tw, mm(5)), XStringFormats.TopLeft);

            // ── QR ──────────────────────────────────────────────────────────
            if (File.Exists(rutaQR))
            {
                var qrImg = XImage.FromFile(rutaQR);
                g.DrawImage(qrImg, mm(63), mm(14), mm(19), mm(19));
            }

            // ── Pie ─────────────────────────────────────────────────────────
            g.DrawRectangle(brushUMG, 0, mm(43), ancho, mm(11));

            g.DrawString("Sede Boca del Monte  —  Ingeniería en Sistemas",
                fPie, XBrushes.White,
                new XRect(0, mm(44.5), ancho, mm(5)),
                XStringFormats.TopCenter);

            g.DrawString("Documento de identificación estudiantil.",
                fPie, XBrushes.White,
                new XRect(0, mm(48), ancho, mm(5)),
                XStringFormats.TopCenter);

            // ── Guardar y abrir ─────────────────────────────────────────────
            documento.Save(rutaPDF);
            documento.Close();

            if (File.Exists(rutaQR)) File.Delete(rutaQR);

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName        = rutaPDF,
                UseShellExecute = true
            });

            MessageBox.Show($"📄 Carnet generado:\n{rutaPDF}",
                "Carnet PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void GenerarQRComoArchivo(string contenido, string rutaSalida)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrData      = qrGenerator.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
            var qrCode      = new QRCode(qrData);
            Bitmap bmp      = qrCode.GetGraphic(5,
                System.Drawing.Color.Black,
                System.Drawing.Color.White, true);
            bmp.Save(rutaSalida, System.Drawing.Imaging.ImageFormat.Png);
            bmp.Dispose();
        }

        private static void DibujarPlaceholder(XGraphics g,
            double x, double y, double w, double h)
        {
            g.DrawRectangle(XPens.Gray,
                new XSolidBrush(XColor.FromArgb(200, 200, 200)),
                x, y, w, h);
            g.DrawString("SIN FOTO",
                new XFont("Verdana", 4, XFontStyleEx.Regular),
                XBrushes.Gray,
                new XRect(x, y, w, h),
                XStringFormats.Center);
        }
    }
}