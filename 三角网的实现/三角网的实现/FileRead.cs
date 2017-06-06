using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;


namespace 三角网的实现
{
    class FileRead
    {
        #region//相应的变量
        private string filename;//定义的文件名用来存储文件

        #endregion


        public FileRead()
        {

            //运用对话框找到文件
            OpenFileDialog ofg = new OpenFileDialog();
            ofg.Filter = "文本文件(*.txt)|*.txt";

            if (ofg.ShowDialog() == DialogResult.OK)
            {
                try { filename = ofg.FileName; }
                catch (Exception ex) { MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }


        }

        #region//文件读取的方法封装

        public bool lookfile(ref List<Points> Pointsdata)
        {

            Pointsdata.Clear();
            //提取数据：1.按照行提取数据2.将每一行数据转化为单个数据
            FileStream file = new FileStream(filename, FileMode.Open);

            StreamReader filesouce = new StreamReader(file);
            while (!filesouce.EndOfStream)
            {
                string line = filesouce.ReadLine();
                string[] str = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
                //将转化的数据放入点集合中去
                Points temp = new Points();
                temp.Name = str[0];
                temp.X = Convert.ToDouble(str[1]);
                temp.Y = Convert.ToDouble(str[2]);
                temp.Z = Convert.ToDouble(str[3]);
                Pointsdata.Add(temp);
            }

            return true;
        }
        #endregion
    }
}
