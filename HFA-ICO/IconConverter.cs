using System.Drawing.Imaging;

namespace HFA_ICO
{
    public class IconConverter
    {
        private IProgressReporter progressBar;
        private readonly Logger logger;

        public IconConverter(IProgressReporter progressBar, Logger logger = null)
        {
            this.progressBar = progressBar;
            this.logger = logger;
        }

        public void ConvertirAIcono(string rutaImagen, string rutaIcono, int[] tamaños)
        {
            using (Image imagen = Image.FromFile(rutaImagen))
            {
                ConvertirAIconoDesdeImagen(imagen, rutaIcono, tamaños);
            }
        }

        public void ConvertirAIconoDesdeImagen(Image imagenBase, string rutaIcono, int[] tamaños)
        {
            logger?.Debug($"Iniciando conversión. Tamaños: {tamaños.Length}");

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // Header ICO
                bw.Write((short)0);
                bw.Write((short)1);
                bw.Write((short)tamaños.Length);

                var imagenes = new MemoryStream[tamaños.Length];
                long offset = 6 + (16 * tamaños.Length);

                if (progressBar != null)
                {
                    int maxProgreso = tamaños.Length * 2;
                    progressBar.Maximum = maxProgreso;
                    logger?.Debug($"ProgressBar configurado: Maximum={maxProgreso}");
                }
                int progreso = 0;

                // Crear cada tamaño
                for (int i = 0; i < tamaños.Length; i++)
                {
                    logger?.Debug($"Procesando tamaño {tamaños[i]}x{tamaños[i]} ({i + 1}/{tamaños.Length})");

                    using (Bitmap resized = new Bitmap(imagenBase, tamaños[i], tamaños[i]))
                    {
                        imagenes[i] = new MemoryStream();
                        resized.Save(imagenes[i], ImageFormat.Png);

                        byte ancho = (byte)(tamaños[i] == 256 ? 0 : tamaños[i]);
                        byte alto = (byte)(tamaños[i] == 256 ? 0 : tamaños[i]);

                        bw.Write(ancho);
                        bw.Write(alto);
                        bw.Write((byte)0);
                        bw.Write((byte)0);
                        bw.Write((short)1);
                        bw.Write((short)32);
                        bw.Write((int)imagenes[i].Length);
                        bw.Write((int)offset);

                        offset += imagenes[i].Length;
                    }

                    progreso++;
                    ActualizarProgreso(progreso);
                }

                logger?.Debug("Escribiendo datos de imágenes");

                // Escribir datos
                for (int i = 0; i < imagenes.Length; i++)
                {
                    bw.Write(imagenes[i].ToArray());
                    imagenes[i].Dispose();
                    progreso++;
                    ActualizarProgreso(progreso);
                }

                File.WriteAllBytes(rutaIcono, ms.ToArray());
                logger?.Info($"Archivo ICO guardado: {new FileInfo(rutaIcono).Length} bytes");
            }
        }

        private void ActualizarProgreso(int valor)
        {
            if (progressBar != null)
            {
                try
                {
                    // Asegurar que no exceda el máximo
                    if (valor > progressBar.Maximum)
                    {
                        logger?.Warning($"Valor de progreso ({valor}) excede máximo ({progressBar.Maximum})");
                        valor = progressBar.Maximum;
                    }

                    progressBar.Value = valor;
                    logger?.Debug($"Progreso actualizado: {valor}/{progressBar.Maximum}");
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    logger?.Error($"Error al actualizar progreso: {ex.Message}");
                }
            }
        }
    }
}