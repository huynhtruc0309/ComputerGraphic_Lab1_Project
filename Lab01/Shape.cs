using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SharpGL;

namespace Lab01
{
    /*
     Class Shape gồm các thuộc tính:
     mảng vertices (đỉnh) để làm affine
     Mảng Points (Mảng các điểm đã vẽ qua)
         
         */
    class Shape
    {
        protected const double epsilone = 0.00000000000001;
        protected Point Pstart;
        protected Point Pend;
        protected List<Point> vertices; //Mảng các đỉnh
        protected List<Point> Points; //Mảng các điểm

        /*check Collision
         kiểm tra khoảng cách giữa X và các điểm trong Points có nhỏ hơn 1 hằng số epsilone hay không, dùng để làm mấy cái chấm xung quanh hình.
        */

        //Hàm kiểm tra xem điểm chọn có nằm trên hình nào không
        public virtual bool checkColision(Point x) 
        {
            for (int i = 0; i < this.Points.Count; i++)
            {
                if (Math.Sqrt((x.X - this.Points[i].X) * (x.X - this.Points[i].X) + (x.Y - this.Points[i].Y) * (x.Y - this.Points[i].Y)) <= epsilone)
                {//Tính khoảng cách giữa điểm vừa được chọn với mảng điểm đã lưu
                    return true;
                }
            }
            return false;
        }

        //Kiểm tra xem có điểm X trong mảng Points hay không
        public virtual bool checkPoint(Point x)
        {
            for (int i = 0; i < this.Points.Count; i++)
            {
                if (x.X == this.Points[i].X && x.Y == this.Points[i].Y)
                {
                    return true;
                }
            }
            return false;
        }
        //thêm điểm vào Points
        public virtual void addPoint(Point x) {
            this.Points.Add(x);
        }

        //Lưu lại Các điểm đã vẽ vào Points,// được override
        public virtual void LuuHinh(Point pStart, Point pEnd)
        {

        }

        //Lấy Points[i]
        public Point Point_at(int i)
        {
            return Points[i];
        }

        //Lấy Points.Count;
        public int Points_Size()
        {
            return Points.Count;
        }

        //Lấy vertices[i] (đỉnh)
        public Point Vertices_at(int i)
        {
            return vertices[i];
        }

        //Lấy vertices.Count (số lượng đỉnh)
        public int Vertices_Size()
        {
            return vertices.Count;
        }

        //trả ra tên class, dùng để làm affine cho trường hợp hình tròn ellipse cũng như ép kiểu ngược từ cha->con//được override
        public virtual string Type()
        {
            return "Shape";
        }

        //trả ra Points (trả ra kiểu ref)
        public virtual List<Point> getPointsList()
        {
            return this.Points;
        }
    }
    class HinhTron : Shape
    {
        public HinhTron(Point start, Point end)
        {
            this.Pstart = start;
            this.Pend = end;
            this.vertices = new List<Point>();
            this.Points = new List<Point>();
            vertices.Add(start);
            vertices.Add(end);
        }

        //Lưu lại hình đã vẽ
        public override void LuuHinh(Point pStart, Point pEnd)
        {
            double radius = Math.Sqrt((pStart.X - pEnd.X) * (pStart.X - pEnd.X) + (pStart.Y - pEnd.Y) * (pStart.Y - pEnd.Y));

            Point temp = pStart;
            temp.X = pStart.X; temp.Y = (int)(Math.Round(pStart.Y + radius));
            this.Points.Add(temp);
            temp.X = pStart.X; temp.Y = (int)(Math.Round(pStart.Y - radius));
            this.Points.Add(temp);
            temp.X = (int)(pStart.X + Math.Round(radius)); temp.Y = pStart.Y;
            this.Points.Add(temp);
            temp.X = (int)(pStart.X - Math.Round(radius)); temp.Y = pStart.Y;
            this.Points.Add(temp);
            //ve tu tam roi tinh tien do do phuong trinh se la x^2 + y^2 = r
            int x0 = 0;
            int y0 = (int)Math.Round(radius);
            double po = 5 / 4.0 - radius;
            int k = 0;
            while (x0 < y0)
            {
                if (po < 0)
                {
                    x0 = x0 + 1;
                    po = po + 2 * x0 + 1;



                }
                else
                {
                    x0 = x0 + 1;
                    y0 = y0 - 1;
                    po = po + 2 * x0 - 2 * y0 + 1;
                }
                temp.X = pStart.X + x0; temp.Y = (pStart.Y + y0);
                this.Points.Add(temp);
                temp.X = pStart.X + y0; temp.Y = (pStart.Y + x0);
                this.Points.Add(temp);
                temp.X = pStart.X + x0; temp.Y = (pStart.Y - y0);
                this.Points.Add(temp);
                temp.X = pStart.X + y0; temp.Y = (pStart.Y - x0);
                this.Points.Add(temp);
                temp.X = pStart.X - x0; temp.Y = (pStart.Y + y0);
                this.Points.Add(temp);
                temp.X = pStart.X - y0; temp.Y = (pStart.Y + x0);
                this.Points.Add(temp);
                temp.X = pStart.X - x0; temp.Y = (pStart.Y - y0);
                this.Points.Add(temp);
                temp.X = pStart.X - y0; temp.Y = (pStart.Y - x0);
                this.Points.Add(temp);
                k = k + 1;
            }
        }
        //Trả về tên hình vẽ
        public override string Type()
        {
            return "HinhTron";
        }
    }

    class DuongThang : Shape
    {
        public DuongThang(Point start, Point end)
        {
            this.Pstart = start;
            this.Pend = end;
            this.vertices = new List<Point>();
            this.Points = new List<Point>();
            vertices.Add(start);
            vertices.Add(end);
        }
        //Lưu lại hình đã vẽ
        public override void LuuHinh(Point pStart, Point pEnd)
        {

            //Tính các thông số cơ bản
            //int x0 = pStart.X;
            //int y0 = pStart.Y;
            int x0, y0;
            x0 = y0 = 0;
            Point temp = pStart;
            int delta_X = Math.Abs(pEnd.X - pStart.X);
            int delta_Y = Math.Abs(pEnd.Y - pStart.Y);
            int x2delta_X = 2 * delta_X;
            int x2delta_Y = 2 * delta_Y;
            //Giải thuật và vẽ
            temp.X = pStart.X; temp.Y = pStart.Y;
            this.Points.Add(temp);
            //gl.Vertex(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);

            int k = 0;
            if (delta_X > delta_Y)
            {
                int p0 = 2 * delta_Y - delta_X;
                while (k != delta_X)
                {
                    x0++;
                    if (p0 < 0)
                    {
                        p0 = p0 + 2 * delta_Y;
                    }
                    else
                    {
                        y0++;
                        p0 = p0 + x2delta_Y - x2delta_X;
                    }

                    if (pStart.X < pEnd.X && pStart.Y <= pEnd.Y)
                    {
                        temp.X = pStart.X + x0;
                        temp.Y = (pStart.Y + y0);
                        this.Points.Add(temp);
                        //gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0)); 
                    }
                    if (pStart.X > pEnd.X && pStart.Y <= pEnd.Y)
                    {
                        temp.X = pStart.X - x0;
                        temp.Y = (pStart.Y + y0);
                        this.Points.Add(temp);
                        // gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0)); //2
                    }
                    if (pStart.X > pEnd.X && pStart.Y > pEnd.Y)
                    {
                        temp.X = pStart.X - x0;
                        temp.Y = pStart.Y - y0;
                        this.Points.Add(temp);
                        //gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0));//3
                    }
                    if (pStart.X < pEnd.X && pStart.Y > pEnd.Y)
                    {
                        temp.X = pStart.X + x0;
                        temp.Y = pStart.Y - y0;
                        this.Points.Add(temp);
                        //gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0)); //4
                    }
                k++;
                }
            }
            else
            {
                int p0 = 2 * delta_X - delta_Y;
                while (k != delta_Y)
                {
                    y0++;
                    if (p0 < 0)
                    {
                        p0 = p0 + 2 * delta_X;
                    }
                    else
                    {
                        x0++;
                        p0 = p0 + x2delta_X - x2delta_Y;
                    }
                    if (pStart.X <= pEnd.X && pStart.Y < pEnd.Y)
                    {
                        temp.X = pStart.X + x0;
                        temp.Y = pStart.Y + y0;
                        this.Points.Add(temp);
                        //gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                    }
                    if (pStart.X > pEnd.X && pStart.Y < pEnd.Y)
                    {
                        temp.X = pStart.X - x0;
                        temp.Y = pStart.Y + y0;
                        this.Points.Add(temp);
                        //gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0)); //2
                    }
                    if (pStart.X > pEnd.X && pStart.Y > pEnd.Y)
                    {
                        temp.X = pStart.X - x0;
                        temp.Y = pStart.Y - y0;
                        this.Points.Add(temp);
                        //   gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0)); //3
                    }
                    if (pStart.X <= pEnd.X && pStart.Y > pEnd.Y)
                    {
                        temp.X = pStart.X + x0;
                        temp.Y = pStart.Y - y0;
                        this.Points.Add(temp);
                        //gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0)); //4
                    }

                    k++;
                }
            }
        }
        
        //Trả về tên hình vẽ
        public override string Type()
        {
            return "DuongThang";
        }
    }

    class Ellipse : Shape
    {
        public Ellipse(Point start, Point end)
        {
            this.Pstart = start;
            this.Pend = end;
            this.vertices = new List<Point>();
            this.Points = new List<Point>();
            vertices.Add(start);
            vertices.Add(end);
        }
        //Lưu lại hình đã vẽ
        public override void LuuHinh(Point pStart, Point pEnd)
        {
            //GEt the OpenGL object
           // gl.Begin(OpenGL.GL_POINTS);

            double rx = Math.Abs(pEnd.X - pStart.X) / 2.0;
            double ry = Math.Abs(pEnd.Y - pStart.Y) / 2.0;
            double x0 = 0, y0 = ry;

            double p1 = (ry * ry) - (rx * rx * ry) + (0.25 * rx * rx);
            double dx = 2 * ry * ry * x0;
            double dy = 2 * rx * rx * y0;

            //ve cac diem (0, ry), (0, -ry), (rx, 0), (-rx, 0)
            Point temp = pStart;
            temp.X = pStart.X;
            temp.Y = (int)Math.Round(pStart.Y + ry);
            this.Points.Add(temp);
            //gl.Vertex(pStart.X, gl.RenderContextProvider.Height - Math.Round(pStart.Y + ry));
            temp.X = pStart.X;
            temp.Y = (int)Math.Round(pStart.Y - ry);
            this.Points.Add(temp);
            //gl.Vertex(pStart.X, gl.RenderContextProvider.Height - Math.Round(pStart.Y - ry));
            temp.X = (int)Math.Round(pStart.X + rx);
            temp.Y = pStart.Y;
            this.Points.Add(temp);
            //gl.Vertex(pStart.X + rx, gl.RenderContextProvider.Height - pStart.Y);
            temp.X = (int)Math.Round(pStart.X - rx);
            temp.Y = pStart.Y;
            this.Points.Add(temp);
            //gl.Vertex(pStart.X - rx, gl.RenderContextProvider.Height - pStart.Y);

            while (dx < dy)
            {
                if (p1 < 0)
                {
                    x0++;
                    dx = dx + (2 * ry * ry);
                    p1 = p1 + dx + (ry * ry);
                }
                else
                {
                    x0++;
                    y0--;
                    dx = dx + (2 * ry * ry);
                    dy = dy - (2 * rx * rx);
                    p1 = p1 + dx - dy + (ry * ry);
                }
                temp.X =(int)Math.Round(pStart.X + x0);
                temp.Y = (int)Math.Round(pStart.Y + y0);
                this.Points.Add(temp);
                //gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                temp.X = (int)Math.Round(pStart.X - x0);
                temp.Y = (int)Math.Round(pStart.Y + y0);
                this.Points.Add(temp);
                //gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                temp.X = (int)Math.Round(pStart.X + x0);
                temp.Y = (int)Math.Round(pStart.Y - y0);
                this.Points.Add(temp);
                //gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
                temp.X = (int)Math.Round(pStart.X - x0);
                temp.Y = (int)Math.Round(pStart.Y - y0);
                this.Points.Add(temp);
                //gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
            }

            // Decision parameter of region 2 
            double p2 = ((ry * ry) * ((x0 + 0.5) * (x0 + 0.5))) + ((rx * rx) * ((y0 - 1) * (y0 - 1))) - (rx * rx * ry * ry);

            while (y0 >= 0)
            {
                // Checking and updating parameter 
                // value based on algorithm 
                if (p2 > 0)
                {
                    y0--;
                    dy = dy - (2 * rx * rx);
                    p2 = p2 + (rx * rx) - dy;
                }
                else
                {
                    y0--;
                    x0++;
                    dx = dx + (2 * ry * ry);
                    dy = dy - (2 * rx * rx);
                    p2 = p2 + dx - dy + (rx * rx);
                }
                //gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                //gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                //gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
                //gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0));

                temp.X = (int)Math.Round(pStart.X + x0);
                temp.Y = (int)Math.Round(pStart.Y + y0);
                this.Points.Add(temp);
                //gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                temp.X = (int)Math.Round(pStart.X - x0);
                temp.Y = (int)Math.Round(pStart.Y + y0);
                this.Points.Add(temp);
                //gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                temp.X = (int)Math.Round(pStart.X + x0);
                temp.Y = (int)Math.Round(pStart.Y - y0);
                this.Points.Add(temp);
                //gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
                temp.X = (int)Math.Round(pStart.X - x0);
                temp.Y = (int)Math.Round(pStart.Y - y0);
                this.Points.Add(temp);
            }
        }
        
        //Trả về loại hình vẽ
        public override string Type()
        {
            return "Ellipse";
        }

    }

    class HinhChuNhat : Shape{
        public HinhChuNhat(Point start,Point end)
        {
            this.Pstart = start;
            this.Pend = end;
            this.vertices = new List<Point>();
            this.Points = new List<Point>();
            Point temp1 = start;
            temp1.Y = end.Y;
            Point temp3 = end;
            Point temp4 = end;
            temp4.Y = start.Y;
            vertices.Add(start);
            vertices.Add(temp1);
            vertices.Add(temp3);
            vertices.Add(temp4);
        }

        //Lưu hình đã vẽ
        public override void LuuHinh(Point pStart, Point pEnd)
        {
            List<DuongThang> temp = new List<DuongThang>();
            for(int i = 0; i < vertices.Count-1; i++)
            {
                temp.Add( new DuongThang(vertices[0], vertices[1]));
                temp[i].LuuHinh(vertices[i],vertices[i+1]);

            }
            DuongThang dt = new DuongThang(vertices[0], vertices[1]);
            dt.LuuHinh(vertices[0], vertices[vertices.Count-1]);
            temp.Add(dt);
            for(int i = 0; i < temp.Count; i++)
            {
                for(int j = 0; j < temp[i].Points_Size(); j++)
                {
                    this.Points.Add(temp[i].Point_at(j));
                }
            }
        }

        //Trả về loại hình vẽ
        public override string Type()
        {
            return "HinhChuNhat";
        }

    }

    class TamGiacDeu : Shape
    {
        public TamGiacDeu(Point start,Point end)
        {
            this.Pstart = start;
            this.Pend = end;
            this.vertices = new List<Point>();
            this.Points = new List<Point>();
            Point test = start;
            Point test1 = end;
            test = end;
            test.Y = start.Y;
            double delta = test.X - start.X;
            test.X = start.X + (int)Math.Round(delta / 2);
            test.Y = (int)Math.Round(start.Y + delta * Math.Sqrt(3) / 2);
            test1.Y = start.Y;
            vertices.Add(start);
            vertices.Add(test);
            vertices.Add(test1);
        }
        public override void LuuHinh(Point pStart, Point pEnd)
        {
            List<DuongThang> temp = new List<DuongThang>();
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                temp.Add(new DuongThang(vertices[0], vertices[1]));
                temp[i].LuuHinh(vertices[i], vertices[i + 1]);

            }
            DuongThang dt = new DuongThang(vertices[0], vertices[1]);
            dt.LuuHinh(vertices[0], vertices[vertices.Count - 1]);
            temp.Add(dt);
            for (int i = 0; i < temp.Count; i++)
            {
                for (int j = 0; j < temp[i].Points_Size(); j++)
                {
                    this.Points.Add(temp[i].Point_at(j));
                }
            }
        }
        public override string Type()
        {
            return "TamGiacDeu";
        }
    }

    class NguGiacDeu : Shape
    {
        public NguGiacDeu(Point start, Point end)
        {
            this.Pstart = start;
            this.Pend = end;
            this.vertices = new List<Point>();
            this.Points = new List<Point>();
            Point test = start;
            Point test1 = end;
            Point tempEnd = end;
            if (tempEnd.X >= start.X)
            {
                tempEnd.Y = start.Y;
                double a = Math.Abs(tempEnd.X - start.X);
                Point temp1 = start;
                double goc1 = 72 * Math.PI / 180.0;//goc 72 rad
                temp1.X = (int)Math.Round(start.X - a * Math.Cos(goc1));
                temp1.Y = (int)Math.Round(start.Y + a * Math.Sin(goc1));
                Point temp2 = start;
                temp2.X = (start.X + tempEnd.X) / 2;
                double goc2 = 54 * Math.PI / 180.0;//goc 72 rad
                temp2.Y = (int)Math.Round(start.Y + a * Math.Tan(goc2) / 2 + a / (2 * Math.Cos(goc2)));
                Point temp3 = tempEnd;
                temp3.X = (int)Math.Round(tempEnd.X + a * Math.Cos(goc1));
                temp3.Y = temp1.Y;
                vertices.Add(start);
                vertices.Add(temp1);
                vertices.Add(temp2);
                vertices.Add(temp3);
                vertices.Add(tempEnd);
            }
            else
            {
                tempEnd.Y = start.Y;
                double a = Math.Abs(tempEnd.X - start.X);
                Point temp1 = start;
                double goc1 = 72 * Math.PI / 180.0;//goc 72 rad
                temp1.X = (int)Math.Round(start.X + a * Math.Cos(goc1));
                temp1.Y = (int)Math.Round(start.Y + a * Math.Sin(goc1));
                Point temp2 = start;
                temp2.X = (start.X + tempEnd.X) / 2;
                double goc2 = 54 * Math.PI / 180.0;//goc 72 rad
                temp2.Y = (int)Math.Round(start.Y + a * Math.Tan(goc2) / 2 + a / (2 * Math.Cos(goc2)));
                Point temp3 = tempEnd;
                temp3.X = (int)Math.Round(tempEnd.X - a * Math.Cos(goc1));
                temp3.Y = temp1.Y;
                vertices.Add(start);
                vertices.Add(temp1);
                vertices.Add(temp2);
                vertices.Add(temp3);
                vertices.Add(tempEnd);
            }

        }
        //Lưu các hình đã vẽ
        public override void LuuHinh(Point pStart, Point pEnd)
        {
            List<DuongThang> temp = new List<DuongThang>();
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                temp.Add(new DuongThang(vertices[0], vertices[1]));
                temp[i].LuuHinh(vertices[i], vertices[i + 1]);

            }
            DuongThang dt = new DuongThang(vertices[0], vertices[1]);
            dt.LuuHinh(vertices[0], vertices[vertices.Count - 1]);
            temp.Add(dt);
            for (int i = 0; i < temp.Count; i++)
            {
                for (int j = 0; j < temp[i].Points_Size(); j++)
                {
                    this.Points.Add(temp[i].Point_at(j));
                }
            }
        }
        //Trả về loại hình vẽ
        public override string Type()
        {
            return "NguGiacDeu";
        }
    }

    class LucGiacDeu:Shape
    {

        public LucGiacDeu(Point start, Point end)
        {
            this.Pstart = start;
            this.Pend = end;
            this.vertices = new List<Point>();
            this.Points = new List<Point>();
            Point tempEnd = end;
            if (tempEnd.X >= start.X)
            {
                tempEnd.Y = start.Y;
                double a = Math.Abs(tempEnd.X - start.X);
                Point temp1 = start;
                double goc1 = 60 * Math.PI / 180.0;
                temp1.X = (int)Math.Round(start.X - a * Math.Cos(goc1));
                temp1.Y = (int)Math.Round(start.Y + a * Math.Sin(goc1));
                Point temp2 = start;
                temp2.Y = (int)Math.Round(start.Y + a * Math.Tan(goc1));
                Point temp3 = tempEnd;
                temp3.Y = temp2.Y;
                Point temp4 = tempEnd;
                temp4.X = (int)Math.Round(tempEnd.X + a * Math.Cos(goc1));
                temp4.Y = temp1.Y;
                /*veDuongThang(start, temp1);
                veDuongThang(temp1, temp2);
                veDuongThang(temp2, temp3);
                veDuongThang(temp3, temp4);
                veDuongThang(temp4, tempEnd);
                veDuongThang(pStart, tempEnd);*/
                vertices.Add(start);
                vertices.Add(temp1);
                vertices.Add(temp2);
                vertices.Add(temp3);
                vertices.Add(temp4);
                vertices.Add(tempEnd);
            }
            else
            {
                tempEnd.Y = start.Y;
                double a = Math.Abs(tempEnd.X - start.X);
                Point temp1 = start;
                double goc1 = 60 * Math.PI / 180.0;
                temp1.X = (int)Math.Round(start.X + a * Math.Cos(goc1));
                temp1.Y = (int)Math.Round(start.Y + a * Math.Sin(goc1));
                Point temp2 = start;
                temp2.Y = (int)Math.Round(start.Y + a * Math.Tan(goc1));
                Point temp3 = tempEnd;
                temp3.Y = temp2.Y;
                Point temp4 = tempEnd;
                temp4.X = (int)Math.Round(tempEnd.X - a * Math.Cos(goc1));
                temp4.Y = temp1.Y;
                vertices.Add(start);
                vertices.Add(temp1);
                vertices.Add(temp2);
                vertices.Add(temp3);
                vertices.Add(temp4);
                vertices.Add(tempEnd);
            }
        }
        //Lưu hình đã vẽ
        public override void LuuHinh(Point pStart, Point pEnd)
        {
            List<DuongThang> temp = new List<DuongThang>();
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                temp.Add(new DuongThang(vertices[0], vertices[1]));
                temp[i].LuuHinh(vertices[i], vertices[i + 1]);

            }
            DuongThang dt = new DuongThang(vertices[0], vertices[1]);
            dt.LuuHinh(vertices[0], vertices[vertices.Count - 1]);
            temp.Add(dt);
            for (int i = 0; i < temp.Count; i++)
            {
                for (int j = 0; j < temp[i].Points_Size(); j++)
                {
                    this.Points.Add(temp[i].Point_at(j));
                }
            }
        }
        
        //Trả về loại hình vẽ
        public override string Type()
        {
            return "LucGiacDeu";
        }
    }
}
