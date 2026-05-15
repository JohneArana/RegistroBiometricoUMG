using Microsoft.Data.Sqlite;
using RegistroBiometricoUMG.Modelos;

namespace RegistroBiometricoUMG.BaseDatos
{
    public class DatabaseHelper
    {
        // La base de datos se crea en la carpeta del ejecutable
        private static string _rutaDB = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "umg_registro.db");

        private static string CadenaConexion => $"Data Source={_rutaDB}";

        // ─── Inicializar / crear tabla si no existe ───────────────────────
        public static void InicializarBaseDatos()
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            conexion.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS Personas (
                    Id            INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre        TEXT    NOT NULL,
                    Apellido      TEXT    NOT NULL,
                    Telefono      TEXT,
                    Correo        TEXT    NOT NULL,
                    TipoPersona   TEXT,
                    Carrera       TEXT,
                    Seccion       TEXT,
                    RutaFoto      TEXT,
                    NumeroCarnet  TEXT    UNIQUE,
                    FechaRegistro TEXT
                );";

            using var cmd = new SqliteCommand(sql, conexion);
            cmd.ExecuteNonQuery();
        }

        // ─── Guardar una persona nueva ────────────────────────────────────
        public static bool GuardarPersona(Persona p)
        {
            try
            {
                using var conexion = new SqliteConnection(CadenaConexion);
                conexion.Open();

                string sql = @"
                    INSERT INTO Personas
                        (Nombre, Apellido, Telefono, Correo, TipoPersona,
                         Carrera, Seccion, RutaFoto, NumeroCarnet, FechaRegistro)
                    VALUES
                        (@Nombre, @Apellido, @Telefono, @Correo, @TipoPersona,
                         @Carrera, @Seccion, @RutaFoto, @NumeroCarnet, @FechaRegistro);";

                using var cmd = new SqliteCommand(sql, conexion);
                cmd.Parameters.AddWithValue("@Nombre",        p.Nombre);
                cmd.Parameters.AddWithValue("@Apellido",      p.Apellido);
                cmd.Parameters.AddWithValue("@Telefono",      p.Telefono);
                cmd.Parameters.AddWithValue("@Correo",        p.Correo);
                cmd.Parameters.AddWithValue("@TipoPersona",   p.TipoPersona);
                cmd.Parameters.AddWithValue("@Carrera",       p.Carrera);
                cmd.Parameters.AddWithValue("@Seccion",       p.Seccion);
                cmd.Parameters.AddWithValue("@RutaFoto",      p.RutaFoto);
                cmd.Parameters.AddWithValue("@NumeroCarnet",  p.NumeroCarnet);
                cmd.Parameters.AddWithValue("@FechaRegistro", p.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss"));

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar persona:\n{ex.Message}",
                    "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ─── Obtener todas las personas ───────────────────────────────────
        public static List<Persona> ObtenerTodasLasPersonas()
        {
            var lista = new List<Persona>();

            using var conexion = new SqliteConnection(CadenaConexion);
            conexion.Open();

            string sql = "SELECT * FROM Personas ORDER BY Id DESC;";
            using var cmd = new SqliteCommand(sql, conexion);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Persona
                {
                    Id           = reader.GetInt32(0),
                    Nombre       = reader.GetString(1),
                    Apellido     = reader.GetString(2),
                    Telefono     = reader.IsDBNull(3)  ? "" : reader.GetString(3),
                    Correo       = reader.GetString(4),
                    TipoPersona  = reader.IsDBNull(5)  ? "" : reader.GetString(5),
                    Carrera      = reader.IsDBNull(6)  ? "" : reader.GetString(6),
                    Seccion      = reader.IsDBNull(7)  ? "" : reader.GetString(7),
                    RutaFoto     = reader.IsDBNull(8)  ? "" : reader.GetString(8),
                    NumeroCarnet = reader.IsDBNull(9)  ? "" : reader.GetString(9),
                    FechaRegistro = DateTime.Parse(reader.GetString(10))
                });
            }

            return lista;
        }

        // ─── Generar número de carnet único ──────────────────────────────
        // Formato: UMG-YYYYMMDD-XXXX  (XXXX = número aleatorio)
        public static string GenerarNumeroCarnet()
        {
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            string random = new Random().Next(1000, 9999).ToString();
            return $"UMG-{fecha}-{random}";
        }
    }
}