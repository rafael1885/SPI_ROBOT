using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.Collections.Generic;

namespace SPI_ROBOT
{
    public partial class Form1 : Form
    {
        private Timer timeX;

        public Form1()                                 
        {
            InitializeComponent();

            timeX = new Timer() { Interval = 1000 };     //instancia a classe Timer e seta o parametro Interval com 1s
        }

        /*************************************************************************************************************************/
       
        /*--- Variaveis do Sistema ---*/
     
        #region[Variaveis do Sistema]
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
     
        private string filename; public string Filename { get { return filename; } set { filename = value; } }

        private bool form_fechado; public bool Form_fechado { get { return form_fechado; } set { form_fechado = value; } }

        private string path; public string Path { get { return path; } set { path = value; } }                                  //aponta par o diretório principal

        private string part_number; public string Part_Number { get { return part_number; } set { part_number = value; } }      //registra o PartNumber da placa

        private string top_bot; public string Top_Bot { get { return top_bot; } set { top_bot = value; } }      //registra lado da placa

        private string serial_number; public string Serial_Number { get { return serial_number; } set { serial_number = value; } } //registra o SerialNumber da placa

        private string linha_spi; public string Linha_spi { get { return linha_spi; } set { linha_spi = value; } }          //registra o modelo da AOI 

        private string componentes_testados_spi; public string Componentes_Testados_SPI { get { return componentes_testados_spi; } set { componentes_testados_spi = value; } }

        private string falhas_spi; public string Falhas_SPI { get { return falhas_spi; } set { falhas_spi = value; } }

        private string pad; public string PAD { get { return pad; } set { pad = value; } }

        private string numero_pad; public string numero_PAD { get { return numero_pad; } set { numero_pad = value; } }

        private string placa_pass; public string Placa_pass { get { return placa_pass; } set { placa_pass = value; } }

        private string placa_rpass; public string Placa_Rpass { get { return placa_rpass; } set { placa_rpass = value; } }

        private string placa_repair; public string Placa_Repair { get { return placa_repair; } set { placa_repair = value; } }

        private string painel_placa; public string pinel_placa { get { return painel_placa; } set { painel_placa = value; } }

        private int log_id; public string Log_ID { get { return Log_ID; } set { Log_ID = value; } }

        //  private string rpass; public string Rpass { get { return rpass; } set { rpass = value; } }

        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_MINIMIZE = 6;
        private const int SW_HIDE = 0;
        private const int SW_RESTORE = 9;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        string logpath = @"C:\Users\rafaelpin\Desktop\logs";  //aponta para o diretório onde são gerados os logs do teste da AIO

        bool conectado;
        private string conn;
        private object console;
        #endregion

        /*************************************************************************************************************************/

        /*************************************************************************************************************************/

        /*--- Inicialização do Formulario ---*/
        #region
        private void Form1_Load(object sender, EventArgs e)
        {
           conectado = false;
            try
            {
                if (Directory.Exists(@"C:\EngTeste"))
                {
                    conectado = true;

                    if (File.Exists("operação.dll")) File.Delete("operação.dll"); // verifica se o diretorio existe
                    File.WriteAllText("operacao.dll", "modo online" + Environment.NewLine);
                    path = @"C:\EngTeste\";
                    lb_on_off.Font = new Font("Arial Black", 50);
                    lb_on_off.ForeColor = System.Drawing.Color.Green;
                    lb_on_off.Text = "ON";
                }
                else
                {
                    conectado = false;
                    if (File.Exists("operacao.dll")) File.Delete("operacao.dll"); //deleta o arquivo operacao.dll
                    File.WriteAllText("operacao.dll", "modo offline" + Environment.NewLine);
                    path = @"C:\EngTeste\";
                    lb_on_off.Font = new Font("Arial Black", 50);
                    lb_on_off.Text = "OFF";
                    lb_on_off.BackColor = lb_on_off.ForeColor = System.Drawing.Color.Transparent;
                    lb_on_off.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show("Operando Off-line, Informe ao time de suporte", "ALERTA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
             }
            catch (Exception)
            {

            }

        }
        #endregion
        /*************************************************************************************************************************/
        /*--- Inicia o Timer ---*/
        #region
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Ciclo();
            timer1.Start();
        }

        #endregion
        /**************************************************************************************************************************/
        /*************************************************************************************************************************/

        /*--- Método para abrir o prompt e inserir comandos ---*/
       #region
        public void DOS(string command)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c " + command;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
        #endregion

        /**************************************************************************************************************************/
        /*************************************************************************************************************************/

        /*---Monitora o surgimento de um novo log de falha ----*/
      #region
            private void Ciclo()
            {
                                                                                          // Verifica se o formulário VisualizarUsuario foi fechado
                if (form_fechado)
                {
                    this.Show();                                                         //exibe o formulário Form1
                    form_fechado = false;                                               //indica que o formulári o VisualizarUsuario foi fechado
                }
                #endregion
        /*************************************************************************************************************************/
        /*************************************************************************************************************************/
        /*--- Verifica a existencia de um log de teste gerado pela SPI---*/
      #region
                string[] arquivos = Directory.GetFiles(logpath, "*.txt");
            foreach (var log in arquivos)                                                   //este laço será executado para cada um dos arquivos .txt encontrados 
            {                                                                               // trata um único arquivo

                System.Threading.Thread.Sleep(1000);                                        //delay de 1 segundos

                DateTime hora_criacao = File.GetCreationTime(log);                          //captura o horári de criação do arquivo de log


                #endregion
                /*************************************************************************************************************************/
                /*************************************************************************************************************************/
                /*----Cria tabela BD ---*/
                #region            


                //    try
                //    {
                //    if (File.Exists(@"C: \Users\rafaelpin\source\Repos\SPI_ROBOT\bin\Debug\spi_robot.db"))
                //    {

                //    }

                //    else
                //    {
                //        SQLiteConnection.CreateFile(@"C:\Users\rafaelpin\source\Repos\SPI_ROBOT\bin\Debug\spi_robot.db");

                //        SQLiteConnection ligacao = new SQLiteConnection();
                //        ligacao.ConnectionString = @"Data Source = C: \Users\rafaelpin\source\Repos\SPI_ROBOT\bin\Debug\spi_robot.db; Version=3;";
                //        ligacao.Open();

                //        string query = "CREATE TABLE statistic" +     // Criar tabela no BD
                //        "(" +
                //        "Log_ID                           TEXT(20), " +
                //        "Serial_number                    TEXT(20), " +
                //        "linha                            TEXT(3), " +
                //        "Data_Hora                        NUMERIC,  " +
                //        "ID_RPass                         TEXT (6), " +
                //        "ID_Repair                        TEXT (6), " +
                //        "PAD                              TEXT (10), " +
                //        "Placa_Pass                       TEXT(6) " +
                //        ")";



                //        SQLiteCommand comando = new SQLiteCommand(query, ligacao); // Comuncicando com BD
                //        comando.ExecuteNonQuery();
                //    }


                //}




                //    catch (Exception)
                //    {
                //        MessageBox.Show("Tabela não Criada", "ALERTA!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //        System.Threading.Thread.CurrentThread.Abort();
                //        this.Close();
                //    }
                #endregion
                /*************************************************************************************************************************/

                /*************************************************************************************************************************/

                /*--- Extração das informações de cada log contido no arquivo----*/
                #region

                int numero_linhas = File.ReadLines(log).Count(); // fazndo leitura do log
                for (int i = 1; i < numero_linhas; i++)

                {
                    if (numero_linhas > 0)
                    {
                        log_id++;
                    }

                    linha_spi = File.ReadLines(log).Skip(i).Take(1).First();   // verifica a linha do log
                    if ((linha_spi.Contains("SPI_")))
                    {
                        string[] ls = linha_spi.Split(';');
                        linha_spi = ls[2];
                        
                    }
                    part_number = File.ReadLines(log).Skip(i).Take(1).First(); // verificar parte number
                    if (part_number.Contains("%"))
                    {
                        string[] pn = part_number.Split('%');
                        part_number = pn[0];

                    }
                    top_bot = File.ReadLines(log).Skip(i).Take(1).First();   // Verifica serial da placa 
                    if ((top_bot.Contains("%")) || (Top_Bot.Contains("%")))
                    {
                        string[] tb = top_bot.Split('%');
                        top_bot = tb[1];
                    }

                    serial_number = File.ReadLines(log).Skip(0).Take(1).First();   // Verifica serial da placa 
                    if ((serial_number.Contains("SN")) || (serial_number.Contains(":")))
                    {
                        string[] sn = serial_number.Split(':');
                        serial_number = sn[1];
                    }

                    else
                    {
                        serial_number = linha_spi + hora_criacao.ToString("yyyyMMdd_HHmmss");

                    }

                    painel_placa = File.ReadLines(log).Skip(i).Take(1).First(); // verifica numero referencia do painel da placa
                    string[] sp = painel_placa.Split(';');
                    painel_placa = sp[4];



                    numero_pad = File.ReadLines(log).Skip(i).Take(2).First();    // verifica  numero do pad 
                    if ((numero_pad.Contains("_")))

                    {
                        numero_pad = File.ReadLines(log).Skip(i).First();
                        string[] np = numero_pad.Split(';');
                        numero_pad = np[6];
                    }
                    else
                    {
                        numero_pad = "0";
                    }

                    placa_rpass = File.ReadLines(log).Skip(i).Take(1).First();  // numero de placa rpass
                    if ((placa_rpass.Contains("Rpass")))
                    {

                        placa_rpass = File.ReadLines(log).Skip(i).First();
                        string[] rpass = placa_rpass.Split(';');
                        placa_rpass = rpass[5];
                    }
                    else
                    {
                        placa_rpass = "0";
                    }

                    placa_repair = File.ReadLines(log).Skip(i).Take(1).First(); // numero de placa repair//
                    if ((placa_repair.Contains("Repair")))

                    {
                        placa_repair = File.ReadLines(log).Skip(i).First();
                        string[] repair = placa_repair.Split(';');
                        placa_repair = repair[5];

                    }

                    else

                    {
                        placa_repair = "0";
                    }

                    placa_pass = File.ReadLines(log).Skip(i).Take(1).First(); // numero de placa pass//
                    if ((placa_pass.Contains("Pass")))
                    {
                        placa_pass = File.ReadLines(log).Skip(i).First();
                        string[] pass = placa_pass.Split(';');
                        placa_pass = pass[5];
                    }

                           else
                            {
                                placa_pass = "0";
                            }








                    
                   
                  



                    #endregion

                    /**************************************************************************************************************************/

                    /*************************************************************************************************************************/

                    /*--- Conexão com BD----*/
                    #region


                    if (File.Exists(@"C:\Users\rafaelpin\source\repos\SPI_ROBOT\bin\Debug\spi_robot.db")) // verefica local onde esta BD

                       
                        {


                                SQLiteConnection ligacao = new SQLiteConnection();
                                ligacao.ConnectionString = @"Data Source = C: \Users\rafaelpin\source\repos\SPI_ROBOT\bin\Debug\spi_robot.db; Version=3;"; // faz a ligação do banco de dados
                                ligacao.Open();

                       
                                string query = " INSERT INTO statistic( Serial_Number, Part_Number  , linha , Top_Bot , Data_Hora , Rpass_ID , Repair_ID , PAD , Placa_pass,Log_ID) VALUES ( '" + serial_number+ "'||'"+ ("_") +  painel_placa + "' , '" + part_number + "', '" + linha_spi +"' , '" + top_bot + "' , '" + hora_criacao + "' , '" + Placa_Rpass + "' , '" + Placa_Repair + "' , '" + numero_pad +  "' , '" + placa_pass + "', '" + log_id + "'); ";  //Grava as infromçãos do log no BD

                      

                        SQLiteCommand comando = new SQLiteCommand(query, ligacao);
                                comando.ExecuteNonQuery();
                                comando.Dispose();
                                ligacao.Dispose();


                        }
                }
                    File.Delete(log);
                }

            }

        private void Int64(string log_id)
        {
            throw new NotImplementedException();
        }

        private void Int64(Func<string, string> isInterned)
        {
            throw new NotImplementedException();
        }

        private object idMaker()
        {
            throw new NotImplementedException();
        }
        #endregion
        /**************************************************************************************************************************/

        /*************************************************************************************************************************/

        /*--- Fechar a aplicação---*/
        #region
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
            {

                DOS(@"taskkill /IM Tri_spi_RS.exe /F");
                DOS(@"taskkill /IM SPI_RUN.exe /F");

                System.Threading.Thread.CurrentThread.Abort();
                this.Close();
            }

            private void lb_ROBOT_Click(object sender, EventArgs e)
            {

            }


        }
    }
    #endregion

