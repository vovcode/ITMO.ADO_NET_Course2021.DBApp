using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace ITMO.ADO_NET_Course2021.DBApp
{
    public partial class Form1 : Form
    {
        Customers model = new Customers();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        void Clear() 
        {
            tbFirstName.Text = tbLastName.Text = tbEmail.Text = tbPhone.Text = tbCity.Text = tbAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            dgvCustomers.AutoGenerateColumns = false;//тут
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = tbFirstName.Text.Trim();
            model.LastName = tbLastName.Text.Trim();
            model.Email = tbEmail.Text.Trim();
            model.Phone = tbPhone.Text.Trim();
            model.City = tbCity.Text.Trim();
            model.Address = tbAddress.Text.Trim();
            using (efdbCustomersEntities db = new efdbCustomersEntities()) 
            {
                if (model.CustomerID == 0)//Добавление записи
                {
                    db.Customers.Add(model);
                    MessageBox.Show("Entry submitted successfully");
                }
                else //Обновление записи
                { 
                    db.Entry(model).State = EntityState.Modified;
                    MessageBox.Show("Entry redacted successfully");
                }
                db.SaveChanges();
            }
            Clear();
            PopulateDataGridView();
        }
        void PopulateDataGridView()
        {
            
            using (efdbCustomersEntities db = new efdbCustomersEntities()) 
            {
                dgvCustomers.DataSource = db.Customers.ToList<Customers>();
            }

        }

        private void dgvCustomers_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["CustomerID"].Value);
                using (efdbCustomersEntities db = new efdbCustomersEntities()) 
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    tbFirstName.Text = model.FirstName;
                    tbLastName.Text = model.LastName;
                    tbEmail.Text = model.Email;
                    tbPhone.Text = model.Phone;
                    tbCity.Text = model.City;
                    tbAddress.Text = model.Address;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this entry?", "Delete confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                using (efdbCustomersEntities db = new efdbCustomersEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                    {
                        db.Customers.Attach(model);
                        db.Customers.Remove(model);
                        db.SaveChanges();
                        PopulateDataGridView();
                        Clear();
                        MessageBox.Show("Entry deleted successfully");
                    }
                }
        }
    }
}
