using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;

namespace SPI_ROBOT
{
    public partial class Form1 : Form
    {
        private Timer timeX;

        public Form1()                                  //inicialização do programa
        {
            InitializeComponent();

            timeX = new Timer() { Interval = 1000 };     //instancia a classe Timer e seta o parametro Interval com 1s


            //string[] arquivos = Directory.GetFiles(logpath, "*.txt");
            //foreach (var log in arquivos)
            //{
            //    File.Delete(log);
            //}

        }

        /*************************************************************************************************************************/
        /*--- Variáveis do sistema---*/

        #region[Variaveis do Sistema]
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);




        private string filename; public string Filename { get { return filename; } set { filename = value; } }

        private bool form_fechado; public bool Form_fechado { get { return form_fechado; } set { form_fechado = value; } }

        private string path; public string Path { get { return path; } set { path = value; } }                                  //aponta par o diretório principal

        private string part_number; public string Part_Number { get { return part_number; } set { part_number = value; } }      //registra o PartNumber da placa

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




        //  private string rpass; public string Rpass { get { return rpass; } set { rpass = value; } }





        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_MINIMIZE = 6;
        private const int SW_HIDE = 0;
        private const int SW_RESTORE = 9;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);



        string logpath = @"C:\Users\rafaelpin\Desktop\spi-sfc";            //aponta para o diretório onde são gerados os logs do teste da AIO

        bool conectado;
        #endregion

        /*************************************************************************************************************************/
        /*--- Inicialização do formulário ---*/

        private void Form1_Load(object sender, EventArgs e)
        {
            /*------ Verificar se é possivel realizer a coneção com servidor----*/
            conectado = false;
            try
            {
                if (Directory.Exists(@"C:\EngTeste"))
                {
                    conectado = true;

                    if (File.Exists("operação.dll")) File.Delete("operação.dll");
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



        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Ciclo();
            timer1.Start();
        }
        /*************************************************************************************************************************/
        /*--- Método para abrir o prompt e inserir comandos ---*/

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
        /*************************************************************************************************************************/
        /*---Monitora o surgimento de um novo log de falha ---*/
        private void Ciclo()
        {
            /*--- Verifica se o formulário VisualizarUsuario foi fechado ---*/
            if (form_fechado)
            {
                this.Show();                                                         //exibe o formulário Form1
                form_fechado = false;                                           //indica que o formulári o VisualizarUsuario foi fechado
            }
            /*--- ----------------------------------------------------------------------------------------------------------- ---*/


            /*--- Verifica a existencia de um log de teste gerado pela SPI---*/

            string[] arquivos = Directory.GetFiles(logpath, "*.txt");


            #region foreach (var log in arquivos)                                                 
            foreach (var log in arquivos)                                                   //este laço será executado para cada um dos arquivos .txt encontrados 
            {   /*** trata um único arquivo ***/

                System.Threading.Thread.Sleep(1000);                                        //delay de 1 segundos

                DateTime hora_criacao = File.GetCreationTime(log);                          //captura o horári de criação do arquivo de log



                /*--- ----------------------------------------------------------------------------------------------------------- ---*/


                /*--- Extração das informações de cada log contido no arquivo ---*/


                int falhas_maquina = 0;

                /*--- Realiza a leitura do cabeçalho do Log ---*/
                #region

                int numero_linhas = File.ReadLines(log).Count();

                for (int i = 1; i < numero_linhas; i++)  /* faz leitura das linhas do log */
                {
                    linha_spi = File.ReadLines(log).Skip(i).Take(1).First();   // verifica a linha da placa
                    if ((linha_spi.Contains("SPI_")))
                    {
                        string[] ls = linha_spi.Split(';');
                        linha_spi = ls[2];

                    }
                    part_number = File.ReadLines(log).Skip(i).Take(1).First();
                    if (part_number.Contains("%"))
                    {
                        string[] pn = part_number.Split('%');
                        part_number = pn[0];

                    }
                    serial_number = File.ReadLines(log).Skip(0).Take(1).First();
                    if ((serial_number.Contains("SN")) || (serial_number.Contains(":"))) /* Verifica serial da placa */
                    {
                        string[] sn = serial_number.Split(':');
                        serial_number = sn[1];
                    }

                    else
                    {
                        serial_number = linha_spi + hora_criacao.ToString("yyyyMMdd_HHmmss");

                    }

                    painel_placa = File.ReadLines(log).Skip(i).Take(1).First(); /* verifica numero referencia do painel da placa */
                    string[] sp = painel_placa.Split(';');
                    painel_placa = sp[4];


                    #endregion
                    /*--- Captura as informações das falhas presentes no Log ---*/

                    #region if ( linha.Contains("PAD")


                    numero_pad = File.ReadLines(log).Skip(i).Take(2).First();     /* verifica  numero do pad */
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

                    placa_rpass = File.ReadLines(log).Skip(i).Take(1).First();  // numero de placa rpass//
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

                    placa_pass = File.ReadLines(log).Skip(i).Take(1).First();
                    if ((placa_pass.Contains("Pass")))
                    {
                        placa_pass = File.ReadLines(log).Skip(i).First();  // numero de placa pass//
                        string[] pass = placa_pass.Split(';');
                        placa_pass = pass[5];
                    }
                    else
                    {
                        placa_pass = "0";
                    }




                    /*--- Se não existir o arquivo de log para a SkyNet cria-o inserindo um cabeçalho na primeira linha---*/

                    try
                    {
                        if (!Directory.Exists(path + @"SPI_DB\Failures\" + linha_spi + @"\" + part_number + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\"))
                        {
                            Directory.CreateDirectory(path + @"SPI_DB\Failures\" + linha_spi + @"\" + part_number + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\");
                        }
                    }
                    catch (Exception)
                    {
                        System.Threading.Thread.Sleep(2000);  //delay

                        try
                        {
                            if (!Directory.Exists(path + @"SPI_DB\Failures\" + linha_spi + @"\" + serial_number + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + part_number + @"\"))
                            {
                                Directory.CreateDirectory(path + @"SPI_DB\Failures\" + linha_spi + @"\" + serial_number + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + part_number + @"\");
                            }
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Line - 860." + "CreateDirectory " + path + "SPI_DB\\Failures\\" + linha_spi + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + part_number + "\\", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            File.Delete(log);

                            System.Threading.Thread.CurrentThread.Abort();
                            this.Close();
                        }
                    }

                    try
                    {
                        if (!File.Exists(path + @"SPI_DB\Failures\" + linha_spi + @"\" + part_number + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + ".log.txt"))
                        {
                            File.WriteAllText(path + @"SPI_DB\Failures\" + linha_spi + @"\" + part_number + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + ".log.txt", "PartNumber | SerialNumber| Linha | PainelPlaca | PAD | Rpass | Repair|" + Environment.NewLine);
                        }
                    }
                    catch (Exception)
                    {

                        System.Threading.Thread.Sleep(2000);  //delay

                        try
                        {
                            if (!File.Exists(path + @"SPI\Failures\" + linha_spi + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + ".log.txt"))
                            {
                                File.WriteAllText(path + @"SPI\Failures\" + linha_spi + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + ".log.txt", "PartNumber| SerialNumber | Linha | PainelPlaca | PAD | Rpass | Repair|" + Environment.NewLine);
                            }
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Line - 894." + "File.WriteAllText " + path + "SPI_DB\\Failures\\" + linha_spi + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + hora_criacao.ToString("HH") + "\\" + part_number + "\\" + serial_number + "_" + part_number + ".log.txt", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);


                            File.Delete(log);


                            System.Threading.Thread.CurrentThread.Abort();
                            this.Close();

                        }

                    }



                    /*---------------------------------------------------------------------------------------------------------- ---*/
                    /*--------------------------------------------------------------- Conexão com BD---------------------------------------------------*/
                   
                 

                    {
                        #region   
                        if (Directory.Exists(@"C:\Users\rafaelpin\source\repos\SPI_ROBOT\bin\Debug\")) // verefica local onde esta BD
                        {
                            if (File.Exists("spi_robot.db")) File.Delete("spi_robot.db"); // Se ja tem um BD deletar
                        }

                        try
                        {

                            //------------------------------------------ criar a base de dados----------------------------------------------------------------------//

                            SQLiteConnection.CreateFile(@"C:\Users\rafaelpin\source\repos\SPI_ROBOT\bin\Debug\spi_robot.db"); // cria um novo BD

                            /*-------------------Estabelecer ligação com a base de dados--------------------------------------------------------*/

                            SQLiteConnection ligacao = new SQLiteConnection();
                            ligacao.ConnectionString = @"Data Source = C: \Users\rafaelpin\source\repos\SPI_ROBOT\bin\Debug\spi_robot.db; Version=3;";
                            ligacao.Open();

                            /*----------------------------  criar Tabela na base de dados---------------------------------------------------*/

                            string query = "CREATE TABLE statistic" +
                                           "(" +
                                           "Part_number                      int, " +
                                           "Serial_number                    varchar(50), " +
                                           "linha                            varchar(5), " +
                                           "Painel_Placa                     int, " +
                                           "Data_Hora                        datetime,  " +
                                           "Placa_RPass                      varchar (10), " +
                                           "Placa_Repair                     varchar (10), " +
                                           "PAD                              varchar (10), " +
                                           "Placa_Pass                       varchar(10) " +
                                           ")";

                            SQLiteCommand comando = new SQLiteCommand(query, ligacao);
                            comando.ExecuteNonQuery();
                        }

                        catch (Exception)
                        {
                            MessageBox.Show("Tabela não Criada", "ALERTA!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            System.Threading.Thread.CurrentThread.Abort();
                            this.Close();
                        }
                    }
                    // Gravar dado registro na base de dados
                   
                    
                   
                    
                    #endregion






                    /*--- Alimenta o log da SkyNet ---*/

                    try
                    {
                            File.AppendAllText(path + @"SPI_DB\Failures\" + linha_spi + @"\" + part_number + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + ".log.txt", part_number + "|" + serial_number + "|" + linha_spi + "|" + painel_placa + "|" + numero_pad + "|" + Placa_Rpass + "|" + Placa_Repair + "|" + Environment.NewLine);

                        }
                        catch (Exception)
                        {
                            System.Threading.Thread.Sleep(2000);  //delay

                            try
                            {
                                File.AppendAllText(path + @"SPI_DB\Failures\" + linha_spi + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + @"\" + ".log.txt", part_number + "|" + serial_number + "|" + linha_spi + "|" + painel_placa + "|" + numero_pad + "|" + Placa_Rpass + "|" + Placa_Repair + "|" + Environment.NewLine);
                            }
                            catch (Exception)
                            {

                                MessageBox.Show("Line - 961." + "File.AppendAllText " + path + "SPI_DB\\Failures\\" + linha_spi + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + hora_criacao.ToString("HH") + "\\" + part_number + "\\" + serial_number + "_" + part_number + ".log.txt", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                File.Delete(log);


                                System.Threading.Thread.CurrentThread.Abort();
                                this.Close();
                            }
                        }


                
                /*--- ------------------------------------------------------------------------------------------------------- ---*/

                #endregion


                #endregion

                falhas_spi = Convert.ToString(placa_repair);      //atribui a falhas_spi o número total de falhas que a máquina indicou
            
            /*--- ------------------------------------------------------------------------------------------------------- ---*/

            if (!Directory.Exists(path + @"SPI_DB\Statistic\" + linha_spi + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\"))
            {
                Directory.CreateDirectory(path + @"SPI_DB\Statistic\" + linha_spi + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\");
                System.Threading.Thread.Sleep(100);
            }

            /*--- Se não existir o arquivo de log para a SkyNet cria-o inserindo um cabeçalho na primeira linha---*/

            if (!File.Exists(path + @"SPI_DB\Statistic\" + linha_spi + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + ".log.txt"))
            {
                try
                {
                    File.WriteAllText(path + @"SPI_DB\Statistic\" + linha_spi + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + ".log.txt", "PartNumber | SerialNumber | Linha | PainelPlaca | Data | Hora |  Placa_Rpass| Placa_Repair | Placa_Pass " + Environment.NewLine);
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(2000); //delay

                    try
                    {
                        File.WriteAllText(path + @"SPI_DB\Statistic\" + linha_spi + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + ".log.txt", "PartNumber | SerialNumber | Linha | PainelPlaca | Data | Hora  | Placa_Rpass | Placa_Repair| Placa_Pass " + Environment.NewLine);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Line - 1037." + "File.AppendAllText " + path + "SPI_DB\\Statistic\\" + linha_spi + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + hora_criacao.ToString("yyyy_MM_dd") + ".log.txt", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        File.Delete(log);

                        DOS(@"taskkill /IM Tri_SPI_RS.exe /F");
                        DOS(@"taskkill /IM SPI_RUN.exe /F");

                        System.Threading.Thread.CurrentThread.Abort();
                        this.Close();
                    }
                }

            }
                   
                    
                    
                    
                    
                    
          /*---- ------------------------------------------------------------------------------------------------------- ---*/

            System.Diagnostics.Process.GetCurrentProcess().Close();
            /*--- Alimenta o log da SkyNet ---*/

            try
            {
                File.AppendAllText(path + @"SPI_DB\Statistic\" + linha_spi + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + ".log.txt", part_number + "|" + serial_number + "|" + linha_spi + "|" + painel_placa + "|" + hora_criacao.ToString("yyyy_MM_dd") + "|" + hora_criacao.ToString("HH:mm") +  "|" + placa_rpass + "|" + placa_repair + "|" + Placa_pass + Environment.NewLine);
            }
            catch (Exception)
            {
                System.Threading.Thread.Sleep(2000); //delay

                try
                {
                    File.AppendAllText(path + @"SPI_DB\Statistic\" + linha_spi + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + serial_number + ".log.txt", part_number + "|" + serial_number + "|" + linha_spi + "|" + painel_placa + "|" + hora_criacao.ToString("yyyy_MM_dd") + "|" + hora_criacao.ToString("HH:mm") + "|" + placa_rpass + "|" + placa_repair + "|" + placa_pass + Environment.NewLine);
                }
                catch (Exception)
                {

                    MessageBox.Show("Line - 1071." + "File.AppendAllText " + path + "SPI_DB\\Statistic\\" + linha_spi + "\\" + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + hora_criacao.ToString("yyyy_MM_dd") + ".log.txt", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    File.Delete(log);

                    DOS(@"taskkill /IM Tri_spi_RS.exe /F");


                    System.Threading.Thread.CurrentThread.Abort();
                    this.Close();
                }
            }
                }
                /*--- ------------------------------------------------------------------------------------------------------- ---*/






                File.Delete(log);
        }


     

    }

  


    /*************************************************************************************************************************/
    /*--- Verifica a existencia de arquivos gerados por testes offline, se existir manda para o servidor ---*/
    private void check_file_offline()
    {

        /*--- Copia os arquivos da pasta local Failures para a rede ---*/

        string[] pasta_Failures8 = Directory.GetDirectories(@"C:\EngTeste\SPI_DB\Failures\");

        #region foreach ( var item_linha in pasta_Failures8 )

        foreach (var item_linha in pasta_Failures8)
        {
            string[] pasta_Failures9 = Directory.GetDirectories(item_linha);
            foreach (var item_posto in pasta_Failures9)
            {
                string[] pasta_Failures = Directory.GetDirectories(item_posto);
                foreach (var caminho1 in pasta_Failures)
                {
                    string[] pasta_horas = Directory.GetDirectories(caminho1);

                    foreach (var caminho2 in pasta_horas)
                    {
                        string[] pasta_PN = Directory.GetDirectories(caminho2);

                        foreach (var caminho3 in pasta_PN)
                        {
                            string[] pasta_arquivos = Directory.GetFiles(caminho3, "*.txt");

                            foreach (var path_Arquivo1 in pasta_arquivos)
                            {
                                string[] path_quebrado = path_Arquivo1.Split('\\');

                                string diretorio_teorico = @"\\10.8.2.73\engl06$\TestTool\SPI_Test\SPI_DB\Failures\" + path_quebrado[4] + @"\" + path_quebrado[5] + @"\" + path_quebrado[6] + @"\" + path_quebrado[7] + @"\" + path_quebrado[8] + @"\";
                                string arquivo_teorico = @"\\10.8.2.73\engl06$\TestTool\SPI_Test\SPI_DB\Failures\" + path_quebrado[4] + @"\" + path_quebrado[5] + @"\" + path_quebrado[6] + @"\" + path_quebrado[7] + @"\" + path_quebrado[8] + @"\" + path_quebrado[9];

                                if (File.Exists(arquivo_teorico))
                                {
                                    int numero_linhas = File.ReadLines(path_Arquivo1).Count();
                                    for (int i = 1; i < numero_linhas; i++)
                                    {
                                        string linha = File.ReadLines(path_Arquivo1).Skip(i).Take(1).First();
                                        File.AppendAllText(arquivo_teorico, linha + Environment.NewLine);
                                    }
                                    File.Delete(path_Arquivo1);
                                }
                                else
                                {
                                    if (!Directory.Exists(diretorio_teorico)) Directory.CreateDirectory(diretorio_teorico);
                                    File.Copy(path_Arquivo1, arquivo_teorico);
                                    File.Delete(path_Arquivo1);
                                }
                            }

                        }

                    }

                }

            }
        }
        #endregion

        /*--- --------------------------------------------------------------------------------------------------------- ---*/

        /*--- Copia os arquivos da pasta local Statistic para a rede ---*/
    string[] pasta_Statistic = Directory.GetDirectories(@"C:\EngTeste\SPI_DB\Statistic\");

        #region foreach ( var caminho1 in pasta_Statistic )

        foreach (var caminho1 in pasta_Statistic)
        {
            string[] pasta_Posto = Directory.GetDirectories(caminho1);
            foreach (var caminho2 in pasta_Posto)
            {
                string[] pasta_Data = Directory.GetDirectories(caminho2);
                foreach (var caminho3 in pasta_Data)
                {
                    string[] pasta_arquivos = Directory.GetFiles(caminho3, "*.txt");
                    foreach (var arquivo_Statistic in pasta_arquivos)
                    {
                        string[] path_quebrado = arquivo_Statistic.Split('\\');

                        string diretorio_teorico = @"\\10.8.2.73\engl06$\TestTool\SPI_Test\SPI_DB\Statistic\" + path_quebrado[4] + @"\" + path_quebrado[5] + @"\" + path_quebrado[6] + @"\";
                        string arquivo_teorico = @"\\10.8.2.73\engl06$\TestTool\SPI_Test\SPI_DB\Statistic\" + path_quebrado[4] + @"\" + path_quebrado[5] + @"\" + path_quebrado[6] + @"\" + path_quebrado[7];

                        if (File.Exists(arquivo_teorico))
                        {
                            int numero_linhas = File.ReadLines(arquivo_Statistic).Count();
                            for (int i = 1; i < numero_linhas; i++)
                            {
                                string linha = File.ReadLines(arquivo_Statistic).Skip(i).Take(1).First();
                                File.AppendAllText(arquivo_teorico, linha + Environment.NewLine);
                            }
                            File.Delete(arquivo_Statistic);
                        }
                        else
                        {
                            if (!Directory.Exists(diretorio_teorico)) Directory.CreateDirectory(diretorio_teorico);
                            File.Copy(arquivo_Statistic, arquivo_teorico);
                            File.Delete(arquivo_Statistic);
                        }
                    }

                }

            }

        }

        #endregion

        /*--- --------------------------------------------------------------------------------------------------------- ---*/
    }

    /*************************************************************************************************************************/

    /*--- Fechar a aplicação ---*/
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
        







        /*************************************************************************************************************************/
    

