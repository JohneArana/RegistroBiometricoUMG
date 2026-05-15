using RegistroBiometricoUMG.Formularios;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using QRCoder;
using RegistroBiometricoUMG.BaseDatos;
using RegistroBiometricoUMG.Modelos;

namespace RegistroBiometricoUMG
{
    public partial class Form1 : Form
    {
        // ── Cámara ──────────────────────────────────────────────────────────
        private VideoCapture? _camara;
        private System.Windows.Forms.Timer _timerCamara = new();
        private bool _camaraActiva = false;
        private Bitmap? _fotoCapturada = null;

        public Form1()
        {
            InitializeComponent();
            DatabaseHelper.InicializarBaseDatos();
            ConfigurarTimer();
        }

        // ── Timer para mostrar video en el PictureBox ───────────────────────
        private void ConfigurarTimer()
        {
            _timerCamara.Interval = 33; // ~30 fps
            _timerCamara.Tick += TimerCamara_Tick;
        }

        private void TimerCamara_Tick(object? sender, EventArgs e)
        {
            if (_camara == null || !_camara.IsOpened()) return;

            using var frame = new Mat();
            _camara.Read(frame);
            if (frame.Empty()) return;

            picFoto.Image?.Dispose();
            picFoto.Image = BitmapConverter.ToBitmap(frame);
        }

        // ── Botón: Abrir / Cerrar cámara ───────────────────────────────────
        private void btnCamara_Click(object sender, EventArgs e)
        {
            if (!_camaraActiva)
            {
                _camara = new VideoCapture(0);
                if (!_camara.IsOpened())
                {
                    MessageBox.Show("No se pudo abrir la cámara.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _timerCamara.Start();
                _camaraActiva = true;
                btnCamara.Text = "⏹ Cerrar Cámara";
                btnCapturar.Enabled = true;
            }
            else
            {
                CerrarCamara();
                btnCamara.Text = "📷 Abrir Cámara";
                btnCapturar.Enabled = false;
            }
        }

        // ── Botón: Capturar foto ────────────────────────────────────────────
        private void btnCapturar_Click(object sender, EventArgs e)
        {
            if (picFoto.Image == null) return;

            _fotoCapturada = new Bitmap(picFoto.Image);
            CerrarCamara();
            picFoto.Image = _fotoCapturada;
            btnCamara.Text = "📷 Abrir Cámara";
            btnCapturar.Enabled = false;

            MessageBox.Show("✅ Foto capturada correctamente.", "Foto",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CerrarCamara()
        {
            _timerCamara.Stop();
            _camara?.Release();
            _camara?.Dispose();
            _camara = null;
            _camaraActiva = false;
        }

        // ── Botón: Registrar persona ────────────────────────────────────────
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(txtCarnet.Text))
            {
                MessageBox.Show("Por favor completa los campos obligatorios:\nNombre, Apellido, Correo y Carnet.",
                    "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!txtCorreo.Text.EndsWith("@miumg.edu.gt") &&
                !txtCorreo.Text.EndsWith("@umg.edu.gt"))
            {
                MessageBox.Show("El correo debe ser institucional de la UMG\n(@miumg.edu.gt o @umg.edu.gt).",
                    "Correo inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_fotoCapturada == null)
            {
                MessageBox.Show("Por favor captura una foto antes de registrar.",
                    "Foto requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Guardar foto en carpeta Carnets
            string carpetaFotos = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Carnets", "Fotos");
            Directory.CreateDirectory(carpetaFotos);

            string nombreFoto = $"{txtCarnet.Text.Replace("-", "_")}.png";
            string rutaFoto = Path.Combine(carpetaFotos, nombreFoto);
            _fotoCapturada.Save(rutaFoto, System.Drawing.Imaging.ImageFormat.Png);

            // Crear objeto persona
            var persona = new Persona
            {
                Nombre       = txtNombre.Text.Trim(),
                Apellido     = txtApellido.Text.Trim(),
                Telefono     = txtTelefono.Text.Trim(),
                Correo       = txtCorreo.Text.Trim(),
                TipoPersona  = cmbTipo.Text,
                Carrera      = txtCarrera.Text.Trim(),
                Seccion      = txtSeccion.Text.Trim(),
                NumeroCarnet = txtCarnet.Text.Trim(),
                RutaFoto     = rutaFoto,
                FechaRegistro = DateTime.Now
            };

            // Guardar en BD
            bool guardado = DatabaseHelper.GuardarPersona(persona);
            if (!guardado) return;

            // Generar carnet PDF
            GeneradorCarnet.Generar(persona);

            MessageBox.Show($"✅ Persona registrada exitosamente.\nCarnet: {persona.NumeroCarnet}",
                "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimpiarFormulario();
        }

        // ── Limpiar formulario ──────────────────────────────────────────────
        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtCarrera.Clear();
            txtSeccion.Clear();
            txtCarnet.Clear();
            cmbTipo.SelectedIndex = -1;
            picFoto.Image = null;
            _fotoCapturada = null;
        }

        // ── Cerrar cámara al cerrar la ventana ─────────────────────────────
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CerrarCamara();
            base.OnFormClosing(e);
        }
    }
}