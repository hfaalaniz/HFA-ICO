using HFA_ICO;
using System;
using System.IO;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace HFA_ICO
{
    public partial class EjemploLogger : Form
    {
        private Logger logger;
        private TextBox txtLog;

        public EjemploLogger()
        {
            //InitializeComponent();
            // Inicializar logger con ejemplos
            InicializarLoggerBasico();
            //InicializarLoggerPersonalizado();
            //InicializarLoggerSoloArchivo();
            // Usar diferentes ejemplos
            UsarDiferentesNiveles();
            LoggingDeExcepciones();
            ConvertirIcono();
            GestionarArchivosLog();
            ProcesoConLogger();
            ConfigurarSegunEntorno();
            UsarLoggerEstatico();
            //ConvertirConLogger();
            CrearMenuContextualLogs();
        }

        // ===== EJEMPLO 1: Uso Básico (con TextBox) =====
        private void InicializarLoggerBasico()
        {
            // Crear TextBox para logs
            txtLog = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Bottom,
                Height = 200,
                BackColor = System.Drawing.Color.Black,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Consolas", 9)
            };
            this.Controls.Add(txtLog);

            // Inicializar logger básico (guarda en carpeta "Logs")
            logger = new Logger(txtLog);

            // Usar logger
            logger.Info("Aplicación iniciada");
            logger.Debug("Modo de depuración activado");
        }

        // ===== EJEMPLO 2: Configuración Personalizada =====
        private void InicializarLoggerPersonalizado()
        {
            var config = new LoggerConfig
            {
                // Directorio personalizado
                LogDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                           "HFA_ICO", "Logs"),

                // Prefijo personalizado
                LogFilePrefix = "HFA_ICO",

                // Tamaño máximo 5 MB
                MaxFileSizeMB = 5,

                // Mantener últimos 20 archivos
                MaxLogFiles = 20,

                // Solo registrar Warning y superiores
                MinimumLogLevel = LogLevel.Warning,

                // Incluir información del hilo
                IncludeThreadInfo = true,

                // Formato de timestamp personalizado
                TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff"
            };

            logger = new Logger(txtLog, config);
            logger.Info("Logger personalizado iniciado");
        }

        // ===== EJEMPLO 3: Logger Solo para Archivo (sin TextBox) =====
        private void InicializarLoggerSoloArchivo()
        {
            var config = new LoggerConfig
            {
                LogDirectory = @"C:\MisLogs\HFA_ICO",
                LogFilePrefix = "app",
                EnableTextBoxLogging = false,  // Deshabilitar TextBox
                EnableFileLogging = true       // Solo archivo
            };

            logger = new Logger(config); // Sin TextBox
            logger.Info("Este mensaje solo va al archivo");
        }

        // ===== EJEMPLO 4: Diferentes Niveles de Log =====
        private void UsarDiferentesNiveles()
        {
            logger.Debug("Información detallada de depuración");
            logger.Info("Operación completada exitosamente");
            logger.Warning("Advertencia: La operación tardó más de lo esperado");
            logger.Error("Error: No se pudo conectar al servidor");
            logger.Critical("Error crítico: Corrupción de datos detectada");
        }

        // ===== EJEMPLO 5: Logging de Excepciones =====
        private void LoggingDeExcepciones()
        {
            try
            {
                // Código que puede fallar
                int resultado = int.Parse("no es un número");
            }
            catch (FormatException ex)
            {
                logger.Error("Error al convertir valor", ex);
            }
            catch (Exception ex)
            {
                logger.Critical("Error crítico inesperado", ex);
            }
        }

        // ===== EJEMPLO 6: Logging en Operaciones Complejas =====
        private void ConvertirIcono()
        {
            logger.Info("=== Iniciando conversión de icono ===");
            logger.Separator('=', 60);

            try
            {
                logger.Debug("Validando imagen de entrada");
                // ValidarImagen();

                logger.Info("Imagen validada correctamente");
                logger.Debug($"Dimensiones: 512x512");

                logger.Info("Iniciando redimensionamiento...");
                int[] tamaños = { 256, 128, 64, 32, 16 };

                foreach (int tamaño in tamaños)
                {
                    logger.Debug($"Redimensionando a {tamaño}x{tamaño}");
                    // RedimensionarImagen(tamaño);
                    logger.Info($"✓ Tamaño {tamaño}x{tamaño} completado");
                }

                logger.Info("Guardando archivo ICO...");
                // GuardarICO();

                logger.Separator('-', 60);
                logger.Info("✓ Conversión completada exitosamente");
            }
            catch (Exception ex)
            {
                logger.Error("Error durante la conversión", ex);
                logger.Separator('-', 60);
                logger.Error("✗ Conversión fallida");
            }
        }

        // ===== EJEMPLO 7: Gestión de Archivos de Log =====
        private void GestionarArchivosLog()
        {
            // Obtener ruta del archivo actual
            string archivoActual = logger.GetCurrentLogFilePath();
            logger.Info($"Archivo de log actual: {archivoActual}");

            // Obtener todos los archivos de log
            var todosLosLogs = logger.GetAllLogFiles();
            logger.Info($"Total de archivos de log: {todosLosLogs.Count}");

            // Obtener tamaño total
            double tamaño = logger.GetTotalLogSizeMB();
            logger.Info($"Tamaño total de logs: {tamaño:F2} MB");

            // Abrir carpeta de logs
            // logger.OpenLogDirectory();
        }

        // ===== EJEMPLO 8: Logger con Progreso =====
        private async void ProcesoConLogger()
        {
            logger.Info("Iniciando proceso largo...");

            for (int i = 0; i <= 100; i += 10)
            {
                logger.Debug($"Progreso: {i}%");
                await System.Threading.Tasks.Task.Delay(500);
            }

            logger.Info("Proceso completado");
        }

        // ===== EJEMPLO 9: Configuración en Producción vs Desarrollo =====
        private void ConfigurarSegunEntorno()
        {
            bool esDesarrollo = System.Diagnostics.Debugger.IsAttached;

            var config = new LoggerConfig
            {
                MinimumLogLevel = esDesarrollo ? LogLevel.Debug : LogLevel.Info,
                MaxFileSizeMB = esDesarrollo ? 5 : 10,
                MaxLogFiles = esDesarrollo ? 5 : 20,
                IncludeThreadInfo = esDesarrollo
            };

            logger = new Logger(txtLog, config);
            logger.Info($"Logger configurado para: {(esDesarrollo ? "DESARROLLO" : "PRODUCCIÓN")}");
        }

        // ===== EJEMPLO 10: Cleanup al Cerrar Aplicación =====
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                logger.Info("Cerrando aplicación...");
                logger.Separator('=', 80);
                logger.Info("Aplicación cerrada correctamente");

                // Importante: Dispose del logger para cerrar archivos
                logger?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar logger: {ex.Message}");
            }

            base.OnFormClosing(e);
        }

        // ===== EJEMPLO 11: Logger Estático (Singleton) =====
        public static class LoggerManager
        {
            private static Logger _instance;
            private static readonly object _lock = new object();

            public static Logger Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (_lock)
                        {
                            if (_instance == null)
                            {
                                var config = new LoggerConfig
                                {
                                    LogDirectory = Path.Combine(
                                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                        "HFA_ICO",
                                        "Logs"
                                    ),
                                    LogFilePrefix = "HFA_ICO",
                                    MaxFileSizeMB = 10,
                                    MaxLogFiles = 15
                                };

                                _instance = new Logger(config);
                            }
                        }
                    }
                    return _instance;
                }
            }

            public static void Initialize(TextBox textBox, LoggerConfig config = null)
            {
                lock (_lock)
                {
                    _instance?.Dispose();
                    _instance = new Logger(textBox, config ?? new LoggerConfig());
                }
            }

            public static void Shutdown()
            {
                lock (_lock)
                {
                    _instance?.Dispose();
                    _instance = null;
                }
            }
        }

        // Uso del Logger estático
        private void UsarLoggerEstatico()
        {
            // Inicializar una vez al inicio
            LoggerManager.Initialize(txtLog);

            // Usar desde cualquier parte
            LoggerManager.Instance.Info("Mensaje desde cualquier clase");

            // Al cerrar la aplicación
            LoggerManager.Shutdown();
        }

        // ===== EJEMPLO 12: Integración con IconConverter =====
        /*private void ConvertirConLogger()
        {
            logger.Info("=== CONVERSIÓN DE ICONO INICIADA ===");
            logger.Separator('=', 80);

            try
            {
                // Validaciones
                if (string.IsNullOrEmpty(imagenSeleccionada))
                {
                    logger.Warning("No hay imagen seleccionada");
                    MessageBox.Show("Selecciona una imagen primero");
                    return;
                }

                logger.Info($"Imagen origen: {Path.GetFileName(imagenSeleccionada)}");
                logger.Debug($"Ruta completa: {imagenSeleccionada}");

                if (clbTamaños.CheckedItems.Count == 0)
                {
                    logger.Warning("No hay tamaños seleccionados");
                    MessageBox.Show("Selecciona al menos un tamaño");
                    return;
                }

                int[] tamaños = ObtenerTamañosSeleccionados();
                logger.Info($"Tamaños seleccionados: {string.Join(", ", tamaños)}");

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Icono|*.ico";
                    sfd.FileName = Path.GetFileNameWithoutExtension(imagenSeleccionada) + ".ico";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        logger.Info($"Archivo destino: {sfd.FileName}");
                        logger.Separator('-', 80);

                        // Crear configuración del logger para el converter
                        var converterConfig = new LoggerConfig
                        {
                            LogDirectory = Path.Combine(logger.GetCurrentLogFilePath(), "..", "Conversiones"),
                            LogFilePrefix = "conversion",
                            MinimumLogLevel = LogLevel.Debug
                        };

                        // Pasar logger al converter
                        var converter = new IconConverter(logger);

                        Image imagenAConvertir = imagenRecortada ?? imagenOriginal;

                        logger.Info("Iniciando proceso de conversión...");
                        converter.ConvertirAIconoDesdeImagen(imagenAConvertir, sfd.FileName, tamaños);

                        logger.Separator('-', 80);
                        logger.Info("✓ CONVERSIÓN COMPLETADA EXITOSAMENTE");
                        logger.Info($"Archivo creado: {sfd.FileName}");

                        FileInfo fi = new FileInfo(sfd.FileName);
                        logger.Info($"Tamaño del archivo: {fi.Length / 1024.0:F2} KB");

                        MessageBox.Show("¡Icono creado exitosamente!", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        logger.Info("Conversión cancelada por el usuario");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error crítico durante la conversión", ex);
                logger.Separator('-', 80);
                logger.Error("✗ CONVERSIÓN FALLIDA");

                MessageBox.Show($"Error al convertir: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                logger.Separator('=', 80);
                logger.Info("Proceso de conversión finalizado");
                logger.Separator('=', 80);
            }
        }*/

        // ===== EJEMPLO 13: Menú Contextual para Logs =====
        private void CrearMenuContextualLogs()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            // Limpiar logs del TextBox
            menu.Items.Add("Limpiar logs", null, (s, e) =>
            {
                logger.Limpiar();
            });

            // Abrir carpeta de logs
            menu.Items.Add("Abrir carpeta de logs", null, (s, e) =>
            {
                logger.OpenLogDirectory();
            });

            // Copiar ruta del archivo actual
            menu.Items.Add("Copiar ruta del archivo actual", null, (s, e) =>
            {
                string ruta = logger.GetCurrentLogFilePath();
                if (!string.IsNullOrEmpty(ruta))
                {
                    Clipboard.SetText(ruta);
                    logger.Info("Ruta copiada al portapapeles");
                }
            });

            menu.Items.Add(new ToolStripSeparator());

            // Mostrar estadísticas
            menu.Items.Add("Mostrar estadísticas", null, (s, e) =>
            {
                var logs = logger.GetAllLogFiles();
                double tamaño = logger.GetTotalLogSizeMB();

                string mensaje = $"Total de archivos: {logs.Count}\n" +
                               $"Tamaño total: {tamaño:F2} MB\n" +
                               $"Archivo actual: {Path.GetFileName(logger.GetCurrentLogFilePath())}";

                MessageBox.Show(mensaje, "Estadísticas de Logs",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                logger.Info("Estadísticas de logs consultadas");
            });

            txtLog.ContextMenuStrip = menu;
        }
    }
}