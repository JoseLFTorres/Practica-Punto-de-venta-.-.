using MySql.Data.MySqlClient;

namespace Punto.Forms
{
    public class Conexion
    {
        private string cadena = "Server=localhost;Database=PuntoDB;User ID=root;Password=;Port=3306;SslMode=Disabled;";

        public MySqlConnection AbrirConexion()
        {
            MySqlConnection conexion = new MySqlConnection(cadena);
            conexion.Open();
            return conexion;
        }
    }
}