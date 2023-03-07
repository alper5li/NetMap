using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace NetMap
{
    public partial class Form4 : Form
    {
        public SQLiteConnection myConnection;
        bool isCorrect;
        bool success=false;
        string activeUser;
        string activeFull;
        public void connect()
        {

            string dataS = "Data Source = ";

            dataS += System.AppContext.BaseDirectory;
            dataS = dataS.Substring(0, dataS.Length - 25);
            dataS += "database\\Databases\\Userlist.db";
            myConnection = new SQLiteConnection(dataS);
            myConnection.Open();

        }

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            label8.Visible = false;
            label2.Visible = true;
            label3.Visible = true;
            button1.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            checkBox1.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            button3.Visible = false;

        }

        public string getLogLoc()
        {
            string dataS = "";
            dataS += System.AppContext.BaseDirectory;
            dataS = dataS.Substring(0, dataS.Length - 25);
            dataS += "database\\Log\\";
            return dataS;
        }

        private void Welcome()
        {
            checkBox1.Visible = true;
            label2.Visible = false;
            label3.Visible = false;
            button1.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            button2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            label8.Visible = true;
            button3.Visible = true;
        }
        private void Authenticate()
        {
            
            String FName="";
            String isAD = "";
            connect(); 
            try { 
                SQLiteCommand commandAuth = new SQLiteCommand("select username, password,isAdmin,FName from users order by 1 ", myConnection);
                SQLiteDataReader sqReader = commandAuth.ExecuteReader();
                while (sqReader.Read())
                {
                    if (textBox1.Text == sqReader["username"].ToString())
                    {
                        if (textBox2.Text == sqReader["password"].ToString())
                        {
                            if (sqReader["isAdmin"].ToString() == "yes")
                            {
                                try
                                {


                                    String uname = sqReader["username"].ToString();
                                    FName = sqReader["FName"].ToString();
                                    label8.Text = "Welcome Back " + FName + " !";
                                    activeUser = uname;
                                    activeFull = FName;
                                    String Log = getLogLoc() + "Log.txt";
                                    Thread.Sleep(100);
                                    String LOGIN = ("  [*] "+ "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss dddd") + "]" + " [ADMIN] ("+FName+":"+uname+") [ACTION] => [LOGIN]");
                                    using (StreamWriter sw = new StreamWriter(Log, true))
                                    {
                                        sw.WriteLine(LOGIN);
                                    }
                                    this.isCorrect = true;
                                }
                                catch (Exception xe)
                                {
                                    MessageBox.Show("Exception occured :"+xe.ToString());
                                }


                            }
                            else if(sqReader["isAdmin"].ToString() == "no")
                            {
                                MessageBox.Show("Seems you dont have admin privileges.");
                                String Log = getLogLoc() + "Log.txt";
                                Thread.Sleep(100);
                                String LOGIN = ("  [WARNING] " + "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss dddd") + "]" + " [ADMIN] " + "User =>  [" + textBox1.Text + "] (Not an Admin)  Attempted to Login Admin Panel.");
                                using (StreamWriter sw = new StreamWriter(Log, true))
                                {
                                    sw.WriteLine(LOGIN);
                                }
                            }
                        }

                    }


                }
                if (this.isCorrect == true)
                {
                    Welcome();
                }
                else
                {
                    MessageBox.Show("Wrong Credentials. Please Check Your Username and Password ! (maybe you are not an admin ?)", "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    String Log = getLogLoc() + "Log.txt";
                    Thread.Sleep(100);
                    String LOGIN = ("  [WARNING] " + "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss dddd") + "]" + " [ADMIN] " + "Strange username =>  [" + textBox1.Text + "] Attempted to Login and failed.");
                    using (StreamWriter sw = new StreamWriter(Log, true))
                    {
                        sw.WriteLine(LOGIN);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(("Exception occured while logging in:\n" + e.Message + "\t" + e.GetType()));
            }

            

        }

        private void AddUser()
        {
            connect();
            try
            {
                SQLiteCommand adduser = new SQLiteCommand();
                adduser.CommandText ="INSERT INTO users (username, password,isAdmin,FName) VALUES (@USER, @PASS, @ISADMIN, @FNAME)";
                adduser.Connection = myConnection;
                String usnm = textBox3.Text.ToString();
                String pas = textBox4.Text.ToString();
                String Fnm = textBox5.Text.ToString();
                String isAdm;
                if (checkBox1.Checked)
                    {
                    isAdm = "yes";
                }
                else
                {
                    isAdm = "no";
                }

                if (usnm !="" && pas !="" && Fnm != "")
                {
                    adduser.Parameters.AddWithValue("@USER", usnm);
                    adduser.Parameters.AddWithValue("@PASS", pas);
                    adduser.Parameters.AddWithValue("@ISADMIN", isAdm);
                    adduser.Parameters.AddWithValue("@FNAME", Fnm);
                    adduser.ExecuteNonQuery();
                    this.success = true;
                    String Log = getLogLoc() + "Log.txt";
                    Thread.Sleep(100);
                    String LOGIN = ("  [*] "+"[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss dddd") + "]" + " [ADMIN] "+ "(" + activeFull + ":" + activeUser + ") " + "[ACTION] => [Added User] Fullname:[" + Fnm + "] Username :[" + usnm+"]");
                    using (StreamWriter sw = new StreamWriter(Log, true))
                    {
                        sw.WriteLine(LOGIN);
                    }
                }
                else
                {
                    MessageBox.Show("Empty Values are not allowed !", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }
            catch (Exception e)
            {
                MessageBox.Show(("Exception occured while creating table:\n" + e.Message + "\t" + e.GetType()));
                this.success = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddUser();
            if (this.success == true)
            {
                MessageBox.Show("User added to database successfully.");
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Authenticate();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            String Log = getLogLoc() + "Log.txt";
            Thread.Sleep(100);
            String LOGIN = ("  [*] "+"[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss dddd") + "]" + " [ADMIN] " + "(" + activeFull + ":" + activeUser + ")  [ACTION] => " + "Opened the Log File");
            using (StreamWriter sw = new StreamWriter(Log, true))
            {
                sw.WriteLine(LOGIN);
            }

            // YENI FORM ACIP LOG DOSYASINI OKUTMA //


            Form5 form5 = new Form5();
            form5.Show();

        }
    }
}
