using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SPI_ROBOT
{
    public partial class Form1 : Form
    {
        private Timer timeX;

        public Form1()                                  //inicialização do programa
        {
            InitializeComponent();
            modo_operacao = false;
            timeX = new Timer() { Interval = 1000 };     //instancia a classe Timer e seta o parametro Interval com 1s
            timeX.Tick += new EventHandler(timeX_Tick);

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

        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        private string filename; public string Filename { get { return filename; } set { filename = value; } }

        private bool form_fechado; public bool Form_fechado { get { return form_fechado; } set { form_fechado = value; } }

        private bool modo_operacao; public bool Modo_Operacao { get { return modo_operacao; } set { modo_operacao = value; } }

        private string path; public string Path { get { return path; } set { path = value; } }                                  //aponta par o diretório principal

        private string part_number; public string Part_Number { get { return part_number; } set { part_number = value; } }      //registra o PartNumber da placa

        private string serial_number; public string Serial_Number { get { return serial_number; } set { serial_number = value; } } //registra o SerialNumber da placa

        private string modelo_spi; public string Modelo_spi { get { return modelo_spi; } set { modelo_spi = value; } }          //registra o modelo da AOI 

        private string componentes_testados_spi; public string Componentes_Testados_SPI { get { return componentes_testados_spi; } set { componentes_testados_spi = value; } }

        private string falhas_spi; public string Falhas_AOI { get { return falhas_spi; } set { falhas_spi = value; } }

        private string falhas_operador; public string Falhas_Operador { get { return falhas_operador; } set { falhas_operador = value; } }

        private string componente; public string Componente { get { return componente; } set { componente = value; } }

        private string pn_componente; public string PN_Componente { get { return pn_componente; } set { pn_componente = value; } }

        private string falha_componente; public string Falha_Componente { get { return falha_componente; } set { falha_componente = value; } }

        private string usuario_confirmou_falha; public string Usuario_Confirmou_Falha { get { return usuario_confirmou_falha; } set { usuario_confirmou_falha = value; } }

        private string linha_SX; public string Linha_SX { get { return linha_SX; } set { linha_SX = value; } }

        private string path_foto_spi; public string path_foto_SPI { get { return path_foto_spi; } set { path_foto_spi = value; } }

        private string nome_foto_spi; public string nome_foto_SPI { get { return path_foto_spi; } set { path_foto_spi = value; } }
        
        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_MINIMIZE = 6;
        private const int SW_HIDE = 0;
        private const int SW_RESTORE = 9;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private string path_local = @"C:\EngTeste\";

        string logpath = @"C:\SPI_OUT\";            //aponta para o diretório onde são gerados os logs do teste da AIO

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
                if (Directory.Exists(@"C:\EngTeste\logs"))
                {
                    conectado = true;
                    
                    if (File.Exists("operação.dll")) File.Delete("operação.dll");
                    File.WriteAllText("operacao.dll", "modo online" + Environment.NewLine);
                    path = @"C:\EngTest\Logs";
                    lb_on_off.Font = new Font("Arial", 50);
                    lb_on_off.ForeColor = System.Drawing.Color.Green;
                    lb_on_off.Text = "ON";
                }
                else
                {
                    conectado = false;
                    if (File.Exists("operacao.dll")) File.Delete("operacao.dll"); //deleta o arquivo operacao.dll
                    File.WriteAllText("operacao.dll", "modo offline" + Environment.NewLine);
                    path = @"C:\EngTeste\logs";
                    lb_on_off.Font = new Font("Arial", 50);
                    lb_on_off.Text = "OFF";
                    lb_on_off.BackColor = lb_on_off.ForeColor = System.Drawing.Color.Black;
                    lb_on_off.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show("Operando Off-line, Informe ao time de suporte", "ALERTA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception)
            {


            }
        
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
        
        internal struct INPUT
        {
            public UInt32 Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }
       
        /*************************************************************************************************************************/

        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }
        /*************************************************************************************************************************/

        internal struct MOUSEINPUT
        {
            public Int32 X;
            public Int32 Y;
            public UInt32 MouseData;
            public UInt32 Flags;
            public UInt32 Time;
            public IntPtr ExtraInfo;
        }
     

        /*************************************************************************************************************************/
        /*---  ---*/
        public static void ClickOnPoint(IntPtr wndHandle, Point clientPoint)    //posicionar o mouse e clicar
        {
            var oldPos = Cursor.Position;

            /// get screen coordinates
            ClientToScreen(wndHandle, ref clientPoint);

            /// set cursor on coords, and press mouse
            Cursor.Position = new Point(clientPoint.X, clientPoint.Y);

            var inputMouseDown = new INPUT();
            inputMouseDown.Type = 0; /// input type mouse
            inputMouseDown.Data.Mouse.Flags = 0x0002; /// left button down

            var inputMouseUp = new INPUT();
            inputMouseUp.Type = 0; /// input type mouse
            inputMouseUp.Data.Mouse.Flags = 0x0004; /// left button up

            var inputs = new INPUT[] { inputMouseDown, inputMouseUp };
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));

            /// return mouse 
            Cursor.Position = oldPos;
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

                if (File.Exists("separadores_log.ini")) File.Delete("separadores_log.ini"); //o arquivo separadores_log.ini é utilizado para separar os logs de serialnumber diferentes presente no mesmo arquivo gerado pela AOI

                int aux_log = 1;

                /*--- Identifica as linhas que separam cada log ---*/

                #region[Busca as linhas que separam os logs no arquivo]

                int numero_linha = File.ReadLines(log).Count();                             //atribui a numero_linha a quantidade de linhas presente no arquivo apontado por log
                int inicio_log = 0;                                                         //indica a linha inicial do log

                for (int i = 0; i < numero_linha; i++)                                      //varre todas as linhas do arquivo
                {
                    string conteudo = File.ReadLines(log).Skip(i).Take(1).First();          //lê uma linha do arquivo

                    if (conteudo == "")                                                     //verifica se esta linha esta vazia
                    {
                        File.AppendAllText(@"separadores_log.ini", inicio_log.ToString() + "," + i.ToString() + Environment.NewLine); //anota o número da linha vazia no arquivo temporário separadores_log.ini

                        inicio_log = i + 1;
                    }
                }
                #endregion
                /*--- ----------------------------------------------------------------------------------------------------------- ---*/

                /*--- Tratamento dos Logs ---*/
                
                int separadores_total = File.ReadLines("separadores_log.ini").Count();                      //atribui a separadores_total a quantidade de separadores que existe no log da AOI
                #region for (int a = 0; a < separadores_total; a++)                                         //realiza um loop para cada log de placa dentro do arquivo .txt
                for (int a = 0; a < separadores_total; a++)                                                //realiza um loop para cada log de placa dentro do arquivo .txt
                {
                    string start_end = File.ReadLines("separadores_log.ini").Skip(a).Take(1).First();       //atribui o inicio e o final de cada log dentro do arquivo .txt

                    string[] ab = start_end.Split(',');                                                     //separa o começo e o final do log

                    int comeco = Convert.ToInt32(ab[0]);                                                    //converte o numero da linha para int
                    int final = Convert.ToInt32(ab[1]);                                                     //converte o numero da linha para int
                    int falhas_maquina = 0;
                    int falha_op = 0;

                    /*--- Extração das informações de cada log contido no arquivo ---*/

                    #region for (int i = comeco; i < final; i++)                                              //varre o arquivo de log entre as linhas que começam um log de placa e a linha final
                    for (int i = comeco; i < final; i++)                                                    //varre o arquivo de log entre as linhas que começam um log de placa e a linha final
                    {
                        /*--- Realiza a leitura do cabeçalho do Log ---*/

                        string linha = File.ReadLines(log).Skip(i).Take(1).First();                         //lê uma linha do arquivo log
                        #region if (linha.Contains("%"))
                        if (linha.Contains("%"))
                        {
                            string[] pn = linha.Split('%');

                            part_number = pn[0];

                            serial_number = File.ReadLines(log).Skip(i + 1).Take(1).First();
                            if ((serial_number.Contains(":")) || (serial_number.Contains("SE")))
                            {
                                serial_number = linha_SX + "_" + hora_criacao.ToString("yyyyMMdd_HHmmss") + "_Board-" + aux_log.ToString();
                                aux_log++;
                            }

                            modelo_spi = File.ReadLines(log).Skip(i + 2).Take(1).First();

                            componentes_testados_spi = File.ReadLines(log).Skip(i + 11).Take(1).First();

                            i = i + 12;
                        }
                        #endregion
                        /*--- ------------------------------------------------------------------------------------------------------- ---*/


                        /*--- Captura as informações das falhas presentes no Log ---*/
                        #region if ( linha.Contains(".jpg"))
                        if (linha.Contains(".jpg"))
                        {
                            falhas_maquina++;                           //cada linha que aponta para uma foto .jpg corresponde a uma falha que a AOI identificou, desta forma incrementamos falhas_maquina

                            if (linha.Contains(";F;")) falha_op++;      //na quanta coluna da linha que descreve a falha encontramos as letras P e F, essas lentras idicam como o operador avaliou a falha

                            string[] falha_aoi = linha.Split(';');      //quebra a linha de acord com o separador ';'

                            string[] comp = falha_aoi[0].Split('_');
                            componente = comp[0];                       //atribui a componente o nome do componente que falhou

                            string[] pncomp = falha_aoi[1].Split('#');
                            pn_componente = pncomp[0];                  //atribui a pn_componente o PartNumber do componente com falha
                            falha_componente = falha_aoi[2];            //atribui a falha_componente o tipo de falha que a AOI identificou. Ex.: (PRESENCE), (ROTATE), (POLARITY)
                            usuario_confirmou_falha = falha_aoi[3];     //atribui a usuario_confirmou_falha a avaliação do operador sobre a falha apresentada

                            string[] fot = falha_aoi[5].Split('\\');


                            string path_temp = falha_aoi[5];

                            #region if (File.Exists(path_temp))    
                            if (File.Exists(path_temp))
                            {
                                if (!Directory.Exists(path + @"AOI_DB\Pictures\" + serial_number + @"\"))
                                {
                                    Directory.CreateDirectory(path + @"AOI_DB\Pictures\" + serial_number + @"\");
                                }

                                try
                                {
                                    File.Copy(path_temp, path + @"AOI_DB\Pictures\" + serial_number + @"\" + serial_number + "_" + part_number + "_" + componente + ".jpg", true);
                                    path_foto_spi = @"\\10.8.2.73\engl06$\TestTool\AOI_Test\AOI_DB\Pictures\" + serial_number + @"\" + serial_number + "_" + part_number + "_" + componente + ".jpg";
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        System.Threading.Thread.Sleep(2000);  //delay 

                                        File.Copy(path_temp, path + @"SPI_DB\Pictures\" + serial_number + @"\" + serial_number + "_" + part_number + "_" + componente + ".jpg", true);
                                        path_foto_spi = @"\\10.8.2.73\engl06$\TestTool\AOI_Test\AOI_DB\Pictures\" + serial_number + @"\" + serial_number + "_" + part_number + "_" + componente + ".jpg";
                                    }
                                    catch (Exception)
                                    {

                                        MessageBox.Show("Line - 815." + "Copy " + path + "SPI_DB\\Pictures\\" + serial_number + "\\" + serial_number + "_" + part_number + "_" + componente + ".jpg", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                        File.Delete(log);

                                        DOS(@"taskkill /IM Tri_Aoi_RS.exe /F");
                                        DOS(@"taskkill /IM AOI_RUN.exe /F");

                                        System.Threading.Thread.CurrentThread.Abort();
                                        this.Close();
                                    }
                                }

                                File.Delete(path_temp);
                                //System.Threading.Thread.Sleep(100);
                            }
                            else
                            {
                                nome_foto_spi = "foto não encontrada";
                                path_foto_spi = "foto não encontrada";
                            }
                            #endregion

                            /*--- Se não existir o arquivo de log para a SkyNet cria-o inserindo um cabeçalho na primeira linha---*/

                            try
                            {
                                if (!Directory.Exists(path + @"SPI_DB\Failures\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("HH") + @"\" + part_number + @"\"))
                                {
                                    Directory.CreateDirectory(path + @"SPI?_DB\Failures\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("HH") + @"\" + part_number + @"\");
                                }
                            }
                            catch (Exception)
                            {
                                System.Threading.Thread.Sleep(2000);  //delay

                                try
                                {
                                    if (!Directory.Exists(path + @"SPI_DB\Failures\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("HH") + @"\" + part_number + @"\"))
                                    {
                                        Directory.CreateDirectory(path + @"SPI_DB\Failures\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("HH") + @"\" + part_number + @"\");
                                    }
                                }
                                catch (Exception)
                                {

                                    MessageBox.Show("Line - 860." + "CreateDirectory " + path + "AOI_DB\\Failures\\" + linha_SX + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + hora_criacao.ToString("HH") + "\\" + part_number + "\\", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    File.Delete(log);

                                    DOS(@"taskkill /IM Tri_SPI_RS.exe /F");
                                    DOS(@"taskkill /IM SPI_RUN.exe /F");

                                    System.Threading.Thread.CurrentThread.Abort();
                                    this.Close();
                                }
                            }

                            try
                            {
                                if (!File.Exists(path + @"SPI_DB\Failures\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("HH") + @"\" + part_number + @"\" + serial_number + "_" + part_number + ".log.txt"))
                                {
                                    File.WriteAllText(path + @"SPI_DB\Failures\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("HH") + @"\" + part_number + @"\" + serial_number + "_" + part_number + ".log.txt", "SerialNumber | PartNumber | Linha | Posto | Modelo AOI | Componente | PartNumber Componente | Falha | Usuario falhou(bool) | Path da imagem.jpg" + Environment.NewLine);
                                }
                            }
                            catch (Exception)
                            {

                                System.Threading.Thread.Sleep(2000);  //delay

                                try
                                {
                                    if (!File.Exists(path + @"SPI\Failures\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("HH") + @"\" + part_number + @"\" + serial_number + "_" + part_number + ".log.txt"))
                                    {
                                        File.WriteAllText(path + @"SPIFailures\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("HH") + @"\" + part_number + @"\" + serial_number + "_" + part_number + ".log.txt", "SerialNumber | PartNumber | Linha  | Modelo SPI | Componente | PartNumber Componente | Falha | Usuario falhou(bool) | Path da imagem.jpg" + Environment.NewLine);
                                    }
                                }
                                catch (Exception)
                                {

                                    MessageBox.Show("Line - 894." + "File.WriteAllText " + path + "SPI_DB\\Failures\\" + linha_SX + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + hora_criacao.ToString("HH") + "\\" + part_number + "\\" + serial_number + "_" + part_number + ".log.txt", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    File.Delete(log);

                                    DOS(@"taskkill /IM Tri_Aoi_RS.exe /F");
                                    DOS(@"taskkill /IM AOI_RUN.exe /F");

                                    System.Threading.Thread.CurrentThread.Abort();
                                    this.Close();
                                }
                            }

                            try
                            {
                                if (!File.Exists(path + @"SPI_DB\Pictures\" + serial_number + @"\" + serial_number + "_" + part_number + ".log.txt"))
                                {
                                    File.WriteAllText(path + @"SPI_DB\Pictures\" + serial_number + @"\" + serial_number + "_" + part_number + ".log.txt", "SerialNumber | PartNumber | Linha | Modelo AOI | Componente | PartNumber Componente | Falha | Usuario falhou(bool) | Path da imagem.jpg" + Environment.NewLine);
                                }
                            }
                            catch (Exception)
                            {
                                System.Threading.Thread.Sleep(2000);  //delay

                                try
                                {
                                    if (!File.Exists(path + @"SPI_DB\Pictures\" + serial_number + @"\" + serial_number + "_" + part_number + ".log.txt"))
                                    {
                                        File.WriteAllText(path + @"SPI_DB\Pictures\" + serial_number + @"\" + serial_number + "_" + part_number + ".log.txt", "SerialNumber | PartNumber | Linha | Modelo AOI | Componente | PartNumber Componente | Falha | Usuario falhou(bool) | Path da imagem.jpg" + Environment.NewLine);
                                    }
                                }
                                catch (Exception)
                                {

                                    MessageBox.Show("Line - 927." + "File.WriteAllText " + path + "SPI_DB\\Pictures\\" + serial_number + "\\" + serial_number + "_" + part_number + ".log.txt", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    File.Delete(log);

                                    DOS(@"taskkill /IM Tri_Aoi_RS.exe /F");
                                    DOS(@"taskkill /IM AOI_RUN.exe /F");

                                    System.Threading.Thread.CurrentThread.Abort();
                                    this.Close();
                                }

                            }

                            /*--- ------------------------------------------------------------------------------------------------------- ---*/


                            /*--- Alimenta o log da SkyNet ---*/

                            try
                            {
                                File.AppendAllText(path + @"SPI_DB\Failures\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("HH") + @"\" + part_number + @"\" + serial_number + "_" + part_number + ".log.txt", serial_number + "|" + part_number + "|" + linha_SX + "|" + modelo_spi + "|" + componente + "|" + pn_componente + "|" + falha_componente + "|" + usuario_confirmou_falha + "|" + path_foto_spi + Environment.NewLine);

                            }
                            catch (Exception)
                            {
                                System.Threading.Thread.Sleep(2000);  //delay

                                try
                                {
                                    File.AppendAllText(path + @"SPI_DB\Failures\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("HH") + @"\" + part_number + @"\" + serial_number + "_" + part_number + ".log.txt", serial_number + "|" + part_number + "|" + linha_SX + "|" + modelo_spi + "|" + componente + "|" + pn_componente + "|" + falha_componente + "|" + usuario_confirmou_falha + "|" + path_foto_spi + Environment.NewLine);
                                }
                                catch (Exception)
                                {

                                    MessageBox.Show("Line - 961." + "File.AppendAllText " + path + "SPI_DB\\Failures\\" + linha_SX + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + hora_criacao.ToString("HH") + "\\" + part_number + "\\" + serial_number + "_" + part_number + ".log.txt", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    File.Delete(log);

                                    DOS(@"taskkill /IM Tri_spi_RS.exe /F");
                                    DOS(@"taskkill /IM SPI_RUN.exe /F");

                                    System.Threading.Thread.CurrentThread.Abort();
                                    this.Close();
                                }
                            }

                            try
                            {
                                File.AppendAllText(path + @"SPI_DB\Pictures\" + serial_number + @"\" + serial_number + "_" + part_number + "_" + ".log.txt", serial_number + "|" + part_number + "|" + linha_SX + "|" + modelo_spi + "|" + componente + "|" + pn_componente + "|" + falha_componente + "|" + usuario_confirmou_falha + "|" + path_foto_spi + Environment.NewLine);
                            }
                            catch (Exception)
                            {
                                System.Threading.Thread.Sleep(2000);  //delay

                                try
                                {
                                    File.AppendAllText(path + @"SPI_DB\Pictures\" + serial_number + @"\" + serial_number + "_" + part_number + "_" + ".log.txt", serial_number + "|" + part_number + "|" + linha_SX + "|" + modelo_spi + "|" + componente + "|" + pn_componente + "|" + falha_componente + "|" + usuario_confirmou_falha + "|" + path_foto_spi + Environment.NewLine);
                                }
                                catch (Exception)
                                {

                                    MessageBox.Show("Line - 988." + "File.AppendAllText " + path + "SPI_DB\\Pictures\\" + serial_number + "\\" + serial_number + "_" + part_number + "_" + ".log.txt", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    File.Delete(log);

                                    DOS(@"taskkill /IM Tri_Aoi_RS.exe /F");
                                    DOS(@"taskkill /IM AOI_RUN.exe /F");

                                    System.Threading.Thread.CurrentThread.Abort();
                                    this.Close();
                                }
                            }

                            /*--- ------------------------------------------------------------------------------------------------------- ---*/
                        }
                        #endregion
                    }

                    #endregion

                    falhas_spi = Convert.ToString(falhas_maquina);      //atribui a falhas_aoi o número total de falhas que a máquina indicou
                    falhas_operador = Convert.ToString(falha_op);       //atribui a falhas_operador o número total de falhas que o operador confirmou

                    /*--- ------------------------------------------------------------------------------------------------------- ---*/

                    if (!Directory.Exists(path + @"SPI_DB\Statistic\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\"))
                    {
                        Directory.CreateDirectory(path + @"SPI_DB\Statistic\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\");
                        System.Threading.Thread.Sleep(100);
                    }

                    /*--- Se não existir o arquivo de log para a SkyNet cria-o inserindo um cabeçalho na primeira linha---*/

                    if (!File.Exists(path + @"SPI_DB\Statistic\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("yyyy_MM_dd") + ".log.txt"))
                    {
                        try
                        {
                            File.WriteAllText(path + @"SPI_DB\Statistic\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("yyyy_MM_dd") + ".log.txt", "PartNumber | SerialNumber | Linha | Usuário | Data | Hora | Componentes Testados | N° de Falhas da SPI| N° de Falhas Usuário " + Environment.NewLine);
                        }
                        catch (Exception)
                        {
                            System.Threading.Thread.Sleep(2000); //delay

                            try
                            {
                                File.WriteAllText(path + @"SPI_DB\Statistic\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("yyyy_MM_dd") + ".log.txt", "PartNumber | SerialNumber | Linha | Usuário | Data | Hora | Componentes Testados | N° de Falhas da SPI | N° de Falhas Usuário " + Environment.NewLine);
                            }
                            catch (Exception)
                            {

                                MessageBox.Show("Line - 1037." + "File.AppendAllText " + path + "SPI_DB\\Statistic\\" + linha_SX + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + hora_criacao.ToString("yyyy_MM_dd") + ".log.txt", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                File.Delete(log);

                                DOS(@"taskkill /IM Tri_SPI_RS.exe /F");
                                DOS(@"taskkill /IM SPI_RUN.exe /F");

                                System.Threading.Thread.CurrentThread.Abort();
                                this.Close();
                            }
                        }

                    }

                    /*--- ------------------------------------------------------------------------------------------------------- ---*/

                    System.Diagnostics.Process.GetCurrentProcess().Close();
                    /*--- Alimenta o log da SkyNet ---*/

                    try
                    {
                        File.AppendAllText(path + @"SPI_DB\Statistic\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("yyyy_MM_dd") + ".log.txt", part_number + "|" + serial_number + "|" + linha_SX + "|" + hora_criacao.ToString("yyyy_MM_dd") + "|" + hora_criacao.ToString("HH:mm") + "|" + componentes_testados_spi + "|" + falhas_spi + "|" + falhas_operador + "|" + Environment.NewLine);
                    }
                    catch (Exception)
                    {
                        System.Threading.Thread.Sleep(2000); //delay

                        try
                        {
                            File.AppendAllText(path + @"SPI_DB\Statistic\" + linha_SX + @"\" + hora_criacao.ToString("yyyy_MM_dd") + @"\" + hora_criacao.ToString("yyyy_MM_dd") + ".log.txt", part_number + "|" + serial_number + "|" + linha_SX + "|" + hora_criacao.ToString("yyyy_MM_dd") + "|" + hora_criacao.ToString("HH:mm") + "|" + componentes_testados_spi + "|" + falhas_spi + "|" + falhas_operador + "|" + Environment.NewLine);
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Line - 1071." + "File.AppendAllText " + path + "SPI_DB\\Statistic\\" + linha_SX + "\\" + "\\" + hora_criacao.ToString("yyyy_MM_dd") + "\\" + hora_criacao.ToString("yyyy_MM_dd") + ".log.txt", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            File.Delete(log);

                            DOS(@"taskkill /IM Tri_Aoi_RS.exe /F");
                            DOS(@"taskkill /IM AOI_RUN.exe /F");

                            System.Threading.Thread.CurrentThread.Abort();
                            this.Close();
                        }
                    }

                    /*--- ------------------------------------------------------------------------------------------------------- ---*/


                }
                #endregion
                /*--- ----------------------------------------------------------------------------------------------------------- ---*/


                File.Delete(log);
            }
            #endregion

            /*------ ----------------------------------------------------------------------------------------------------------- ---*/

            timeX.Start();  //inicia o contador
        }

        /*************************************************************************************************************************/
        /*--- Método para tratar o estouro do timer ---*/
        void timeX_Tick(object sender, EventArgs e)
        {
            timeX.Stop();
            Ciclo();
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

                                    string diretorio_teorico = @"\\10.8.2.73\engl06$\TestTool\AOI_Test\AOI_DB\Failures\" + path_quebrado[4] + @"\" + path_quebrado[5] + @"\" + path_quebrado[6] + @"\" + path_quebrado[7] + @"\" + path_quebrado[8] + @"\";
                                    string arquivo_teorico = @"\\10.8.2.73\engl06$\TestTool\AOI_Test\AOI_DB\Failures\" + path_quebrado[4] + @"\" + path_quebrado[5] + @"\" + path_quebrado[6] + @"\" + path_quebrado[7] + @"\" + path_quebrado[8] + @"\" + path_quebrado[9];

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


            /*--- Copia os arquivos da pasta local Pictures para a rede ---*/

            string[] pasta_Pictures = Directory.GetDirectories(@"C:\EngTeste\SPI_DB\Pictures\");

            #region foreach ( var caminho4 in pasta_Pictures )
            foreach (var caminho4 in pasta_Pictures)
            {
                string[] pasta_arquivos = Directory.GetFiles(caminho4, "*.txt");

                foreach (var path_Arquivo2 in pasta_arquivos)
                {
                    string[] path_quebrado = path_Arquivo2.Split('\\');

                    string diretorio_teorico = @"\\10.8.2.73\engl06$\TestTool\AOI_Test\AOI_DB\Pictures\" + path_quebrado[4] + @"\";
                    string arquivo_teorico = @"\\10.8.2.73\engl06$\TestTool\AOI_Test\AOI_DB\Pictures\" + path_quebrado[4] + @"\" + path_quebrado[5];

                    if (File.Exists(arquivo_teorico))
                    {
                        int numero_linhas = File.ReadLines(path_Arquivo2).Count();
                        for (int i = 1; i < numero_linhas; i++)
                        {
                            string linha = File.ReadLines(path_Arquivo2).Skip(i).Take(1).First();
                            File.AppendAllText(arquivo_teorico, linha + Environment.NewLine);
                        }
                        File.Delete(path_Arquivo2);
                    }
                    else
                    {
                        if (!Directory.Exists(diretorio_teorico)) Directory.CreateDirectory(diretorio_teorico);
                        File.Copy(path_Arquivo2, arquivo_teorico);
                        File.Delete(path_Arquivo2);
                    }
                }

                string[] pasta_arquivos12 = Directory.GetFiles(caminho4, "*.jpg");
                foreach (var path_Arquivo2 in pasta_arquivos12)
                {
                    string[] path_quebrado2 = path_Arquivo2.Split('\\');

                    string diretorio_teorico = @"\\10.8.2.73\engl06$\TestTool\AOI_Test\AOI_DB\Pictures\" + path_quebrado2[4] + @"\";
                    string arquivo_teorico = @"\\10.8.2.73\engl06$\TestTool\AOI_Test\AOI_DB\Pictures\" + path_quebrado2[4] + @"\" + path_quebrado2[5];

                    if (!Directory.Exists(diretorio_teorico)) Directory.CreateDirectory(diretorio_teorico);
                    File.Copy(path_Arquivo2, arquivo_teorico, true);
                    File.Delete(path_Arquivo2);

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

                            string diretorio_teorico = @"\\10.8.2.73\engl06$\TestTool\AOI_Test\AOI_DB\Statistic\" + path_quebrado[4] + @"\" + path_quebrado[5] + @"\" + path_quebrado[6] + @"\";
                            string arquivo_teorico = @"\\10.8.2.73\engl06$\TestTool\AOI_Test\AOI_DB\Statistic\" + path_quebrado[4] + @"\" + path_quebrado[5] + @"\" + path_quebrado[6] + @"\" + path_quebrado[7];

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








        /*************************************************************************************************************************/
    }
}
