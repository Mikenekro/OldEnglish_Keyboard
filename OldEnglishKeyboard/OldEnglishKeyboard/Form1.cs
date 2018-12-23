using System;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace OldEnglishKeyboard
{
    public partial class Form1 : Form
    {
        private bool isOld;
        private bool isUpper;
        private bool accent;

        private InputSimulator sim;
        
        private Modifiers modKey;

        NativeMethods activate;
        NativeMethods accentHook;
        NativeMethods upperHook;

        NativeMethods thorn;
        NativeMethods eth;
        NativeMethods wynn;
        NativeMethods ae;
        NativeMethods aeM;
        NativeMethods oe;
        
        NativeMethods a;
        NativeMethods e1;
        NativeMethods i;
        NativeMethods o;
        NativeMethods u;
        NativeMethods y;

        NativeMethods c;
        NativeMethods g;

        public Form1()
        {
            InitializeComponent();

            sim = new InputSimulator();

            comboBox1.DataSource = Enum.GetValues(typeof(Modifiers));
            modKey = (Modifiers)Enum.Parse(typeof(Modifiers), Properties.Settings.Default.savModKey);
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(modKey);
            //Console.WriteLine("ModKey is: " + ((int)modKey).ToString());

            UpdateModLabels(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            activate = new NativeMethods((int)Modifiers.Control, Keys.P, this);
            accentHook = new NativeMethods((int)Modifiers.None, Keys.F6, this);
            upperHook = new NativeMethods((int)Modifiers.None, Keys.F7, this);
            
            activate.Register();
            accentHook.Register();
            upperHook.Register();

            isOld = false;
            isUpper = false;
            accent = false;
            label1.Text = "Old English is Off";
        }

        private void EnableBtn1_Click(object sender, EventArgs e)
        {
            Enable();
        }

        private void DisableBtn1_Click(object sender, EventArgs e)
        {
            Disable();
        }

        /// <summary>
        /// Enable Old English hotkeys
        /// </summary>
        public void Enable()
        {
            if (!isOld)
            {
                isOld = true;
                
                thorn.Register();
                eth.Register();
                wynn.Register();

                ae.Register();
                aeM.Register();
                oe.Register();

                a.Register();
                e1.Register();
                i.Register();
                o.Register();
                u.Register();
                y.Register();

                c.Register();
                g.Register();

                label1.Text = "Old English is On";
            }
        }
        /// <summary>
        /// Disable Old English hotkeys
        /// </summary>
        public void Disable()
        {
            if (isOld)
            {
                isOld = false;
                
                thorn.Unregister();
                eth.Unregister();
                wynn.Unregister();

                ae.Register();
                aeM.Register();
                oe.Register();

                a.Register();
                e1.Unregister();
                i.Unregister();
                o.Unregister();
                u.Unregister();
                y.Unregister();

                c.Unregister();
                g.Unregister();

                label1.Text = "Old English is Off";
            }
        }
        
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            modKey = (Modifiers)Enum.Parse(typeof(Modifiers), comboBox1.SelectedValue.ToString());
            Console.WriteLine("ModKey is: " + modKey.ToString());
            UpdateModLabels(true);
        }

        /// <summary>
        /// Makes sure each Hotkey label has the correct values
        /// </summary>
        public void UpdateModLabels(bool newKeys)
        {
            string val = "";
            bool firstDisable = false;

            // Are we changing each keys modifiers?
            if (newKeys)
            {
                // Disable any hooks if they have been registered
                if (isOld)
                {
                    firstDisable = true;
                    Disable();
                }

                Console.WriteLine("ModKey int: " + ((int)modKey).ToString());
                eth = new NativeMethods((int)modKey, Keys.D, this);
                thorn = new NativeMethods((int)modKey, Keys.T, this);
                wynn = new NativeMethods((int)modKey, Keys.W, this);

                ae = new NativeMethods((int)modKey, Keys.D1, this);
                aeM = new NativeMethods((int)modKey, Keys.D2, this);
                oe = new NativeMethods((int)modKey, Keys.D3, this);

                a = new NativeMethods((int)modKey, Keys.A, this);
                e1 = new NativeMethods((int)modKey, Keys.E, this);
                i = new NativeMethods((int)modKey, Keys.I, this);
                o = new NativeMethods((int)modKey, Keys.O, this);
                u = new NativeMethods((int)modKey, Keys.U, this);
                y = new NativeMethods((int)modKey, Keys.Y, this);

                c = new NativeMethods((int)modKey, Keys.C, this);
                g = new NativeMethods((int)modKey, Keys.G, this);

                // Re-enable if the keyboard was already enabled
                if (firstDisable)
                {
                    Enable();
                }
            }

            if (modKey == Modifiers.Control)
                val = "CTRL";
            else if (modKey == Modifiers.Shift)
                val = "SHFT";
            else
                val = modKey.ToString().ToUpper();

            label19.Text = "Variant: " + ((accent) ? ("Accent") : ("Macron"));

            // label4 = label17
            label4.Text = "Thorn (" + ((isUpper)?("Þ"):("þ")) + ") : " + ((modKey != Modifiers.None)?(val + " + "):("")) + "T";
            label5.Text = "Eth (" + ((isUpper)?("Ð") :("ð")) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "D";
            label6.Text = "Wynn (" + ((isUpper)?("Ƿ") :("ƿ")) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "W";
            label7.Text = "AE Normal (" + ((isUpper) ? ("Æ") : ("æ")) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "1";
            label8.Text = "AE Macron (" + ((isUpper) ? ("Ǣ") : ("ǣ")) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "2";
            label9.Text = "OE Normal (" + ((isUpper) ? ("Œ") : ("œ")) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "3";
            label10.Text = "Long C (" + ((isUpper) ? ("Ċ") : ("ċ")) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "C";
            label11.Text = "Long G (" + ((isUpper) ? ("Ġ") : ("ġ")) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "G";
            label12.Text = "(" + ((isUpper) ? ( ((accent)?("Á") :("Ā")) ) : ( ((accent)?("á") :("ā")) )) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "A";
            label13.Text = "(" + ((isUpper) ? ( ((accent)?("É") :("Ē")) ) : ( ((accent)?("é") :("ē")) )) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "E";
            label14.Text = "(" + ((isUpper) ? ( ((accent)?("Í") :("Ī")) ) : ( ((accent)?("í") :("ī")) )) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "I";
            label15.Text = "(" + ((isUpper) ? ( ((accent)?("Ó") :("Ō")) ) : ( ((accent)?("ó") :("ō")) )) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "O";
            label16.Text = "(" + ((isUpper) ? ( ((accent)?("Ú") :("Ū")) ) : ( ((accent)?("ú") :("ū")) )) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "U";
            label17.Text = "(" + ((isUpper) ? ( ((accent)?("Ý") :("Ȳ")) ) : ( ((accent)?("ý") :("ȳ")) )) + ") : " + ((modKey != Modifiers.None) ? (val + " + ") : ("")) + "Y";

        }

        /// <summary>
        /// Checks if the modifier key is actually down. 
        /// Used to prevent a bug that happens when changing from no mod key to another mod key and typing either 1 2 or 3
        /// </summary>
        /// <returns></returns>
        private bool IsModKeyDown()
        {
            bool isDown = false;

            switch(modKey)
            {
                case Modifiers.None:
                    isDown = true;
                    break;
                case Modifiers.Shift:
                    isDown = sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.SHIFT);
                    break;
                case Modifiers.Control:
                    isDown = sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.CONTROL);
                    break;
                default:
                    break;
            }

            return isDown;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
                HandleHotkey(); // a Registered key was pressed
            
            base.WndProc(ref m);
        }

        /// <summary>
        /// Handle replacing the keys pressed with Old English Keys
        /// </summary>
        private void HandleHotkey()
        {
            // ALT + Code
            //sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, new[] { VirtualKeyCode.NUMPAD0, VirtualKeyCode.NUMPAD2, VirtualKeyCode.NUMPAD5, VirtualKeyCode.NUMPAD4 });
            //sim.Keyboard.KeyDown(VirtualKeyCode.LMENU).KeyPress(VirtualKeyCode.NUMPAD0).KeyPress(VirtualKeyCode.NUMPAD2).KeyPress(VirtualKeyCode.NUMPAD5).KeyPress(VirtualKeyCode.NUMPAD4).KeyUp(VirtualKeyCode.LMENU).KeyPress(VirtualKeyCode.VK_B);

            if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.CONTROL) && sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_P))
            {
                if (isOld)
                    Disable();
                else
                    Enable();
            }

            if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.F6))
            {
                checkBox1.Checked = !checkBox1.Checked;
                accent = checkBox1.Checked;
                UpdateModLabels(false);
            }

            if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.F7))
            {
                checkBox2.Checked = !checkBox2.Checked;
                isUpper = checkBox2.Checked;
                UpdateModLabels(false);
            }
            
            // Sleep for 1/10th of a second to wait for user to press another key

            if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_T))
            {
                if (isOld)
                {
                    // Thorn
                    if (!isUpper)
                        sim.Keyboard.TextEntry(Convert.ToChar(254));
                    else
                        sim.Keyboard.TextEntry(Convert.ToChar(222));
                }
                else
                {
                    sim.Keyboard.TextEntry("t");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_W))
            {
                if (isOld)
                {
                    // Wynn
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u01bf");
                    else
                        sim.Keyboard.TextEntry("\u01f7");
                }
                else
                {
                    sim.Keyboard.TextEntry("w");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_D))
            {
                if (isOld)
                {
                    // Eth
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u00f0");
                    else
                        sim.Keyboard.TextEntry("\u00d0");
                }
                else
                {
                    sim.Keyboard.TextEntry("d");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_1))
            {
                if (isOld && IsModKeyDown())
                {
                    // AE
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u00e6");
                    else
                        sim.Keyboard.TextEntry("\u00c6");
                }
                else
                {
                    if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.SHIFT))
                        sim.Keyboard.TextEntry("!");
                    else
                        sim.Keyboard.TextEntry("1");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_2))
            {
                if (isOld && IsModKeyDown())
                {
                    // AE Macron
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u01e3");
                    else
                        sim.Keyboard.TextEntry("\u01e2");
                }
                else
                {
                    if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.SHIFT))
                        sim.Keyboard.TextEntry("@");
                    else
                        sim.Keyboard.TextEntry("2");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_3))
            {
                if (isOld && IsModKeyDown())
                {
                    // OE
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u0153");
                    else
                        sim.Keyboard.TextEntry("\u0152");
                }
                else
                {
                    if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.SHIFT))
                        sim.Keyboard.TextEntry("#");
                    else
                        sim.Keyboard.TextEntry("3");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_A) && !accent)
            {
                if (isOld)
                {
                    // A Macron
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u0101");
                    else
                        sim.Keyboard.TextEntry("\u0100");
                }
                else
                {
                    sim.Keyboard.TextEntry("a");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_E) && !accent)
            {
                if (isOld)
                {
                    // E Macron
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u0113");
                    else
                        sim.Keyboard.TextEntry("\u0112");
                }
                else
                {
                    sim.Keyboard.TextEntry("e");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_I) && !accent)
            {
                if (isOld)
                {
                    // I Macron
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u012b");
                    else
                        sim.Keyboard.TextEntry("\u012a");
                }
                else
                {
                    sim.Keyboard.TextEntry("i");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_O) && !accent)
            {
                if (isOld)
                {
                    // O Macron
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u014d");
                    else
                        sim.Keyboard.TextEntry("\u014c");
                }
                else
                {
                    sim.Keyboard.TextEntry("o");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_U) && !accent)
            {
                if (isOld)
                {
                    // U Macron
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u016b");
                    else
                        sim.Keyboard.TextEntry("\u016a");
                }
                else
                {
                    sim.Keyboard.TextEntry("u");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_Y) && !accent)
            {
                if (isOld)
                {
                    // Y Macron
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u0233");
                    else
                        sim.Keyboard.TextEntry("\u0232");
                }
                else
                {
                    sim.Keyboard.TextEntry("y");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_C))
            {
                if (isOld)
                {
                    // C Dot
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u010b");
                    else
                        sim.Keyboard.TextEntry("\u010a");
                }
                else
                {
                    sim.Keyboard.TextEntry("c");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_G))
            {
                if (isOld)
                {
                    // G Dot
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u0121");
                    else
                        sim.Keyboard.TextEntry("\u0120");
                }
                else
                {
                    sim.Keyboard.TextEntry("g");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_A))
            {
                if (isOld)
                {
                    // A with Accent
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u00e1");
                    else
                        sim.Keyboard.TextEntry("\u00c1");
                }
                else
                {
                    sim.Keyboard.TextEntry("a");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_E))
            {
                if (isOld)
                {
                    // E with Accent
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u00e9");
                    else
                        sim.Keyboard.TextEntry("\u00c9");
                }
                else
                {
                    sim.Keyboard.TextEntry("e");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_I))
            {
                if (isOld)
                {
                    // I with Accent
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u00ed");
                    else
                        sim.Keyboard.TextEntry("\u00cd");
                }
                else
                {
                    sim.Keyboard.TextEntry("i");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_O))
            {
                if (isOld)
                {
                    // O with Accent
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u00f3");
                    else
                        sim.Keyboard.TextEntry("\u00d3");
                }
                else
                {
                    sim.Keyboard.TextEntry("o");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_U))
            {
                if (isOld)
                {
                    // U with Accent
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u00fa");
                    else
                        sim.Keyboard.TextEntry("\u00da");
                }
                else
                {
                    sim.Keyboard.TextEntry("u");
                }
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(VirtualKeyCode.VK_Y))
            {
                if (isOld)
                {
                    // Y with Accent
                    if (!isUpper)
                        sim.Keyboard.TextEntry("\u00fd");
                    else
                        sim.Keyboard.TextEntry("\u00dd");
                }
                else
                {
                    sim.Keyboard.TextEntry("y");
                }
            }
            
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            accent = checkBox1.Checked;

            UpdateModLabels(false);
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            isUpper = checkBox2.Checked;

            UpdateModLabels(false);
        }

        private void SaveTextAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save Text Area to Text File";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = ".txt";

            DialogResult res = saveFileDialog1.ShowDialog();
            
            if (res == DialogResult.OK && saveFileDialog1.FileName != "")
            {
                string path = saveFileDialog1.FileName;
                string onlyFile = path.Split('\\')[path.Split('\\').Length - 1];

                System.Console.WriteLine("Only Filename: " + onlyFile);

                if (Directory.Exists(path.Replace(onlyFile, "")))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();

                    TextWriter tw = new StreamWriter(path);

                    for (int i = 0; i < textBox1.Lines.Length; ++i)
                    {
                        tw.WriteLine(textBox1.Lines[i]);
                    }

                    tw.Close();
                }
                
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close the application
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Console.WriteLine("Closing System... Disabling all hooks");

            activate.Unregister();
            accentHook.Unregister();
            upperHook.Unregister();

            Disable();

            // Save the Modifier Key before closing
            Properties.Settings.Default.PropertyValues["savModKey"].PropertyValue = ((int)modKey).ToString();
            Properties.Settings.Default.Save();
        }
    }
}
