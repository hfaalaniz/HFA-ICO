using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HFA_ICO
{
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
        Critical = 4
    }

    public class LoggerConfig
    {
        /// <summary>
        /// Directorio donde se guardarán los archivos de log
        /// </summary>
        public string LogDirectory { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        /// <summary>
        /// Prefijo para los archivos de log (ej: "HFA_ICO")
        /// </summary>
        public string LogFilePrefix { get; set; } = "app";

        /// <summary>
        /// Tamaño máximo del archivo en MB antes de crear uno nuevo
        /// </summary>
        public int MaxFileSizeMB { get; set; } = 10;

        /// <summary>
        /// Número máximo de archivos de log a mantener
        /// </summary>
        public int MaxLogFiles { get; set; } = 10;

        /// <summary>
        /// Nivel mínimo de log a registrar
        /// </summary>
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Debug;

        /// <summary>
        /// Habilitar escritura en archivo
        /// </summary>
        public bool EnableFileLogging { get; set; } = true;

        /// <summary>
        /// Habilitar escritura en TextBox
        /// </summary>
        public bool EnableTextBoxLogging { get; set; } = true;

        /// <summary>
        /// Formato de timestamp
        /// </summary>
        public string TimestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// Incluir información del hilo
        /// </summary>
        public bool IncludeThreadInfo { get; set; } = false;

        /// <summary>
        /// Codificación del archivo
        /// </summary>
        public Encoding FileEncoding { get; set; } = Encoding.UTF8;
    }

    public partial class Logger : IDisposable
    {
        private readonly TextBox textBox;
        private readonly LoggerConfig config;
        private readonly object lockObject = new object();
        private string currentLogFile;
        private StreamWriter logWriter;
        private bool disposed = false;

        #region Constructores

        public Logger(TextBox textBox) : this(textBox, new LoggerConfig())
        {
        }

        public Logger(TextBox textBox, LoggerConfig config)
        {
            this.textBox = textBox;
            this.config = config ?? new LoggerConfig();

            if (config.EnableFileLogging)
            {
                InitializeFileLogging();
            }
        }

        /// <summary>
        /// Constructor sin TextBox (solo archivo)
        /// </summary>
        public Logger(LoggerConfig config)
        {
            this.textBox = null;
            this.config = config ?? new LoggerConfig();
            this.config.EnableTextBoxLogging = false;

            if (config.EnableFileLogging)
            {
                InitializeFileLogging();
            }
        }

        #endregion

        #region Inicialización

        private void InitializeFileLogging()
        {
            try
            {
                // Crear directorio si no existe
                if (!Directory.Exists(config.LogDirectory))
                {
                    Directory.CreateDirectory(config.LogDirectory);
                }

                // Limpiar archivos viejos
                CleanOldLogFiles();

                // Crear o abrir archivo de log
                CreateNewLogFile();

                // Escribir encabezado
                WriteLogHeader();
            }
            catch (Exception ex)
            {
                // Si falla la inicialización, deshabilitar logging de archivo
                config.EnableFileLogging = false;
                Console.WriteLine($"Error inicializando logger: {ex.Message}");
            }
        }

        private void CreateNewLogFile()
        {
            lock (lockObject)
            {
                try
                {
                    // Cerrar archivo anterior si existe
                    CloseLogFile();

                    // Generar nombre de archivo con timestamp
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    currentLogFile = Path.Combine(
                        config.LogDirectory,
                        $"{config.LogFilePrefix}_{timestamp}.log"
                    );

                    // Crear StreamWriter
                    logWriter = new StreamWriter(currentLogFile, true, config.FileEncoding)
                    {
                        AutoFlush = true // Flush automático para no perder logs
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creando archivo de log: {ex.Message}");
                    config.EnableFileLogging = false;
                }
            }
        }

        private void WriteLogHeader()
        {
            if (logWriter == null) return;

            try
            {
                logWriter.WriteLine("".PadRight(80, '='));
                logWriter.WriteLine($"Log iniciado: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                logWriter.WriteLine($"Aplicación: {AppDomain.CurrentDomain.FriendlyName}");
                logWriter.WriteLine($"Versión: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");
                logWriter.WriteLine($"Sistema: {Environment.OSVersion}");
                logWriter.WriteLine($"Usuario: {Environment.UserName}");
                logWriter.WriteLine("".PadRight(80, '='));
                logWriter.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error escribiendo encabezado: {ex.Message}");
            }
        }

        #endregion

        #region Métodos Públicos de Logging

        public void Debug(string mensaje)
        {
            Log(LogLevel.Debug, mensaje);
        }

        public void Info(string mensaje)
        {
            Log(LogLevel.Info, mensaje);
        }

        public void Warning(string mensaje)
        {
            Log(LogLevel.Warning, mensaje);
        }

        public void Error(string mensaje)
        {
            Log(LogLevel.Error, mensaje);
        }

        public void Error(string mensaje, Exception exception)
        {
            string mensajeCompleto = $"{mensaje}\nExcepción: {exception.GetType().Name}\nMensaje: {exception.Message}\nStackTrace: {exception.StackTrace}";
            Log(LogLevel.Error, mensajeCompleto);
        }

        public void Critical(string mensaje)
        {
            Log(LogLevel.Critical, mensaje);
        }

        public void Critical(string mensaje, Exception exception)
        {
            string mensajeCompleto = $"{mensaje}\nExcepción: {exception.GetType().Name}\nMensaje: {exception.Message}\nStackTrace: {exception.StackTrace}";
            Log(LogLevel.Critical, mensajeCompleto);
        }

        /// <summary>
        /// Escribe una línea separadora en el log
        /// </summary>
        public void Separator(char character = '-', int length = 80)
        {
            string separator = "".PadRight(length, character);
            WriteToTextBox("", separator, "Gray");
            WriteToFile("", separator);
        }

        /// <summary>
        /// Escribe un mensaje con nivel personalizado
        /// </summary>
        public void Log(LogLevel level, string mensaje)
        {
            if (level < config.MinimumLogLevel) return;

            string nivelStr = level.ToString().ToUpper();
            string color = GetColorForLevel(level);

            WriteToTextBox(nivelStr, mensaje, color);
            WriteToFile(nivelStr, mensaje);
        }

        #endregion

        #region Escritura en Destinos

        private void WriteToTextBox(string nivel, string mensaje, string color)
        {
            if (!config.EnableTextBoxLogging || textBox == null || textBox.IsDisposed)
                return;

            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new Action(() => WriteToTextBox(nivel, mensaje, color)));
                return;
            }

            try
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                string linea = string.IsNullOrEmpty(nivel)
                    ? mensaje
                    : $"[{timestamp}] [{nivel}] {mensaje}";

                textBox.AppendText(linea + Environment.NewLine);

                // Limitar el tamaño del TextBox (mantener últimas 1000 líneas)
                if (textBox.Lines.Length > 1000)
                {
                    var lines = textBox.Lines.Skip(textBox.Lines.Length - 1000).ToArray();
                    textBox.Lines = lines;
                }

                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error escribiendo en TextBox: {ex.Message}");
            }
        }

        private void WriteToFile(string nivel, string mensaje)
        {
            if (!config.EnableFileLogging || logWriter == null)
                return;

            lock (lockObject)
            {
                try
                {
                    // Verificar tamaño del archivo
                    CheckAndRotateLogFile();

                    string timestamp = DateTime.Now.ToString(config.TimestampFormat);
                    string threadInfo = config.IncludeThreadInfo
                        ? $"[Thread-{System.Threading.Thread.CurrentThread.ManagedThreadId}] "
                        : "";

                    string linea = string.IsNullOrEmpty(nivel)
                        ? mensaje
                        : $"[{timestamp}] {threadInfo}[{nivel}] {mensaje}";

                    logWriter.WriteLine(linea);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error escribiendo en archivo: {ex.Message}");
                }
            }
        }

        #endregion

        #region Gestión de Archivos

        private void CheckAndRotateLogFile()
        {
            if (logWriter == null || string.IsNullOrEmpty(currentLogFile))
                return;

            try
            {
                FileInfo fileInfo = new FileInfo(currentLogFile);
                long maxBytes = config.MaxFileSizeMB * 1024 * 1024;

                if (fileInfo.Exists && fileInfo.Length >= maxBytes)
                {
                    logWriter.WriteLine();
                    logWriter.WriteLine($"Archivo alcanzó tamaño máximo ({config.MaxFileSizeMB} MB). Rotando...");
                    CreateNewLogFile();
                    WriteLogHeader();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verificando tamaño de archivo: {ex.Message}");
            }
        }

        private void CleanOldLogFiles()
        {
            try
            {
                if (!Directory.Exists(config.LogDirectory))
                    return;

                // Obtener todos los archivos de log
                var logFiles = Directory.GetFiles(config.LogDirectory, $"{config.LogFilePrefix}_*.log")
                    .Select(f => new FileInfo(f))
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();

                // Eliminar archivos excedentes
                if (logFiles.Count > config.MaxLogFiles)
                {
                    var filesToDelete = logFiles.Skip(config.MaxLogFiles);
                    foreach (var file in filesToDelete)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"No se pudo eliminar {file.Name}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error limpiando archivos viejos: {ex.Message}");
            }
        }

        private void CloseLogFile()
        {
            if (logWriter != null)
            {
                try
                {
                    logWriter.WriteLine();
                    logWriter.WriteLine("".PadRight(80, '='));
                    logWriter.WriteLine($"Log cerrado: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    logWriter.WriteLine("".PadRight(80, '='));
                    logWriter.Flush();
                    logWriter.Close();
                    logWriter.Dispose();
                }
                catch { }
                finally
                {
                    logWriter = null;
                }
            }
        }

        #endregion

        #region Utilidades

        private string GetColorForLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return "Gray";
                case LogLevel.Info:
                    return "Lime";
                case LogLevel.Warning:
                    return "Yellow";
                case LogLevel.Error:
                    return "Orange";
                case LogLevel.Critical:
                    return "Red";
                default:
                    return "White";
            }
        }

        public void Limpiar()
        {
            if (textBox == null || textBox.IsDisposed) return;

            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new Action(Limpiar));
                return;
            }

            textBox.Clear();
            Info("Log limpiado");
        }

        /// <summary>
        /// Obtiene la ruta del archivo de log actual
        /// </summary>
        public string GetCurrentLogFilePath()
        {
            return currentLogFile;
        }

        /// <summary>
        /// Obtiene lista de todos los archivos de log
        /// </summary>
        public List<string> GetAllLogFiles()
        {
            try
            {
                if (!Directory.Exists(config.LogDirectory))
                    return new List<string>();

                return Directory.GetFiles(config.LogDirectory, $"{config.LogFilePrefix}_*.log")
                    .OrderByDescending(f => File.GetCreationTime(f))
                    .ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Abre el directorio de logs en el explorador
        /// </summary>
        public void OpenLogDirectory()
        {
            try
            {
                if (Directory.Exists(config.LogDirectory))
                {
                    System.Diagnostics.Process.Start("explorer.exe", config.LogDirectory);
                }
            }
            catch (Exception ex)
            {
                Error($"No se pudo abrir directorio de logs: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene el tamaño total de todos los archivos de log en MB
        /// </summary>
        public double GetTotalLogSizeMB()
        {
            try
            {
                if (!Directory.Exists(config.LogDirectory))
                    return 0;

                long totalBytes = Directory.GetFiles(config.LogDirectory, $"{config.LogFilePrefix}_*.log")
                    .Sum(f => new FileInfo(f).Length);

                return totalBytes / (1024.0 * 1024.0);
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    CloseLogFile();
                }
                disposed = true;
            }
        }

        ~Logger()
        {
            Dispose(false);
        }

        #endregion
    }
}