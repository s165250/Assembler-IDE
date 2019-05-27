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
            this.TextEditor.ForeColor = Color.FromArgb(169, 183, 198);
            this.TextEditor.AppendText((linecounter).ToString() + ".\t");
            this.richTextBox1.BackColor = Color.FromArgb(60, 63, 65);
            this.richTextBox1.ForeColor = Color.FromArgb(169, 183, 198);
            this.stepIND.BackColor = Color.FromArgb(60, 63, 65);
            this.stepIND.ForeColor = Color.FromArgb(204, 120, 50);
            this.ahTX.BackColor = Color.FromArgb(43, 43, 43);
            this.alTX.BackColor = Color.FromArgb(43, 43, 43);
            this.bhTX.BackColor = Color.FromArgb(43, 43, 43);
            this.blTX.BackColor = Color.FromArgb(43, 43, 43);
            this.chTX.BackColor = Color.FromArgb(43, 43, 43);
            this.clTX.BackColor = Color.FromArgb(43, 43, 43);
            this.dhTX.BackColor = Color.FromArgb(43, 43, 43);
            this.dlTX.BackColor = Color.FromArgb(43, 43, 43);
            this.ahTX.ForeColor = Color.FromArgb(204, 120, 50);
            this.alTX.ForeColor = Color.FromArgb(204, 120, 50);
            this.ahTX.ForeColor = Color.FromArgb(204, 120, 50);
            this.bhTX.ForeColor = Color.FromArgb(204, 120, 50);
            this.blTX.ForeColor = Color.FromArgb(204, 120, 50);
            this.chTX.ForeColor = Color.FromArgb(204, 120, 50);
            this.clTX.ForeColor = Color.FromArgb(204, 120, 50);
            this.dhTX.ForeColor = Color.FromArgb(204, 120, 50);
            this.dlTX.ForeColor = Color.FromArgb(204, 120, 50);
            this.ahTX.SelectionAlignment = HorizontalAlignment.Center;
            this.alTX.SelectionAlignment = HorizontalAlignment.Center;
            this.bhTX.SelectionAlignment = HorizontalAlignment.Center;
            this.blTX.SelectionAlignment = HorizontalAlignment.Center;
            this.chTX.SelectionAlignment = HorizontalAlignment.Center;
            this.clTX.SelectionAlignment = HorizontalAlignment.Center;
            this.dhTX.SelectionAlignment = HorizontalAlignment.Center;
            this.dlTX.SelectionAlignment = HorizontalAlignment.Center;
            this.AH.Text = "00000000";
            this.AL.Text = "00000000";
            this.BH.Text = "00000000";
            this.BL.Text = "00000000";
            this.CH.Text = "00000000";
            this.CL.Text = "00000000";
            this.DH.Text = "00000000";
            this.DL.Text = "00000000";
            this.AH.SelectionAlignment = HorizontalAlignment.Center;
            this.AL.SelectionAlignment = HorizontalAlignment.Center;
            this.BH.SelectionAlignment = HorizontalAlignment.Center;
            this.BL.SelectionAlignment = HorizontalAlignment.Center;
            this.CH.SelectionAlignment = HorizontalAlignment.Center;
            this.CL.SelectionAlignment = HorizontalAlignment.Center;
            this.DH.SelectionAlignment = HorizontalAlignment.Center;
            this.DL.SelectionAlignment = HorizontalAlignment.Center;
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
                this.TextEditor.SelectionColor = Color.FromArgb(169, 183, 198);
                this.TextEditor.AppendText((++linecounter).ToString() + ".\t");
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
            }
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            builded = false;
            if(this.TextEditor.Text == "" || this.TextEditor.Text == "1" || this.TextEditor.Text == "1.")
            {
                this.TextEditor.Text = "1.\t";
                this.TextEditor.SelectionStart = this.TextEditor.Text.Length;
            }
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
            this.TextEditor.Text = "";
            linecounter = 1;
            while (true)
            {
                line = reader.ReadLine();
                if(line != null)
                {
                    if (line != "")
                    {
                        linecounter++;
                    }
                    this.TextEditor.AppendText(line + "\n");
                }
                else
                {
                    break;
                }
            }
            this.TextEditor.Text = this.TextEditor.Text.Remove(0, 3);
            this.TextEditor.Text.Remove(this.TextEditor.Text.Length - 4, 4);
            this.TextEditor.SelectionStart = this.TextEditor.Text.Length;
            this.TextEditor.AppendText(linecounter.ToString() + ".\t");
            reader.Close();
        }

        private void pracaKrokowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(builded)
            {
                this.nextStep.Visible = true;
                this.nextStep.Enabled = true;
            }
            else
            {
                MessageBox.Show("Najpierw należy skompilować program!", "Uwaga!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void nextStep_Click(object sender, EventArgs e)
        {
            string line = "";
            if (currentStep == 0)
            {
                this.stepReader = new System.IO.StringReader(this.TextEditor.Text);
                this.stepIND.Visible = true;
            }
            currentStep++;
            this.stepIND.Text = "Aktualnie wykonywany rozkaz: " + currentStep.ToString();
            line = this.stepReader.ReadLine();
            if(line!=null)
            {
                line = line.Substring(line.IndexOf("\t") + 1);
                executeOrder(line);
            }
            else
            {
                this.errorBox.AppendText("Zakończono wykonywanie programu!");
                this.stepReader.Close();
                currentStep = 0;
                this.stepIND.Visible = false;
            }
        }

        private void uruchomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click_1(null, null);
        }
    }
}