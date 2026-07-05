using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Punto.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            if (txtUser.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Ingrese usuario y contraseña.");
                return;
            }

            try
            {
                Conexion conexion = new Conexion();

                using (MySqlConnection conn = conexion.AbrirConexion())
                {
                    string consulta = "SELECT nombre_completo FROM usuarios WHERE username = @usuario AND password = @password";

                    MySqlCommand comando = new MySqlCommand(consulta, conn);
                    comando.Parameters.AddWithValue("@usuario", txtUser.Text);
                    comando.Parameters.AddWithValue("@password", txtPassword.Text);

                    object resultado = comando.ExecuteScalar();

                    if (resultado != null)
                    {
                        MessageBox.Show("Bienvenido " + resultado.ToString());

                        frmPrincipal principal = new frmPrincipal();
                        this.Hide();
                        principal.Show();
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message);
            }
        }

        private void txtUser_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
