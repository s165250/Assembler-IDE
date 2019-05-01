using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssemblerIDE
{
    public partial class Assembly : Form
    {
        public Assembly()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(60, 63, 65);
            this.TextEditor.BackColor = Color.FromArgb(43, 43, 43);
            this.TextEditor.SelectionColor = Color.FromArgb(204, 120, 50);
            this.TextEditor.AppendText((linecounter).ToString() + ".\t");
            this.TextEditor.SelectionColor = Color.FromArgb(169, 183, 198);
            this.menuStrip1.BackColor = this.BackColor = Color.FromArgb(60, 63, 65);
            this.menuStrip1.ForeColor = Color.FromArgb(169, 183, 198);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentLine = 0;
            System.IO.StringReader Reader = new System.IO.StringReader(this.TextEditor.Text);
            string line = null;
            bool suceeded = true;
            while(true)
            {
                line = Reader.ReadLine();
                if(line != null)
                {
                    currentLine++;
                    line = line.Substring(line.IndexOf("\t") + 1);
                    if(!isOrderRight(line))
                    {
                        suceeded = false;
                    }
                }
                else
                {
                    break;
                }
            }
            if(suceeded)
            {
                MessageBox.Show("Kompilacja udana!");
                builded = true;
                ErrorList += "Kompilacja zakończona powodzeniem!\n";
            }
            else
            {
                MessageBox.Show("Kompilacja się nie powiodła!");
            }
            this.errorBox.Visible = true;
            this.errorBox.Text = ErrorList; 
            ErrorList = "";
            Reader.Close();
        }

        private void TextEditor_Enter(object sender, EventArgs e)
        {
            if (this.TextEditor.Text == "Write your code here!")
            {
                this.TextEditor.Clear();
            }
        }
        private void TextEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if(this.TextEditor.Text == "1.\t")
                {
                    e.Handled = true;
                }
            }
        }

        private void TextEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ENTER)
            {
                e.Handled = true;
                this.TextEditor.SelectionColor = Color.FromArgb(204, 120, 50);
                this.TextEditor.AppendText((++linecounter).ToString() + ".\t");
                this.TextEditor.SelectionColor = Color.FromArgb(169, 183, 198);
            }
            else if(e.KeyChar == BACKSPACE)
            {
                e.Handled = true;
                if (this.TextEditor.Text.Length > 3)
                {
                    LastChars = this.TextEditor.Text.Substring(this.TextEditor.Text.Length - linecounter.ToString().Length - 2);
                    if (LastChars == ("\n" + linecounter.ToString() + "."))
                    {
                        this.TextEditor.Text = this.TextEditor.Text.Remove(this.TextEditor.Text.Length - linecounter.ToString().Length - 2);
                        this.TextEditor.SelectionStart = this.TextEditor.Text.Length;
                        linecounter--;
                    }
                }
                else
                {
                    this.TextEditor.SelectionColor = Color.FromArgb(169, 183, 198);
                }
            }
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            builded = false;
        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.TextEditor.Text != "1.\t")
            {
                this.saveFileDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nie można zapisać pustego programu!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                this.TextEditor.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.UnicodePlainText);
            }
            catch(System.IO.IOException er)
            {
                MessageBox.Show("Wystąpił błąd! Treść błędu: " + er.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(builded)
            {
                System.IO.StringReader Reader = new System.IO.StringReader(this.TextEditor.Text);
                string line = null;
                while (true)
                {
                    line = Reader.ReadLine();
                    if (line != null)
                    {
                        currentLine++;
                        line = line.Substring(line.IndexOf("\t") + 1);
                        executeOrder(line);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Najpierw należy skompilować program!");
            }
        }

        private void plikToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            this.plikToolStripMenuItem.BackColor = Color.FromArgb(70, 73, 75);
        }

        private void plikToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            this.plikToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
        }

        private void wczytajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(this.openFileDialog1.FileName, System.Text.Encoding.Unicode);
            string line = "";
            string linenumber = "";
            this.TextEditor.Text = "";
            while (true)
            {
                line = reader.ReadLine();
                if(line != null)
                {
                    linenumber = line.Substring(0, line.IndexOf("\t"));
                    line = line.Substring(line.IndexOf("\t"));
                    this.TextEditor.SelectionColor = Color.FromArgb(204, 120, 50);
                    this.TextEditor.AppendText(linenumber);
                    this.TextEditor.SelectionColor = Color.FromArgb(169, 183, 198);
                    this.TextEditor.AppendText(line);
                }
                else
                {
                    break;
                }
            }
            reader.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Rejestr AX = " + registers[0].ToString() + "\n");
            Console.WriteLine("Rejestr BX = " + registers[1].ToString() + "\n");
            Console.WriteLine("Rejestr CX = " + registers[2].ToString() + "\n");
            Console.WriteLine("Rejestr DX = " + registers[3].ToString() + "\n");

        }
    }
}
