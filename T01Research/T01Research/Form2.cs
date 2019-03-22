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

namespace T01Research
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("日期",typeof(string));
            dt.Columns.Add("时间", typeof(string));
            dt.Columns.Add("串号", typeof(string));            
            dt.Columns.Add("端口", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("下线项", typeof(string));
            dt.Columns.Add("最小值", typeof(string));
            dt.Columns.Add("最大值", typeof(string));
            dt.Columns.Add("下线值", typeof(string));
            dt.Columns.Add("单位", typeof(string));
           // dt.Columns.Add("limit", typeof(string));

           using (StreamReader read=new StreamReader(@"C:\data\buffer.txt",Encoding.Default))
            {
                string line;
                while((line=read.ReadLine())!=null)
                {
                    //string[] data = read.ReadLine().Replace("+"," ").Split(' ');
                    string[] data = line.Split('*');

                   // dataGridView1.Rows.Add(data);

                    DataRow dr = dt.NewRow();
                    dr[0] = data[0];
                    dr[1] = data[1];
                   // Console.WriteLine(data[1]);
                    dr[2] = data[2];
                    dr[3] = data[3];
                    dr[4] = data[4];
                    dr[5] = data[5];
                    dr[6] = data[6];
                    dr[7] = data[7];
                    dr[8] = data[8];
                    dr[9] = data[9];
                   //dr[10] = data[10];
                    dt.Rows.Add(dr);
                }
            }
          this.dataGridView1.DataSource = dt;
        }
    }
}
