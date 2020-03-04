using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace SPI_ROBOT
{
    public partial class HabilitarOperacaoOffline : Form
    {
        public HabilitarOperacaoOffline()
        {
            InitializeComponent();
        }

        bool checked_folder = false;
        int countictop = 0;
        bool releaseop = false;

        /*************************************************************************************************************************/
        /*--- Identificação do Suporte ---*/
        private void Tb_suporte_TextChanged(object sender, EventArgs e)
        {
            if (tb_suporte.Text.Length <= 5)
            {

            }
            else
            {
                try                           //passa o nº de linhas do arquivo AOI_OpDatabase para countictop
                {
                    //countictop = File.ReadLines(@"\\10.8.2.73\engl06$\TestTool\AOI_Test\Parameters\Users\AOI_SuporteDatabase.csv").Count();
                    countictop = File.ReadLines(@"C:\EngTeste\Parameters\Users\AOI_SuporteDatabase.csv").Count();
                    checked_folder = true;
                }
                catch (Exception)              //informa que o bd de usuários não pode ser acessado
                {
                    //MessageBox.Show("ARQUIVO  \\10.8.2.73\\engl06$\\TestTool\\AOI_Test\\Parameters\\Users\\AOI_SuporteDatabase.csv NÃO ENCONTRADO!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("ARQUIVO  C:\\EngTeste\\Parameters\\Users\\AOI_SuporteDatabase.csv NÃO ENCONTRADO!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    checked_folder = false;
                }                                                       //passa o nº de linhas do arquivo AOI_OpDatabase para countictop

                if (checked_folder)            // se db de usuários foi encontrado
                {
                    for (int x = 0; x < countictop; x++)
                    {
                        //string lineread0 = File.ReadLines(@"\\10.8.2.73\engl06$\TestTool\AOI_Test\Parameters\Users\AOI_SuporteDatabase.csv").Skip(x).Take(1).First(); //atribui a lineread0 uma linha do arquivo de db
                        string lineread0 = File.ReadLines(@"C:\EngTeste\Parameters\Users\AOI_SuporteDatabase.csv").Skip(x).Take(1).First();

                        if (lineread0.Contains(tb_suporte.Text))
                        {
                            string[] nome = lineread0.Split(',');      //quebra a linha de acordo com o separador ','

                            releaseop = true;                           //se o ID do operador inserido em tb_operator for encontrado no arquivo torna realeaseop vedadeiro
                        }
                    }

                    if (releaseop == true)
                    {

                        bt_HabilitarOffline.Visible = true;                                                 //visualizar o botão de habilitação de modo offline
                        bt_HabilitarOffline.Select();

                    }
                    else
                    {
                        MessageBox.Show("USUÁRIO NÃO ENCONTRADO!", "ATENÇÂO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
        }
        /*************************************************************************************************************************/

        /*************************************************************************************************************************/
        /*---  ---*/
        private void Bt_HabilitarOffline_Click(object sender, EventArgs e)
        {
            if (File.Exists("operacao.dll")) File.Delete("operacao.dll");

            if (!File.Exists("operacao.dll"))
            {
                File.WriteAllText("operacao.dll", "modo offline" + Environment.NewLine);
            }

            Close();

        }

        private void Fechar_Click(object sender, EventArgs e)
        {

            if (File.Exists("operacao.dll")) File.Delete("operacao.dll");

            if (!File.Exists("operacao.dll"))
            {
                File.WriteAllText("operacao.dll", "modo online" + Environment.NewLine);
            }

            Close();
        }
        /*************************************************************************************************************************/
    }
}
