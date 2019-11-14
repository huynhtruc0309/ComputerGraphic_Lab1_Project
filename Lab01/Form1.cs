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
        class vungto
        {
           public List<Point> p;
            public Color cl;
            public vungto()
            {
                p = new List<Point>();
                cl = Color.Black;
            }
        }
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
                gl.PointSize(lineSize + 3);
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
                gl.PointSize(lineSize + 3);
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
                gl.PointSize(lineSize + 3);
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
                gl.PointSize(lineSize + 3);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(1.0, 0, 0, 0);
                
                gl.Vertex(center.X + R, gl.RenderContextProvider.Height - center.Y - 2 * R);
                gl.Vertex(center.X + R, gl.RenderContextProvider.Height - center.Y - R);
                gl.Vertex(center.X + R, gl.RenderContextProvider.Height - center.Y);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - center.Y - 2 * R);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - center.Y - R);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - center.Y);
                gl.Vertex(center.X - R, gl.RenderContextProvider.Height - center.Y - 2 * R);
                gl.Vertex(center.X - R, gl.RenderContextProvider.Height - center.Y - R);
                gl.Vertex(center.X - R, gl.RenderContextProvider.Height - center.Y);
                
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
                gl.PointSize(lineSize + 3);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(1.0, 0, 0, 0);

                gl.Vertex(center.X + Rx, gl.RenderContextProvider.Height - center.Y - Ry);
                gl.Vertex(center.X + Rx, gl.RenderContextProvider.Height - center.Y);
                gl.Vertex(center.X + Rx, gl.RenderContextProvider.Height - center.Y + Ry);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - center.Y - Ry);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - center.Y);
                gl.Vertex(center.X, gl.RenderContextProvider.Height - center.Y + Ry);
                gl.Vertex(center.X - Rx, gl.RenderContextProvider.Height - center.Y - Ry);
                gl.Vertex(center.X - Rx, gl.RenderContextProvider.Height - center.Y);
                gl.Vertex(center.X - Rx, gl.RenderContextProvider.Height - center.Y + Ry);

                gl.End();
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
                gl.PointSize(lineSize + 3);
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
                gl.PointSize(lineSize + 3);
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
        private List<vungto> tatcavungto;

        private Stopwatch st = new Stopwatch();

        // Khởi tạo màn hình ban đầu
        private Point toadomau;
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
            toadomau.X = -1;
            toadomau.Y = -1;
            tatcavungto = new List<vungto>();
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
        Color GetColor(int x,int y, OpenGL gl)
        {

            byte[] pixel = new byte[3];
            gl.ReadPixels(x, y, 1, 1, OpenGL.GL_RGB, OpenGL.GL_UNSIGNED_BYTE, pixel);
            Color cl = Color.FromArgb(0,(int)pixel[0], (int)pixel[1], (int)pixel[2]);
            return cl;
        }
        private void tomau(Point p, Color color, Object[] arr, OpenGL gl )
        {

            List<Color> all_b_color = new List<Color>();
            for(int i = 0; i < numObj; i++)
            {
                int flag = 1;
               for(int j = 0; j < all_b_color.Count; j++)
                {
                    if (arr[j].lineColor == arr[i].lineColor) {flag = 0;
                        break;
                        }
                }
               if(flag == 1)
                {
                    all_b_color.Add(arr[i].lineColor);
                }
            }
            Queue<Point> diemdangxet = new Queue<Point>();
            diemdangxet.Enqueue(p);
            vungto vt = new vungto();
            vt.cl = color;
            while (diemdangxet.Count > 0)
            {
                Point temp = diemdangxet.Dequeue();
                vt.p.Add(temp);
                gl.Color(color.R / 255.0, color.G / 255.0, color.B / 255.0, 0);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Vertex(temp.X, temp.Y);
                gl.End();
                gl.Flush();
                Point p1 = temp;
                Point p2 = temp;
                Point p3 = temp;
                Point p4=temp;
                p1.X = temp.X - 1;
                p2.X = temp.X + 1;
                p3.Y = temp.Y - 1;
                p4.Y = temp.Y + 1;
                Color color1 = GetColor(p1.X, p1.Y, gl);
                Color color2 = GetColor(p2.X, p2.Y, gl);
                Color color3 = GetColor(p3.X, p3.Y, gl);
                Color color4 = GetColor(p4.X, p4.Y, gl);
                int flag1 = 1, flag2 = 1, flag3 = 1, flag4 = 1;
                for(int i = 0; i < all_b_color.Count; i++)
                {
                    if ((color1.R == all_b_color[i].R && color1.G == all_b_color[i].G) &&(color1.B == all_b_color[i].B) ||
                        (color1.R == color.R && color1.G == color.G && color1.B == color.B)) flag1 = 0;
                    if ((color2.R == all_b_color[i].R && color2.G == all_b_color[i].G) && (color2.B == all_b_color[i].B) ||
                        (color2.R == color.R && color2.G == color.G && color2.B == color.B)) flag2 = 0;
                    if ((color3.R == all_b_color[i].R && color3.G == all_b_color[i].G) && (color3.B == all_b_color[i].B) ||
                        (color3.R == color.R && color3.G == color.G && color3.B == color.B)) flag3 = 0;
                    if ((color4.R == all_b_color[i].R && color4.G == all_b_color[i].G) && (color4.B == all_b_color[i].B) ||
                        (color4.R == color.R && color4.G == color.G && color4.B == color.B)) flag4 = 0;
                }
                if (p1.X < 0 || p1.Y < 0 || p1.X > gl.RenderContextProvider.Width || p1.Y > gl.RenderContextProvider.Height||diemdangxet.Contains(p1)) flag1 = 0;
                if (p2.X < 0 || p2.Y < 0 || p2.X > gl.RenderContextProvider.Width || p2.Y > gl.RenderContextProvider.Height || diemdangxet.Contains(p2)) flag2 = 0;
                if (p3.X < 0 || p3.Y < 0 || p3.X > gl.RenderContextProvider.Width || p3.Y > gl.RenderContextProvider.Height || diemdangxet.Contains(p3)) flag3 = 0;
                if (p4.X < 0 || p4.Y < 0 || p4.X > gl.RenderContextProvider.Width || p4.Y > gl.RenderContextProvider.Height || diemdangxet.Contains(p4)) flag4 = 0;
                if (flag1 == 1) diemdangxet.Enqueue(p1);
                if (flag2 == 1) diemdangxet.Enqueue(p2);
                if (flag3 == 1) diemdangxet.Enqueue(p3);
                if (flag4 == 1) diemdangxet.Enqueue(p4);
            }

            tatcavungto.Add(vt);
            
        }


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
             //   case 7: arrObj[numObj].colorObject(gl); break;
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
            if(shShape == 7)
            {
                userBgColor = userLineColor;
                if(toadomau.X != -1 && toadomau.Y != -1)
                {
                    toadomau.Y = gl.RenderContextProvider.Height - toadomau.Y;
                    gl.PointSize(1);
                    gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
                    tomau(toadomau, userBgColor,arrObj,gl);
                }
                toadomau.X = -1;
                toadomau.Y = -1;
            }
            gl.PointSize(1);
            gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
            for(int i = 0; i < tatcavungto.Count; i++)
            {
                gl.Color(tatcavungto[i].cl.R / 255.0, tatcavungto[i].cl.G / 255.0, tatcavungto[i].cl.B / 255.0, 0);
                gl.Begin(OpenGL.GL_POINTS);
                for(int j = 0; j < tatcavungto[i].p.Count; j++)
                {
                    gl.Vertex(tatcavungto[i].p[j].X, tatcavungto[i].p[j].Y);
                }
                gl.End();
                gl.Flush();
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

        private void bt_fill_Click(object sender, EventArgs e)
        {
            shShape = 7;
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
            if (shShape != 7)
            {
                numObj += 1; //Tăng biến đếm số hình hiện có

                nowId = -1;
            }
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
            if (shShape == 7)
            {
                toadomau = pStart;
            }
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