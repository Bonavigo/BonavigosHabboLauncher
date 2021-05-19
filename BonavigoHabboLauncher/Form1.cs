using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace BonavigoHabboLauncher
{
    public partial class Form1 : Form
    {
        protected String hotelCode;
        protected String logCode;
        protected bool checkboxNewInstance;
        protected String executingPath = AppDomain.CurrentDomain.BaseDirectory;
        protected String gamePath;
        protected String XMLPath;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (getPath())
            {
                if (String.IsNullOrEmpty(this.logCode))
                {
                    MessageBox.Show("Você precisa inserir um código de login!", "Erro!");
                }
                else
                {
                    openGame();
                }
            }
        }
        
        private bool getPath()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string[] path = { this.executingPath, "config.xml" };
                string fullPath = Path.Combine(path);
                doc.Load(fullPath);
                var elementos = doc.SelectNodes("//*");
                for (int i = 0; i < elementos.Count; i++)
                {
                    var elemento = elementos[i];
                    if (elemento.Name == "path")
                    {
                        this.gamePath = elemento.InnerText;
                    }
                }
                string[] pathXML = { this.gamePath, "META-INF", "AIR", "application.xml" };
                string fullPathXML = Path.Combine(pathXML);
                this.XMLPath = fullPathXML;
                return true;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                return false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string[] subs = textBox1.Text.Split(new[] { '.' }, 2);
            if (subs.Count() == 2)
            {
                this.hotelCode = subs[0];
                this.logCode = subs[1];
            } else
            {
                this.hotelCode = null;
                this.logCode = null;
            }
        }

        private void openGame()
        {
            if (this.checkboxNewInstance)
            {
                this.newIDApplictionXML();
            }
            Process.Start(new ProcessStartInfo("Habbo.exe")
            {
                WorkingDirectory = this.gamePath,
                Arguments = $"-server {this.hotelCode} -ticket \"{this.logCode}\""
            });
            if (this.checkboxNewInstance)
            {
                this.resetApplicationXML();
            }
        }

        private void newIDApplictionXML()
        {
            try {
                string new_id = $"com.sulake.habboair.{this.hotelCode}" + DateTimeOffset.Now.ToUnixTimeSeconds();
                XmlDocument doc = new XmlDocument();
                doc.Load(this.XMLPath);
                var elementos = doc.SelectNodes("//*");
                for (int i = 0; i < elementos.Count; i++)
                {
                    var elemento = elementos[i];
                    if (elemento.Name == "id")
                    {
                        elemento.InnerText = new_id;
                    }
                }
                doc.Save(this.XMLPath);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void resetApplicationXML()
        {
            try
            {
                string new_id = $"com.sulake.habboair.{this.hotelCode}";
                XmlDocument doc = new XmlDocument();
                doc.Load(this.XMLPath);
                var elementos = doc.SelectNodes("//*");
                for (int i = 0; i < elementos.Count; i++)
                {
                    var elemento = elementos[i];
                    if (elemento.Name == "id")
                    {
                        elemento.InnerText = new_id;
                    }
                }
                doc.Save(this.XMLPath);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.checkboxNewInstance = checkBox1.Checked;
        }
    }
}
