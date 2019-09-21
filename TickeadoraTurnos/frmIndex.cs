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
        private const string CONST_IMPRESORA = "IMPRESORA";
        private const string CONST_ANTEULTIMALINEA = "ANTEULTIMALINEA";
        private const string CONST_ULTIMALINEA = "ULTIMALINEA";
        private const string CONST_IMAGEN = "IMAGEN";
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
            CallCenterTurns callCenterTurn;
            int? NumeroSecuencia;
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            DateTime startDateTimeAux = DateTime.Today;
            int EstadoInicial = db.Status.Where(x => x.Orden == 1).Select(x => x.Id).FirstOrDefault();
            bool boTurnoVencido = false;

            //Con el DNI me fijo si existe la persona
            //Verifico con los datos que vienen del CallCenter
            List<Settings> setting = db.Settings.ToList();
            
            callCenterTurn = db.CallCenterTurns.Where(x => x.DNI == strDNI && x.FechaTurno >= startDateTime && x.FechaTurno <= endDateTime).FirstOrDefault();
            int? TiempoMaximoEspera = setting.Where(x => x.Clave == "TIEMPO_MAXIMO_ESPERA").FirstOrDefault()?.Numero1;
            int? DiasDeCorridoDeEspera = setting.Where(x => x.Clave == "DIAS_DE_CORRIDO_DE_ESPERA").FirstOrDefault()?.Numero1;
            if (DiasDeCorridoDeEspera!=null)
                 startDateTimeAux = DateTime.Today.AddDays(-1*(DiasDeCorridoDeEspera.Value));

            if (callCenterTurn == null)
            {
                //No tiene turno
                //Puede ser que se le haya asignada turno en dias anteriores
                    //Pregunto si desea que se le de turno igual

                if (DiasDeCorridoDeEspera!=null)
                {
                     callCenterTurn = db.CallCenterTurns.Where(x => x.DNI == strDNI && x.FechaTurno >= startDateTimeAux && x.FechaTurno <= endDateTime).FirstOrDefault();

                    if (callCenterTurn == null)
                    {
                        MessageBox.Show(strMsgNoTieneTurno);
                    }
                    else
                    {
                        string strMensajeVencido = "El Vecino tenía turno para el dia: " + callCenterTurn.FechaTurno.ToLongDateString() + ". ¿Desea imprimir el turno de todas formas?";
                        DialogResult dialogResult = MessageBox.Show(strMensajeVencido, "Turno Vencido", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            boTurnoVencido = true;
                            MessageBox.Show(strMsgTurnoVencido);
                        }
                        //else if (dialogResult == DialogResult.No)
                        //{
                            
                        //}
                    }

                }
                else
                {
                    MessageBox.Show(strMsgNoTieneTurno);
                }


                

            }

            //No utilizo el else porque puede ser cargue callCenterTurn en el medio
            if (callCenterTurn != null && boTurnoVencido==false)

            {
                //si esta asignado vuelvo a emitir el mismo turno
                if (callCenterTurn.Asignado)
                {
                    //Busco El turno
                    Turns turnoold = db.Turns.Where(x => x.CallCenterTurnId == callCenterTurn.Id).FirstOrDefault();
                    //Reeimprimo
                    Imprimir(turnoold);
                    txtDni.Text="";
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
                            string strMensajeVencido = "El Vecino tenía turno para la hora: " + callCenterTurn.FechaTurno.ToShortTimeString() + ". ¿Desea imprimir el turno de todas formas?";
                            DialogResult dialogResult = MessageBox.Show(strMensajeVencido, "Turno Vencido", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.No)
                            {
                                //Se le paso el turno
                                MessageBox.Show(strMsgTurnoVencido);
                                txtDni.Text = "";
                                return;
                            }

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
                txtDni.Text = "";
                Imprimir(turn);
                

            }

            txtDni.Text = "";
        }

        private void Imprimir(Turns turn)
        {
            List<Settings> settings = db.Settings.ToList();

           
            string strAnteultimaLinea = settings.Where(x => x.Clave == CONST_ANTEULTIMALINEA).FirstOrDefault().Texto1;
            string strUltimaLinea = settings.Where(x => x.Clave == CONST_ULTIMALINEA).FirstOrDefault().Texto1;
            bool? boImagen = settings.Where(x => x.Clave == CONST_IMAGEN).FirstOrDefault().Logico1;

            Tickets ticket = new Tickets();
            if (boImagen.Value) {
                var bmp = new Bitmap(TickeadoraTurnos.Properties.Resources.Fondo_color);
                ticket.HeaderImage = bmp;
            }


            if (!String.IsNullOrEmpty(strAnteultimaLinea))
                ticket.AddFooterLine(strAnteultimaLinea);
            if (!String.IsNullOrEmpty(strUltimaLinea))
                ticket.AddFooterLine(strUltimaLinea);


            ticket.AddContentLine("");
            ticket.AddContentLine("");
            ticket.AddContentLine("Vecino: " + turn.People.Apellido + ", " + turn.People.Nombre);
            ticket.AddContentLine("Fecha de Turno: " + turn.CallCenterTurns?.FechaTurno.ToString("dd/MM/yyyy"));
            ticket.AddContentLine("Hora de Turno: " + turn.CallCenterTurns?.FechaTurno.ToString("HH:mm") + "hs") ;
            ticket.AddContentLine("");
            ticket.AddContentLine("");
            ticket.AddTurnoLine("──────────────────────────────────");
            ticket.AddTurnoLine("");
            ticket.AddTurnoLine("TURNO: " + turn.Turno);
            ticket.AddTurnoLine("");
            ticket.AddTurnoLine("──────────────────────────────────");
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
