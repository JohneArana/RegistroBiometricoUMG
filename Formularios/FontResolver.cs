using PdfSharp.Fonts;

namespace RegistroBiometricoUMG.Formularios
{
    public class UmgFontResolver : IFontResolver
    {
        public string DefaultFontName => "Verdana";

        public byte[] GetFont(string faceName)
        {
            // Buscar la fuente en el sistema Windows
            string[] rutas = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), faceName + ".ttf"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), faceName.ToLower() + ".ttf"),
                // Verdana como fallback seguro
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "verdana.ttf"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf"),
            };

            foreach (var ruta in rutas)
            {
                if (File.Exists(ruta))
                    return File.ReadAllBytes(ruta);
            }

            throw new FileNotFoundException($"No se encontró ninguna fuente disponible.");
        }

        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Mapear nombres de fuente a archivos TTF del sistema
            string archivo = familyName.ToLower() switch
            {
                "arial"   => "arial",
                "verdana" => "verdana",
                "segoe ui"=> "segoeui",
                _         => "verdana" // fallback
            };

            if (isBold && isItalic) archivo += "bi";
            else if (isBold)        archivo += "bd";
            else if (isItalic)      archivo += "i";

            return new FontResolverInfo(archivo);
        }
    }
}