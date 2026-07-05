using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Punto.Forms
{
    public partial class frmProductos : Form
    {
        public frmProductos()
        {
            InitializeComponent();

            this.Load -= frmProductos_Load;
            this.Load += frmProductos_Load;

            btnNuevo.Click -= btnNuevo_Click;
            btnNuevo.Click += btnNuevo_Click;

            btnEditar.Click -= btnEditar_Click;
            btnEditar.Click += btnEditar_Click;

            btnEliminar.Click -= btnEliminar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvProductos.CellClick -= dgvProductos_CellClick;
            dgvProductos.CellClick += dgvProductos_CellClick;

            txtBusqueda.TextChanged -= txtBusqueda_TextChanged;
            txtBusqueda.TextChanged += txtBusqueda_TextChanged;
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void CargarProductos()
        {
            try
            {
                Conexion conexion = new Conexion();

                using (MySqlConnection conn = conexion.AbrirConexion())
                {
                    string consulta = "SELECT producto_id AS ID, codigo AS Codigo, descripcion AS Nombre, precio AS Precio, stock AS Stock FROM productos";

                    MySqlDataAdapter adaptador = new MySqlDataAdapter(consulta, conn);
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);

                    dgvProductos.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            lblId.Text = "0";
            txtCodigo.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text == "" || txtNombre.Text == "" || txtPrecio.Text == "" || txtStock.Text == "")
            {
                MessageBox.Show("Llene todos los campos.");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio debe ser un número válido.");
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("El stock debe ser un número entero.");
                return;
            }

            try
            {
                Conexion conexion = new Conexion();

                using (MySqlConnection conn = conexion.AbrirConexion())
                {
                    string consulta = "INSERT INTO productos (codigo, descripcion, precio, stock) VALUES (@codigo, @descripcion, @precio, @stock)";

                    MySqlCommand comando = new MySqlCommand(consulta, conn);
                    comando.Parameters.AddWithValue("@codigo", txtCodigo.Text);
                    comando.Parameters.AddWithValue("@descripcion", txtNombre.Text);
                    comando.Parameters.AddWithValue("@precio", precio);
                    comando.Parameters.AddWithValue("@stock", stock);

                    comando.ExecuteNonQuery();

                    MessageBox.Show("Producto guardado correctamente.");
                    CargarProductos();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar producto: " + ex.Message);
            }
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                lblId.Text = fila.Cells["ID"].Value.ToString();
                txtCodigo.Text = fila.Cells["Codigo"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtPrecio.Text = fila.Cells["Precio"].Value.ToString();
                txtStock.Text = fila.Cells["Stock"].Value.ToString();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (lblId.Text == "0" || lblId.Text == "")
            {
                MessageBox.Show("Seleccione un producto para editar.");
                return;
            }

            if (txtCodigo.Text == "" || txtNombre.Text == "" || txtPrecio.Text == "" || txtStock.Text == "")
            {
                MessageBox.Show("Llene todos los campos.");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio debe ser un número válido.");
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("El stock debe ser un número entero.");
                return;
            }

            try
            {
                Conexion conexion = new Conexion();

                using (MySqlConnection conn = conexion.AbrirConexion())
                {
                    string consulta = "UPDATE productos SET codigo = @codigo, descripcion = @descripcion, precio = @precio, stock = @stock WHERE producto_id = @id";

                    MySqlCommand comando = new MySqlCommand(consulta, conn);
                    comando.Parameters.AddWithValue("@codigo", txtCodigo.Text);
                    comando.Parameters.AddWithValue("@descripcion", txtNombre.Text);
                    comando.Parameters.AddWithValue("@precio", precio);
                    comando.Parameters.AddWithValue("@stock", stock);
                    comando.Parameters.AddWithValue("@id", lblId.Text);

                    comando.ExecuteNonQuery();

                    MessageBox.Show("Producto actualizado correctamente.");
                    CargarProductos();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar producto: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (lblId.Text == "0" || lblId.Text == "")
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar este producto?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (respuesta == DialogResult.No)
            {
                return;
            }

            try
            {
                Conexion conexion = new Conexion();

                using (MySqlConnection conn = conexion.AbrirConexion())
                {
                    string consulta = "DELETE FROM productos WHERE producto_id = @id";

                    MySqlCommand comando = new MySqlCommand(consulta, conn);
                    comando.Parameters.AddWithValue("@id", lblId.Text);

                    comando.ExecuteNonQuery();

                    MessageBox.Show("Producto eliminado correctamente.");
                    CargarProductos();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar producto: " + ex.Message);
            }
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Conexion conexion = new Conexion();

                using (MySqlConnection conn = conexion.AbrirConexion())
                {
                    string consulta = "SELECT producto_id AS ID, codigo AS Codigo, descripcion AS Nombre, precio AS Precio, stock AS Stock FROM productos WHERE descripcion LIKE @busqueda OR codigo LIKE @busqueda";

                    MySqlCommand comando = new MySqlCommand(consulta, conn);
                    comando.Parameters.AddWithValue("@busqueda", "%" + txtBusqueda.Text + "%");

                    MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);

                    dgvProductos.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar producto: " + ex.Message);
            }
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvProductos_CellClick(sender, e);
        }
    }
}