using System;
using System.Windows.Forms;

namespace HFA_ICO
{
    /// <summary>
    /// Interfaz para controles que reportan progreso.
    /// NOTA: Solo debe existir UNA definición de esta interfaz en todo el proyecto.
    /// Si ves errores de ambigüedad, busca y elimina otras definiciones.
    /// </summary>
    public interface IProgressReporter
    {
        /// <summary>
        /// Obtiene o establece el valor actual del progreso
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// Obtiene o establece el valor máximo del progreso
        /// </summary>
        int Maximum { get; set; }
    }

    /// <summary>
    /// Adaptador para BotonGlifo que implementa IProgressReporter.
    /// Permite usar BotonGlifo con IconConverter de forma thread-safe.
    /// </summary>
    public class BotonGlifoProgressAdapter : IProgressReporter
    {
        private readonly BotonGlifo boton;

        public BotonGlifoProgressAdapter(BotonGlifo boton)
        {
            this.boton = boton ?? throw new ArgumentNullException(nameof(boton));
        }

        public int Value
        {
            get
            {
                if (boton.InvokeRequired)
                {
                    return (int)boton.Invoke(new Func<int>(() => boton.ProgressValue));
                }
                return boton.ProgressValue;
            }
            set
            {
                if (boton.InvokeRequired)
                {
                    boton.Invoke(new Action(() =>
                    {
                        boton.ProgressValue = value;
                    }));
                }
                else
                {
                    boton.ProgressValue = value;
                }
            }
        }

        public int Maximum
        {
            get
            {
                if (boton.InvokeRequired)
                {
                    return (int)boton.Invoke(new Func<int>(() => boton.ProgressMaximum));
                }
                return boton.ProgressMaximum;
            }
            set
            {
                if (boton.InvokeRequired)
                {
                    boton.Invoke(new Action(() =>
                    {
                        boton.ProgressMaximum = value;
                    }));
                }
                else
                {
                    boton.ProgressMaximum = value;
                }
            }
        }
    }

    /// <summary>
    /// Adaptador para ProgressBar estándar que implementa IProgressReporter.
    /// Permite usar ProgressBar con IconConverter de forma thread-safe.
    /// </summary>
    public class ProgressBarAdapter : IProgressReporter
    {
        private readonly ProgressBar progressBar;

        public ProgressBarAdapter(ProgressBar progressBar)
        {
            this.progressBar = progressBar ?? throw new ArgumentNullException(nameof(progressBar));
        }

        public int Value
        {
            get
            {
                if (progressBar.InvokeRequired)
                {
                    return (int)progressBar.Invoke(new Func<int>(() => progressBar.Value));
                }
                return progressBar.Value;
            }
            set
            {
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() =>
                    {
                        progressBar.Value = Math.Max(0, Math.Min(progressBar.Maximum, value));
                    }));
                }
                else
                {
                    progressBar.Value = Math.Max(0, Math.Min(progressBar.Maximum, value));
                }
            }
        }

        public int Maximum
        {
            get
            {
                if (progressBar.InvokeRequired)
                {
                    return (int)progressBar.Invoke(new Func<int>(() => progressBar.Maximum));
                }
                return progressBar.Maximum;
            }
            set
            {
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() =>
                    {
                        progressBar.Maximum = Math.Max(1, value);
                    }));
                }
                else
                {
                    progressBar.Maximum = Math.Max(1, value);
                }
            }
        }
    }

    /// <summary>
    /// Extensiones para facilitar el uso de adaptadores
    /// </summary>
    public static class ProgressReporterExtensions
    {
        /// <summary>
        /// Convierte un BotonGlifo en IProgressReporter.
        /// Uso: var adapter = miBoton.AsProgressReporter();
        /// </summary>
        public static IProgressReporter AsProgressReporter(this BotonGlifo boton)
        {
            return new BotonGlifoProgressAdapter(boton);
        }

        /// <summary>
        /// Convierte un ProgressBar en IProgressReporter.
        /// Uso: var adapter = miProgressBar.AsProgressReporter();
        /// </summary>
        public static IProgressReporter AsProgressReporter(this ProgressBar progressBar)
        {
            return new ProgressBarAdapter(progressBar);
        }
    }
}