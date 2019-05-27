using System.Collections.Generic;
using System.Windows.Forms;

namespace AssemblerIDE
{
    partial class Assembly
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private const char BACKSPACE = (char)8;
        private const char ENTER = (char)13;
        private const int stackSize = 32;
        private short SP = stackSize - 1;
        private int linecounter = 1;
        private int currentLine = 0;
        private int currentStep = 0;
        private int[] registers = new int[4];
        private short[] STACK = new short[stackSize];
        private string LastChars = "0";
        private string ErrorList = "";
        private bool builded = false;
        private System.IO.StringReader stepReader;
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
                                    if (operands[1] == "AX" || operands[1] == "BX" || operands[1] == "CX" || operands[1] == "DX" || int.TryParse(operands[1], out result) || int.TryParse(operands[1].Remove(operands[1].Length-1), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out result))
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
                    case "PUSH":
                        {
                            if (operandsNB == 1)
                            {
                                if (operands[0] == "AX" || operands[0] == "BX" || operands[0] == "CX" || operands[0] == "DX")
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
                    case "POP":
                        {
                            if (operandsNB == 1)
                            {
                                if (operands[0] == "AX" || operands[0] == "BX" || operands[0] == "CX" || operands[0] == "DX")
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
                    case "INT":
                        {
                            if(operandsNB == 1)
                            {
                                if(operands[0] == "21H")
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

        void updateRegisters()
        {
            for(int i = 0; i < registers.Length; i++)
            {
                registers[i] = registers[i] - 65535 * (registers[i] / 65535);
            }
            string[] tmp = new string[2];
            tmp = convertTo16bits(System.Math.Abs(registers[0]));
            this.AH.Text = tmp[0];
            this.AL.Text = tmp[1];
            tmp = convertTo16bits(System.Math.Abs(registers[1]));
            this.BH.Text = tmp[0];
            this.BL.Text = tmp[1];
            tmp = convertTo16bits(System.Math.Abs(registers[2]));
            this.CH.Text = tmp[0];
            this.CL.Text = tmp[1];
            tmp = convertTo16bits(System.Math.Abs(registers[3]));
            this.DH.Text = tmp[0];
            this.DL.Text = tmp[1];
            this.AH.SelectionAlignment = HorizontalAlignment.Center;
            this.AL.SelectionAlignment = HorizontalAlignment.Center;
            this.BH.SelectionAlignment = HorizontalAlignment.Center;
            this.BL.SelectionAlignment = HorizontalAlignment.Center;
            this.CH.SelectionAlignment = HorizontalAlignment.Center;
            this.CL.SelectionAlignment = HorizontalAlignment.Center;
            this.DH.SelectionAlignment = HorizontalAlignment.Center;
            this.DL.SelectionAlignment = HorizontalAlignment.Center;
        }

        string [] convertTo16bits(int a)
        {
            string[] numbers = new string[2];
            numbers[0] = "0";
            numbers[1] = "2";
            string tmp = System.Convert.ToString(a, 2);
            while(tmp.Length<16)
            {
                tmp = "0" + tmp;
            }
            numbers[0] = tmp.Substring(0, 8);
            numbers[1] = tmp.Substring(8);

            return numbers;
        }
        private void interrupt21H()
        {
            switch(registers[0])
            {
                case 1:  // 1H Wyświetl podany z klawiatury znak
                    {
                        System.Console.WriteLine(System.Console.ReadKey(true).KeyChar);
                        break;
                    }
                case 2:  // 2H Wyświetl char znajdujacy sie w DL
                    {
                        System.Console.WriteLine((char)registers[3]);
                        break;
                    }
                case 3:  // 3H Uruchomienie cmd
                    {
                        System.Diagnostics.Process cmd = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        startInfo.FileName = "cmd.exe";
                        cmd.StartInfo = startInfo;
                        cmd.Start();
                        break;
                    }
                case 25: // 19H Litera aktualnego dysku
                    {
                        char driveLetter = System.IO.Path.GetPathRoot(System.Environment.CurrentDirectory)[0];
                        registers[0] = (int)driveLetter;
                        break;
                    }
                case 42: // 2AH - Aktualna data do rejestru CX - rok, DH - miesiac, DL - dzien 
                    {
                        string time = System.DateTime.Now.ToString("MM/dd/yyyy");
                        string[] times = time.Split('.');
                        for(int i = 0; i<times.Length; i++)
                        {
                            System.Console.WriteLine(times[i]);
                        }
                        registers[2] = int.Parse(times[2]);
                        registers[3] = int.Parse(times[0]) * 256 + int.Parse(times[1]);
                        break;
                    }
                case 44: // 2CH - Aktualna godzina do rejestrów - CH - godzina, CL - minuty, DH - sekundy, DL - 1/100 sekundy (zwykle zero)
                    {
                        string time = System.DateTime.Now.ToString("HH:mm:ss");
                        string[] times = time.Split(':');
                        registers[2] = int.Parse(times[0]) * 256 + int.Parse(times[1]);
                        registers[3] = int.Parse(times[2]) * 256;
                        break;
                    }
                case 57: // 39H - Utworz katalog
                    {
                        System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory("./przyklad");
                        break;
                    }
                case 73: // 49H -  Zerowanie rejestrów 
                    {
                        for(int i = 0; i< 4; i++)
                        {
                            registers[i] = 0;
                        }
                        break;
                    }
                case 76: // 4CH -  Wyłączenie aplikacji
                    {
                        Application.Exit();
                        break;
                    }
                case 86: // 56H - Przeniesienie pliku
                    {
                        System.IO.File.Delete("./przeniesiony.txt");
                        System.IO.File.Move("./doprzeniesienia.txt", "./przeniesiony.txt");
                        break;
                    }
                case 237: // EDH - Usuwa dowolny plik, tutaj specjalnie przygotowany plik dousuniecia.txt
                    {
                        System.IO.File.Delete("./dousuniecia.txt");
                        break;
                    }
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
                    secondOperand = secondOperand - 65535 * (secondOperand / 65535);
                }
                else if(int.TryParse(operands[1].Remove(operands[1].Length - 1), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out secondOperand))
                {
                    isSecondOperandRegister = false;
                    secondOperand = secondOperand - 65535 * (secondOperand / 65535);
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
                case "PUSH":
                    {
                        STACK[SP] = (short)this.registers[register[operands[0]]];
                        SP--;
                        break;
                    }
                case "POP":
                    {
                        SP++;
                        this.registers[register[operands[0]]] = (int)STACK[SP];
                        break;
                    }
                case "INT":
                    {
                        switch(operands[0])
                        {
                            case "21H":
                                {
                                    interrupt21H();
                                    break;
                                }
                        }
                        break;
                    }
            }
            updateRegisters();
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
            this.uruchamianieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pracaKrokowaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uruchomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.DH = new System.Windows.Forms.RichTextBox();
            this.DL = new System.Windows.Forms.RichTextBox();
            this.dhTX = new System.Windows.Forms.RichTextBox();
            this.dlTX = new System.Windows.Forms.RichTextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.CH = new System.Windows.Forms.RichTextBox();
            this.CL = new System.Windows.Forms.RichTextBox();
            this.chTX = new System.Windows.Forms.RichTextBox();
            this.clTX = new System.Windows.Forms.RichTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BH = new System.Windows.Forms.RichTextBox();
            this.BL = new System.Windows.Forms.RichTextBox();
            this.bhTX = new System.Windows.Forms.RichTextBox();
            this.blTX = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AL = new System.Windows.Forms.RichTextBox();
            this.AH = new System.Windows.Forms.RichTextBox();
            this.alTX = new System.Windows.Forms.RichTextBox();
            this.ahTX = new System.Windows.Forms.RichTextBox();
            this.nextStep = new System.Windows.Forms.Button();
            this.stepIND = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextEditor
            // 
            this.TextEditor.AcceptsTab = true;
            this.TextEditor.BackColor = System.Drawing.Color.DarkOrange;
            this.TextEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TextEditor.ForeColor = System.Drawing.Color.Cornsilk;
            this.TextEditor.Location = new System.Drawing.Point(27, 62);
            this.TextEditor.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.TextEditor.Name = "TextEditor";
            this.TextEditor.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.TextEditor.Size = new System.Drawing.Size(600, 554);
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
            this.Compile.Location = new System.Drawing.Point(656, 62);
            this.Compile.Margin = new System.Windows.Forms.Padding(4);
            this.Compile.Name = "Compile";
            this.Compile.Size = new System.Drawing.Size(100, 28);
            this.Compile.TabIndex = 1;
            this.Compile.Text = "Build";
            this.Compile.UseVisualStyleBackColor = true;
            this.Compile.Click += new System.EventHandler(this.button1_Click);
            // 
            // errorBox
            // 
            this.errorBox.Location = new System.Drawing.Point(656, 526);
            this.errorBox.Margin = new System.Windows.Forms.Padding(4);
            this.errorBox.Name = "errorBox";
            this.errorBox.Size = new System.Drawing.Size(525, 89);
            this.errorBox.TabIndex = 2;
            this.errorBox.Text = "";
            this.errorBox.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plikToolStripMenuItem,
            this.uruchamianieToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1199, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // plikToolStripMenuItem
            // 
            this.plikToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wczytajToolStripMenuItem,
            this.zapiszToolStripMenuItem});
            this.plikToolStripMenuItem.Name = "plikToolStripMenuItem";
            this.plikToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.plikToolStripMenuItem.Text = "Plik";
            // 
            // wczytajToolStripMenuItem
            // 
            this.wczytajToolStripMenuItem.Name = "wczytajToolStripMenuItem";
            this.wczytajToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.wczytajToolStripMenuItem.Text = "Wczytaj";
            this.wczytajToolStripMenuItem.Click += new System.EventHandler(this.wczytajToolStripMenuItem_Click);
            // 
            // zapiszToolStripMenuItem
            // 
            this.zapiszToolStripMenuItem.Name = "zapiszToolStripMenuItem";
            this.zapiszToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.zapiszToolStripMenuItem.Text = "Zapisz";
            this.zapiszToolStripMenuItem.Click += new System.EventHandler(this.zapiszToolStripMenuItem_Click);
            // 
            // uruchamianieToolStripMenuItem
            // 
            this.uruchamianieToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pracaKrokowaToolStripMenuItem,
            this.uruchomToolStripMenuItem});
            this.uruchamianieToolStripMenuItem.Name = "uruchamianieToolStripMenuItem";
            this.uruchamianieToolStripMenuItem.Size = new System.Drawing.Size(112, 24);
            this.uruchamianieToolStripMenuItem.Text = "Uruchamianie";
            // 
            // pracaKrokowaToolStripMenuItem
            // 
            this.pracaKrokowaToolStripMenuItem.Name = "pracaKrokowaToolStripMenuItem";
            this.pracaKrokowaToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.pracaKrokowaToolStripMenuItem.Text = "Praca Krokowa";
            this.pracaKrokowaToolStripMenuItem.Click += new System.EventHandler(this.pracaKrokowaToolStripMenuItem_Click);
            // 
            // uruchomToolStripMenuItem
            // 
            this.uruchomToolStripMenuItem.Image = global::AssemblerIDE.Properties.Resources.trojkat;
            this.uruchomToolStripMenuItem.Name = "uruchomToolStripMenuItem";
            this.uruchomToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F9)));
            this.uruchomToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.uruchomToolStripMenuItem.Text = "Uruchom";
            this.uruchomToolStripMenuItem.Click += new System.EventHandler(this.uruchomToolStripMenuItem_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "\"Plik tekstowy|*.txt|Plik asemblera|*.asm|Wszystkie pliki|*.*\"";
            this.saveFileDialog1.Title = "Wybierz plik do zapisu";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(656, 98);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
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
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(910, 84);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(152, 31);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "REJESTRY";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(788, 135);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(380, 320);
            this.panel1.TabIndex = 7;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.DH);
            this.panel5.Controls.Add(this.DL);
            this.panel5.Controls.Add(this.dhTX);
            this.panel5.Controls.Add(this.dlTX);
            this.panel5.Location = new System.Drawing.Point(0, 240);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(380, 80);
            this.panel5.TabIndex = 3;
            // 
            // DH
            // 
            this.DH.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DH.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DH.Location = new System.Drawing.Point(0, 40);
            this.DH.Name = "DH";
            this.DH.Size = new System.Drawing.Size(190, 40);
            this.DH.TabIndex = 4;
            this.DH.Text = "AH";
            // 
            // DL
            // 
            this.DL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DL.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DL.Location = new System.Drawing.Point(190, 40);
            this.DL.Name = "DL";
            this.DL.Size = new System.Drawing.Size(190, 40);
            this.DL.TabIndex = 3;
            this.DL.Text = "AH";
            // 
            // dhTX
            // 
            this.dhTX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dhTX.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dhTX.Location = new System.Drawing.Point(0, 0);
            this.dhTX.Name = "dhTX";
            this.dhTX.Size = new System.Drawing.Size(190, 40);
            this.dhTX.TabIndex = 2;
            this.dhTX.Text = "DH";
            // 
            // dlTX
            // 
            this.dlTX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dlTX.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlTX.Location = new System.Drawing.Point(190, 0);
            this.dlTX.Name = "dlTX";
            this.dlTX.Size = new System.Drawing.Size(190, 40);
            this.dlTX.TabIndex = 1;
            this.dlTX.Text = "DL";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.CH);
            this.panel4.Controls.Add(this.CL);
            this.panel4.Controls.Add(this.chTX);
            this.panel4.Controls.Add(this.clTX);
            this.panel4.Location = new System.Drawing.Point(0, 160);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(380, 80);
            this.panel4.TabIndex = 2;
            // 
            // CH
            // 
            this.CH.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CH.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CH.Location = new System.Drawing.Point(0, 40);
            this.CH.Name = "CH";
            this.CH.Size = new System.Drawing.Size(190, 40);
            this.CH.TabIndex = 4;
            this.CH.Text = "AH";
            // 
            // CL
            // 
            this.CL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CL.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CL.Location = new System.Drawing.Point(190, 40);
            this.CL.Name = "CL";
            this.CL.Size = new System.Drawing.Size(190, 40);
            this.CL.TabIndex = 3;
            this.CL.Text = "AH";
            // 
            // chTX
            // 
            this.chTX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chTX.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chTX.Location = new System.Drawing.Point(0, 0);
            this.chTX.Name = "chTX";
            this.chTX.Size = new System.Drawing.Size(190, 40);
            this.chTX.TabIndex = 2;
            this.chTX.Text = "CH";
            // 
            // clTX
            // 
            this.clTX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clTX.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clTX.Location = new System.Drawing.Point(190, 0);
            this.clTX.Name = "clTX";
            this.clTX.Size = new System.Drawing.Size(190, 40);
            this.clTX.TabIndex = 1;
            this.clTX.Text = "CL";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.BH);
            this.panel3.Controls.Add(this.BL);
            this.panel3.Controls.Add(this.bhTX);
            this.panel3.Controls.Add(this.blTX);
            this.panel3.Location = new System.Drawing.Point(0, 80);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(380, 80);
            this.panel3.TabIndex = 1;
            // 
            // BH
            // 
            this.BH.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BH.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BH.Location = new System.Drawing.Point(0, 40);
            this.BH.Name = "BH";
            this.BH.Size = new System.Drawing.Size(190, 40);
            this.BH.TabIndex = 4;
            this.BH.Text = "AH";
            // 
            // BL
            // 
            this.BL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BL.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BL.Location = new System.Drawing.Point(190, 40);
            this.BL.Name = "BL";
            this.BL.Size = new System.Drawing.Size(190, 40);
            this.BL.TabIndex = 3;
            this.BL.Text = "AH";
            // 
            // bhTX
            // 
            this.bhTX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bhTX.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bhTX.Location = new System.Drawing.Point(0, 0);
            this.bhTX.Name = "bhTX";
            this.bhTX.Size = new System.Drawing.Size(190, 40);
            this.bhTX.TabIndex = 2;
            this.bhTX.Text = "BH";
            // 
            // blTX
            // 
            this.blTX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.blTX.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blTX.Location = new System.Drawing.Point(190, 0);
            this.blTX.Name = "blTX";
            this.blTX.Size = new System.Drawing.Size(190, 40);
            this.blTX.TabIndex = 1;
            this.blTX.Text = "BL";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.AL);
            this.panel2.Controls.Add(this.AH);
            this.panel2.Controls.Add(this.alTX);
            this.panel2.Controls.Add(this.ahTX);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(380, 80);
            this.panel2.TabIndex = 0;
            // 
            // AL
            // 
            this.AL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AL.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AL.Location = new System.Drawing.Point(190, 40);
            this.AL.Name = "AL";
            this.AL.Size = new System.Drawing.Size(190, 40);
            this.AL.TabIndex = 3;
            this.AL.Text = "AH";
            // 
            // AH
            // 
            this.AH.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AH.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AH.Location = new System.Drawing.Point(0, 40);
            this.AH.Name = "AH";
            this.AH.Size = new System.Drawing.Size(190, 40);
            this.AH.TabIndex = 2;
            this.AH.Text = "AH";
            // 
            // alTX
            // 
            this.alTX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.alTX.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alTX.Location = new System.Drawing.Point(190, 0);
            this.alTX.Name = "alTX";
            this.alTX.Size = new System.Drawing.Size(190, 40);
            this.alTX.TabIndex = 1;
            this.alTX.Text = "AL";
            // 
            // ahTX
            // 
            this.ahTX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ahTX.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ahTX.Location = new System.Drawing.Point(0, 0);
            this.ahTX.Name = "ahTX";
            this.ahTX.Size = new System.Drawing.Size(190, 40);
            this.ahTX.TabIndex = 0;
            this.ahTX.Text = "AH";
            // 
            // nextStep
            // 
            this.nextStep.Enabled = false;
            this.nextStep.Location = new System.Drawing.Point(656, 133);
            this.nextStep.Name = "nextStep";
            this.nextStep.Size = new System.Drawing.Size(100, 49);
            this.nextStep.TabIndex = 8;
            this.nextStep.Text = "Następny Krok";
            this.nextStep.UseVisualStyleBackColor = true;
            this.nextStep.Visible = false;
            this.nextStep.Click += new System.EventHandler(this.nextStep_Click);
            // 
            // stepIND
            // 
            this.stepIND.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stepIND.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepIND.Location = new System.Drawing.Point(800, 40);
            this.stepIND.Name = "stepIND";
            this.stepIND.Size = new System.Drawing.Size(359, 38);
            this.stepIND.TabIndex = 9;
            this.stepIND.Text = "";
            this.stepIND.Visible = false;
            // 
            // Assembly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1199, 670);
            this.Controls.Add(this.stepIND);
            this.Controls.Add(this.nextStep);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.errorBox);
            this.Controls.Add(this.Compile);
            this.Controls.Add(this.TextEditor);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Assembly";
            this.Text = "Assembly";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
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
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RichTextBox dhTX;
        private System.Windows.Forms.RichTextBox dlTX;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RichTextBox chTX;
        private System.Windows.Forms.RichTextBox clTX;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RichTextBox bhTX;
        private System.Windows.Forms.RichTextBox blTX;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox alTX;
        private System.Windows.Forms.RichTextBox ahTX;
        private System.Windows.Forms.RichTextBox DH;
        private System.Windows.Forms.RichTextBox DL;
        private System.Windows.Forms.RichTextBox CH;
        private System.Windows.Forms.RichTextBox CL;
        private System.Windows.Forms.RichTextBox BH;
        private System.Windows.Forms.RichTextBox BL;
        private System.Windows.Forms.RichTextBox AL;
        private System.Windows.Forms.RichTextBox AH;
        private ToolStripMenuItem uruchamianieToolStripMenuItem;
        private ToolStripMenuItem pracaKrokowaToolStripMenuItem;
        private Button nextStep;
        private RichTextBox stepIND;
        private ToolStripMenuItem uruchomToolStripMenuItem;
    }
}

