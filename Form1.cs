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
using System.Xml;
using System.Globalization;

namespace SPI_ROBOT
{
    public partial class Form1 : Form
    {
        public Form1()                                 
        {
            InitializeComponent();

        }


        /*--- Variaveis do Sistema ---*/
        #region[Variaveis do Sistema]

        CultureInfo cultureBR = new CultureInfo("pt-BR");   //define o calendário brasileiro

        CultureInfo cultureUSA = new CultureInfo("en-US");  //define o calendário americano

        private Int64 indexStatistic_ID;

        private Int64 indexFailure_ID;

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

        //  private string rpass; public string Rpass { get { return rpass; } set { rpass = value; } }



        string logpath = @"C:\SPI_ROBOT\Logs";  //aponta para o diretório onde são gerados os logs do teste da AIO

        #endregion

        /*************************************************************************************************************************/


        /*************************************************************************************************************************/
        /*--- Inicialização do Formulario ---*/
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Stop( );                                                 //paralisa o timer
            indexStatistic_ID = 0;                                          //atribui 0 no inicio da aplicação
            indexFailure_ID = 0;                                            //atribui 0 no indeice assim iniciar a aplicação
            try
            {
                if (Directory.Exists(@"\\10.8.2.73\engl06$\TestTool\"))     //verifica se o drive da engenharia esta mapeado
                {

                    lb_on_off.Font = new Font("Arial Black", 50);
                    lb_on_off.ForeColor = System.Drawing.Color.Green;
                    lb_on_off.Text = "ON";
                }
                else                                                        //se não encontrar o diver emite uma mansagem de alerta ao usuário
                {
                    MessageBox.Show("Drive de rede não encontrado! Conecte-se ao drive \\10.8.2.73\\engl06$", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);               
            }

        }

        /*************************************************************************************************************************/

        /*************************************************************************************************************************/
        /*--- Inicia o processamento dos Logs - Iniciar timer1 ---*/
        private void bt_Play_Click(object sender, EventArgs e)
        {
            try
            {
                if ( Directory.Exists(@"\\10.8.2.73\engl06$\TestTool\") )     //verifica se o drive da engenharia esta mapeado
                {

                    lb_on_off.Font = new Font("Arial Black", 50);
                    lb_on_off.ForeColor = System.Drawing.Color.Green;
                    lb_on_off.Text = "ON";
                    lb_RUN.Visible = true;

                    timer1.Start( );                                        //inicia o timer1
                }
                else                                                        //se não encontrar o diver emite uma mansagem de alerta ao usuário
                {
                    lb_RUN.Visible = false;
                    timer1.Stop( );
                    MessageBox.Show("Drive de rede não encontrado! Conecte-se ao drive \\10.8.2.73\\engl06$", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message, "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*************************************************************************************************************************/

        /*************************************************************************************************************************/
        /*--- Inicia o Timer ---*/
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Ciclo();
            timer1.Start();
        }

        /**************************************************************************************************************************/


        /*************************************************************************************************************************/
        /*---Monitora o surgimento de um novo log de falha ----*/    
        private void Ciclo()
        {
            string nomeDB = DateTime.Now.ToString("MM_yyyy") + "SPI.db";                                            //estabelece o nome do banco de dados

            /*----Cria tabela BD ---*/

            #region [cria o banco de dados do respectivo mês]

            try
            {
                if ( !File.Exists(@"\\10.8.2.73\engl06$\TestTool\SPI_Test\SPI_DB\" + nomeDB) )                      //verifica se o banco de dados do mês corrente existe
                {
                    SQLiteConnection.CreateFile(@"\\10.8.2.73\engl06$\TestTool\SPI_Test\SPI_DB\" + nomeDB);         //cria o banco de dados

                    System.Threading.Thread.Sleep(1000);                                                            //delay de 1 segundos

                    SQLiteConnection ligacao = new SQLiteConnection(@"Data Source=\\10.8.2.73\engl06$\TestTool\SPI_Test\SPI_DB\"+nomeDB+"; Version=3;", true); //cria a conexão com o DB

                    ligacao.Open( );                                                                                //estabelece a conexão

                    string query =  "CREATE TABLE Statistic" +                                                       // Criar tabela no BD
                                    "(" +
                                    "Statistic_ID       INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                    "Serial_Number      TEXT(25), " +
                                    "Part_Number        TEXT(15), " +
                                    "Station            TEXT(10), " +
                                    "Top_Bot            TEXT (3), " +
                                    "Date               TEXT(12), " +
                                    "Hour               TEXT(10), " +
                                    "S_Status           TEXT(8), " +
                                    "User               TEXT(10), " +
                                    "Component_Number   NUMERIC, " +
                                    "Defect_Number      NUMERIC " +
                                    ")";

                    SQLiteCommand comando1 = new SQLiteCommand(query, ligacao); // Comuncicando com BD
                    comando1.ExecuteNonQuery( );


                    query = "CREATE TABLE Failures" +                                                       // Criar tabela no BD
                            "(" +
                            "Failures_ID        INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                            "Statistic_ID       INTEGER , " +
                            "F_Status           TEXT(8), " +
                            "Component          TEXT(15), " +
                            "Type_Fail          TEXT (15), " +
                            "PAD                TEXT (10) " +
                            ")";

                    SQLiteCommand comando2 = new SQLiteCommand(query, ligacao); // Comuncicando com BD
                    comando2.ExecuteNonQuery( );

                    ligacao.Close( );

                }

            }
            catch ( Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close( );
            }

            #endregion

            /*--- --------------------------------------------------------------------------------------------------------------- ---*/

            /*--- Lê as tabelas Statistic e Failures e captura o indice do ultimo registro ---*/

            #region [Captura o indice (chave primaria) do ultimo Registro]

            try
            {
                SQLiteConnection ligacao = new SQLiteConnection(@"Data Source=\\10.8.2.73\engl06$\TestTool\SPI_Test\SPI_DB\"+nomeDB+"; Version=3;", true);  //cria a conexão com o DB
                ligacao.Open( );                                                                                                                            //estabelece a conexão

                SQLiteDataAdapter index_Statistic = new SQLiteDataAdapter("SELECT MAX(Statistic_ID) FROM Statistic", ligacao);
                DataTable tabelaAux = new DataTable();                                                                                                      //cria tabela auxiliar
                index_Statistic.Fill(tabelaAux);                                                                                                            //imputa os valores do banco de dados na tabela

                int numeroLinhas = tabelaAux.Rows.Count;                                                                                                    //conta quantas linhas foram retornadas da consulta

                string conteudoS = tabelaAux.Rows[0][0].ToString( );

                if ( conteudoS != "" )
                {
                    indexStatistic_ID = Int32.Parse(tabelaAux.Rows[0][0].ToString( ));                                                                      //imputa o ultimo valor do indice da tabela Statistic
                    indexStatistic_ID++;                                                                                                                    //incrementa o indice Statistic

                    /*--- Captura o ultimo indice da tabela Failures ---*/

                    SQLiteDataAdapter index_Failures = new SQLiteDataAdapter("SELECT MAX(Failures_ID) FROM Failures", ligacao);                              //seleciona o ultimo valor da chave primaria (Failure_ID)
                    DataTable tabelaAuxF = new DataTable();                                                                                                 //cria tabela auxiliar
                    index_Failures.Fill(tabelaAuxF);                                                                                                        //execulta a consulta e insere o resultado na tabela

                    int numeroFalhas = tabelaAuxF.Rows.Count;                                                                                               //conta o numero de linhas da tabela auxiliar

                    indexFailure_ID = Int32.Parse(tabelaAuxF.Rows[0][0].ToString( ));                                                                       //imputa o valor do ultimo index da tabela na varialve de index
                    indexFailure_ID++;                                                                                                                      //incrementa a mesma

                    /*--- --------------------------------------------------------------------------------------------------------------- ---*/
                }

                ligacao.Clone( );
            }
            catch ( Exception ex )
            {

                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close( );
            }

            #endregion

            /*--- --------------------------------------------------------------------------------------------------------------- ---*/

            /*--- Verifica a existencia de um log de teste gerado pela SPI---*/

            string[] arquivos = Directory.GetFiles(logpath, "*.XML");
            foreach (var log in arquivos)                                                       //este laço será executado para cada um dos arquivos .txt encontrados 
            {
                try
                {
                    /*--- Lê as informações do log da SPI ---*/

                    #region [Lê as informações do log da SPI]

                    System.Threading.Thread.Sleep(5000);                                              //delay de 5000 ms

                    XmlDocument logXML = new XmlDocument();                                         //instancia o objeto que vai conter o documento XML

                    logXML.Load(log);                                                               //carrega o arquivo XML dentro do objeto

                    /*--- Lê informações comuns a todas as placas do log ---*/

                    string StartTime = "'" + logXML.SelectSingleNode("TestXML").Attributes[0].Value + "'";

                    string EndTime = "'" + logXML.SelectSingleNode("TestXML").Attributes[1].Value + "'";

                    string Part_Number = "'" + logXML.SelectSingleNode("TestXML").Attributes[2].Value.Split('%')[0] + "'";      //captura o código do part number

                    string Station = "'" + logXML.SelectSingleNode("TestXML").Attributes[3].Value + "'" ;                       //captura a estação de teste

                    string Top_Bot = "'" + logXML.SelectSingleNode("TestXML").Attributes[2].Value.Split('%')[1] + "'";                        //captura a indicação de qual lado da placa esta sendo testada

                    DateTime Date_Hour = DateTime.Parse(logXML.SelectSingleNode("TestXML").Attributes[0].Value, cultureUSA);    //captura a data e hora do teste no formato DateTime

                    string Date = "'" + Date_Hour.ToString("dd/MM/yyyy") + "'";                                                 //converte a data para o formato de string

                    string Hour = "'" + Date_Hour.ToString("HH:mm:ss") + "'";                                                   //converte a hora para string

                    string User = "'" + logXML.SelectSingleNode("TestXML").Attributes[5].Value + "'";                           //captura o ID do operador

                    Int32 Component_Number = Int32.Parse(logXML.SelectSingleNode("TestXML").Attributes[7].Value);               //captura o numero de componentes inspecionados pela SPI

                    Int32 Defect_Number = Int32.Parse(logXML.SelectSingleNode("TestXML").Attributes[8].Value);                  //captura o numero de componentes reprovados

                    //strings panelstates = logXML.SelectSingleNode("TestXML").Attributes[4].Value;                             //captura o status geral do painel


                    /*--- Lê informações específicas de cada placa ---*/

                    SQLiteConnection ligacao = new SQLiteConnection(@"Data Source=\\10.8.2.73\engl06$\TestTool\SPI_Test\SPI_DB\"+nomeDB+"; Version=3;", true); //cria a conexão com o DB
                    ligacao.Open( );                                    //abrir a conexão com o banco de dados

                    int numeroPlacas = logXML.SelectSingleNode("TestXML").ChildNodes.Count;                                     //captura o número de placas existente no log
                    for ( int i = 0; i < numeroPlacas; i++ )                                                                    //um loop para cada placa
                    {
                        string Serial_Number = "'" + logXML.SelectSingleNode("TestXML").ChildNodes[i].Attributes[0].Value + "_" +
                                                logXML.SelectSingleNode("TestXML").ChildNodes[i].Attributes[1].Value + "'";       //captura o serial number da placa mais o numero da mesma no painel
                        string S_Status = "'" + logXML.SelectSingleNode("TestXML").ChildNodes[i].Attributes[2].Value + "'";

                        //string imulti = logXML.SelectSingleNode("TestXML").ChildNodes[i].Attributes[1].Value;

                        /*--- Alimenta o Banco de Dados  Statistic ---*/

                        #region [Alimenta o Banco de Dados  Statistic]

                        string queryStatistic = " INSERT INTO Statistic" +
                                                "( " +
                                                    "Statistic_ID, " +
                                                    "Serial_Number, " +
                                                    "Part_Number, " +
                                                    "Station, " +
                                                    "Top_Bot, " +
                                                    "Date, " +
                                                    "Hour, " +
                                                    "S_Status, " +
                                                    "User, "+
                                                    "Component_Number, "+
                                                    "Defect_Number "+
                                                ") " +
                                              "VALUES "+
                                                "( " +
                                                    indexStatistic_ID   + ", " +
                                                    Serial_Number       + ", " +
                                                    Part_Number         + ", " +
                                                    Station             + ", " +
                                                    Top_Bot             + ", " +
                                                    Date                + ", " +
                                                    Hour                + ", " +
                                                    S_Status            + ", " +
                                                    User                + ", " +
                                                    Component_Number    + ", " +
                                                    Defect_Number       +
                                                ")";

                        SQLiteCommand comandoStatistic = new SQLiteCommand(queryStatistic, ligacao);     //cria o comando SQLite
                        comandoStatistic.ExecuteNonQuery( );                                             //executa o comando SQLite
                        comandoStatistic.Dispose( );

                        #endregion

                        /*--- --------------------------------------------------------------------------------------------------------------- ---*/

                        if ( (S_Status == "'FAIL'") || (S_Status == "'RPASS'") )                                                                //se a placa for RPASS ou FAIL verifica os componentes que falharam
                        {
                            int numeroFalhas = logXML.SelectSingleNode("TestXML").ChildNodes[i].ChildNodes.Count;                           //captura o número de componentes com falhas na placa
                            for ( int a = 0; a < numeroFalhas; a++ )
                            {
                                string F_Status = "'" + logXML.SelectSingleNode("TestXML").ChildNodes[i].ChildNodes[a].Attributes[0].Value + "'";       //captura o status de falha do componente RPASS ou FAIL
                                string Component = "'" + logXML.SelectSingleNode("TestXML").ChildNodes[i].ChildNodes[a].Attributes[1].Value + "'";      //captura o componente que falhou
                                string PAD = "'" + logXML.SelectSingleNode("TestXML").ChildNodes[i].ChildNodes[a].Attributes[2].Value + "'";            //captura o numero do PAD do componente
                                string Type_Fail = "'" + logXML.SelectSingleNode("TestXML").ChildNodes[i].ChildNodes[a].Attributes[3].Value + "'";      //captura o tipo de falha


                                /*--- Alimenta o Banco de Dados  Failures ---*/

                                #region [Alimenta o Banco de Dados  Failures]

                                string queryFailures = " INSERT INTO Failures" +
                                                        "( " +
                                                            "Failures_ID, " +
                                                            "Statistic_ID, " +
                                                            "F_Status, " +
                                                            "Component, " +
                                                            "Type_Fail, " +
                                                            "PAD " +
                                                        ") " +
                                                    "VALUES "+
                                                        "( " +
                                                            indexFailure_ID     + "," +
                                                            indexStatistic_ID   + "," +
                                                            F_Status            + "," +
                                                            Component           + "," +
                                                            Type_Fail           + "," +
                                                            PAD                 +
                                                        ")";

                                SQLiteCommand comandoFailures = new SQLiteCommand(queryFailures, ligacao);      //cria o comando SQLite
                                comandoFailures.ExecuteNonQuery( );                                             //executa o comando SQLite
                                comandoFailures.Dispose( );

                                indexFailure_ID++;                                                              //incrementa o indice da tabela

                                #endregion

                                /*--- --------------------------------------------------------------------------------------------------------------- ---*/
                            }
                        }

                        indexStatistic_ID++;                                                                    //incrementa o indice da tabela
                    }

                    ligacao.Clone( );                                                                           //encerra a ligação com o banco de dados

                    /*--- --------------------------------------------------------------------------------------------------------------- ---*/

                    #endregion

                    /*--- --------------------------------------------------------------------------------------------------------------- ---*/
                    
                }
                catch ( Exception ex )
                {
                    MessageBox.Show("Flag 1 - " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close( );
                }

                try
                {
                    System.Threading.Thread.Sleep(100);                                              //delay de 100 ms
                    File.Delete(log);   //deleta o log gerado pela SPI
                }
                catch ( Exception )
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1500);                                              //delay de 1500 ms
                        File.Delete(log);   //deleta o log gerado pela SPI
                    }
                    catch ( Exception ex)
                    {

                        MessageBox.Show("Delete Log - " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close( );
                    }
                    
                }
                
            }

        }

        /**************************************************************************************************************************/
    }
}




