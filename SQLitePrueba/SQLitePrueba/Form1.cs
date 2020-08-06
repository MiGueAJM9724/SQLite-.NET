using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace SQLitePrueba
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private SQLiteConnection SQL_Cn;
        private SQLiteCommand SQL_Command;
        private SQLiteDataAdapter DB;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();

        private void SetConnection()
        {
            SQL_Cn = new SQLiteConnection("Data Source = prueba.db; Version = 3; New = False; Compress = True;");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void ExecuteQuery(string Query)
        {
            SetConnection();
            SQL_Cn.Open();
            SQL_Command = SQL_Cn.CreateCommand();
            SQL_Command.CommandText = Query;
            SQL_Command.ExecuteNonQuery();
            SQL_Cn.Close();

        }

        private void LoadData()
        {
            SetConnection();
            SQL_Cn.Open();
            SQL_Command = SQL_Cn.CreateCommand();
            string Query = "SELECT * FROM Producto";
            DB = new SQLiteDataAdapter(Query, SQL_Cn);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
            Grid.DataSource = DT;
            SQL_Cn.Close();

        }
        // button Save (add)
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPrice.Text == "")
            {
                MessageBox.Show("Llene el formulario");
            }
            else
            {
                string Query = "INSERT INTO Producto(nombre, precio) VALUES('" + txtName.Text + "', " + txtPrice.Text + ")";
                ExecuteQuery(Query);
                LoadData();
            }
                
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = Grid.SelectedRows[0].Cells[0].Value.ToString();
            txtName.Text = Grid.SelectedRows[0].Cells[1].Value.ToString();
            txtPrice.Text = Grid.SelectedRows[0].Cells[2].Value.ToString();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPrice.Text == "")
            {
                MessageBox.Show("Llene el formulario");
            }
            else
            {
                string Query = "UPDATE Producto SET nombre ='" + txtName.Text + "', precio = " + txtPrice.Text + " WHERE id_producto = " + txtID.Text + "";
                ExecuteQuery(Query);
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Inserte un ID");
            } 
            else
            {
                string Query = "DELETE FROM Producto WHERE id_producto = " + txtID.Text + "";
                ExecuteQuery(Query);
                LoadData();
                txtID.Text = "";
                txtName.Text = "";
                txtPrice.Text = "";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(txtID.Text == "")
            {
                MessageBox.Show("Ingrese el ID a buscar");
            }
            else
            {
                SetConnection();
                SQL_Cn.Open();
                string Query = "SELECT  nombre, precio FROM Producto WHERE id_producto = " + txtID.Text + "";
                SQLiteCommand cmd = new SQLiteCommand(Query, SQL_Cn);
                SQLiteDataReader lector = cmd.ExecuteReader();
                if (lector.Read())
                {
                    txtName.Text = lector[0].ToString();
                    txtPrice.Text = lector[1].ToString();
                }
                else MessageBox.Show("Error al ejecutar consulta");
            }
        }
    }
}
