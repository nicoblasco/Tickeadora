using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TickeadoraTurnos.Models;
using TickeadoraTurnos.Properties;
using Settings = TickeadoraTurnos.Models.Settings;

namespace TickeadoraTurnos
{
    public partial class frmIndex : Form
    {
        TransitoEntities db = new TransitoEntities();
        string strImpresora;
        public frmIndex()
        {
            InitializeComponent();
            
        }

        private void frmIndex_Load(object sender, EventArgs e)
        {
            //Primero verifico que tenga seteada una impresora


            
            Settings setting = new Settings();
            setting = db.Settings.Where(x => x.Clave == "IMPRESORA").FirstOrDefault();
            if (setting != null)
            {
                if (!string.IsNullOrEmpty(setting.Texto1))
                    strImpresora = setting.Texto1;
                else
                    MessageBox.Show("Debe configurar la impresora");
            }
            else
            {
                MessageBox.Show("Debe configurar la impresora");
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //Variables
            string strMsgNoTieneTurno = "INEXISTENTE - No existe turno para el dia de hoy";
            string strMsgTurnoVencido = "TURNO VENCIDO - Su Turno a pasado el tiempo maximo de espera";
            string strDNI = txtDni.Text.Trim();  
            
            int? NumeroSecuencia;
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            int EstadoInicial = db.Status.Where(x => x.Orden == 1).Select(x => x.Id).FirstOrDefault();

            //Con el DNI me fijo si existe la persona
            //Verifico con los datos que vienen del CallCenter
            List<Settings> setting = db.Settings.ToList();
            
            CallCenterTurns callCenterTurn = db.CallCenterTurns.Where(x => x.DNI == strDNI && x.FechaTurno >= startDateTime && x.FechaTurno <= endDateTime).FirstOrDefault();
            int? TiempoMaximoEspera = setting.Where(x => x.Clave == "TIEMPO_MAXIMO_ESPERA").FirstOrDefault().Numero1;

            if (callCenterTurn == null)
            {
                //No tiene turno
                MessageBox.Show(strMsgNoTieneTurno);

            }
            else

            {
                //si esta asignado vuelvo a emitir el mismo turno
                if (callCenterTurn.Asignado)
                {
                    //Busco El turno
                    Turns turnoold = db.Turns.Where(x => x.CallCenterTurnId == callCenterTurn.Id).FirstOrDefault();
                    //Reeimprimo
                    Imprimir(turnoold);
                    return;
                }

                //Valido si no se paso de tiempo
                if (TiempoMaximoEspera != null)
                {
                    //Si es 0 no lo tomo en cuenta
                    if (TiempoMaximoEspera != 0)
                    {
                        DateTime fechaMax = callCenterTurn.FechaTurno.AddMinutes(TiempoMaximoEspera.Value);
                        if (DateTime.Now > fechaMax)
                        {
                            //Se le paso el turno
                            MessageBox.Show(strMsgTurnoVencido);
                        }
                    }
                }

                //Obtengo el tipo de turno
                //La relacion entre como viene y como esta en el sistema
                TypesLicenses typesLicense = db.TypesLicenses.Where(x => x.Referencia == callCenterTurn.TipoTramite).FirstOrDefault();

                if (typesLicense == null)
                {
                    //Le asigno un turno por defecto??
                    typesLicense = db.TypesLicenses.Where(x => x.Codigo == "OR").FirstOrDefault();

                }


                //Si paso todas las validaciones

                //Verifico si la persona existe, si no existe la doy de alta en el maestro de personas

                People person = db.People.Where(x => x.Dni == strDNI ).FirstOrDefault();
                if (person == null)
                {
                    person = new People
                    {
                        Nombre = callCenterTurn.Nombre,
                        Apellido = callCenterTurn.Apellido,
                        Dni = callCenterTurn.DNI,
                        Barrio = callCenterTurn.Barrio,
                        Tel_Celular = callCenterTurn.Tel_Celular,
                        Tel_Particular = callCenterTurn.Tel_Particular
                    };

                    //No le paso fecha de vencimiento de la licencia porque esta pasando mal el webservice

                    //if (!String.IsNullOrEmpty(callCenterTurn.Vencimiento_licencia))
                    //{
                    //    person.Vencimiento_licencia = Convert.ToDateTime(callCenterTurn.Vencimiento_licencia.Substring(0,9));
                    //}

                    db.People.Add(person);
                    db.SaveChanges();
                }

                //Asigno el turno
                callCenterTurn.Asignado = true;
                db.Entry(callCenterTurn).State = EntityState.Modified;



                //GENERO EL TURNO y Emito el ticket                   



                if (!db.Turns.Where(x => x.FechaIngreso >= startDateTime && x.FechaIngreso <= endDateTime).Any())
                    NumeroSecuencia = 1;
                else
                    NumeroSecuencia = db.Turns.Where(x => x.FechaIngreso >= startDateTime && x.FechaIngreso <= endDateTime).Max(x => x.Secuencia);

                if (NumeroSecuencia == null)
                    NumeroSecuencia = (typesLicense.NumeroInicial == 0) ? 1 : typesLicense.NumeroInicial;
                else
                    NumeroSecuencia = NumeroSecuencia + 1;


                Turns turn = new Turns
                {
                    FechaIngreso = DateTime.Now,
                    PersonID = person.Id,
                    TypesLicenseID = typesLicense.Id,
                    //Armo el codigo del dia
                    Turno = typesLicense.Codigo + NumeroSecuencia.Value.ToString("0000"),
                    Secuencia = NumeroSecuencia.Value,
                    FechaTurno = callCenterTurn.FechaTurno,
                    Enable = true,
                    CallCenterTurnId = callCenterTurn.Id

                };
                db.Turns.Add(turn);



                db.SaveChanges();


                //Obtengo el primer Sector del Workflow para el tipo de tramite
                List<Workflows> workflows = db.Workflows.Where(x => x.TypesLicenseID == typesLicense.Id).ToList();

                SectorWorkflows sectorWorkflow = db.SectorWorkflows.Where(x => x.Workflows.TypesLicenseID == typesLicense.Id && x.Orden == 1).FirstOrDefault();


                Trackings tracking = new Trackings
                {
                    SectorID = sectorWorkflow.SectorID,
                    TurnID = turn.Id,
                    FechaCreacion = DateTime.Now,
                    StatusID = EstadoInicial,
                    Enable = true,
                    Orden = 1
                };

                db.Trackings.Add(tracking);
                db.SaveChanges();



                //Imprimo 
                Imprimir(turn);

            }


        }

        private void Imprimir(Turns turn)
        {
            Tickets ticket = new Tickets();
            var bmp = new Bitmap(TickeadoraTurnos.Properties.Resources.Fondo_color);
             ticket.HeaderImage = bmp;
            ticket.AddHeaderLine("──────────────────────────────────");
            ticket.AddHeaderLine  ("TURNO: " + turn.Turno);                      
            ticket.AddHeaderLine("──────────────────────────────────");
            ticket.AddContentLine("");
            ticket.AddContentLine("");
            ticket.AddContentLine("DNI: " + turn.People.Dni);
            ticket.AddContentLine("Nombre: " + turn.People.Nombre);
            ticket.AddContentLine("Apellido: " + turn.People.Apellido);
            ticket.AddContentLine("Fecha: " + DateTime.Now.ToShortDateString());
            ticket.AddContentLine("");
            ticket.AddContentLine("");
            ticket.AddContentLine("");
            ticket.AddFooterLine("Dirección de Transito");
            ticket.AddFooterLine("Municipalidad de Florencio Varela");

            //if (!string.IsNullOrEmpty(objDatosImpresion.StrComercio))
            //    ticket.AddHeaderLine(objDatosImpresion.StrComercio);//Nombre del comercio

            //if (!string.IsNullOrEmpty(objDatosImpresion.StrDireccion) || !string.IsNullOrEmpty(objDatosImpresion.StrProvincia) || !string.IsNullOrEmpty(objDatosImpresion.StrLocalidad))
            //    ticket.AddHeaderLine("EXPEDIDO EN:");

            //if (!string.IsNullOrEmpty(objDatosImpresion.StrDireccion))
            //    ticket.AddHeaderLine(objDatosImpresion.StrDireccion);//Direccion

            //if (!string.IsNullOrEmpty(objDatosImpresion.StrProvincia) && !string.IsNullOrEmpty(objDatosImpresion.StrProvincia))
            //    ticket.AddHeaderLine(objDatosImpresion.StrProvincia + ", " + objDatosImpresion.StrLocalidad);//Provincia, Localidad
            //else if (string.IsNullOrEmpty(objDatosImpresion.StrProvincia) && !string.IsNullOrEmpty(objDatosImpresion.StrProvincia))
            //    ticket.AddHeaderLine(objDatosImpresion.StrLocalidad);//Provincia, Localidad
            //else if (!string.IsNullOrEmpty(objDatosImpresion.StrProvincia) && string.IsNullOrEmpty(objDatosImpresion.StrProvincia))
            //    ticket.AddHeaderLine(objDatosImpresion.StrProvincia);//Provincia, Localidad

            //if (!string.IsNullOrEmpty(objDatosImpresion.StrCodigoInterno))
            //    ticket.AddHeaderLine(objDatosImpresion.StrCodigoInterno);//Codigo Interno

            ////El metodo AddSubHeaderLine es lo mismo al de AddHeaderLine con la diferencia
            ////de que al final de cada linea agrega una linea punteada "=========="
            ////ticket.AddSubHeaderLine("Presupuesto # " + objVentas.IntCodigo);//Numero de caja y ticket
            ////ticket.AddSubHeaderLine("CAJA: " + Convert.ToString(objConfiguracion.IntNumeroCaja));//CAJA
            ////ticket.AddSubHeaderLine("Ha sido atendido por: " + cboVendedor.Text);//Empleado que lo atendio
            //ticket.AddSubHeaderLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

            ////El metodo AddItem requeire 3 parametros, el primero es cantidad, el segundo es la descripcion
            ////del producto y el tercero es el precio
            ////ticket.AddItem("1", "Articulo Prueba", "15.00");

            ////foreach (var c in objVentas.ListArticulosPorVenta)
            ////{
            ////    ticket.AddItem(Convert.ToString(c.IntCantidad), c.ObjArticulo.StrDescripcion, Convert.ToString(Redondeo(c.DoTotalConEfectivo)));
            ////}

            //////El metodo AddTotal requiere 2 parametros, la descripcion del total, y el precio
            ////ticket.AddTotal("SUBTOTAL: ", Convert.ToString(objVentas.DoNeto));
            ////ticket.AddTotal("DESCUENTO: ", Convert.ToString(objVentas.DoDescuento));
            ////ticket.AddTotal("TOTAL: ", Convert.ToString(objVentas.DoTotal));
            ////ticket.AddTotal("", ""); //Ponemos un total en blanco que sirve de espacio
            ////ticket.AddTotal("PAGO: ", Convert.ToString(objVentas.DoPago));
            ////ticket.AddTotal("DEBE: ", Convert.ToString(objVentas.DoDebe));
            ////ticket.AddTotal("", ""); //Ponemos un total en blanco que sirve de espacio
            ////ticket.AddTotal("RECIBIDO", "50.00");
            ////ticket.AddTotal("CAMBIO", "15.00");
            //ticket.AddTotal("", "");//Ponemos un total en blanco que sirve de espacio
            ////ticket.AddTotal("USTED AHORRO", "0.00");

            ////El metodo AddFooterLine funciona igual que la cabecera
            //ticket.AddFooterLine(objDatosImpresion.StrComentarioLinea1);
            //ticket.AddFooterLine(objDatosImpresion.StrComentarioLinea2);
            //ticket.AddFooterLine(objDatosImpresion.StrComertarioLinea3);

            ////Y por ultimo llamamos al metodo PrintTicket para imprimir el ticket, este metodo necesita un
            ////parametro de tipo string que debe de ser el nombre de la impresora.


            ////ticket.PrintTicket(objDatosImpresion.StrImpresora);
            ticket.PrintTicket(strImpresora);
        }

        private void txtDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            //// only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }
    }


}
