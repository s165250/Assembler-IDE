using System.Collections.Generic;

namespace AssemblerIDE
{
    partial class Assembly
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private const char BACKSPACE = (char)8;
        private const char ENTER = (char)13;
        private int linecounter = 1;
        private int currentLine = 0;
        //private int TextLength = 0;
        //private int LineLength = 3;
        private int[] registers = new int[4];
        private string LastChars = "0";
        private string ErrorList = "";
        private bool builded = false;
        Dictionary<string, int> register = new Dictionary<string, int>()
                                {
                                    {"AX", 0},
                                    {"BX", 1},
                                    {"CX", 2},
                                    {"DX", 3}
                                };
        private System.ComponentModel.IContainer components = null;

        private bool isOrderRight(string line)
        {
            string order = "";
            string[] operands;
            int operandsNB = 0;

            if (line.IndexOf("\t") != -1)
            {
                order = line.Substring(0, line.IndexOf("\t"));
                order = order.ToUpper();
                operands = line.Substring(line.IndexOf("\t") + 1).Split(',');
                operandsNB = operands.Length;
                for (int i = 0; i < operandsNB; i++)
                {
                    if(operands[i].IndexOf(" ") != -1)
                    {
                        operands[i] = operands[i].Replace(" ", "");
                    }
                    operands[i] = operands[i].ToUpper();
                }
                switch (order)
                {
                    case "MOV":
                        {
                            if (operandsNB == 2)
                            {
                                if(operands[0] == "AX" || operands[0] == "BX" || operands[0] == "CX" || operands[0] == "DX")
                                {
                                    int result;
                                    if (operands[1] == "AX" || operands[1] == "BX" || operands[1] == "CX" || operands[1] == "DX" || int.TryParse(operands[1], out result))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        ErrorList += "Błąd operandów! Linia: " + currentLine.ToString() + "\n";
                                        return false;
                                    }
                                }
                                else
                                {
                                    ErrorList += "Błąd operandów! Linia: " + currentLine.ToString() + "\n";
                                    return false;
                                }
                            }
                            else
                            {
                                ErrorList += "Błąd operandów! Linia: " + currentLine.ToString() + "\n";
                                return false;
                            }
                        }
                    case "ADD":
                        {
                            if (operandsNB == 2)
                            {
                                if (operands[0] == "AX" || operands[0] == "BX" || operands[0] == "CX" || operands[0] == "DX")
                                {
                                    int result;
                                    if (operands[1] == "AX" || operands[1] == "BX" || operands[1] == "CX" || operands[1] == "DX" || int.TryParse(operands[1], out result))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        ErrorList += "Błąd operandów! Linia: " + currentLine.ToString() + "\n";
                                        return false;
                                    }
                                }
                                else
                                {
                                    ErrorList += "Błąd operandów! Linia: " + currentLine.ToString() + "\n";
                                    return false;
                                }
                            }
                            else
                            {
                                ErrorList += "Błąd operandów! Linia: " + currentLine.ToString() + "\n";
                                return false;
                            }
                        }
                    case "SUB":
                        {
                            if (operandsNB == 2)
                            {
                                if (operands[0] == "AX" || operands[0] == "BX" || operands[0] == "CX" || operands[0] == "DX")
                                {
                                    int result;
                                    if (operands[1] == "AX" || operands[1] == "BX" || operands[1] == "CX" || operands[1] == "DX" || int.TryParse(operands[1], out result))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        ErrorList += "Błąd operandów! Linia: " + currentLine.ToString() + "\n";
                                        return false;
                                    }
                                }
                                else
                                {
                                    ErrorList += "Błąd operandów! Linia: " + currentLine.ToString() + "\n";
                                    return false;
                                }
                            }
                            else
                            {
                                ErrorList += "Błąd operandów! Linia: " + currentLine.ToString() + "\n";
                                return false;
                            }
                        }
                    default:
                        ErrorList += "Nieznany rozkaz! Linia: " + currentLine.ToString() + "\n";
                        return false;
                }
            }
            else
            {
                ErrorList += ("Błąd składniowy! Linia: " + currentLine.ToString() + "\n");
                return false;
            }
        }

        void executeOrder(string line)
        {
            string[] operands;
            string order;
            bool isSecondOperandRegister = false;
            int secondOperand = 0;
            int operandsNB;
            order = line.Substring(0, line.IndexOf("\t"));
            order = order.ToUpper();
            operands = line.Substring(line.IndexOf("\t") + 1).Split(',');
            operandsNB = operands.Length;
            for (int i = 0; i < operandsNB; i++)
            {
                if (operands[i].IndexOf(" ") != -1)
                {
                    operands[i] = operands[i].Replace(" ", "");
                }
                operands[i] = operands[i].ToUpper();
            }
            if (operands.Length == 2)
            {
                if(int.TryParse(operands[1], out secondOperand))
                {
                    isSecondOperandRegister = false;
                }
                else
                {
                    isSecondOperandRegister = true;
                }
            }
            else
            {
                isSecondOperandRegister = false;
            }
            switch (order)
            {
                case "MOV":
                    {
                        if(isSecondOperandRegister)
                        {
                            this.registers[register[operands[0]]] = this.registers[register[operands[1]]];
                        }
                        else
                        {
                            this.registers[register[operands[0]]] = secondOperand;
                        }
                        break;
                    }
                case "ADD":
                    {
                        if (isSecondOperandRegister)
                        {
                            this.registers[register[operands[0]]] += this.registers[register[operands[1]]];
                        }
                        else
                        {
                            this.registers[register[operands[0]]] += secondOperand;
                        }
                        break;
                    }
                case "SUB":
                    {
                        if (isSecondOperandRegister)
                        {
                            this.registers[register[operands[0]]] -= this.registers[register[operands[1]]];
                        }
                        else
                        {
                            this.registers[register[operands[0]]] -= secondOperand;
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextEditor = new System.Windows.Forms.RichTextBox();
            this.Compile = new System.Windows.Forms.Button();
            this.errorBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.plikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wczytajToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zapiszToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextEditor
            // 
            this.TextEditor.AcceptsTab = true;
            this.TextEditor.BackColor = System.Drawing.Color.DarkOrange;
            this.TextEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TextEditor.ForeColor = System.Drawing.Color.Cornsilk;
            this.TextEditor.Location = new System.Drawing.Point(20, 50);
            this.TextEditor.Margin = new System.Windows.Forms.Padding(5);
            this.TextEditor.Name = "TextEditor";
            this.TextEditor.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.TextEditor.Size = new System.Drawing.Size(450, 450);
            this.TextEditor.TabIndex = 0;
            this.TextEditor.Text = "";
            this.TextEditor.WordWrap = false;
            this.TextEditor.TextChanged += new System.EventHandler(this.TextEditor_TextChanged);
            this.TextEditor.Enter += new System.EventHandler(this.TextEditor_Enter);
            this.TextEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextEditor_KeyDown);
            this.TextEditor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextEditor_KeyPress);
            // 
            // Compile
            // 
            this.Compile.Location = new System.Drawing.Point(492, 50);
            this.Compile.Name = "Compile";
            this.Compile.Size = new System.Drawing.Size(75, 23);
            this.Compile.TabIndex = 1;
            this.Compile.Text = "Build";
            this.Compile.UseVisualStyleBackColor = true;
            this.Compile.Click += new System.EventHandler(this.button1_Click);
            // 
            // errorBox
            // 
            this.errorBox.Location = new System.Drawing.Point(492, 427);
            this.errorBox.Name = "errorBox";
            this.errorBox.Size = new System.Drawing.Size(395, 73);
            this.errorBox.TabIndex = 2;
            this.errorBox.Text = "";
            this.errorBox.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plikToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(899, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // plikToolStripMenuItem
            // 
            this.plikToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wczytajToolStripMenuItem,
            this.zapiszToolStripMenuItem});
            this.plikToolStripMenuItem.Name = "plikToolStripMenuItem";
            this.plikToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.plikToolStripMenuItem.Text = "Plik";
            this.plikToolStripMenuItem.MouseLeave += new System.EventHandler(this.plikToolStripMenuItem_MouseLeave);
            this.plikToolStripMenuItem.MouseHover += new System.EventHandler(this.plikToolStripMenuItem_MouseHover);
            // 
            // wczytajToolStripMenuItem
            // 
            this.wczytajToolStripMenuItem.Name = "wczytajToolStripMenuItem";
            this.wczytajToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.wczytajToolStripMenuItem.Text = "Wczytaj";
            this.wczytajToolStripMenuItem.Click += new System.EventHandler(this.wczytajToolStripMenuItem_Click);
            // 
            // zapiszToolStripMenuItem
            // 
            this.zapiszToolStripMenuItem.Name = "zapiszToolStripMenuItem";
            this.zapiszToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.zapiszToolStripMenuItem.Text = "Zapisz";
            this.zapiszToolStripMenuItem.Click += new System.EventHandler(this.zapiszToolStripMenuItem_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "\"Plik tekstowy|*.txt|Plik asemblera|*.asm|Wszystkie pliki|*.*\"";
            this.saveFileDialog1.Title = "Wybierz plik do zapisu";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(492, 80);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "RUN";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(492, 110);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Write Registers";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Assembly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(899, 544);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.errorBox);
            this.Controls.Add(this.Compile);
            this.Controls.Add(this.TextEditor);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Assembly";
            this.Text = "Assembly";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox TextEditor;
        private System.Windows.Forms.Button Compile;
        private System.Windows.Forms.RichTextBox errorBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem plikToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wczytajToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zapiszToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
    }
}

