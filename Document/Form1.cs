﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Document.Form1;

namespace Document
{
    public partial class Form1 : Form
    {

        private Stack<Erro> erros = new Stack<Erro>();
        public Form1()
        {
            InitializeComponent();
            InitializeView();        
        }

        private void InitializeView()
        {
            listView1.View = View.Details;
            listView1.Columns.Add("Text message:", -2, HorizontalAlignment.Right);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                // Abre uma janela de diálogo para salvar o arquivo
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Text file|*.txt";
                saveFileDialog1.Title = "Save text file";
                saveFileDialog1.ShowDialog();

                // Se o usuário pressionar OK
                if (saveFileDialog1.FileName != "")
                {
                    // Cria um StreamWriter para escrever no arquivo
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        //Escreve cada item do ListView no arquivo
                        foreach (ListViewItem item in listView1.Items)
                        {
                            sw.WriteLine(item.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                erros.Push(new Erro(ex.Message, DateTime.Now));
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                //Abre uma janela de dialogo para selecionar o arquivo
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Text file|*.txt";
                openFileDialog1.Title = "Select text file";
                openFileDialog1.ShowDialog();

                //Se o usuário pressionar OK
                if (openFileDialog1.FileName != "")
                {
                    listView1.Items.Clear();
                    //Cria um StreamReader para ler o arquivo
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line;
                        // Lê cada linha do arquivo e adiciona ListView
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            listView1.Items.Add(new ListViewItem(parts));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                erros.Push(new Erro(ex.Message, DateTime.Now));
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            listView1.Items.Add("Edit Here (click 3x):");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            listView1.View = View.Details;

            if (listView1.Items.Count > 0)
                listView1.Items.RemoveAt(listView1.Items.Count - 1);
        }

        private void btnErros_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Erro erro in erros)
            {
                sb.AppendLine($"[{erro.Horario}] {erro.Mensagem}");
            }
            MessageBox.Show(sb.ToString(), "Error Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
 }

        public class Erro
        {
            public string Mensagem { get; set; }
            public DateTime Horario { get; set; }

            public Erro(string mensagem, DateTime horario)
            {
                Mensagem = mensagem;
                Horario = horario;
            }
        }
    }

