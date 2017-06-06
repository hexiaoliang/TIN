using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 三角网的实现
{
    class Method
    {
        //定义一个求距离的方法
        public static double Distance(Points a, Points b)
        {
            return Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2));
        }

        //定义一个求角度 的cos的方法
        public static double Cos(Points a, Points b, Points p)
        {
            //分别求出三边的边长
            double ab = Distance(a, b);
            double ap = Distance(a, p);
            double bp = Distance(b, p);
            //余弦定理求出<apb的cos 值
            double cosJapb = (double)(ap * ap + bp * bp - ab * ab) / (2 * ap * bp);
            return cosJapb;
        }
        //定义一个判断p点在ab连线左边还是右边的判断方法。
        //左边：equation<=0;
        public static int Direction(Edge ab, Points p)
        {
            Points a = new Points(); a = ab.GetPoints[0]; Points b = new Points();b= ab.GetPoints[1];
            
            //double Equation = (p3.y - p1.y) * (p2.x - p1.x) - (p2.y - p1.y) * (p3.x - p1.x);
            //double Equation = (b.Y - p.Y) * (a.X - p.X) - (a.Y -p.Y) * (b.X - p.X);
            
            double  Equation=(a.X-p.X)*(b.Y-p.Y)-(a.Y-p.Y)*(b.X-p.X);
            if (Equation <0)
            {
                return 1;
            }
            else
            {
                return -1;//点在直线左侧
            }
            
        }

    }
}
