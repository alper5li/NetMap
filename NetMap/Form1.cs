using System.Net.NetworkInformation;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace NetMap
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        bool isCorrect;
        string Uname;
        String activeUser;
        String FN;
        public SQLiteConnection myConnection;


       public string getLogLoc()
        {
            string dataS="";
            dataS += System.AppContext.BaseDirectory;
            dataS = dataS.Substring(0, dataS.Length - 25);
            dataS += "database\\Log\\";
            return dataS;
        }



        private void Welcome()
        {
            this.Hide();
            Form3 Command = new Form3();
            Command.Closed += (s, args) => this.Close();
            Command.RD(activeUser,FN);
            Command.Show();
            
            
        }


        public void connect()
        {
            string dataS = "Data Source = ";
            dataS += System.AppContext.BaseDirectory;
            dataS = dataS.Substring(0, dataS.Length - 25);
            dataS += "database\\Databases\\" + Uname + ".db";
            myConnection = new SQLiteConnection(dataS);
            myConnection.Open();

            /*myConnection = new SQLiteConnection("Data Source = C:\\..\\database\\FinalProject.db");
           
            myConnection.Open();*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {



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
                MessageBox.Show("Logo Bulunamadý.\n" + ed);
            }




        }

   



        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            Form4 form4 = new Form4();
            form4.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            
        }




        private void Authenticate()
        {
            String uname = "";
            String FName = "";
            Uname = "Userlist";
            connect();



            if (textBox1.Text !="" && textBox2.Text != "")
            
            
            
            {
                try
                {
                    SQLiteCommand commandAuth = new SQLiteCommand("select username, password,FName from users order by 1 ", myConnection);
                    SQLiteDataReader sqReader = commandAuth.ExecuteReader();
                    while (sqReader.Read())
                    {
                        if (textBox1.Text == sqReader["username"].ToString())
                        {
                            if (textBox2.Text == sqReader["password"].ToString())
                            {

                                try
                                {

                                    uname = sqReader["username"].ToString();

                                    FName = sqReader["FName"].ToString();
                                    activeUser = uname;
                                    FN = FName;
                                    // LOG ENTER 
                                    String Log = getLogLoc()+"Log.txt";
                                    Thread.Sleep(100);
                                    String LOGIN =("  [*] "+"["+DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss dddd") +"]"+ " [LOGIN] "+"Fullname :["+FName+"] User :["+uname+"]");                      
                                    using (StreamWriter sw = new StreamWriter(Log, true))
                                    {
                                        sw.WriteLine(LOGIN);
                                    }

                                    this.isCorrect = true;
                                }
                                catch (Exception xe)
                                {
                                    MessageBox.Show(xe.ToString());
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
                        String LOGIN = ("  [WARNING] " + "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss dddd") + "]" + " [LOGIN] " + "Username =>  [" + textBox1.Text + "] Attempted to Login and failed.");
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
            else
            {
                MessageBox.Show("Empty values not allowed...");
            }
            myConnection.Close();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Authenticate();

        
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
           
        }
    }
}