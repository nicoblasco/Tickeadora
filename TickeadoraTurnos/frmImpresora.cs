using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
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
        private const string CONST_IMPRESORA = "IMPRESORA";
        private const string CONST_ANTEULTIMALINEA = "ANTEULTIMALINEA";
        private const string CONST_ULTIMALINEA = "ULTIMALINEA";
        private const string CONST_IMAGEN = "IMAGEN";
        TransitoEntities db = new TransitoEntities();
        public frmImpresora()
        {
            InitializeComponent();
            
        }

        private void frmImpresora_Load(object sender, EventArgs e)
        {
            
            CargarImpresoras(cboImpresora);
            

            List<Settings> settings = db.Settings.ToList();

            string strImpresora = settings.Where(x => x.Clave == CONST_IMPRESORA).FirstOrDefault().Texto1;
            string strAnteultimaLinea = settings.Where(x => x.Clave == CONST_ANTEULTIMALINEA).FirstOrDefault().Texto1;
            string strUltimaLinea = settings.Where(x => x.Clave == CONST_ULTIMALINEA).FirstOrDefault().Texto1;
            bool? boImagen = settings.Where(x => x.Clave == CONST_IMAGEN).FirstOrDefault().Logico1;

            if (settings != null)
            {
                if (!string.IsNullOrEmpty(strImpresora))
                    cboImpresora.Text = strImpresora;
                if (!string.IsNullOrEmpty(strAnteultimaLinea))
                    txtAnteultimaLinea.Text = strAnteultimaLinea;
                if (!string.IsNullOrEmpty(strUltimaLinea))
                    txtUltimaLinea.Text = strUltimaLinea;

                ckImagen.Checked = boImagen.Value;
                
                    
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
                //Verifico si existe la impresora, de lo contrario actualizo

                Settings settingsImpresoraaux = db.Settings.Where(x => x.Clave == CONST_IMPRESORA).FirstOrDefault();

                if (settingsImpresoraaux != null)
                {
                    settingsImpresoraaux.Texto1 = cboImpresora.Text;
                    db.Entry(settingsImpresoraaux).State = EntityState.Modified;
                }
                else
                {                
                    Settings settingsImpresora = new Settings
                    {
                        Clave = CONST_IMPRESORA,
                        Texto1 = cboImpresora.Text
                    };

                    db.Settings.Add(settingsImpresora);
                }


                
                //-ANTEULTIMALINEA
                Settings settingsAnteultimaLineaaux = db.Settings.Where(x => x.Clave == CONST_ANTEULTIMALINEA).FirstOrDefault();

                if (settingsAnteultimaLineaaux != null)
                {
                    settingsAnteultimaLineaaux.Texto1 = txtAnteultimaLinea.Text;
                    db.Entry(settingsAnteultimaLineaaux).State = EntityState.Modified;
                }
                else
                {
                    Settings settingsAnteultimaLinea = new Settings
                    {
                        Clave = CONST_ANTEULTIMALINEA,
                        Texto1 = txtAnteultimaLinea.Text
                    };

                    db.Settings.Add(settingsAnteultimaLinea);
                }

                //-ULTIMALINEA

                Settings settingsUltimaLineaaux = db.Settings.Where(x => x.Clave == CONST_ULTIMALINEA).FirstOrDefault();

                if (settingsUltimaLineaaux != null)
                {
                    settingsUltimaLineaaux.Texto1 = txtUltimaLinea.Text;
                    db.Entry(settingsUltimaLineaaux).State = EntityState.Modified;
                }
                else
                {
                    Settings settingsUltimaLinea = new Settings
                    {
                        Clave = CONST_ULTIMALINEA,
                        Texto1 = txtUltimaLinea.Text
                    };

                    db.Settings.Add(settingsUltimaLinea);
                }

                //-Imagen
                Settings settingsImagenaux = db.Settings.Where(x => x.Clave == CONST_IMAGEN).FirstOrDefault();

                if (settingsImagenaux != null)
                {
                    settingsImagenaux.Logico1 = ckImagen.Checked;
                    db.Entry(settingsImagenaux).State = EntityState.Modified;
                }
                else
                {
                    Settings settingsImagen = new Settings
                    {
                        Clave = CONST_IMAGEN,
                        Logico1 = ckImagen.Checked
                    };

                    db.Settings.Add(settingsImagen);
                }


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
