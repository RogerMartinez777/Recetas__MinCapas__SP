using Recetas__SP_MinCapas.Datos;
using Recetas__SP_MinCapas.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recetas__SP_MinCapas
{
    public partial class FrmAltaReceta : Form
    {
        private Receta nuevaReceta;
        private GestorBD gestor;

        public FrmAltaReceta()
        {
            InitializeComponent();
            nuevaReceta = new Receta();
            gestor = new GestorBD();
            siguienteReceta();
        }

        private void siguienteReceta()
        {
            lblNroReceta.Text = "Receta Nro: " + gestor.proximaReceta();
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void FrmAltaReceta_Load(object sender, EventArgs e)
        {
            cargarCombo();
            limpiarCampos();
        }

        private void cargarCombo()
        {
            DataTable tabla = gestor.ObtenerIngredientes();
            cboProducto.DataSource = tabla;
            cboProducto.ValueMember = tabla.Columns[0].ColumnName;
            cboProducto.DisplayMember = tabla.Columns[1].ColumnName;
            cboProducto.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void limpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtNombre.Focus();
            txtCheff.Text = string.Empty;
            cboTipo.Text = string.Empty;
            dgvDetalles.Rows.Clear();
            lblTotalDeIngredientes.Text = "Total de ingredientes: ";
            siguienteReceta();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboProducto.Text.Equals(string.Empty))
            {
                MessageBox.Show("No ha seleccionado un ingrediente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(nudCantidad.Text) || !int.TryParse(nudCantidad.Text, out _))
            {
                MessageBox.Show("No ha ingresado una cantidad válida", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                if (row.Cells["Ingrediente"].Value.ToString().Equals(cboProducto.Text))
                {
                    MessageBox.Show("No puede repetir los ingredientes", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }


            DataRowView item = (DataRowView)cboProducto.SelectedItem;
            int ingr = Convert.ToInt32(item.Row.ItemArray[0]);
            string nom = item.Row.ItemArray[1].ToString();

            Ingrediente i = new Ingrediente(ingr, nom);
            int cant = Convert.ToInt32(nudCantidad.Value);
            DetalleReceta detalle = new DetalleReceta(i, cant);

            nuevaReceta.AgregarReceta(detalle);

            dgvDetalles.Rows.Add(new object[] { ingr, nom, cant });

            TotalDeIngredientes();
        }

        private void TotalDeIngredientes()
        {
            lblTotalDeIngredientes.Text = "Total de ingredientes:" + dgvDetalles.Rows.Count;
        }
        private bool existe(string text)
        {
            foreach (DataGridViewRow fila in dgvDetalles.Rows)
            {
                if (fila.Cells["producto"].Value.Equals(text))
                    return true;
            }
            return false;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtCheff.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar un Cheff", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCheff.Focus();
                return;
            }

            if (dgvDetalles.Rows.Count < 3)
            {
                MessageBox.Show("Debe añadir 3 ingredientes como mínimo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboProducto.Focus();
                return;

            }
            if (txtNombre.Text == string.Empty)
            {
                MessageBox.Show("La receta debe contener un nombre", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNombre.Focus();
                return;
            }
            nuevaReceta.RecetaNro = gestor.proximaReceta();
            nuevaReceta.Nombre = txtNombre.Text;
            nuevaReceta.Chef = txtCheff.Text;
            nuevaReceta.TipoReceta = Convert.ToInt32(cboTipo.SelectedIndex);
            if (gestor.ejecutarSP(nuevaReceta))
            {
                MessageBox.Show("La receta ha sido guardada", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                limpiarCampos();

            }
            else
            {
                MessageBox.Show("La receta no ha sido guardada", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
