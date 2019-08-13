using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TickeadoraTurnos.Models;

namespace TickeadoraTurnos
{
    public partial class frmImpresora : Form
    {
        TransitoEntities db = new TransitoEntities();
        public frmImpresora()
        {
            InitializeComponent();
            
        }

        private void frmImpresora_Load(object sender, EventArgs e)
        {
            
            CargarImpresoras(cboImpresora);

            Settings setting = new Settings();
            setting= db.Settings.Where(x => x.Clave == "IMPRESORA").FirstOrDefault();
            if (setting!=null)
            {
                if (!string.IsNullOrEmpty(setting.Texto1))
                    cboImpresora.Text = setting.Texto1;  
            }
             

        }


        private void CargarImpresoras(ComboBox combo)
        {
            PrinterSettings impresora = new PrinterSettings();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                impresora.PrinterName = PrinterSettings.InstalledPrinters[i].ToString();
                /*if (impresora.IsDefaultPrinter)
                    return PrinterSettings.InstalledPrinters[i].ToString();*/
                combo.Items.Add(PrinterSettings.InstalledPrinters[i].ToString());
            }
            //return String.Empty;

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                Settings settings = new Settings
                {
                    Clave = "IMPRESORA",
                    Texto1 = cboImpresora.Text
                };

                db.Settings.Add(settings);
                db.SaveChanges();

                MessageBox.Show("Se ha grabado correctamente"); 

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: "+ ex.Message);  
            }        



        }
    }
}
