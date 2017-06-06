using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 三角网的实现
{
    class BuildTin
    {
        /*定义一些变量用来存储 点 边  三角形*/
        public List<Points> Pointsdata = new List<Points>();//点集


        public Queue<Edge> Edgedata = new Queue<Edge>();//边集1
        public Queue<Edge> UsedEdgedata = new Queue<Edge>();//边集2
        public Queue<Edge> OnlyEdgedata = new Queue<Edge>();//边集3


        public Queue<Triangle> Triangledata = new Queue<Triangle>();//三角形集
        public int SumPoints { get { return Pointsdata.Count; } }
        public int SumTriangle { get { return Triangledata.Count; } }
        private Edge FirstEdge=new Edge ();//搜索开始的第一条边
        private Points ThirdPoint=new Points ();//需要找到的第三点
        private Points centerPoint=new Points ();//中间点
   
        public int SumEdge { get { return Edgedata.Count; } }
        
        public  void init()
        {
            double xmin = Pointsdata[0].X;

            foreach (Points x in Pointsdata)
            {
                if (xmin > x.X) { xmin = x.X; }
            }

            double xmax = Pointsdata[0].X;
            foreach (Points x in Pointsdata)
            {
                if (xmax < x.X) { xmax = x.X; }
            }
            double  ymin = Pointsdata[0].Y;
            foreach (Points x in Pointsdata)
            {
                if (ymin > x.Y) { ymin = x.Y; }
            }
            double  ymax = Pointsdata[0].Y;
            foreach (Points x in Pointsdata)
            {
                if (ymax < x.Y) { ymax = x.Y; }
            }
            double zmin = Pointsdata[0].Z;
            foreach (Points x in Pointsdata)
            {
                if (zmin > x.Z) { zmin = x.Z; }
            }
            double zmax = Pointsdata[0].Z;
            foreach (Points x in Pointsdata)
            {
                if (zmax < x.Z) { zmax = x.Z; }
            }

            double xx = (xmax + xmin) / 2;
            double  yy = (ymax + ymin) / 2;
            double zz = (zmax + zmin) / 2;
            centerPoint = new Points(xx, yy, zz);
 
        }
        //初始化，调用FileRead中的方法读取数据存入Pointdata中去
        public Points CenterPoint
        {

            get
            {
                return centerPoint;

            }
        }

        public bool GetPointData()
        {

            FileRead R = new FileRead();
            R.lookfile(ref Pointsdata);
            return true;
        }
        //定义一个方法来找到点群的中心点
        
        //1.寻找第一个点，找到x坐标最小的点做为第一点来往右搜寻
        public Points GetFirstPoint()
        {
            Points temp = Pointsdata[SumPoints / 2+1];
            foreach (Points x in Pointsdata)
            {
                if (x.X < temp.X) { temp = x; }
            }
            return temp;
        }
        //2.寻找第一条初始基边。方法：距离第一点最近的点
        public bool GetFounmentEdge()
        {
            Points firstpoint = GetFirstPoint();
            Points TempPoint = Pointsdata[SumPoints/2];

            double DistanceTemp = Method.Distance(firstpoint, TempPoint);
            for (int i = 0; i < SumPoints; i++)
            {
                if (Pointsdata[i] == firstpoint) { continue; }
                double distance = Method.Distance(firstpoint, Pointsdata[i]);
                if (distance <= DistanceTemp) { TempPoint = Pointsdata[i]; DistanceTemp = distance; }
            }
            Edge FundmentalLine = new Edge(firstpoint, TempPoint);
            //返回找到的第一条基线，使用次数第一次加1
             Edgedata.Enqueue(FundmentalLine);
            return true;
        }
        //将边集合中的第一条边出队列1,进入对列2参与寻找第三点的过程
        public Edge GetFirstEdge()
        {
            if (Edgedata.Count == 0)
            {
                MessageBox.Show("边集合中为空"); 
            }
            else
            {
                //第一条边出对列
                FirstEdge = Edgedata.Dequeue();
                UsedEdgedata.Enqueue(FirstEdge);
               
            }
            return FirstEdge;
        }
        //3.以第一条边向外扩展，搜索下一个点，形成三角形
        public bool Process()
        {
          
           GetFounmentEdge();//2.找到基边
            while (Edgedata.Count != 0)
            {
                Edge StartE = new Edge(); StartE = Edgedata.Dequeue();

                if (JudgeUsetime(StartE) == false) { continue; }
                Points ThirdP = new Points();
                ThirdP = GetThirdPoint(StartE);//3.以基边为基础找第三边
                if (ThirdP.X == 0 && ThirdP.Y == 0 && ThirdP.Z == 0) { UsedEdgedata.Enqueue(StartE); StartE.Usetimes++; continue; }//该基边无法扩展到其他的边
                bool tag1 = false; bool tag2 = false;
                Edge Edge1, Edge2;//用来存储基边扩展到的两条边
                Edge1 = new Edge(ThirdP,StartE.GetPointa);
                Edge2 = new Edge(ThirdP,StartE.GetPointb);
                if (JudgeNowEdge(Edge1) == true || JudegeUsedEdge(Edge1) == true) //扩展到的边已经存在
                {
                    tag1 = true;

                }
                if (JudegeUsedEdge(Edge2) == true || JudgeNowEdge(Edge2) == true)
                {
                    tag2 = true;
                }
                //判断边，然后进入边集合。
                if (tag1 == false) {Edgedata.Enqueue(Edge1); }
                if (tag2 == false) { Edgedata.Enqueue(Edge2); }
                if (tag1 == false || tag2 == false) 
                { 
                    StartE.Usetimes++;
                    Triangle temp = new Triangle(StartE.GetPointa,StartE.GetPointb,ThirdP);
                    Triangledata.Enqueue(temp);
                }
                UsedEdgedata.Enqueue(StartE);
                if(tag1==true||tag2==true){OnlyEdgedata.Enqueue(StartE);}
            
            }
            return true;
           
        }
        public Points GetThirdPoint(Edge temp)
        {
            double TempCos = 1;
            Points TempPoint = new Points(0, 0, 0);
            for (int i = 0; i < SumPoints; i++)
            {
                if (Method.Direction(temp, Pointsdata[i]) == 1) //搜索方向为右边
                {
                    //求出这些点的COS值最小的值
                    double length = Method.Cos(temp.GetPointa, temp.GetPointb, Pointsdata[i]);
                    if (TempCos > length)
                    { TempPoint = Pointsdata[i]; TempCos = length; }

                }
            }
            return  TempPoint;

        }
        //4.找寻第三点形成的其他的三角形边
       
        public bool JudegeUsedEdge(Edge temp)
        {
            bool tag1=false;
            foreach (Edge one in Edgedata)
            {
                if (one == temp) { tag1 = true;}
 
            }
            return tag1;
 
        }
        public bool JudgeNowEdge(Edge temp)
        {
            bool tag2 = false;
            foreach (Edge two in UsedEdgedata)
            {
                if (two == temp) { tag2 = true; }
            }
            return tag2;
 
        }
        public bool JudgeUsetime(Edge temp)
        {
            int usetime = 0; bool tag=false;
            Edge Contrtemp = new Edge(temp.GetPointb,temp.GetPointa);
            usetime = temp.Usetimes + Contrtemp.Usetimes;
            if (usetime < 2) {tag= true; }
            return tag;
           
        }
      
    

    }
}
