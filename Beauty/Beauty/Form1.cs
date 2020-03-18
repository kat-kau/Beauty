using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Beauty
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"data source=DESKTOP-FMPF1TD\SQLSERVER;initial catalog=test;integrated security=true;");
        double maxPage;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.prevForm = this.Name;
            f2.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand com = new SqlCommand("SELECT TOP(6) [Наименование_товара], [Цена], [Главное_изображение] FROM [product_b_import]", con);
            SqlDataReader dr = com.ExecuteReader();
            int xScale = 210;
            int elOnFirstRow = 0;
            int elOnSecondRow = 0;
            while (dr.Read())
            {
                UserControl1 temp = new UserControl1();
                panel1.Controls.Add(temp);

                temp.Location = new Point(xScale * elOnFirstRow, 0);
                elOnFirstRow += 1;
                if (elOnFirstRow > 3)
                {
                    temp.Location = new Point(xScale * elOnSecondRow, 210);
                    elOnSecondRow += 1;
                }
                temp.productImg.Image = Image.FromFile(dr[2].ToString());
                temp.label1.Text = dr[0].ToString();
                temp.label2.Text = dr[1].ToString() + " руб.";
            }
            dr.Close();

            com = new SqlCommand("SELECT COUNT(id) FROM [product_b_import]", con);
            dr = com.ExecuteReader();
            dr.Read();
            maxPage = Math.Ceiling(Double.Parse(dr[0].ToString()) / 6.0);
            label2.Text = "из " + maxPage.ToString();
            con.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Вы хотите найти все товары начинающиеся на: " + textBox1.Text);
        }

        private void openPage_Click(object sender, EventArgs e)
        {
            int newPage = Convert.ToInt32(currPage.Text);
            con.Open();
            SqlCommand com = new SqlCommand("SELECT [id], [Наименование_товара], [Цена], [Главное_изображение] FROM [product_b_import]", con);
            SqlDataReader dr = com.ExecuteReader();
            panel1.Controls.Clear();
            int xScale = 210;
            int elOnFirstRow = 0;
            int elOnSecondRow = 0;
            while (dr.Read())
            {
                if (Convert.ToInt32(dr[0]) > (6 * (newPage-1)))
                {
                    UserControl1 temp = new UserControl1();
                    panel1.Controls.Add(temp);

                    temp.Location = new Point(xScale * elOnFirstRow, 0);
                    elOnFirstRow += 1;
                    if (elOnFirstRow > 3)
                    {
                        temp.Location = new Point(xScale * elOnSecondRow, 210);
                        elOnSecondRow += 1;
                    }
                    temp.productImg.Image = Image.FromFile(dr[3].ToString());
                    temp.label1.Text = dr[1].ToString();
                    temp.label2.Text = dr[2].ToString() + " руб.";
                    if (elOnSecondRow == 3) break;
                }
            }
            dr.Close();
            con.Close();
        }
    }
}
