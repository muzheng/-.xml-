using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace T01Research
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.WriteAllText(@"C:\data\buffer.txt",string.Empty);

            DateTime dateDayBegin = new DateTime();      //创建Day控件实例
            DateTime dateDayEnd = new DateTime();
            DateTime dateTimeBgein = new DateTime();    //创建time控件实例
            DateTime dateTimeEnd = new DateTime();

            /**********************begin day and time***************************/
            dateDayBegin = dateTimePicker1.Value;
            string yearBgein =Convert.ToString(dateDayBegin.Year);
            string monthBegin =Convert.ToString(dateDayBegin.Month);
            string dayBegin =Convert.ToString(dateDayBegin.Day);

            dateTimeBgein = dateTimePicker2.Value;
            string hourBegin =Convert.ToString(dateTimeBgein.Hour);
            string minuteBegin =Convert.ToString(dateTimeBgein.Minute);
            string secondBegin =Convert.ToString(dateTimeBgein.Second);

            string strDayAndTimeBegin = yearBgein + "/" + monthBegin + "/" + dayBegin + " " + hourBegin + ":"+minuteBegin+":"+secondBegin;

            DateTime DayAndTimeBegin = new DateTime();
            DayAndTimeBegin = DateTime.Parse(strDayAndTimeBegin);


           // Console.WriteLine(strDayAndTimeBegin);

            /************************end day and time**********************************/
            dateDayEnd = dateTimePicker3.Value;
            string yearEnd =Convert.ToString(dateDayEnd.Year);
            string monthEnd =Convert.ToString(dateDayEnd.Month);
            string dayEnd =Convert.ToString(dateDayEnd.Day);

            dateTimeEnd = dateTimePicker4.Value;
            string hourEnd =Convert.ToString(dateTimeEnd.Hour);
            string minuteEnd =Convert.ToString(dateTimeEnd.Minute);
            string secondEnd =Convert.ToString(dateTimeEnd.Second);

            string strDayAndTimeEnd = yearEnd + "/" + monthEnd + "/" + dayEnd +" "+ hourEnd + ":" + minuteEnd + ":" + secondEnd;

            DateTime DayAndTimeEnd = new DateTime();
            DayAndTimeEnd = DateTime.Parse(strDayAndTimeEnd);

            /********************************* var ******************************************/
            string id = "";
            string description = "";
            string unit = "";
            string strMin = "";
            string strMax = "";
            string strValue = "";

            int j = 0;     
            int k = 0;

           /*******************************************************************************/

            string path = @"C:\Continental\Report";         //@"Z:\Continental_work\Report"C:\Continental\Report
            DirectoryInfo file = new DirectoryInfo(path);

            if(file.Exists)
            {
                string[] fileNumber = Directory.GetFiles(path,"*xml",SearchOption.TopDirectoryOnly);
                int number = fileNumber.Length;

                textBox1.Text = Convert.ToString(number);

                for (int i = 0; i < number; i++)
                {


                    textBox2.Text = Convert.ToString(i);

                    string xmlName = Path.GetFileName(fileNumber[i]);
                    string fullPath = path + "\\" + xmlName;

                        try
                        {
                            XmlDocument xmlDocument = new XmlDocument();
                            xmlDocument.Load(fullPath);
                            XmlNodeList node = xmlDocument.SelectNodes("/Header");

                            foreach (XmlNode itemHeader in node)
                            {
                                string xmlTime = itemHeader.Attributes["Timestamp"].Value;
                                string xmlStatus = itemHeader.Attributes["Status"].Value;
                                string xmlSerial = itemHeader.Attributes["Serial"].Value;
                                string xmlNestId = itemHeader.Attributes["NestId"].Value;
                              
                                string xmlYear = xmlTime.Substring(0, 4);
                                string xmlMonth = xmlTime.Substring(5, 2);
                                string xmlDay = xmlTime.Substring(8, 2);

                                string xmlHour = xmlTime.Substring(11, 2);
                                string xmlMinute = xmlTime.Substring(14, 2);
                                string xmlSecond = xmlTime.Substring(17, 2);

                                string strdate = xmlYear + "/" + xmlMonth + "/" + xmlDay;
                                string strTime = xmlHour + ":" + xmlMinute + ":" + xmlSecond;

                                string strXmlDayAndTime = xmlYear + "/" + xmlMonth + "/" + xmlDay + " " + xmlHour + ":" + xmlMinute + ":" + xmlSecond;

                                DateTime XmlDayAndTime = new DateTime();
                                XmlDayAndTime = DateTime.Parse(strXmlDayAndTime);

                                if (DayAndTimeBegin > DayAndTimeEnd)
                                {
                                    MessageBox.Show("起始时间不能大于终止时间");
                                    return;
                                }
                                

                                    if (XmlDayAndTime >= DayAndTimeBegin && XmlDayAndTime <= DayAndTimeEnd)
                                    {
                                j = j + 1;
                                textBox3.Text = Convert.ToString(j);

                                        if (xmlStatus == "Fail")
                                        {
                                    k = k + 1;
                                    textBox4.Text = Convert.ToString(k);
                                            XmlNodeList nodeTest = xmlDocument.SelectNodes("/Header/Test");
                                            foreach (XmlNode itemTest in nodeTest)
                                            {
                                                string type = itemTest.Attributes["Type"].Value;

                                                if (type == "Real")
                                                {
                                                    double min = Convert.ToDouble(itemTest.Attributes["LSL"].Value);
                                                    double max = Convert.ToDouble(itemTest.Attributes["USL"].Value);
                                                    double value = Convert.ToDouble(itemTest.Attributes["Value"].Value);

                                                    if (!(value >= min && value <= max))
                                                    {
                                                        id = itemTest.Attributes["Id"].Value;
                                                        description = itemTest.Attributes["Description"].Value;
                                                        unit = itemTest.Attributes["Unit"].Value;
                                                        strMin = itemTest.Attributes["LSL"].Value;
                                                        strMax = itemTest.Attributes["USL"].Value;
                                                        strValue = itemTest.Attributes["Value"].Value;

                                                        FileStream bufferFile = new FileStream(@"C:\data\buffer.txt", FileMode.Append);
                                                        StreamWriter sw = new StreamWriter(bufferFile);
                                                        sw.WriteLine(strdate + "*" + strTime + "*" + xmlSerial + "*" + xmlNestId + "*" + id + "*" + description + "*" + strMin + "*" + strMax + "*" + strValue + "*" + unit);
                                                        sw.Close();
                                                    }
                                                }
                                                else if (type == "Boolean")
                                                {
                                                    string boValue = itemTest.Attributes["Value"].Value;
                                                    if (boValue == "0")
                                                    {
                                                        id = itemTest.Attributes["Id"].Value;
                                                        description = itemTest.Attributes["Description"].Value;
                                                        unit = itemTest.Attributes["Unit"].Value;
                                                        strMin = itemTest.Attributes["LSL"].Value;
                                                        strMax = itemTest.Attributes["USL"].Value;

                                                        FileStream bufferFile = new FileStream(@"C:\data\buffer.txt", FileMode.Append);
                                                        StreamWriter sw = new StreamWriter(bufferFile);
                                                        sw.WriteLine(strdate + "*" + strTime + "*" + xmlSerial + "*" + xmlNestId + "*" + id + "*" + description + "*" + strMin + "*" + strMax + "*" + boValue + "*" + unit);
                                                        sw.Close();
                                                    }
                                                }
                                            }
                                        }
                                    }

                                

                            }
                        }
                        catch
                        {
                            continue;
                        }
                   // }
                    Application.DoEvents();
                }
                Form2 formResult = new Form2();
                formResult.ShowDialog();
                Application.DoEvents();
            }
            else
            {
                MessageBox.Show("未找到.xml文件存储的文件夹或者存储文件夹路径不正确！");
            }

           
            
        }
    }
}
