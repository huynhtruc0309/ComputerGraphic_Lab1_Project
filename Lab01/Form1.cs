using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Lab01
{   
    public partial class Form1 : Form
    {
        public abstract class Object
        {
            public Point pStart, pEnd;
            public Color lineColor, bgColor; //Màu viền và màu nền
            public int lineSize; //Kích cỡ nét
            
            //Constructor
            public Object()
            {
                lineColor = Color.White;
                bgColor = Color.Black;
                lineSize = 5;
            }

            //Cài đặt
            public void set(Point start, Point end)
            {
                pStart = start;
                pEnd = end;
            }

            //Màu viền
            public void setLineColor(Color line)
            {
                lineColor = line;
            }

            //Màu nền
            public void setBGColor(Color bg)
            {
                bgColor = bg;
            }

            //Độ dày nét
            public void setLineSize(int userChoice)
            {
                switch(userChoice)
                {
                    case 1: lineSize = 5; break; //Small
                    case 2: lineSize = 10; break; //Medium
                    case 3: lineSize = 15; break; //Big
                }
            }

            //Thiết lập toạ độ khi di chuyển chuột
            public virtual void setPoint(Point start, Point end) {}

            //Hàm vẽ hình, tô màu và hiển thị điểm điều khiển
            public virtual void drawObject(OpenGL gl) { }
            public virtual void colorObject(OpenGL gl)
            {
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                //filled_area area;
                //area.color = color;
                //area.t = new List<Point>();
                //OpenGL gl = openGLControl.OpenGL;
                //gl.Color(color.R / 255.0, color.G / 255.0, color.B / 255.0, 0);
                //List<Point> searched = new List<Point>();
                //Queue<Point> frontier = new Queue<Point>();
                //frontier.Enqueue(Start);
                //Point temp = Start;
                //gl.Begin(OpenGL.GL_POINTS);
                //while (frontier.Count > 0)
                //{
                //    temp = frontier.Dequeue();

                //    gl.Vertex(temp.X, gl.RenderContextProvider.Height - temp.Y);
                //    area.t.Add(temp);

                //    if (!inList(temp, searched)) searched.Add(temp);
                //    //phat sinh con 4 huong
                //    Point temp1 = temp;
                //    temp1.X = temp.X + 1;
                //    Point temp2 = temp;
                //    temp2.Y = temp.Y + 1;
                //    Point temp3 = temp;
                //    temp3.Y = temp.Y - 1;
                //    Point temp4 = temp;
                //    temp4.X = temp.X - 1;
                //    int flag1 = 0;
                //    int flag2 = 0;
                //    int flag3 = 0;
                //    int flag4 = 0;// !inList(temp1, RasterPoint),frontier.Enqueue(temp4);
                //    if ((temp1.X >= 0 && temp1.X <= gl.RenderContextProvider.Width) && (temp1.Y >= 0 && temp1.Y <= gl.RenderContextProvider.Height && !inList(temp1, searched)) && !frontier.Contains(temp1)) flag1 = 1;
                //    if ((temp2.X >= 0 && temp2.X <= gl.RenderContextProvider.Width) && (temp2.Y >= 0 && temp2.Y <= gl.RenderContextProvider.Height && !inList(temp2, searched)) && !frontier.Contains(temp2)) flag2 = 1;
                //    if ((temp3.X >= 0 && temp3.X <= gl.RenderContextProvider.Width) && (temp3.Y >= 0 && temp3.Y <= gl.RenderContextProvider.Height && !inList(temp3, searched)) && !frontier.Contains(temp3)) flag3 = 1;
                //    if ((temp4.X >= 0 && temp4.X <= gl.RenderContextProvider.Width) && (temp4.Y >= 0 && temp4.Y <= gl.RenderContextProvider.Height && !inList(temp4, searched)) && !frontier.Contains(temp4)) flag4 = 1;
                //    for (int i = 0; i < Hinh.Count; i++)
                //    {
                //        for (int j = 0; j < Hinh[i].Points_Size(); j++)
                //        {
                //            if (inList(temp1, Hinh[i].getPoinsList()))
                //            {
                //                flag1 = 0;
                //                break;
                //            }
                //        }
                //        if (flag1 == 0) break;
                //    }
                //    if (flag1 == 1) frontier.Enqueue(temp1);
                //    for (int i = 0; i < Hinh.Count; i++)
                //    {
                //        for (int j = 0; j < Hinh[i].Points_Size(); j++)
                //        {
                //            if (inList(temp2, Hinh[i].getPoinsList()))
                //            {
                //                flag2 = 0;
                //                break;
                //            }
                //        }
                //        if (flag2 == 0) break;
                //    }
                //    if (flag2 == 1) frontier.Enqueue(temp2);

                //    for (int i = 0; i < Hinh.Count; i++)
                //    {
                //        for (int j = 0; j < Hinh[i].Points_Size(); j++)
                //        {
                //            if (inList(temp3, Hinh[i].getPoinsList()))
                //            {
                //                flag3 = 0;
                //                break;
                //            }
                //        }
                //        if (flag3 == 0) break;
                //    }
                //    if (flag3 == 1) frontier.Enqueue(temp3);

                //    for (int i = 0; i < Hinh.Count; i++)
                //    {
                //        for (int j = 0; j < Hinh[i].Points_Size(); j++)
                //        {
                //            if (inList(temp4, Hinh[i].getPoinsList()))
                //            {
                //                flag4 = 0;
                //                break;
                //            }
                //        }
                //        if (flag4 == 0) break;
                //    }
                //    if (flag4 == 1) frontier.Enqueue(temp4);
                //}
                //gl.End();
                //gl.Flush();
                //sw.Stop();
                //time.Text = sw.Elapsed.ToString();
                //filled.Add(area);
            }
            public virtual void viewControlPoint(OpenGL gl) { }
            
            //Hàm xoay ảnh
            public virtual void affine(float degree) { }

            //Hàm di chuyển ảnh
            public virtual void move(int dX, int dY) { }
        }

        // Class đường thẳng
        public class Line : Object
        {
            private Point A, B;
            public Line() { }
            public override void setPoint(Point start, Point end)
            {
                A = start;
                B = end;
            }

            public override void drawObject(OpenGL gl)
            {
                gl.Begin(OpenGL.GL_LINES);
                gl.Color(lineColor.R / 255.0, lineColor.G / 255.0, lineColor.B / 255.0, 0);

                gl.Vertex(A.X, gl.RenderContextProvider.Height - A.Y);
                gl.Vertex(B.X, gl.RenderContextProvider.Height - B.Y);

                gl.End();
                gl.LineWidth(lineSize);
            }

            // Hiện cái control point
            public override void viewControlPoint(OpenGL gl)
            {
                gl.PointSize(lineSize + 10);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(1.0, 0, 0, 0);
                gl.Vertex(A.X, gl.RenderContextProvider.Height - A.Y);
                gl.Vertex(B.X, gl.RenderContextProvider.Height - B.Y);

                gl.End();
            }

            //Hàm xoay ảnh
            public override void affine(float degree)
            {
                Point Atmp = A;
                Point Btmp = B;
                Point ct = A;
                ct.X = (A.X + B.X) / 2;
                ct.Y = (A.Y + B.Y) / 2;
                Atmp.X -= ct.X;
                Atmp.Y -= ct.Y;
                Btmp.X -= ct.X;
                Btmp.Y -= ct.Y;
                float grad = (float)((degree * 3.14) / 180);
                A.X = (int)(Math.Cos(grad) * Atmp.X - Math.Sin(grad) * Atmp.Y) + ct.X;
                A.Y = (int)(Math.Sin(grad) * Atmp.X + Math.Cos(grad) * Atmp.Y) + ct.Y;
                B.X = (int)(Math.Cos(grad) * Btmp.X - Math.Sin(grad) * Btmp.Y) + ct.X;
                B.Y = (int)(Math.Sin(grad) * Btmp.X + Math.Cos(grad) * Btmp.Y) + ct.Y;
            }

            //Hàm tịnh tiến ảnh
            public override void move(int dX, int dY)
            {
                A.X += dX;
                A.Y += dY;
                B.X += dX;
                B.Y += dY;
            }
        }

        // Class hình chữ nhật
        public class Rectangle : Object
        {
            private Point A, B, C, D;
            public Rectangle() { }
            //Hàm đặt điểm
            public override void setPoint(Point start, Point end)
            {
                A = start;
                C = end;
                B = start;
                B.X = end.X;
                D = end;
                D.X = start.X;
            }
            //Hàm vẽ hình
            public override void drawObject(OpenGL gl)
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
                gl.Color(lineColor.R / 255.0, lineColor.G / 255.0, lineColor.B / 255.0, 0);

                gl.Vertex(A.X, gl.RenderContextProvider.Height - A.Y);
                gl.Vertex(B.X, gl.RenderContextProvider.Height - B.Y);
                gl.Vertex(C.X, gl.RenderContextProvider.Height - C.Y);
                gl.Vertex(D.X, gl.RenderContextProvider.Height - D.Y);

                gl.End();
                gl.LineWidth(lineSize);
            }
            
            //Hàm hiện điểm control
            public override void viewControlPoint(OpenGL gl)
            {
                gl.PointSize(lineSize + 10);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(1.0, 0, 0, 0);
                gl.Vertex(A.X, gl.RenderContextProvider.Height - A.Y);
                gl.Vertex(B.X, gl.RenderContextProvider.Height - B.Y);
                gl.Vertex(C.X, gl.RenderContextProvider.Height - C.Y);
                gl.Vertex(D.X, gl.RenderContextProvider.Height - D.Y);
                gl.End();
            }

            //Hàm xoay ảnh
            public override void affine(float degree)
            {
                Point Atmp = A, Btmp = B, Ctmp = C, Dtmp = D;
                Point ct = A;
                ct.X = (A.X + C.X) / 2;
                ct.Y = (A.Y + C.Y) / 2;
                Atmp.X -= ct.X;
                Atmp.Y -= ct.Y;
                Btmp.X -= ct.X;
                Btmp.Y -= ct.Y;
                Ctmp.X -= ct.X;
                Ctmp.Y -= ct.Y;
                Dtmp.X -= ct.X;
                Dtmp.Y -= ct.Y;
                float grad = (float)((degree * 3.14) / 180);
                A.X = (int)(Math.Cos(grad) * Atmp.X - Math.Sin(grad) * Atmp.Y) + ct.X;
                A.Y = (int)(Math.Sin(grad) * Atmp.X + Math.Cos(grad) * Atmp.Y) + ct.Y;
                B.X = (int)(Math.Cos(grad) * Btmp.X - Math.Sin(grad) * Btmp.Y) + ct.X;
                B.Y = (int)(Math.Sin(grad) * Btmp.X + Math.Cos(grad) * Btmp.Y) + ct.Y;
                C.X = (int)(Math.Cos(grad) * Ctmp.X - Math.Sin(grad) * Ctmp.Y) + ct.X;
                C.Y = (int)(Math.Sin(grad) * Ctmp.X + Math.Cos(grad) * Ctmp.Y) + ct.Y;
                D.X = (int)(Math.Cos(grad) * Dtmp.X - Math.Sin(grad) * Dtmp.Y) + ct.X;
                D.Y = (int)(Math.Sin(grad) * Dtmp.X + Math.Cos(grad) * Dtmp.Y) + ct.Y;
            }

            //Hàm tịnh tiến ảnh
            public override void move(int dX, int dY)
            {
                A.X += dX;
                A.Y += dY;
                B.X += dX;
                B.Y += dY;
                C.X += dX;
                C.Y += dY;
                D.X += dX;
                D.Y += dY;
            }
        }

        // Class hình tam giác
        public class Triangle : Object
        {
            private Point A, B, C;
            public Triangle() { }
            //Hàm đặt điểm
            public override void setPoint(Point start, Point end)
            {
                A.X = (start.X + end.X)/2;
                A.Y = start.Y;
                B = end;
                C.X = start.X;
                C.Y = end.Y;
            }
            //Hàm vẽ hình
            public override void drawObject(OpenGL gl)
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
                gl.Color(lineColor.R / 255.0, lineColor.G / 255.0, lineColor.B / 255.0, 0);

                gl.Vertex(A.X, gl.RenderContextProvider.Height - A.Y);
                gl.Vertex(B.X, gl.RenderContextProvider.Height - B.Y);
                gl.Vertex(C.X, gl.RenderContextProvider.Height - C.Y);

                gl.End();
                gl.LineWidth(lineSize);
            }
            //Hàm tô màu
            public override void viewControlPoint(OpenGL gl)
            {
                gl.PointSize(lineSize + 10);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(255 / 255.0, 0, 0, 0);
                gl.Vertex(A.X, gl.RenderContextProvider.Height - A.Y);
                gl.Vertex(B.X, gl.RenderContextProvider.Height - B.Y);
                gl.Vertex(C.X, gl.RenderContextProvider.Height - C.Y);
                gl.End();
            }

            //Hàm xoay ảnh
            public override void affine(float degree)
            {
                Point Atmp = A, Btmp = B, Ctmp = C;
                Point ct = A;
                ct.X = A.X;
                ct.Y = (A.Y + C.Y) / 2;
                Atmp.X -= ct.X;
                Atmp.Y -= ct.Y;
                Btmp.X -= ct.X;
                Btmp.Y -= ct.Y;
                Ctmp.X -= ct.X;
                Ctmp.Y -= ct.Y;
                float grad = (float)((degree * 3.14) / 180);
                A.X = (int)(Math.Cos(grad) * Atmp.X - Math.Sin(grad) * Atmp.Y) + ct.X;
                A.Y = (int)(Math.Sin(grad) * Atmp.X + Math.Cos(grad) * Atmp.Y) + ct.Y;
                B.X = (int)(Math.Cos(grad) * Btmp.X - Math.Sin(grad) * Btmp.Y) + ct.X;
                B.Y = (int)(Math.Sin(grad) * Btmp.X + Math.Cos(grad) * Btmp.Y) + ct.Y;
                C.X = (int)(Math.Cos(grad) * Ctmp.X - Math.Sin(grad) * Ctmp.Y) + ct.X;
                C.Y = (int)(Math.Sin(grad) * Ctmp.X + Math.Cos(grad) * Ctmp.Y) + ct.Y;
            }

            //Hàm tịnh tiến ảnh
            public override void move(int dX, int dY)
            {
                A.X += dX;
                A.Y += dY;
                B.X += dX;
                B.Y += dY;
                C.X += dX;
                C.Y += dY;
            }
        }

        // Class hình tròn
        public class Circle : Object
        {
            public int R;
            private Point center;

            public Circle() { }
            //Hàm đặt điểm
            public override void setPoint(Point start, Point end)
            {
                center.X = (start.X + end.X) / 2;
                center.Y = (start.Y + start.Y) / 2;
                if (Math.Abs(start.X - center.X) >= Math.Abs(start.Y - center.Y))
                    R = Math.Abs(start.X - center.X);
                else R = Math.Abs(start.Y - center.Y);
            }

            //phương thức hiển thị 8 điểm đặc biết đối xứng trong hình tròn
            private void put8Pixel(OpenGL gl, int x, int y)
            {
                gl.Vertex(center.X + x, gl.RenderContextProvider.Height - (center.Y + y + R));
                gl.Vertex(center.X + y, gl.RenderContextProvider.Height - (center.Y + x + R));
                gl.Vertex(center.X + y, gl.RenderContextProvider.Height - (center.Y - x + R));
                gl.Vertex(center.X + x, gl.RenderContextProvider.Height - (center.Y - y + R));
                gl.Vertex(center.X - x, gl.RenderContextProvider.Height - (center.Y - y + R));
                gl.Vertex(center.X - y, gl.RenderContextProvider.Height - (center.Y - x + R));
                gl.Vertex(center.X - y, gl.RenderContextProvider.Height - (center.Y + x + R));
                gl.Vertex(center.X - x, gl.RenderContextProvider.Height - (center.Y + y + R));
            }
            //Hàm vẽ hình
            public override void drawObject(OpenGL gl)
            {
                gl.PointSize(lineSize);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(lineColor.R / 255.0, lineColor.G / 255.0, lineColor.B / 255.0, 0);

                int a = 0, b = R;
                put8Pixel(gl, a, b);
                int p = 1 - R;
                while (a < b)
                {
                    if (p < 0)
                        p += 2 * a + 3;
                    else
                    {
                        p += 2 * (a - b) + 5;
                        b--;
                    }
                    a++;
                    put8Pixel(gl, a, b);
                }

                gl.End();
                gl.PointSize(lineSize);
            }
            //Hàm đặt điểm control
            public override void viewControlPoint(OpenGL gl)
            {
                gl.PointSize(lineSize + 10);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(1.0, 0, 0, 0);

                gl.PointSize(20);
                gl.Vertex(center.X + R, gl.RenderContextProvider.Height - center.Y - R);
                gl.Vertex(center.X + R, gl.RenderContextProvider.Height - center.Y);
                gl.Vertex(center.X + R, gl.RenderContextProvider.Height - center.Y + R);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - center.Y - R);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - center.Y);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - center.Y + R);
                gl.Vertex(center.X - R, gl.RenderContextProvider.Height - center.Y - R);
                gl.Vertex(center.X - R, gl.RenderContextProvider.Height - center.Y);
                gl.Vertex(center.X - R, gl.RenderContextProvider.Height - center.Y + R);
                
                gl.End();
            }

            //Hàm tịnh tiến ảnh
            public override void move(int dX, int dY)
            {
                center.X += dX;
                center.Y += dY;
            }
        }

        // Class hình ellipse
        public class Ellipse : Object
        {
            private int Rx, Ry;
            private Point center;
            public Ellipse() { }
            //Hàm đặt điểm
            public override void setPoint(Point start, Point end)
            {
                center.X = (start.X + end.X) / 2;
                center.Y = (start.Y + end.Y) / 2;
                Rx = Math.Abs(start.X - center.X);
                Ry = Math.Abs(start.Y - center.Y);
            }

            //Xuất 4 điểm đối xứng
            private void put4Pixel(OpenGL gl, int x, int y)
            {
                gl.Vertex(center.X + x, gl.RenderContextProvider.Height - (center.Y + y));
                gl.Vertex(center.X + x, gl.RenderContextProvider.Height - (center.Y - y));
                gl.Vertex(center.X - x, gl.RenderContextProvider.Height - (center.Y + y));
                gl.Vertex(center.X - x, gl.RenderContextProvider.Height - (center.Y - y));
            }
            //Hàm vẽ hình
            public override void drawObject(OpenGL gl)
            {
                gl.PointSize(lineSize);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(lineColor.R / 255.0, lineColor.G / 255.0, lineColor.B / 255.0, 0);

                int a = 0, b = Ry;
                put4Pixel(gl, a, b);
                int fx = 0, fy = 2 * Rx * Rx * Ry;
                float fp = Ry * Ry - Rx * Rx * Ry + 1 / 4 * Rx * Rx;
                while (fx < fy)
                {
                    if (fp < 0)
                    {
                        a++;
                        put4Pixel(gl, a, b);
                        fx = 2 * Ry * Ry * (a - 1) + 2 * Ry * Ry;
                        fp = fp + fx + Ry * Ry;
                    }
                    else
                    {
                        a++;
                        b--;
                        put4Pixel(gl, a, b);
                        fx = 2 * Ry * Ry * (a - 1) + 2 * Ry * Ry;
                        fy = 2 * Rx * Rx * (b + 1) - 2 * Rx * Rx;
                        fp = fp + fx - fy + Ry * Ry;
                    }
                }
                fx = 2 * Rx * Rx * b;
                fy = 2 * Ry * Ry * a;
                fp = Ry * Ry * (a + 1 / 2) * (a + 1 / 2) + Rx * Rx * (b - 1) * (b - 1) - Rx * Rx * Ry * Ry;
                while (b != 0)
                {
                    if (fp > 0)
                    {
                        b--;
                        put4Pixel(gl, a, b);
                        fx = 2 * Rx * Rx * (b - 1) - 2 * Rx * Rx;
                        fp = fp - fx + Rx * Rx;
                    }
                    else
                    {
                        a++;
                        b--;
                        put4Pixel(gl, a, b);
                        fy = 2 * Ry * Ry * (a - 1) + 2 * Ry * Ry;
                        fx = 2 * Rx * Rx * (b + 1) - 2 * Rx * Rx;
                        fp = fp + fy - fx + Rx * Rx;
                    }
                }

                gl.End();
                gl.PointSize(lineSize);
            }
            //Hàm đặt điểm control
            public override void viewControlPoint(OpenGL gl)
            {
                //Chưa làm
            }

            //Hàm xoay ảnh
            public override void affine(float degree)
            {
                if (degree == 90)
                {
                    int tmp = Rx;
                    Rx = Ry;
                    Ry = tmp;
                }
            }

            //Hàm tịnh tiến ảnh
            public override void move(int dX, int dY)
            {
                center.X += dX;
                center.Y += dY;
            }
        }

        // Class hình ngũ giác đều
        public class Pentagon : Object
        {
            private int R;
            private Point center;
            public Pentagon() { }
            //Hàm đặt điểm
            public override void setPoint(Point start, Point end)
            {
                center.X = (start.X + end.X) / 2;
                center.Y = (start.Y + start.Y) / 2;
                if (Math.Abs(start.X - center.X) >= Math.Abs(start.Y - center.Y))
                    R = Math.Abs(start.X - center.X);
                else R = Math.Abs(start.Y - center.Y);
            }
            //Hàm vẽ hình
            public override void drawObject(OpenGL gl)
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
                gl.Color(lineColor.R / 255.0, lineColor.G / 255.0, lineColor.B / 255.0, 0);

                //Chuyển đồi từ độ sang radius
                float grad = (float)((72 * 3.14) / 180);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - (center.Y - R));
                for (int i = 1; i < 5; i++)
                {
                    //Thực hiện xoay pixel từ điểm ảnh chuẩn sang các đỉnh khác
                    int x = (int)(-Math.Sin(i * grad) * R);
                    int y = (int)(Math.Cos(i * grad) * R);
                    gl.Vertex(center.X + x, gl.RenderContextProvider.Height - (center.Y - y));
                }
                gl.End();
                gl.LineWidth(lineSize);
            }
            //Hàm đặt điểm control
            public override void viewControlPoint(OpenGL gl)
            {
                gl.PointSize(lineSize + 10);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(1.0, 0, 0, 0);
                //Chuyển đổi từ độ sang Radian
                float grad = (float)((72 * 3.14) / 180);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - (center.Y - R));
                for (int i = 1; i < 5; i++)
                {
                    //Thực hiện phép xoay pixel
                    int x = (int)(-Math.Sin(i * grad) * R);
                    int y = (int)(Math.Cos(i * grad) * R);
                    gl.Vertex(center.X + x, gl.RenderContextProvider.Height - (center.Y - y));
                }
                gl.End();
            }

            //Hàm xoay ảnh
            public override void affine(float degree)
            {
                
            }

            //Hàm tịnh tiến ảnh
            public override void move(int dX, int dY)
            {
                center.X += dX;
                center.Y += dY;
            }
        }

        // Class hình lục giác
        public class Hexagon : Object
        {
            private int R;
            private Point center;
            public Hexagon() { }
            //Hàm đặt điểm
            public override void setPoint(Point start, Point end)
            {
                center.X = (start.X + end.X) / 2;
                center.Y = (start.Y + start.Y) / 2;
                if (Math.Abs(start.X - center.X) >= Math.Abs(start.Y - center.Y))
                    R = Math.Abs(start.X - center.X);
                else R = Math.Abs(start.Y - center.Y);
            }
            //Hàm vẽ hình
            public override void drawObject(OpenGL gl)
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
                gl.Color(lineColor.R / 255.0, lineColor.G / 255.0, lineColor.B / 255.0, 0);

                //chuyển đổi từ độ sang radius
                float grad = (float)((60 * 3.14) / 180);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - (center.Y - R));
                for (int i = 1; i < 6; i++)
                {
                    //Xoay pixel
                    int x = (int)(-Math.Sin(i * grad) * R);
                    int y = (int)(Math.Cos(i * grad) * R);
                    gl.Vertex(center.X + x, gl.RenderContextProvider.Height - (center.Y - y));
                }
                gl.End();
                gl.LineWidth(lineSize);
            }
            //Hàm đặt điểm control
            public override void viewControlPoint(OpenGL gl)
            {
                gl.PointSize(lineSize + 10);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(1.0, 0, 0, 0);
                float grad = (float)((60 * 3.14) / 180);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - (center.Y - R));
                for (int i = 1; i < 6; i++)
                {
                    int x = (int)(-Math.Sin(i * grad) * R);
                    int y = (int)(Math.Cos(i * grad) * R);
                    gl.Vertex(center.X + x, gl.RenderContextProvider.Height - (center.Y - y));
                }
                gl.End();
            }

            //Hàm xoay ảnh
            public override void affine(float degree)
            {
                
            }

            //Hàm tịnh tiến ảnh
            public override void move(int dX, int dY)
            {
                center.X += dX;
                center.Y += dY;
            }
        }

        //Khu vực khai báo các biến toàn cục để thao tác với ứng dụng
        //Biển kiểm soát con trỏ chuột, biến cho phép affine hình, biến tô màu nền cho hình, biến vẽ hình
        private Color userLineColor; //Màu nét vẽ
        private Color userBgColor; //Màu nền
        private int userLineSize; //Kích cỡ nét

        private int shShape;//Biến chọn chế độ vẽ 0 = Line,1 = Circle, 2 = Hinh Chu nhat, 3 = Ellipse, 4 = Tam Giac Deu, 5 = Ngu Giac Deu, 6 = Luc Giac Deu

        private Point pStart, pEnd; // Biến control màn hình
        
        private int numObj, nowId, idViewPoint; //Biến số lượng hình vẽ, biến kiểm soát hình nào được chọn, biến hiện controlPoint
        Object[] arrObj = new Object[100]; //Mảng các hình được vẽ
   

        private Stopwatch st = new Stopwatch();

        // Khởi tạo màn hình ban đầu
        public Form1()
        {
            InitializeComponent();

            //Cài đặt mặc định với đối tượng hình
            shShape = 0; //Mở đầu là đường thẳng
            userLineColor = Color.White; //Màu nét
            userBgColor = Color.Black; //Màu nền
            userLineSize = 1; //Kích cỡ nét
            pStart.X = pStart.Y = pEnd.X = pEnd.Y = 0;
            
            numObj = 0; //số lượng vẽ bằng 1
            nowId = -1; //Mở đầu chưa có đối tượng nào được chọn
            idViewPoint = numObj;
        }

        //Nút vẽ hình được chọn
        private void btLine_Click(object sender, EventArgs e)
        {
            shShape = 0;
        }

        private void btCircle_Click(object sender, EventArgs e)
        {
            shShape = 1;
        }

        private void bt_hinh_chu_nhat_Click(object sender, EventArgs e)
        {
            shShape = 2;
        }

        private void btEllipse_Click(object sender, EventArgs e)
        {
            shShape = 3;
        }

        private void bt_tam_giac_deu_Click(object sender, EventArgs e)
        {
            shShape = 4;
        }

        private void bt_ngu_giac_deu_Click(object sender, EventArgs e)
        {
            shShape = 5;
        }

        private void bt_luc_giac_deu_Click(object sender, EventArgs e)
        {
            shShape = 6;
        }

        //Nút chọn màu nền
        private void btMauNen_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                userBgColor = colorDialog1.Color;
            }
        }

        //Nút chọn màu viền
        private void btMauVien_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                userLineColor = colorDialog1.Color;
            }
        }

        //Nút tô màu hình
        private void bt_fill_MouseClick(object sender, MouseEventArgs e)
        {
            shShape = 7;
        }
        
        //Nút chọn kích thước nét
        private void bt_DoDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Begin(OpenGL.GL_POINTS);
            
            if ((string)bt_DoDay.SelectedItem == "Small")
            {
                userLineSize = 1;
            }
            else if ((string)bt_DoDay.SelectedItem == "Medium")
            {
                userLineSize = 2;
            }
            else if ((string)bt_DoDay.SelectedItem == "Big")
            {
                userLineSize = 3;
            }
            gl.End();
            gl.Flush();
        }

        //Các hàm vẽ mặc định trong SharpGL
        //Hàm vẽ chính trong chương trình
        private void openGLControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            // Lấy object của OpenGL
            OpenGL gl = openGLControl.OpenGL;
            // Xoá màu và bộ nhớ
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //Tạo ra một thực thể hình mới
            switch(shShape)
            {
                case 0: arrObj[numObj] = new Line(); break;
                case 1: arrObj[numObj] = new Circle(); break;
                case 2: arrObj[numObj] = new Rectangle(); break;
                case 3: arrObj[numObj] = new Ellipse(); break;
                case 4: arrObj[numObj] = new Triangle(); break;
                case 5: arrObj[numObj] = new Pentagon(); break;
                case 6: arrObj[numObj] = new Hexagon(); break;
                case 7: arrObj[numObj].colorObject(gl); break;
                default: break;
            }

            //Đặt các tham số khởi tạo
            arrObj[numObj].setPoint(pStart, pEnd);
            arrObj[numObj].set(pStart, pEnd);
            arrObj[numObj].setLineColor(userLineColor);

            if (numObj > 0)
                arrObj[numObj - 1].setLineSize(userLineSize);
            else arrObj[numObj].setLineSize(userLineSize);

            //Vẽ lại tất cả các hình
            for (int i = 0; i <= numObj; i++)
            {
                arrObj[i].drawObject(gl);
                arrObj[i].colorObject(gl);
            }

            if (idViewPoint != -1) //Nếu đang vẽ một hình thì hiện control point của hình đó lên
                arrObj[idViewPoint].viewControlPoint(gl);

            gl.Flush();
        }

        //Hàm cài đặt
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //Khai báo biến OpenGL gl
            OpenGL gl = openGLControl.OpenGL;
            //Xóa màng hình, trả chế độ và load view
            gl.ClearColor(0, 0, 0, 0);
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
        }
        
        //Hàm resized
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            // Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            // Load the identity.
            gl.LoadIdentity();
            // Create a perspective transformation.
            gl.Viewport(0, 0, gl.RenderContextProvider.Width, gl.RenderContextProvider.Height);
            gl.Ortho2D(0, gl.RenderContextProvider.Width, 0, gl.RenderContextProvider.Height);
        }

        //Các sự kiện về chuột
        //Khi nhấn chuột vào màn hình và sau đó giữ
        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            st.Reset();
            st.Start();
            // Cập nhật toạ độ điểm đầu và cuối
            pStart = e.Location; // e là tham số liên quan đến sự kiện này
            pEnd = pStart;
        }

        //Khi di chuyển chuột trên màn hình
        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                pEnd = e.Location;
                idViewPoint = numObj;
            }
        }

        //Khi thả chuột ra
        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            numObj += 1; //Tăng biến đếm số hình hiện có

            nowId = -1;
            //reset lại pStart, pEnd
            pStart.X = 0;
            pStart.Y = 0;
            pEnd.X = 0;
            pEnd.Y = 0;
            //Hiển thị thời gian
            st.Stop();
            time.Text = st.Elapsed.ToString();
        }

        //Khi nhấp chuột vào màn hình
        private void openGLControl_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = numObj - 1; i >= 0; i--)
            {//Kiểm tra điểm được chọn có nằm trong hình đã vẽ không
                if (e.Location.X <= arrObj[i].pEnd.X && e.Location.X >= arrObj[i].pStart.X && e.Location.Y <= arrObj[i].pEnd.Y && e.Location.Y >= arrObj[i].pStart.Y)
                {
                    idViewPoint = i;
                    break;
                }
                if (i == 0) idViewPoint = -1; //Nếu không thì coi như không hiện viewPoint lên
            }
        }
    }
}