using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace 三角网的实现
{
    #region //点
    class Points
    {
        //数据点的相应属性变量
        private string name;//点名
        private double   x;//x坐标值
        private double   y;//y
        private double   z;//z
        private double  [] normal = new double  [3];
        public string Name { set { name = value; } get { return name; } }
        public double   X { set { x = value; } get { return x; } }
        public double  Y { set { y = value; } get { return y; } }
        public double   Z { set { z = value; } get { return z; } }
        public Points(double   xx,double   yy, double   zz)
        {
            x = xx; y = yy; z = zz;

        }
        public Points()
        { }
        public static  bool operator ==(Points a, Points b)
        {
            if (a.X == b.X && a.Y == b.Y && a.Z == b.Z)
            { return true; }
            else { return false; }
        }
        public  static bool operator !=(Points a, Points b)
        {
            if (a.X != b.X && a.Y != b.Y && a.Z != b.Z)
            { return true; }
            else { return false; }
        }
    }
    #endregion
    #region//边
    class Edge
    {
        private Points[] points;//两个点
        private string name;//名字
        private string[] triname;//边的两邻接三角形名字
        private double length;//边的长度
        private int usetimes;//使用的次数

        public Points[] GetPoints { get { return points; } }
        public Points GetPointa { get { return points[0]; } }
        public Points GetPointb { get { return points[1]; } }
        public Edge()
        { }
        public Edge(Points a, Points b)
        {
            Init();
            points[0] = a; points[1] = b;//点赋值
            name = "Edge:" + a.Name +"--"+ b.Name;//边的名字赋值
            length = Method.Distance(a, b);//距离赋值



        }
        public int Usetimes
        {
            set 
            {
                if (usetimes >= 2) { MessageBox.Show("边的使用次数超过限制"); }
                else { usetimes++; }
            }
            get { return usetimes; }

        }
        private bool Init()
        {

            triname = new string[2];
            points = new Points[2];
            return true;
        }
        public static bool operator ==(Edge a, Edge b)
        {
            if (a.GetPointa == b.GetPointa && a.GetPointb == b.GetPointb )
            { return true; }
            
            else { return false ; }
        }
        public static bool operator !=(Edge a, Edge b)
        {
            if (a.GetPointa != b.GetPointa && a.GetPointb != b.GetPointb)
            { return true; }
            
            else { return false; }
 
        }

    }
    #endregion
    #region //三角形
    class Triangle
    {

        private Points[] tripoints;
        private string name;//三角形的名字
        private Edge[] triedge;//三角形的边
        public Points GetTriPointA { get { return tripoints[0]; } }
        public Points GetTriPointB { get { return tripoints[1]; } }
        public Points GetTriPointC { get { return tripoints[2]; } }
        public bool Init()
        {
            tripoints = new Points[3];
            triedge = new Edge[3];
            return true;
        }
        public Triangle(Points a, Points b, Points c)//顺时针方向
        {
            Init();
            name = "Tri" + a.Name + "-" + b.Name + "-" + c.Name;//三角形名字

            tripoints[0] = a; tripoints[1] = b; tripoints[2] = c;//三角形的点集
            triedge[0] = new Edge(a, b); triedge[1] = new Edge(a, c); triedge[2] = new Edge(b, c);//三角形边集



        }
        ~Triangle()
        {

        }



    }
    #endregion

}
