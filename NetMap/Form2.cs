using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Xml;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace NetMap
{

    public partial class Form2 : Form
    {
        int stopwatchint = 0;
        string activeUser;
        bool isCon = true;
        string IPAdd1;
        string IPAdd2;
        string IPAdd3;
        string IPAdd4;
        string IPa;
        string DName;
        int Nmask=32;
        int searchType = 1;
        // 1 = IPSEARCH 2=DOMAINSEARCH
        Stopwatch stopwatch = new Stopwatch();


        bool newDB=false;
        string newDBNAME;
        string tableName="ipDB";
        
        int TAMSAYI;
        
        string datalocation;
        public SQLiteConnection myConnection;
        string Uname = "IPLIST";

     

        public void dataLocation(String path)
        {
            datalocation += System.AppContext.BaseDirectory;
            datalocation = datalocation.Substring(0, datalocation.Length - 25);
            datalocation += path;
        }

        public void connect()
        {
            try
            {
                string dataS = "Data Source = ";
                dataS += System.AppContext.BaseDirectory;
                dataS = dataS.Substring(0, dataS.Length - 25);
                dataS += "database\\Databases\\" + Uname + ".db";
                myConnection = new SQLiteConnection(dataS);
                myConnection.Open();
            }
            catch(Exception e)
            {
                MessageBox.Show("Exception occured while connecting to the database\nMessage :" + e);
            }

            /*myConnection = new SQLiteConnection("Data Source = C:\\..\\database\\IPLIST.db");
           
            myConnection.Open();*/
        }





        public Form2()
        {
            InitializeComponent();
        }
 
        private void Form2_Load(object sender, EventArgs e)
        {

           
            button2.Visible = false;
            
            Control.CheckForIllegalCrossThreadCalls = false;
              
        }
       
        async void startStopwatch()
        {
            while(stopwatchint ==1)
            {
                label7.Text=stopwatch.Elapsed.ToString();
            }
        }
        //asaync task
        public void PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;
           

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;

                


            }
            catch (PingException sd)
            {
               
            }
            finally
            {
                 
                if (pinger != null)
                {
                    pinger.Dispose();
                    String PCName="";
                    String online;
                    String Mac="";
                    String deneme;
                    DateTime dtTarihsaat = DateTime.Now;

                    if(searchType == 1)
                    {
                        try
                        {
                            //ASYNC ().HostName//
                            PCName = Dns.GetHostEntry(nameOrAddress).HostName;

                        }
                        catch (Exception)
                        {
                            PCName = "[?]";
                        }
                        try
                        {

                            Mac = GetMac(nameOrAddress);

                        }
                        catch (Exception)
                        {
                            Mac = "[?]";
                        }

                    }
                    else if(searchType == 2)
                    {
                        try
                        {
                            //ASYNC ().HostName//
                            PCName = nameOrAddress;
                            var address = Dns.GetHostAddresses(nameOrAddress)[0];
                            nameOrAddress = address.ToString();
                            try
                            {

                                Mac = GetMac(address.ToString());

                            }
                            catch (Exception)
                            {
                                Mac = "[?]";
                            }


                        }
                        catch (Exception)
                        {
                            nameOrAddress = "[?]";
                        }


                    }
                    


                    /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT /////SUSPECT 




                    connect();
                        SQLiteCommand command = new SQLiteCommand();
                    if (pingable == true)
                    {
                        online = "[+] ONLINE [+]";
                    }
                    else
                    {
                        online = "[-] OFFLINE [-]";
                    }
                    
                        command.CommandText = "INSERT INTO " + tableName + " (IPAddress, Status, MachineName, MAC, DTime) VALUES (@IPA, @STATS, @MACNAM, @MAC, @DayTime)";
                        command.Connection = myConnection;
                        command.Parameters.AddWithValue("@IPA", nameOrAddress);
                        command.Parameters.AddWithValue("@STATS", online);
                        command.Parameters.AddWithValue("@MACNAM", PCName);
                        command.Parameters.AddWithValue("@MAC", Mac);
                        command.Parameters.AddWithValue("@DayTime", dtTarihsaat.ToString());
                        command.ExecuteNonQuery();
                    
                        
                        
                    richTextBox1.Text += "| [*] "+nameOrAddress+"  ~ "+online+ "  ~ "+Mac+ "  ~ "+PCName+"  ~ " + dtTarihsaat+"  |"+ "\n-------------------------------------------------------------------------------------------\n";
                   
                }
                label7.Text = stopwatch.Elapsed.ToString().Substring(0, stopwatch.Elapsed.ToString().Length - 8);
            }

            
        }

        public String GetMac(String IP)
        {
            var macIpPairs = GetAllMacAddressesAndIppairs();
            int index = macIpPairs.FindIndex(x => x.IpAddress == IP);
            if (index >= 0)
            {
                return macIpPairs[index].MacAddress.ToUpper();
            }
            else
            {
                return "[*]";
            }
        }
        public List<MacIpPair> GetAllMacAddressesAndIppairs()
        {
            List<MacIpPair> mip = new List<MacIpPair>();
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a ";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string cmdOutput = pProcess.StandardOutput.ReadToEnd();
            string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";

            foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
            {
                mip.Add(new MacIpPair()
                {
                    MacAddress = m.Groups["mac"].Value,
                    IpAddress = m.Groups["ip"].Value
                });
            }

            return mip;
        }
        
        public struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
        }
        internal void receiveIPData(string IP, string IP2, string IP3, string IP4, string NETMASK,string newDB,string newDBNAME,String tableName,String activeus,String key2)
        {
            if (key2 == "ABCD")
            {
                if (Convert.ToBoolean(newDB) == true)
                {
                    this.newDB = Convert.ToBoolean(newDB);
                    this.newDBNAME = newDBNAME.ToString();
                    this.tableName = tableName.ToString();
                }
                activeUser = activeus;
                this.IPAdd1 = IP;
                this.IPAdd2 = IP2;
                this.IPAdd3 = IP3;
                this.IPAdd4 = IP4;
                this.Nmask = Convert.ToInt32(NETMASK);
                searchType = 1;
            }

            
            
        }
        internal void receiveDomainData(String domainName, String activeus, String key1)
        {
            if(key1 == "DCBA") 
            {
                Uname = "DomainDB";
                tableName = "DomainSearch";
                activeUser = activeus;
                DName = domainName;
                searchType = 2;
                Nmask = 32;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            button1.Visible = false;
            button2.Visible = true;
            

            if (newDB == true)
            {
                //create new db
                string dataS;
                dataS = System.AppContext.BaseDirectory;
                dataS = dataS.Substring(0, dataS.Length - 25);
                dataS += "database\\Databases\\" + newDBNAME + ".db";
                
                SQLiteConnection.CreateFile(dataS);
                Uname = newDBNAME;
                connect();
                SQLiteCommand createT = new SQLiteCommand("CREATE TABLE " + tableName + "(IPAddress TEXT, Status TEXT, MachineName TEXT,  NameSurname TEXT, MAC TEXT UNIQUE, DTime TEXT, count INTEGER UNIQUE, PRIMARY KEY(count AUTOINCREMENT))", myConnection);
                createT.ExecuteNonQuery();
            }

            richTextBox1.Text += "Scanning ....\n \n----------------------------------------------------------------------------------\n";
            Thread.Sleep(100);
            
            stopwatch.Start();
            
            if (searchType == 1)
            {
                IPa = CALCULATEEEE();
                LRC();
            }
            else if(searchType == 2)
            {
                IPa = DName;
                label2.Text = "1";
                label3.Text = "1";
                PingHost(IPa);
                richTextBox1.Text += "\n\n[*] Scan Stopped.\t=>  ["+DateTime.Now+"]";
                MessageBox.Show("Completed !");
                stopwatch.Stop();
                stopwatchint = 0;
            }
            //// formload düzenle
            ///
           
            
           
           


        }
       
        async void LRC()
        {
            var task1 = await Task.Run(() =>
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                Brute(IPa);
               
                if (isCon == false)
                {
                    cts.Cancel();
                    richTextBox1.Text += "\n\n\t[*] Scan Stopped.\t=>  [" + DateTime.Now + "]";
                }
                return "";
            });
            MessageBox.Show("Completed !");
            stopwatch.Stop();
            stopwatchint = 0;
           
        }
        public void STOP()
        {
            isCon = false;
            MessageBox.Show("İşlem Durduruldu.");
            

        }
       
        private void Brute(String IPP)
        {
            Task[] tasks = new Task[50];
            int count = 0;
            label2.Text = count.ToString() ;

            if (TAMSAYI == 0)
            {
                int i;
                int j;
                int k;
                int l;
                double totalTry = Convert.ToDouble(IPAdd1) * 254 * 254 * 254;
                label3.Text = (totalTry).ToString();
                for (i = 1; i < Convert.ToInt32(IPAdd1); i++)
                {
                    for (j = 1; j < 255; j++)
                    {
                        for (k = 1; k < 255; k++)
                        {

                            for (l = 1; l < 255; l++)
                            {
                                
                                    String IPX = i.ToString() + "." + j.ToString() + "." + k.ToString() + "." + l.ToString();
                                    PingHost(IPX);
                                    count++;
                                    label2.Text = count.ToString();


                                
                                

                            }

                            l = 1;


                        }
                        k = 1;
                    }
                    j = 1;
                }
            }
            else if (TAMSAYI == 1)
            {
                int i;
                int j;
                int k;
                double totalTry = Convert.ToDouble(IPAdd2) * 254 * 254;
                label3.Text = (totalTry).ToString();
                for (i = 1; i < Convert.ToInt32(IPAdd2); i++)
                {
                    for (j = 1; j < 255; j++)
                    {
                        for (k = 1; k < 255; k++)
                        {
                           
                                String IPX = IPAdd1 + "." + i.ToString() + "." + j.ToString() + "." + k.ToString();
                                PingHost(IPX);
                                count++;
                                label2.Text = count.ToString();
                            
                        }
                        k = 1;
                    }
                    j = 1;
                }
            }
            else if (TAMSAYI == 2)
            {
                int i;
                int j;
                double totalTry = Convert.ToDouble(IPAdd3) * 254;
                label3.Text = (totalTry).ToString();
                for (i = 1; i < Convert.ToInt32(IPAdd3); i++)
                {
                    for (j = 1; j < 255; j++)
                    {
                        
                            String IPX = IPAdd1 + "." + IPAdd2 + "." + i.ToString() + "." + j.ToString();
                            PingHost(IPX);
                            count++;
                            label2.Text = count.ToString();
                        
                    }
                    j = 1;
                }
            }
            else if (TAMSAYI == 3)
            {
                /// HATA
                double totalTry = Convert.ToDouble(IPAdd4);
                label3.Text = (totalTry).ToString();
                for (int i = 1; i < Convert.ToInt32(IPAdd4); i++)
                {
                    
                    
                        String IPX = IPAdd1 + "." + IPAdd2 + "." + IPAdd3 + "." + i.ToString();
                        PingHost(IPX);
                        count++;
                        label2.Text = count.ToString();
                    
                }
            }
            else if (TAMSAYI == 4)
            {
                
                    label3.Text = "1";
                    PingHost(IPP);
                    count++;
                    label2.Text = count.ToString();
                
            }
            else
            {
                MessageBox.Show("Scan Stopped.");
            }
                /// SUSPECCCCC
               
                
            }
           
        
        private String CALCULATEEEE()
        {
            
            String CompleteIP = IPAdd1 + "." + IPAdd2 + "." + IPAdd3 + "." + IPAdd4;
            

            if (Nmask > 0 && Nmask < 9 && Nmask != 8)
            {
                //Her şey değişken

                IPAdd1 = BitCalc(Nmask,0).ToString();
                IPAdd2 = "255";
                IPAdd3 = "255";
                IPAdd4 = "255";
                this.TAMSAYI = 0;


            }
            else if (Nmask > 8 && Nmask < 17 && Nmask != 16)
            {
                //IPAdd1              Sabit
                IPAdd2 = BitCalc(Nmask,1).ToString();
                IPAdd3 = "255";
                IPAdd4 = "255";
                this.TAMSAYI = 1;

            }
            else if (Nmask > 16 && Nmask < 25 && Nmask != 24)
            {
                //IPAdd1 IPAdd2          Sabit
                IPAdd3 = BitCalc(Nmask,2).ToString();
                IPAdd4 = "255";
                this.TAMSAYI = 2;
            }
            else if (Nmask > 24 && Nmask < 33 && Nmask != 32)
            {
                //IPAdd1 IPAdd2 IPAdd3      Sabit
                IPAdd4 = BitCalc(Nmask,3).ToString();
                this.TAMSAYI = 3;
            }
            else if (Nmask == 8)
            {
               
                IPAdd2 = "255";
                IPAdd3 = "255";
                IPAdd4 = "255";
                this.TAMSAYI = 1;
            }
            else if (Nmask == 16)
            {
                IPAdd3 = "255";
                IPAdd4 = "255";
                this.TAMSAYI = 2;
            }
            else if (Nmask == 24)
            {
                IPAdd4 = "255";
                this.TAMSAYI = 3;
            }
            else if (Nmask == 32)
            {
                //nothing
                this.TAMSAYI = 4;


            }
            return IPAdd1 + "." + IPAdd2 + "." + IPAdd3 + "." + IPAdd4;
            /// <summary>
            /// ////////////////////// KONTROL EDİLECEK
            /// </summary>
            /// 
            /// <returns></returns>
            
        }
        public static int BitCalc(int value, int tamSayı)
        {
            int x = value % 8;
            int Sonuc=0;
            // x = kalan    

            if (tamSayı == 0)
            {
                Sonuc = Convert.ToInt32((Math.Pow(2, (32 - x))) - 2);
            }
            else if (tamSayı == 1)
            {
                Sonuc = Convert.ToInt32((Math.Pow(2, (24 - x))) - 2);
            }
            else if (tamSayı == 2)
            {
                Sonuc = Convert.ToInt32((Math.Pow(2, (16 - x))) - 2);
            }
            else if (tamSayı == 3)
            {
                Sonuc = Convert.ToInt32((Math.Pow(2, (8 - x))) - 2);
            }
           

            return Sonuc;

            

            // Bitcalc değeri range döndürür; ÖRNEK = BitCalc(28) == 14              BitCalc(15) == 124

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           STOP();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    } 
}
