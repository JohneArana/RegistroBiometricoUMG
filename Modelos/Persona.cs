namespace RegistroBiometricoUMG.Modelos
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string TipoPersona { get; set; } = "";
        public string Carrera { get; set; } = "";
        public string Seccion { get; set; } = "";
        public string RutaFoto { get; set; } = "";
        public string NumeroCarnet { get; set; } = "";
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}