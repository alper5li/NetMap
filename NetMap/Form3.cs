using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetMap
{
    public partial class Form3 : Form
    {
        String FName;
        String activeUser;
        String IPAD;
        String IPAD2;
        String IPAD3;
        String IPAD4;
        String NETMASK="32";

        public string getLogLoc()
        {
            string dataS = "";
            dataS += System.AppContext.BaseDirectory;
            dataS = dataS.Substring(0, dataS.Length - 25);
            dataS += "database\\Log\\";
            return dataS;
        }

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
           
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                comboBox1.Visible = false;
                comboBox1.Enabled = false;
                label2.Visible = false;
                textBox2.Visible = false;
            }
            else if (checkBox1.Checked==true)
            {
                comboBox1.Visible = true;
                comboBox1.Enabled = true;
                label2.Visible = true;
                textBox2.Visible = true;
            }
           
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox2.Enabled = false;
                textBox2.Text = "8";
                textBox3.Enabled = false;
                textBox4.Enabled = false;         
                textBox5.Enabled = false;              
                label5.Text = textBox1.Text + ".1.1.1/8";


            }
            else if (comboBox1.SelectedIndex == 1)
            {
               
                textBox1.Enabled = true;
                textBox2.Enabled= false;
                textBox2.Text = "16";
                textBox3.Enabled = true;
                textBox4.Enabled = false;
                
                textBox5.Enabled = false;
                
                label5.Text = textBox1.Text +"."+ textBox3.Text+".1.1/16";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                textBox2.Enabled = false;
                textBox2.Text = "24";
                textBox1.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;  
                textBox5.Enabled = false;
                label5.Text = textBox1.Text +"."+ textBox3.Text +"."+textBox4.Text+ ".1/24";
                ////MessageBox.Show(comboBox1.SelectedItem.ToString());
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                meth();
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
            }

            else
            {
                label2.Visible = false;
                textBox2.Visible = false;
                textBox2.Enabled = false;
                
            }
            void meth()
            {
                label2.Visible = true;
                textBox2.Visible = true;
                textBox2.Enabled = true;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                if (textBox1.Text != "")
                {
                    IPAD = textBox1.Text;
                    IPAD2 = textBox3.Text;
                    IPAD3 = textBox4.Text;
                    IPAD4 = textBox5.Text;
                    if (textBox2.Text != "")
                    {
                        NETMASK = textBox2.Text;
                    }
                    else
                    {
                        NETMASK = "32";
                    }


                    String Log = getLogLoc() + "Log.txt";
                    Thread.Sleep(100);
                    string vc = textBox6.Text;
                    string cv = textBox7.Text;
                    string vcc = label5.Text;
                    if (cv == "" || vc == "" || vcc == "")
                    {
                        cv = "Default";
                        vc = "Default";
                        vcc = "Default";
                    }

                    String LOGIN = ("  [*] " + "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss dddd") + "]" + " [CONSOLE] " + "Fullname :[" + FName + "] User :[" + activeUser + "] [ACTION] => CONSOLE => [IP][:" + textBox1.Text + "." + textBox3.Text + "." + textBox4.Text + "." + textBox5.Text + "/" + textBox2.Text + " | " + label5.Text + " | " + "]" + " [DB:" + vc + " | TABLE:" + cv + "]");
                    using (StreamWriter sw = new StreamWriter(Log, true))
                    {
                        sw.WriteLine(LOGIN);
                    }




                    Form2 IPLIST = new Form2();

                    bool newDB = checkBox2.Checked;
                    IPLIST.receiveIPData(IPAD.ToString(), IPAD2.ToString(), IPAD3.ToString(), IPAD4.ToString(), NETMASK.ToString(), newDB.ToString(), textBox6.Text, textBox7.Text, activeUser, "ABCD");
                    IPLIST.Show();
                }

            }
            else if (checkBox3.Checked == true)
            {
                if (textBox8.Text != "")
                {
                    String domain = textBox8.Text;




                    String Log = getLogLoc() + "Log.txt";
                    Thread.Sleep(100);               
                    String LOGIN = ("  [*] " + "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss dddd") + "]" + " [CONSOLE] " + "Fullname :[" + FName + "] User :[" + activeUser + "] [ACTION] => CONSOLE => [DOMAIN][:" + textBox8.Text +"]" + " [DB:DomainDB | TABLE: DomainSearch]");
                    using (StreamWriter sw = new StreamWriter(Log, true))
                    {
                        sw.WriteLine(LOGIN);
                    }

                    Form2 DOM = new Form2();
                    DOM.receiveDomainData(domain,activeUser,"DCBA");
                    DOM.Show();




                }
            }
            else
            {
                MessageBox.Show("You must choose the scan type and fill required boxes.");
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox6.Visible = true;
                textBox6.Enabled = true;
                textBox7.Visible = true;
                textBox7.Enabled = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
            }
            else if (checkBox2.Checked == false)
            {
                textBox6.Visible = false;
                textBox6.Enabled = false;
                textBox7.Visible = false;
                textBox7.Enabled = false;
                label3.Visible = false;
                label5.Visible = false;
                label4.Visible = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
             string bincal(int y)
            {
                int c = y%8;
                int mat = 8 - c;
                int x = Convert.ToInt32(Math.Pow(2, mat) - 2);
                return x.ToString();
            }
            label5.Text = textBox1.Text + "." + textBox3.Text + "." + textBox4.Text + "/" + textBox2.Text ;
            if (Convert.ToInt32(textBox2.Text)>0 && Convert.ToInt32(textBox2.Text)<8)
            {
               String val =bincal ( Convert.ToInt32(textBox1.Text));

                label5.Text = val + ".1.1.1/"+textBox2.Text;
            }
           
            else if (Convert.ToInt32(textBox2.Text) ==8)
            {
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                label5.Text = textBox1.Text + ".1.1.1/8";
            }
            else if (Convert.ToInt32(textBox2.Text) > 8 && Convert.ToInt32(textBox2.Text) < 16)
            {
                String val = bincal(Convert.ToInt32(textBox3.Text));

                label5.Text =textBox1.Text+"."+ val + ".1.1/" + textBox2.Text;
            }
            else if(Convert.ToInt32(textBox2.Text) ==16 )
            {

                textBox1.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = false;

                textBox5.Enabled = false;

                label5.Text = textBox1.Text + "." + textBox3.Text + ".1.1/16";
            }
            else if (Convert.ToInt32(textBox2.Text) > 16 && Convert.ToInt32(textBox2.Text) < 24)
            {
                String val = bincal(Convert.ToInt32(textBox3.Text));

                label5.Text = textBox1.Text +"."+ textBox3.Text+"." + val + ".1/" + textBox2.Text;
            }
            else if (Convert.ToInt32(textBox2.Text) ==24)
            {
                textBox1.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = false;
                label5.Text = textBox1.Text + "." + textBox3.Text + "." + textBox4.Text + ".1/24";
            }
            else if (Convert.ToInt32(textBox2.Text) > 24 && Convert.ToInt32(textBox2.Text) < 32)
            {
                String val = bincal(Convert.ToInt32(textBox3.Text));

                label5.Text = textBox1.Text + "." + textBox3.Text + "." + textBox4.Text+"."+val + "/" + textBox2.Text;
            }
            else
            {
                textBox2.Text = "32";
            }
        }

        private void Form3_Load_1(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            comboBox1.Enabled = false;
            label2.Visible = false;
            textBox2.Visible = false;
            textBox2.Enabled = false;
            textBox6.Visible = false;
            textBox6.Enabled = false;
            textBox7.Visible = false;
            textBox7.Enabled = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;


            checkBox4.Checked = false;
            textBox8.Enabled = false;
            label6.Enabled = false;
            label1.Enabled = false;
            label3.Enabled = false;
            label4.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            comboBox1.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            label5.Enabled = false;
            label2.Enabled = false;

            string dataS = "";

            dataS += System.AppContext.BaseDirectory;
            
            dataS = dataS.Substring(0, dataS.Length - 25);
            
            dataS += "images\\NRes\\amblem.png";
            // dataS += "images\\index.jpg";";      


            try
            {
                pictureBox1.Image = Image.FromFile(dataS);
            }
            catch (Exception ed)
            {
                MessageBox.Show("Logo Bulunamadı.\n" + ed);
            }


        }
        internal void RD(String activeus,String fullname)
        {
            activeUser = activeus;
            FName = fullname;

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox4.Checked = false;
                textBox8.Enabled = true;
                label6.Enabled = true;
                label1.Enabled = false;
                label3.Enabled = false;
                label4.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                comboBox1.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                label5.Enabled = false;
                label2.Enabled = false;
            }
            else if(checkBox3.Checked == false)
            {
                checkBox4.Checked = true;
                textBox8.Enabled = false;
                label6.Enabled = false;
                label1.Enabled = true;
                label3.Enabled = true;
                label4.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                comboBox1.Enabled = true;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                label5.Enabled = true;
                label2.Enabled =true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                checkBox3.Checked = false;
                textBox8.Enabled = false;
                label6.Enabled = false;
                label1.Enabled = true;
                label3.Enabled = true;
                label4.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                comboBox1.Enabled = true;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                label5.Enabled = true;
                label2.Enabled = true;
            }
            else if (checkBox4.Checked == false)
            {
                checkBox3.Checked = true;
                textBox8.Enabled = true;
                label6.Enabled = true;
                label1.Enabled = false;
                label3.Enabled = false;
                label4.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                comboBox1.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                label5.Enabled = false;
                label2.Enabled = false;
            } 
        }
    }
   
}
