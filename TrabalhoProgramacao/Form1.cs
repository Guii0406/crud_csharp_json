using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TrabalhoProgramacao
{
    public partial class Form1 : Form
    {
        string diretorio = $@"{AppDomain.CurrentDomain.BaseDirectory}\ArquivoFilmes.json";
        List<Pessoa> listaPessoas = new List<Pessoa>();
        List<Pessoa> listaPessoasFiltrada = new List<Pessoa>();
        Point DragCursor;
        Point DragForm;
        bool Dragging;
        public Form1()
        {
            InitializeComponent();
            this.ActiveControl = textBoxNome;
            LerJson();
            button8.Enabled = false;
        }
        //LER ARQUIVO JSON;
        private void LerJson()
        {
            try
            {
                Arquivo.Ler(ref listaPessoas, diretorio);
                MostrarPesssoas(listaPessoas);
            }
            catch (Exception erro) {  }
        }
        //ADICIONAR TAREFA NA LISTA;
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNome.Text) || string.IsNullOrWhiteSpace(textBoxSobrenome.Text) || string.IsNullOrWhiteSpace(maskedTextBoxCelular.Text))
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }
            listaPessoas.Add(new Pessoa(textBoxNome.Text, textBoxSobrenome.Text, maskedTextBoxCelular.Text));
            LimparCamposInserir();
            MostrarPesssoas(listaPessoas);
            textBoxNome.Focus();
        }
        //LIMPAR CAMPOS DE ADIÇÃO;
        private void LimparCamposInserir()
        {
            textBoxNome.Clear();
            textBoxSobrenome.Clear();
            maskedTextBoxCelular.Clear();
        }
        //EDITAR PESSOAS DA LISTA
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNome2.Text) || string.IsNullOrWhiteSpace(textBoxSobrenome2.Text) || string.IsNullOrWhiteSpace(maskedTextBoxCelular2.Text))
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }

            listaPessoas[int.Parse(textBoxMostrar.Text)].AlterarPessoa(textBoxNome2.Text, textBoxSobrenome2.Text, maskedTextBoxCelular2.Text);

            LimparCamposAlterar();
            MostrarPesssoas(listaPessoas);
        }
        //REMOVER PESSOAS DA LISTA;
        private void button4_Click(object sender, EventArgs e)
        {
            listaPessoas.RemoveAt(int.Parse(textBoxMostrar.Text));
            LimparCamposAlterar();
            MostrarPesssoas(listaPessoas);
        }
        //LIMPAR CAMPOS DE EDIÇÃO;
        private void LimparCamposAlterar()
        {
            textBoxNome2.Clear();
            textBoxSobrenome2.Clear();
            maskedTextBoxCelular2.Clear();
            textBoxMostrar.Clear();
        }
        //MOSTRAR LISTAS NO LISTVIEW;
        private void MostrarPesssoas(List<Pessoa> lista)
        {
            listView1.Items.Clear();
            foreach (Pessoa p in lista)
            {
                string[] item = new string[4] { listaPessoas.IndexOf(p).ToString(), p.Nome, p.Sobrenome, p.Celular };
                listView1.Items.Add(new ListViewItem(item));
            }
            label9.Visible = false;
        }
        //PESQUISAR PESSOAS DENTRO DA LISTA;
        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPesquisar.Text) && string.IsNullOrWhiteSpace(maskedTextBoxPesquisar.Text))
            {
                MessageBox.Show("Preencha o campo de pesquisa");
                return;
            }
            if (comboBox1.Text == "Nome")
            {
                listaPessoasFiltrada = listaPessoas.Where(x => x.Nome.Equals(textBoxPesquisar.Text)).ToList();
            }
            else if (comboBox1.Text == "Sobrenome")
            {
                listaPessoasFiltrada = listaPessoas.Where(x => x.Sobrenome.Equals(textBoxPesquisar.Text)).ToList();
            }
            else if (comboBox1.Text == "Celular")
            {
                listaPessoasFiltrada = listaPessoas.Where(x => x.Celular.Equals(maskedTextBoxPesquisar.Text)).ToList();
            }
            else
            {
                return;
            }
            MostrarPesssoas(listaPessoasFiltrada);
        }
        //VERIFICAR QUAL CAMPO O USUÁRIO DESEJA PESQUISAR;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Celular")
            {
                textBoxPesquisar.Visible = false;
                maskedTextBoxPesquisar.Visible = true;
                maskedTextBoxPesquisar.Focus();
            }
            else
            {
                textBoxPesquisar.Visible = true;
                maskedTextBoxPesquisar.Visible = false;
                textBoxPesquisar.Focus();
            }
        }
        //LIMPAR FILTROS DE PESQUISA;
        private void button7_Click(object sender, EventArgs e)
        {
            MostrarPesssoas(listaPessoas);

        }
        //MOSTRAR DADOS DA PESSOA NOS CAMPOS DE EDIÇÃO;
        private void textBoxMostrar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int index = int.Parse(textBoxMostrar.Text);
                if (index > listaPessoas.Count - 1 || textBoxMostrar.Text == "")
                {
                    LimparCamposAlterar();
                    HabilitarCampos(false);
                    label9.Visible = true;
                    return;
                }
                label9.Visible = false;
                textBoxNome2.Text = listaPessoas[index].Nome;
                textBoxSobrenome2.Text = listaPessoas[index].Sobrenome;
                maskedTextBoxCelular2.Text = listaPessoas[index].Celular;
                HabilitarCampos(true);

            }
            catch (Exception erro)
            {
                LimparCamposAlterar();
                HabilitarCampos(false);
                label9.Visible = true;
                return;
            }

        }
        //HABILITAR/DESABILITAR CAMPOS;
        private void HabilitarCampos(bool a)
        {
            textBoxNome2.Enabled = a;
            textBoxSobrenome2.Enabled = a;
            maskedTextBoxCelular2.Enabled = a;
            button2.Enabled = a;
            button4.Enabled = a;
            //label9.Visible = false;
        }
        //MOSTRAR DADOS DA PESSOA NOS CAMPOS DE EDIÇÃO POR MEIO DO LISTVIEW;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectTab(1);
                textBoxMostrar.Text = listView1.SelectedItems[0].SubItems[0].Text;
            }
            catch (Exception erro) { }
        }
        //SALVAR E SAIR;
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja mesmo fechar a aplicação?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    Arquivo.Salvar(listaPessoas, diretorio);
                    Application.Exit();
                }
                catch (Exception erro) { MessageBox.Show("Erro inesperado!!"); }
            }
            else
            {
                return;
            }

        }
        //BOTÕES DAS GUIAS;
        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
            button8.Enabled = false;
            button6.Enabled = true;
            button9.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(1);
            button6.Enabled = false;
            button9.Enabled = true;
            button8.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(2);
            button9.Enabled = false;
            button6.Enabled = true;
            button8.Enabled = true;
        }
        //MOVER FORMULÁRIO COM PAINEL
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Dragging = false;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Dragging = true;
            DragCursor = Cursor.Position;
            DragForm = this.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Dragging == true)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(DragCursor));
                this.Location = Point.Add(DragForm, new Size(dif));
            }
        }
    }
}
