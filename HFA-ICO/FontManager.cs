
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace HFA_ICO
{
    public static class FontManager
    {
        private static PrivateFontCollection _fontCollection;
        private static Dictionary<string, byte[]> _fontResources;
        private static bool _isInitialized;
        private static readonly Logger _logger;

        public static void Initialize()
        {
            if (_isInitialized)
                return;

            try
            {
                _logger?.Debug("Initializing FontManager");
                _fontCollection = new PrivateFontCollection();
                _fontResources = new Dictionary<string, byte[]>
                {
                    { "FontAwesome", Properties.Resources.FontAwesome },
                    { "FA7-Solid-900", Properties.Resources.FA7_Solid_900 },
                    { "FA7-Regular-400", Properties.Resources.FA7_Regular_400 },
                    { "FA7-Brand-400", Properties.Resources.FA7_Brand_400 },
                    { "ElegantIcons", Properties.Resources.ElegantIcons }
                };

                foreach (var fontResource in _fontResources)
                {
                    if (fontResource.Value == null || fontResource.Value.Length == 0)
                    {
                        _logger?.Warning($"Font resource {fontResource.Key} is null or empty");
                        continue;
                    }

                    IntPtr fontPtr = Marshal.AllocCoTaskMem(fontResource.Value.Length);
                    Marshal.Copy(fontResource.Value, 0, fontPtr, fontResource.Value.Length);
                    _fontCollection.AddMemoryFont(fontPtr, fontResource.Value.Length);
                    Marshal.FreeCoTaskMem(fontPtr);
                    _logger?.Debug($"Loaded font resource {fontResource.Key}");
                }

                if (_fontCollection.Families.Length == 0)
                {
                    _logger?.Error("No font families loaded into _fontCollection");
                }

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error initializing font resources: {ex.Message}");
                _fontCollection = null;
                _isInitialized = false;
            }
        }

        public static Font GetFontAwesomeFont(float size)
        {
            try
            {
                if (!_isInitialized || _fontCollection == null || _fontCollection.Families.Length == 0)
                {
                    _logger?.Debug("Font collection not initialized, initializing now");
                    Initialize();
                }

                if (_fontCollection == null || _fontCollection.Families.Length == 0)
                {
                    _logger?.Error("Font collection is empty after initialization");
                    return new Font("Arial", size, FontStyle.Regular);
                }

                foreach (var family in _fontCollection.Families)
                {
                    if (family.Name.Contains("FontAwesome") || family.Name.Contains("Solid"))
                    {
                        _logger?.Debug($"FontAwesome font found: {family.Name}");
                        return new Font(family, size, FontStyle.Regular);
                    }
                }

                _logger?.Warning("FontAwesome font not found, returning default font");
                return new Font("Arial", size, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error getting FontAwesome font: {ex.Message}");
                return new Font("Arial", size, FontStyle.Regular);
            }
        }

        public static Bitmap DrawGlyph(string fontName, char unicode, float fontSize, Color color, Size bitmapSize)
        {
            try
            {
                if (!_isInitialized || _fontCollection == null || _fontCollection.Families.Length == 0)
                {
                    _logger?.Debug("Font collection not initialized, initializing now");
                    Initialize();
                }

                if (_fontCollection == null || _fontCollection.Families.Length == 0)
                {
                    _logger?.Error($"Font collection is empty for {fontName}");
                    return new Bitmap(bitmapSize.Width, bitmapSize.Height);
                }

                FontFamily selectedFamily = null;
                foreach (var family in _fontCollection.Families)
                {
                    if (family.Name.Contains(fontName) || (fontName == "FontAwesome" && family.Name.Contains("Solid")))
                    {
                        selectedFamily = family;
                        break;
                    }
                }

                if (selectedFamily == null)
                {
                    _logger?.Warning($"Font {fontName} not found for glyph drawing");
                    return new Bitmap(bitmapSize.Width, bitmapSize.Height);
                }

                using (var font = new Font(selectedFamily, fontSize, FontStyle.Regular))
                using (var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    graphics.Clear(Color.Transparent);
                    using (var brush = new SolidBrush(color))
                    {
                        string glyph = unicode.ToString();
                        SizeF textSize = graphics.MeasureString(glyph, font);
                        float x = (bitmapSize.Width - textSize.Width) / 2;
                        float y = (bitmapSize.Height - textSize.Height) / 2;
                        graphics.DrawString(glyph, font, brush, x, y);
                        _logger?.Debug($"Glyph drawn: {fontName}, Unicode: \\u{Convert.ToInt32(unicode):X4}");
                        return new Bitmap(bitmap);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error drawing glyph for {fontName}: {ex.Message}");
                return new Bitmap(bitmapSize.Width, bitmapSize.Height);
            }
        }

        public static Icon CreateIcon(string fontName, char unicode, float fontSize, Color color, Size iconSize)
        {
            try
            {
                using (var bitmap = DrawGlyph(fontName, unicode, fontSize, color, iconSize))
                {
                    IntPtr hIcon = bitmap.GetHicon();
                    Icon icon = Icon.FromHandle(hIcon);
                    _logger?.Debug($"Icon created for {fontName}, Unicode: \\u{Convert.ToInt32(unicode):X4}");
                    return icon;
                }
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error creating icon for {fontName}: {ex.Message}");
                return null;
            }
        }
    }
}



























/*using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using CAJA_POS.Properties

namespace CAJA_POS.Clases
{
    public static class FontManager
    {
        private static PrivateFontCollection _fontCollection;
        private static Dictionary<string, byte[]> _fontResources;
        private static bool _isInitialized;

        public static void Initialize()
        {
            if (_isInitialized)
                return;

            var logger = CustomLoggerFactory.GetLogger(); // Declare logger once
            try
            {
                _fontCollection = new PrivateFontCollection();
                _fontResources = new Dictionary<string, byte[]>
                {
                    { "FontAwesome", Properties.Resources.FA7_Solid_900 },
                    { "FA7-Solid-900", Properties.Resources.FA7_Solid_900 },
                    { "FA7-Regular-400", Properties.Resources.FA7_Regular_400 },
                    { "FA7-Brand-400", Properties.Resources.FA7_Brand_400 },
                    { "ElegantIcons", Properties.Resources.ElegantIcons }
                };

                foreach (var fontResource in _fontResources)
                {
                    if (fontResource.Value == null || fontResource.Value.Length == 0)
                    {
                        logger?.Error($"Font resource {fontResource.Key} is null or empty", null, "FontManager.Initialize");
                        continue;
                    }

                    IntPtr fontPtr = Marshal.AllocCoTaskMem(fontResource.Value.Length);
                    Marshal.Copy(fontResource.Value, 0, fontPtr, fontResource.Value.Length);
                    _fontCollection.AddMemoryFont(fontPtr, fontResource.Value.Length);
                    Marshal.FreeCoTaskMem(fontPtr);
                    logger?.Debug($"Loaded font resource {fontResource.Key}", null, "FontManager.Initialize");
                }

                if (_fontCollection.Families.Length == 0)
                {
                    logger?.Error("No font families loaded into _fontCollection", null, "FontManager.Initialize");
                }

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                logger?.Error($"Error loading font resources: {ex.Message}", ex, "FontManager.Initialize");
                _fontCollection = null; // Reset to force re-initialization if needed
            }
        }

        public static Font GetFontAwesomeFont(float size)
        {
            var logger = CustomLoggerFactory.GetLogger(); // Declare logger once
            try
            {
                if (_fontCollection == null || _fontCollection.Families.Length == 0)
                    Initialize();

                if (_fontCollection == null || _fontCollection.Families.Length == 0)
                {
                    logger?.Error("Font collection is empty after initialization", null, "FontManager.GetFontAwesomeFont");
                    return new Font("Arial", size, FontStyle.Regular);
                }

                foreach (var family in _fontCollection.Families)
                {
                    if (family.Name.Contains("FontAwesome") || family.Name.Contains("Solid"))
                        return new Font(family, size, FontStyle.Regular);
                }

                logger?.Error("FontAwesome font not found, returning default font", null, "FontManager.GetFontAwesomeFont");
                return new Font("Arial", size, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                logger?.Error($"Error getting FontAwesome font: {ex.Message}", ex, "FontManager.GetFontAwesomeFont");
                return new Font("Arial", size, FontStyle.Regular);
            }
        }

        public static Bitmap DrawGlyph(string fontName, char unicode, float fontSize, Color color, Size bitmapSize)
        {
            var logger = CustomLoggerFactory.GetLogger(); // Declare logger once
            try
            {
                if (_fontCollection == null || _fontCollection.Families.Length == 0)
                    Initialize();

                if (_fontCollection == null || _fontCollection.Families.Length == 0)
                {
                    logger?.Error($"Font collection is empty for {fontName}", null, "FontManager.DrawGlyph");
                    return new Bitmap(bitmapSize.Width, bitmapSize.Height);
                }

                FontFamily selectedFamily = null;
                foreach (var family in _fontCollection.Families)
                {
                    if (family.Name.Contains(fontName) || (fontName == "FontAwesome" && family.Name.Contains("Solid")))
                    {
                        selectedFamily = family;
                        break;
                    }
                }

                if (selectedFamily == null)
                {
                    logger?.Error($"Font {fontName} not found for glyph drawing", null, "FontManager.DrawGlyph");
                    return new Bitmap(bitmapSize.Width, bitmapSize.Height);
                }

                using (var font = new Font(selectedFamily, fontSize, FontStyle.Regular))
                using (var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                    using (var brush = new SolidBrush(color))
                    {
                        string glyph = unicode.ToString();
                        SizeF textSize = graphics.MeasureString(glyph, font);
                        float x = (bitmapSize.Width - textSize.Width) / 2;
                        float y = (bitmapSize.Height - textSize.Height) / 2;
                        graphics.DrawString(glyph, font, brush, x, y);
                        return new Bitmap(bitmap);
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.Error($"Error drawing glyph for {fontName}: {ex.Message}", ex, "FontManager.DrawGlyph");
                return new Bitmap(bitmapSize.Width, bitmapSize.Height);
            }
        }

        public static Icon CreateIcon(string fontName, char unicode, float fontSize, Color color, Size iconSize)
        {
            var logger = CustomLoggerFactory.GetLogger();
            try
            {
                using (var bitmap = DrawGlyph(fontName, unicode, fontSize, color, iconSize))
                {
                    IntPtr hIcon = bitmap.GetHicon();
                    Icon icon = Icon.FromHandle(hIcon);
                    return icon;
                }
            }
            catch (Exception ex)
            {
                logger?.Error($"Error creating icon for {fontName}: {ex.Message}", ex, "FontManager.CreateIcon");
                return null;
            }
        }
    }
}
*/
































/*using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CAJA_POS.Clases
{
    public static class FontManager
    {
        private static Dictionary<string, PrivateFontCollection> _fontCollections;
        private static bool _isInitialized;
        private static PrivateFontCollection _fontCollection;
        private static Dictionary<string, byte[]> _fontResources; // Declare the missing field*/

/* public static void Initialize()
 {
     if (_isInitialized)
         return;

     var logger = CustomLoggerFactory.GetLogger();
     _fontCollections = new Dictionary<string, PrivateFontCollection>();
     try
     {
         // Define the path to the fonts directory
         string fontDirectory = @"C:\HFA-POS\HFA-POS\Fonts";
         if (!Directory.Exists(fontDirectory))
         {
             logger?.Error($"Directorio de fuentes {fontDirectory} no encontrado.", null, "FontManager.Initialize");
             return;
         }

         // Define the font files to load (map font names to their file names)
         var fontFiles = new Dictionary<string, string>
         {
             { "FontAwesome", "FontAwesome.ttf" }, // Adjust file names to match actual files
             { "ElegantIcons", "ElegantIcons.ttf" },
             { "FA7-Brand-400", "FA7-Brand-400.otf" },
             { "FA7-Regular-400", "FA7-Regular-400.otf" },
             { "FA7-Solid-900", "FA7-Solid-900.otf" }
         };

         foreach (var font in fontFiles)
         {
             string fontName = font.Key;
             string fontFilePath = Path.Combine(fontDirectory, font.Value);

             if (!File.Exists(fontFilePath))
             {
                 logger?.Error($"Archivo de fuente {fontFilePath} no encontrado.", null, "FontManager.Initialize");
                 continue;
             }

             var pfc = new PrivateFontCollection();
             try
             {
                 // Load the font file into memory
                 byte[] fontData = File.ReadAllBytes(fontFilePath);
                 IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(fontData.Length);
                 try
                 {
                     System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
                     pfc.AddMemoryFont(fontPtr, fontData.Length);
                     if (pfc.Families.Length == 0)
                     {
                         logger?.Error($"No se pudieron cargar las familias de la fuente {fontName}.", null, "FontManager.Initialize");
                         pfc.Dispose();
                         continue;
                     }
                     _fontCollections.Add(fontName, pfc);
                 }
                 finally
                 {
                     System.Runtime.InteropServices.Marshal.FreeHGlobal(fontPtr);
                 }
             }
             catch (Exception ex)
             {
                 pfc.Dispose();
                 logger?.Error($"Error al cargar la fuente {fontName}: {ex.Message}", ex, "FontManager.Initialize");
             }
         }

         if (_fontCollections.Count == 0)
         {
             logger?.Error("No se cargó ninguna fuente válida.", null, "FontManager.Initialize");
         }

         _isInitialized = true;
     }
     catch (Exception ex)
     {
         logger?.Error($"Error al inicializar fuentes: {ex.Message}", ex, "FontManager.Initialize");
     }
 }*/



/* public static void Initialize()
 {
     try
     {
         if (_fontCollection == null)
         {
             _fontCollection = new PrivateFontCollection();
             // Map font names to correct resources
             _fontResources = new Dictionary<string, byte[]>
     {
         { "FontAwesome", Properties.Resources.FA7_Solid_900 }, // Use FA7-Solid-900
         { "FA7-Solid-900", Properties.Resources.FA7_Solid_900 },
         { "FA7-Regular-400", Properties.Resources.FA7_Regular_400 },
         { "FA7-Brand-400", Properties.Resources.FA7_Brand_400 },
         { "ElegantIcons", Properties.Resources.ElegantIcons }
     };

             foreach (var fontResource in _fontResources.Values)
             {
                 if (fontResource != null && fontResource.Length > 0)
                 {
                     IntPtr fontPtr = Marshal.AllocCoTaskMem(fontResource.Length);
                     Marshal.Copy(fontResource, 0, fontPtr, fontResource.Length);
                     _fontCollection.AddMemoryFont(fontPtr, fontResource.Length);
                     Marshal.FreeCoTaskMem(fontPtr);
                 }
             }
         }
     }
     catch (Exception ex)
     {
         var logger = CustomLoggerFactory.GetLogger();
         logger?.Error($"Error loading font resources: {ex.Message}", ex, "FontManager.Initialize");
     }
 }*/
/*public static void Initialize()
{
    if (_isInitialized)
        return;

    var logger = CustomLoggerFactory.GetLogger();
    _fontCollections = new Dictionary<string, PrivateFontCollection>();
    try
    {
        var fontResources = new Dictionary<string, byte[]>
        {
            { "FontAwesome", Properties.Resources.FontAwesome },
            { "ElegantIcons", Properties.Resources.FontAwesome },
            { "FA7-Brand-400", Properties.Resources.FontAwesome },
            { "FA7-Regular-400", Properties.Resources.FontAwesome },
            { "FA7-Solid-900", Properties.Resources.FontAwesome }
        };

        foreach (var resource in fontResources)
        {
            string fontName = resource.Key;
            byte[] fontData = resource.Value;

            if (fontData == null || fontData.Length == 0)
            {
                logger?.Error($"Fuente {fontName} no encontrada o vacía en Resources.resx.", null, "FontManager.Initialize");
                continue;
            }

            var pfc = new PrivateFontCollection();
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(fontData.Length);
            try
            {
                System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
                pfc.AddMemoryFont(fontPtr, fontData.Length);
                if (pfc.Families.Length == 0)
                {
                    logger?.Error($"No se pudieron cargar las familias de la fuente {fontName}.", null, "FontManager.Initialize");
                    pfc.Dispose();
                    continue;
                }
                _fontCollections.Add(fontName, pfc);
            }
            catch (Exception ex)
            {
                pfc.Dispose();
                logger?.Error($"Error al cargar la fuente {fontName}: {ex.Message}", ex, "FontManager.Initialize");
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.FreeHGlobal(fontPtr);
            }
        }

        if (_fontCollections.Count == 0)
        {
            logger?.Error("No se cargó ninguna fuente válida.", null, "FontManager.Initialize");
        }

        _isInitialized = true;
    }
    catch (Exception ex)
    {
        logger?.Error($"Error al inicializar fuentes: {ex.Message}", ex, "FontManager.Initialize");
    }
}*/

/*public static Font GetFontAwesomeFont(float size)
{
    if (!_isInitialized)
        Initialize();

    var logger = CustomLoggerFactory.GetLogger();
    try
    {
        if (_fontCollections.ContainsKey("FA7-Solid-900") && _fontCollections["FA7-Solid-900"].Families.Length > 0)
        {
            return new Font(_fontCollections["FA7-Solid-900"].Families[0], size, FontStyle.Regular);
        }
        logger?.Warning("Fuente FA7-Solid-900 no disponible, usando Segoe UI como respaldo.", null, "FontManager.GetFontAwesomeFont");
        return new Font("Segoe UI", size);
    }
    catch (Exception ex)
    {
        logger?.Error($"Error al obtener fuente FA7-Solid-900: {ex.Message}", ex, "FontManager.GetFontAwesomeFont");
        return new Font("Segoe UI", size);
    }
}

private static FontFamily _fontAwesomeFamily;
public static FontFamily GetFontAwesomeFamily()
{
    Initialize();
    return _fontAwesomeFamily ?? new FontFamily("Segoe UI");
}

public static Bitmap DrawGlyph(string fontName, char unicode, float fontSize, Color color, Size bitmapSize)
{
    if (!_isInitialized)
        Initialize();

    var logger = CustomLoggerFactory.GetLogger();
    if (!_fontCollections.ContainsKey(fontName) || _fontCollections[fontName].Families.Length == 0)
    {
        logger?.Error($"Fuente {fontName} no está inicializada correctamente.", null, "FontManager.DrawGlyph");
        throw new ArgumentException($"Fuente {fontName} no está inicializada correctamente.", nameof(fontName));
    }

    if (bitmapSize.Width <= 0 || bitmapSize.Height <= 0)
    {
        logger?.Error("El tamaño del bitmap debe ser mayor que cero.", null, "FontManager.DrawGlyph");
        throw new ArgumentException("El tamaño del bitmap debe ser mayor que cero.", nameof(bitmapSize));
    }

    if (fontSize <= 0)
    {
        logger?.Error("El tamaño de la fuente debe ser mayor que cero.", null, "FontManager.DrawGlyph");
        throw new ArgumentException("El tamaño de la fuente debe ser mayor que cero.", nameof(fontSize));
    }

    Bitmap bitmap = null;
    try
    {
        bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.Transparent);
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            using (Font font = new Font(_fontCollections[fontName].Families[0], fontSize, FontStyle.Regular))
            {
                string glyph = unicode.ToString();
                using (SolidBrush brush = new SolidBrush(color))
                {
                    g.DrawString(glyph, font, brush, new PointF(0, 0));
                }
            }
        }

        if (!DesignMode)
        {
            try
            {
                bitmap.Save($"debug_glyph_{unicode:X4}.png");
            }
            catch (Exception ex)
            {
                logger?.Warning($"Error al guardar el bitmap de depuración para el glifo {unicode:X4}: {ex.Message}", ex, "FontManager.DrawGlyph");
            }
        }

        return bitmap;
    }
    catch (Exception ex)
    {
        if (bitmap != null)
            bitmap.Dispose();
        logger?.Error($"Error al dibujar el glifo U+{unicode:X4}: {ex.Message}", ex, "FontManager.DrawGlyph");
        throw new Exception($"Error al dibujar el glifo U+{unicode:X4}: {ex.Message}", ex);
    }
}

public static Icon CreateIcon(string fontName, char unicode, float fontSize, Color color, Size iconSize)
{
    var logger = CustomLoggerFactory.GetLogger();
    if (iconSize.Width <= 0 || iconSize.Height <= 0)
    {
        logger?.Error("El tamaño del ícono debe ser mayor que cero.", null, "FontManager.CreateIcon");
        throw new ArgumentException("El tamaño del ícono debe ser mayor que cero.", nameof(iconSize));
    }

    Bitmap bitmap = null;
    try
    {
        bitmap = DrawGlyph(fontName, unicode, fontSize, color, iconSize);
        IntPtr hIcon = bitmap.GetHicon();
        try
        {
            return Icon.FromHandle(hIcon);
        }
        catch (Exception ex)
        {
            logger?.Error($"Error al convertir el bitmap a ícono para el glifo U+{unicode:X4}: {ex.Message}", ex, "FontManager.CreateIcon");
            throw new Exception($"Error al convertir el bitmap a ícono para el glifo U+{unicode:X4}: {ex.Message}", ex);
        }
    }
    finally
    {
        if (bitmap != null)
            bitmap.Dispose();
    }
}

public static void Dispose()
{
    var logger = CustomLoggerFactory.GetLogger();
    if (_fontCollections != null)
    {
        foreach (var pfc in _fontCollections.Values)
        {
            pfc?.Dispose();
        }
        _fontCollections.Clear();
        _fontCollections = null;
        _isInitialized = false;
    }
}

private static bool DesignMode
{
    get
    {
        return System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime;
    }
}
}
}*/




