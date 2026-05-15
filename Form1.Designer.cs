namespace RegistroBiometricoUMG
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo, lblNombre, lblApellido, lblTelefono,
                      lblCorreo, lblTipo, lblCarrera, lblSeccion,
                      lblCarnet, lblFoto;

        private TextBox txtNombre, txtApellido, txtTelefono,
                        txtCorreo, txtCarrera, txtSeccion, txtCarnet;

        private ComboBox cmbTipo;
        private PictureBox picFoto;
        private Button btnCamara, btnCapturar, btnRegistrar, btnLimpiar;
        private Panel panelIzquierdo, panelDerecho, panelTitulo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo    = new Label();
            lblNombre    = new Label();
            lblApellido  = new Label();
            lblTelefono  = new Label();
            lblCorreo    = new Label();
            lblTipo      = new Label();
            lblCarrera   = new Label();
            lblSeccion   = new Label();
            lblCarnet    = new Label();
            lblFoto      = new Label();

            txtNombre    = new TextBox();
            txtApellido  = new TextBox();
            txtTelefono  = new TextBox();
            txtCorreo    = new TextBox();
            txtCarrera   = new TextBox();
            txtSeccion   = new TextBox();
            txtCarnet    = new TextBox();

            cmbTipo      = new ComboBox();
            picFoto      = new PictureBox();

            btnCamara    = new Button();
            btnCapturar  = new Button();
            btnRegistrar = new Button();
            btnLimpiar   = new Button();

            panelTitulo    = new Panel();
            panelIzquierdo = new Panel();
            panelDerecho   = new Panel();

            // ── FORM ────────────────────────────────────────────────────────
            Text            = "UMG — Registro Biométrico";
            Size            = new System.Drawing.Size(980, 540);
            MinimumSize     = new System.Drawing.Size(980, 540);
            StartPosition   = FormStartPosition.CenterScreen;
            BackColor       = System.Drawing.Color.FromArgb(240, 242, 245);
            Font            = new System.Drawing.Font("Segoe UI", 9.5f);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox     = false;

            // ── PANEL TÍTULO ────────────────────────────────────────────────
            panelTitulo.Dock      = DockStyle.Top;
            panelTitulo.Height    = 55;
            panelTitulo.BackColor = System.Drawing.Color.FromArgb(0, 51, 102);

            lblTitulo.Text      = "🎓  Registro Biométrico — Universidad Mariano Gálvez";
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Font      = new System.Drawing.Font("Segoe UI", 13f, System.Drawing.FontStyle.Bold);
            lblTitulo.AutoSize  = false;
            lblTitulo.Dock      = DockStyle.Fill;
            lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            panelTitulo.Controls.Add(lblTitulo);

            // ── PANEL IZQUIERDO ─────────────────────────────────────────────
            panelIzquierdo.Location  = new System.Drawing.Point(10, 65);
            panelIzquierdo.Size      = new System.Drawing.Size(590, 455);
            panelIzquierdo.BackColor = System.Drawing.Color.White;

            // Columna A: x=15  Columna B: x=310
            // Fila 1: Nombre | Apellido
            Lbl(panelIzquierdo, lblNombre,  "Nombre *",      15,  15);
            Txt(panelIzquierdo, txtNombre,               15,  35, 270);
            Lbl(panelIzquierdo, lblApellido,"Apellido *",    305, 15);
            Txt(panelIzquierdo, txtApellido,             305, 35, 270);

            // Fila 2: Teléfono | Carnet
            Lbl(panelIzquierdo, lblTelefono,"Teléfono",      15,  85);
            Txt(panelIzquierdo, txtTelefono,             15,  105, 270);
            Lbl(panelIzquierdo, lblCarnet,  "No. Carnet *",  305, 85);
            Txt(panelIzquierdo, txtCarnet,               305, 105, 270);

            // Fila 3: Correo (ancho completo)
            Lbl(panelIzquierdo, lblCorreo,  "Correo UMG *",  15, 155);
            Txt(panelIzquierdo, txtCorreo,               15, 175, 560);

            // Fila 4: Tipo persona (ComboBox ancho completo)
            Lbl(panelIzquierdo, lblTipo, "Tipo de Persona *", 15, 220);
            cmbTipo.Location      = new System.Drawing.Point(15, 240);
            cmbTipo.Size          = new System.Drawing.Size(560, 28);
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.FlatStyle     = FlatStyle.Flat;
            cmbTipo.Items.AddRange(new object[]
                { "Estudiante", "Catedrático", "Administrativo", "Operativo" });
            panelIzquierdo.Controls.Add(cmbTipo);

            // Fila 5: Carrera | Sección
            Lbl(panelIzquierdo, lblCarrera, "Carrera",       15,  285);
            Txt(panelIzquierdo, txtCarrera,              15,  305, 390);
            Lbl(panelIzquierdo, lblSeccion, "Sección",       420, 285);
            Txt(panelIzquierdo, txtSeccion,              420, 305, 155);

            // Botones
            btnRegistrar.Text      = "✅  Registrar";
            btnRegistrar.Location  = new System.Drawing.Point(15, 360);
            btnRegistrar.Size      = new System.Drawing.Size(270, 42);
            btnRegistrar.BackColor = System.Drawing.Color.FromArgb(0, 153, 76);
            btnRegistrar.ForeColor = System.Drawing.Color.White;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.Font      = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
            btnRegistrar.Cursor    = Cursors.Hand;
            btnRegistrar.Click    += btnRegistrar_Click;
            panelIzquierdo.Controls.Add(btnRegistrar);

            btnLimpiar.Text      = "🗑  Limpiar";
            btnLimpiar.Location  = new System.Drawing.Point(305, 360);
            btnLimpiar.Size      = new System.Drawing.Size(270, 42);
            btnLimpiar.BackColor = System.Drawing.Color.FromArgb(200, 50, 50);
            btnLimpiar.ForeColor = System.Drawing.Color.White;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font      = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
            btnLimpiar.Cursor    = Cursors.Hand;
            btnLimpiar.Click    += (s, e) => LimpiarFormulario();
            panelIzquierdo.Controls.Add(btnLimpiar);

            var lblNota = new Label
            {
                Text      = "* Campos obligatorios. El correo debe ser institucional UMG (@miumg.edu.gt o @umg.edu.gt)",
                Location  = new System.Drawing.Point(15, 415),
                Size      = new System.Drawing.Size(560, 25),
                ForeColor = System.Drawing.Color.Gray,
                Font      = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Italic)
            };
            panelIzquierdo.Controls.Add(lblNota);

            // ── PANEL DERECHO (cámara) ──────────────────────────────────────
            panelDerecho.Location  = new System.Drawing.Point(610, 65);
            panelDerecho.Size      = new System.Drawing.Size(345, 455);
            panelDerecho.BackColor = System.Drawing.Color.White;

            lblFoto.Text      = "📷  Fotografía";
            lblFoto.Location  = new System.Drawing.Point(15, 15);
            lblFoto.AutoSize  = false;
            lblFoto.Size      = new System.Drawing.Size(315, 24);
            lblFoto.Font      = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
            lblFoto.ForeColor = System.Drawing.Color.FromArgb(0, 51, 102);
            panelDerecho.Controls.Add(lblFoto);

            picFoto.Location    = new System.Drawing.Point(15, 45);
            picFoto.Size        = new System.Drawing.Size(315, 270);
            picFoto.BorderStyle = BorderStyle.FixedSingle;
            picFoto.SizeMode    = PictureBoxSizeMode.Zoom;
            picFoto.BackColor   = System.Drawing.Color.FromArgb(220, 225, 235);
            panelDerecho.Controls.Add(picFoto);

            btnCamara.Text      = "📷  Abrir Cámara";
            btnCamara.Location  = new System.Drawing.Point(15, 330);
            btnCamara.Size      = new System.Drawing.Size(315, 42);
            btnCamara.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
            btnCamara.ForeColor = System.Drawing.Color.White;
            btnCamara.FlatStyle = FlatStyle.Flat;
            btnCamara.Font      = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
            btnCamara.Cursor    = Cursors.Hand;
            btnCamara.Click    += btnCamara_Click;
            panelDerecho.Controls.Add(btnCamara);

            btnCapturar.Text      = "📸  Capturar Foto";
            btnCapturar.Location  = new System.Drawing.Point(15, 385);
            btnCapturar.Size      = new System.Drawing.Size(315, 42);
            btnCapturar.BackColor = System.Drawing.Color.FromArgb(255, 140, 0);
            btnCapturar.ForeColor = System.Drawing.Color.White;
            btnCapturar.FlatStyle = FlatStyle.Flat;
            btnCapturar.Font      = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
            btnCapturar.Cursor    = Cursors.Hand;
            btnCapturar.Enabled   = false;
            btnCapturar.Click    += btnCapturar_Click;
            panelDerecho.Controls.Add(btnCapturar);

            // ── Agregar todo al Form ────────────────────────────────────────
            Controls.Add(panelTitulo);
            Controls.Add(panelIzquierdo);
            Controls.Add(panelDerecho);
        }

        // ── Helpers ─────────────────────────────────────────────────────────
        private void Lbl(Panel p, Label l, string texto, int x, int y)
        {
            l.Text      = texto;
            l.Location  = new System.Drawing.Point(x, y);
            l.AutoSize  = true;
            l.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            p.Controls.Add(l);
        }

        private void Txt(Panel p, TextBox t, int x, int y, int w)
        {
            t.Location    = new System.Drawing.Point(x, y);
            t.Size        = new System.Drawing.Size(w, 28);
            t.BorderStyle = BorderStyle.FixedSingle;
            t.Font        = new System.Drawing.Font("Segoe UI", 9.5f);
            p.Controls.Add(t);
        }
    }
}