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
using System.Collections;

namespace Lab01
{
    public partial class Form1 : Form
    {
        const float epsilon = 5;
        int height = 0;
        int width = 0;
        Point pkeogian;
        int indexnum = 0;
        class Mat
        {
            private List<List<float>> matrix;
            public Mat(int rows, int cols)
            {
                List<List<float>> result = new List<List<float>>();
                for (int i = 0; i < rows; i++)
                {
                    List<float> temp = new List<float>();
                    result.Add(temp);
                    for (int j = 0; j < cols; j++)
                    {
                        result[i].Add(0);
                    }
                }
                this.matrix = result;
            }
            public int getRows()
            {
                return matrix.Count;
            }
            public int getCols()
            {
                if (matrix.Count > 0)
                {
                    return matrix[0].Count;
                }
                else return 0;
            }
            public float at(int i, int j)
            {
                return matrix[i][j];
            }
            public void set(int i, int j, float c)
            {
                matrix[i][j] = c;
            }
            public Mat multiply(Mat a, Mat b)
            {
                Mat c = new Mat(a.getRows(), b.getCols());
                for (int i = 0; i < a.getRows(); i++)
                {
                    for (int j = 0; j < b.getCols(); j++)
                    {
                        float sum = 0;
                        for (int z = 0; z < a.getCols(); z++)
                        {
                            sum += a.at(i, z) * b.at(z, j);
                        }
                        c.set(i, j, sum);
                    }
                }
                return c;
            }


        }

        class AffineTransform
        {
            Mat _matrixTransform;
            public AffineTransform()
            {
                _matrixTransform = new Mat(3, 3);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        _matrixTransform.set(i, j, 0);
                    }
                }
                for (int i = 0; i < 3; i++) _matrixTransform.set(i, i, 1);
            }
            public void Translate(float dx, float dy)
            {
                Mat temp = new Mat(3, 3);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        temp.set(i, j, 0);
                    }
                    temp.set(i, i, 1);
                }
                temp.set(0, 2, dx);
                temp.set(1, 2, dy);
                _matrixTransform = _matrixTransform.multiply(temp, _matrixTransform);

            }
            public void Rotate(float degree)
            {
                degree = (float)(degree * Math.PI / 180);
                Mat temp = new Mat(3, 3);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        temp.set(i, j, 0);
                    }
                }
                temp.set(0, 0, (float)Math.Cos(degree));
                temp.set(0, 1, -(float)Math.Sin(degree));
                temp.set(1, 0, (float)Math.Sin(degree));
                temp.set(1, 1, (float)Math.Cos(degree));
                temp.set(2, 2, 1);
                _matrixTransform = _matrixTransform.multiply(temp, _matrixTransform);
            }
            public void Scale(float sx, float sy)
            {
                //degree = (float)(degree * Math.PI / 180);
                Mat temp = new Mat(3, 3);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        temp.set(i, j, 0);
                    }


                }
                temp.set(0, 0, sx);
                temp.set(1, 1, sy);
                temp.set(2, 2, 1);

                _matrixTransform = _matrixTransform.multiply(temp, _matrixTransform);
            }
            public void TransformPoint(ref float x, ref float y)
            {
                Mat temp = new Mat(3, 1);
                temp.set(0, 0, x);
                temp.set(1, 0, y);
                temp.set(2, 0, 1);
                temp = _matrixTransform.multiply(_matrixTransform, temp);
                x = temp.at(0, 0);
                y = temp.at(1, 0);
            }
            public float tichhuuhuong(Point a, Point b, Point c)
            {
                Point temp1 = b;
                Point temp2 = c;
                temp1.X = b.X - a.X;
                temp1.Y = b.Y - a.Y;
                temp2.X = c.X - a.X;
                temp2.Y = c.Y - a.Y;
                return temp1.X * temp2.Y - temp1.Y * temp2.X;
            }
            public bool ismiddle(Object inobject, Point End)
            {
                List<Point> p = inobject.GetVertices();
                if (p.Count < 2) return false;
                if (p.Count == 2)
                {
                    return End.X >= p[0].X && End.X <= p[1].X;
                }
                else
                {
                    bool flag = true;
                    for (int i = 0; i < p.Count - 1; i++)
                    {
                        Point temp1 = p[i];
                        Point temp2 = p[i + 1];
                        if (tichhuuhuong(temp1, temp2, End) < 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                    return flag;
                }
                //return false;
            }
            virtual public void CoGian(Object inobject, Point pstart, Point pend)//toa do cua glcontrol
            {
                List<Point> p = inobject.GetVertices();
                Point diemchon = pend;
                bool middle = ismiddle(inobject, pend);
                float scaleratio = (float)((pend.X + pend.Y) * 1.0 / (pstart.X + pstart.Y));
                // float scaleratioy = (float)((pend.Y) * 1.0 / (pstart.Y));
                //float scaleratio = scaleratiox > scaleratioy ? scaleratiox : scaleratioy;
                /*  if (middle == false)
                  {
                      if (pend.X <= pstart.X) scaleratio = 1.0f / scaleratio;
                  }
                  if (middle == true)
                  {
                      if (scaleratio > 1) scaleratio = 1.0f / scaleratio;
                  }*/
                // scaleratio = 2;

                List<Point> temp = new List<Point>();
                this.Scale(scaleratio, scaleratio);
                for (int i = 0; i < p.Count; i++)
                {
                    Point t = pend;
                    float x = p[i].X;
                    float y = p[i].Y;
                    this.TransformPoint(ref x, ref y);
                    t.X = (int)x;
                    t.Y = (int)y;
                    temp.Add(t);
                }
                inobject.setP(temp, scaleratio);
            }
        }

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

        List<List<List<int>>> bitmap;
        public abstract class Object
        {
            public Point pStart, pEnd;
            public List<Point> p = new List<Point>();
            public Color lineColor, bgColor; //Màu viền và màu nền
            public int lineSize; //Kích cỡ nét
            /// <summary>
            /// xMin -> yMin -> xMax -> yMax
            /// </summary>
            public List<int> pMinMax = new List<int>(4);
            //Constructor
            public Object()
            {
                lineColor = Color.White;
                bgColor = Color.Black;
                lineSize = 5;
            }

            /// <summary>
            /// Hàm lấy giá trị xMin, yMin, xMax, yMax
            /// </summary>
            private void getpMinMax()
            {
                pMinMax.Add(9999);
                pMinMax.Add(9999);
                pMinMax.Add(-1);
                pMinMax.Add(-1);
                foreach (var pt in p)
                {
                    if (pt.X > pMinMax[2]) pMinMax[2] = pt.X;
                    if (pt.Y > pMinMax[3]) pMinMax[3] = pt.Y;
                    if (pt.X < pMinMax[0]) pMinMax[0] = pt.X;
                    if (pt.Y < pMinMax[1]) pMinMax[1] = pt.Y;
                }
            }

            /// <summary>
            /// Hàm kiểm tra điểm p có nằm trong hình không
            /// </summary>
            /// <param name="p">Tọa độ điểm click chuột</param>
            /// <returns></returns>
            public bool IsInside(Point p)
            {
                getpMinMax();
                if ((pMinMax[0] <= p.X && p.X <= pMinMax[2]) && (pMinMax[1] <= p.Y && p.Y <= pMinMax[3]))
                    return true;
                return false;
            }

            public int Round(double x)
            {
                return (int)(x + 0.5);
            }

            //Cài đặt
            public void set(Point start, Point end)
            {
                pStart = start;
                pEnd = end;
            }

            //Màu viền
            public void setLineColor(Color color)
            {
                lineColor = color;
            }

            public virtual void updateP() { }

            //Màu nền
            public void setBGColor(Color bg)
            {
                bgColor = bg;
            }

            //Độ dày nét
            public void setLineSize(int userChoice)
            {
                switch (userChoice)
                {
                    case 1: lineSize = 5; break; //Small
                    case 2: lineSize = 10; break; //Medium
                    case 3: lineSize = 15; break; //Big
                }
            }

            public int getLineSize()
            {
                return lineSize;
            }

            //Thiết lập toạ độ khi di chuyển chuột
            public virtual void setPoint(Point start, Point end) { }

            //Hàm vẽ hình, tô màu và hiển thị điểm điều khiển
            public virtual void drawObject(OpenGL gl) { }
            public virtual void colorObject(OpenGL gl)
            {
            }
            public virtual void viewControlPoint(OpenGL gl)
            {
                gl.PointSize(lineSize + 3);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(1.0, 0, 0, 0);
                for (int i = 0; i < p.Count(); i++)
                {
                    gl.Vertex(p[i].X, gl.RenderContextProvider.Height - p[i].Y);
                }
                gl.End();
            }

            //Hàm xoay ảnh
            public virtual void affine(float degree) { }

            //Hàm di chuyển ảnh
            public virtual void move(int dX, int dY) { }

            public virtual List<Point> GetVerticesForScanline()
            {
                return this.p;
            }
            //Lấy đỉnh
            public virtual List<Point> GetVertices()
            {
                return p;
            }
            public virtual void setP(List<Point> p, float ration)
            {

            }
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
                p.Add(A);
                p.Add(B);
            }

            public override void setP(List<Point> p, float ratio)
            {
                int deltaX = p[0].X - A.X;

                int deltaY = p[0].Y - A.Y;
                A.X = p[0].X - deltaX;
                A.Y = (p[0].Y - deltaY);
                B.X = p[1].X - deltaX;
                B.Y = (p[1].Y - deltaY);
            }
            public override void updateP()
            {
                this.p[0] = A;
                this.p[1] = B;
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
                p.Add(A); p.Add(B); p.Add(C); p.Add(D);
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
            public override void setP(List<Point> p, float ratio)
            {
                int deltaX = p[0].X - A.X;
                int deltaY = p[0].Y - A.Y;
                A.X = p[0].X - deltaX;
                A.Y = p[0].Y - deltaY;
                B.X = p[1].X - deltaX;
                B.Y = p[1].Y - deltaY;
                C.X = p[2].X - deltaX;
                C.Y = p[2].Y - deltaY;
                D.X = p[3].X - deltaX;
                D.Y = p[3].Y - deltaY;
            }
            public override void updateP()
            {
                this.p[0] = A;
                this.p[1] = B;
                this.p[2] = C;
                this.p[3] = D;
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
            public override void setP(List<Point> p, float ration)
            {
                int deltaX = p[0].X - A.X;
                int deltaY = p[0].Y - A.Y;
                A.X = p[0].X - deltaX;
                A.Y = p[0].Y - deltaY;
                B.X = p[1].X - deltaX;
                B.Y = p[1].Y - deltaY;
                C.X = p[2].X - deltaX;
                C.Y = p[2].Y - deltaY;
            }
            public override void updateP()
            {
                this.p[0] = A;
                this.p[1] = B;
                this.p[2] = C;
            }
            public override void setPoint(Point start, Point end)
            {
                A.X = (start.X + end.X) / 2;
                A.Y = start.Y;
                B = end;
                C.X = start.X;
                C.Y = end.Y;
                p.Add(A); p.Add(B); p.Add(C);
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
            public int oldR;
            private Point center;
            public override void setP(List<Point> p, float ratio)
            {
                //float ratio = 1.0f*p[0].X / this.p[0].X;
                R = (int)(oldR * ratio);
            }
            public Circle() { }
            public override void updateP()
            {

                Point C = new Point(center.X - R, center.Y + 2 * R);
                Point B = new Point(center.X - R, center.Y + R);
                Point A = new Point(center.X - R, center.Y);
                Point H = new Point(center.X, center.Y);
                Point G = new Point(center.X + R, center.Y);
                Point F = new Point(center.X + R, center.Y + R);
                Point E = new Point(center.X + R, center.Y + 2 * R);
                Point D = new Point(center.X, center.Y + 2 * R);
                Point K = new Point(center.X, center.Y + R);
                p[0] = A;
                p[1] = B;
                p[2] = C;
                p[3] = D;
                p[4] = E;
                p[5] = F;
                p[6] = G;
                p[7] = H;
                p[8] = K;
                oldR = R;
                // p.Add(A); p.Add(B); p.Add(C); p.Add(D); p.Add(E); p.Add(F); p.Add(G); p.Add(H); p.Add(K);
            }
            //Hàm đặt điểm
            public override void setPoint(Point start, Point end)
            {
                center.X = (start.X + end.X) / 2;
                center.Y = (start.Y + start.Y) / 2;
                if (Math.Abs(start.X - center.X) >= Math.Abs(start.Y - center.Y))
                    R = Math.Abs(start.X - center.X);
                else R = Math.Abs(start.Y - center.Y);
                oldR = R;

                Point C = new Point(center.X - R, center.Y + 2 * R);
                Point B = new Point(center.X - R, center.Y + R);
                Point A = new Point(center.X - R, center.Y);
                Point H = new Point(center.X, center.Y);
                Point G = new Point(center.X + R, center.Y);
                Point F = new Point(center.X + R, center.Y + R);
                Point E = new Point(center.X + R, center.Y + 2 * R);
                Point D = new Point(center.X, center.Y + 2 * R);
                Point K = new Point(center.X, center.Y + R);
                p.Add(A); p.Add(B); p.Add(C); p.Add(D); p.Add(E); p.Add(F); p.Add(G); p.Add(H); p.Add(K);
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

            //Hàm tịnh tiến ảnh
            public override void move(int dX, int dY)
            {
                center.X += dX;
                center.Y += dY;
            }

            public override List<Point> GetVerticesForScanline()
            {
                List<Point> pVertices = new List<Point>();
                int totalSegments = 60;
                Point p1 = new Point(p[0].X, p[0].Y);
                Point p2 = new Point(p[4].X, p[4].Y);
                p1 = new Point(p1.X, p1.Y);
                p2 = new Point(p2.X, p2.Y);
                double r = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
                r = r / (2 * Math.Sqrt(2));
                int xc = Round((p1.X + p2.X) / 2);
                int yc = Round((p1.Y + p2.Y) / 2);
                int x = 0;
                int y = Round(r);
                Point pV; // Cac dinh cua obj vao

                for (int alpha = 0; alpha < 360; alpha += 360 / totalSegments)
                {
                    // Đổi về radian
                    double alpha_rad = alpha * Math.PI / 180;
                    // Tinh x, y
                    pV = new Point(Round(xc + x * Math.Cos(alpha_rad) - y * Math.Sin(alpha_rad))
                        , Round(yc + x * Math.Sin(alpha_rad) + y * Math.Cos(alpha_rad)));
                    // Them cac dinh cua obj vao
                    pVertices.Add(pV);
                }
                return pVertices;
            }
        }

        // Class hình ellipse
        public class Ellipse : Object
        {
            private int Rx, Ry;
            private int oldRx, oldRy;
            private Point center;
            public Ellipse() { }
            public override void setP(List<Point> p, float ratio)
            {
                //float ratio = 1.0f * p[0].X / this.p[0].X;
                Rx = (int)(oldRx * ratio);
                Ry = (int)(oldRy * ratio);

            }
            public override void updateP()
            {

                Point C = new Point(center.X - Rx, center.Y + Ry);
                Point B = new Point(center.X - Rx, center.Y);
                Point A = new Point(center.X - Rx, center.Y - Ry);
                Point H = new Point(center.X, center.Y - Ry);
                Point G = new Point(center.X + Rx, center.Y - Ry);
                Point F = new Point(center.X + Rx, center.Y);
                Point E = new Point(center.X + Rx, center.Y + Ry);
                Point D = new Point(center.X, center.Y + Ry);
                Point K = new Point(center.X, center.Y);
                p[0] = A;
                p[1] = B;
                p[2] = C;
                p[3] = D;
                p[4] = E;
                p[5] = F;
                p[6] = G;
                p[7] = H;
                p[8] = K;
                oldRx = Rx;
                oldRy = Ry;
                // p.Add(A); p.Add(B); p.Add(C); p.Add(D); p.Add(E); p.Add(F); p.Add(G); p.Add(H); p.Add(K);
            }
            //Hàm đặt điểm
            public override void setPoint(Point start, Point end)
            {
                center.X = (start.X + end.X) / 2;
                center.Y = (start.Y + end.Y) / 2;
                Rx = Math.Abs(start.X - center.X);
                Ry = Math.Abs(start.Y - center.Y);
                oldRx = Rx;
                oldRy = Ry;

                Point C = new Point(center.X - Rx, center.Y + Ry);
                Point B = new Point(center.X - Rx, center.Y);
                Point A = new Point(center.X - Rx, center.Y - Ry);
                Point H = new Point(center.X, center.Y - Ry);
                Point G = new Point(center.X + Rx, center.Y - Ry);
                Point F = new Point(center.X + Rx, center.Y);
                Point E = new Point(center.X + Rx, center.Y + Ry);
                Point D = new Point(center.X, center.Y + Ry);
                Point K = new Point(center.X, center.Y);
                p.Add(A); p.Add(B); p.Add(C); p.Add(D); p.Add(E); p.Add(F); p.Add(G); p.Add(H); p.Add(K);
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

            public override List<Point> GetVerticesForScanline()
            {
                List<Point> pVertices = new List<Point>();
                int totalSegments = 60;
                Point p1 = new Point(p[0].X, p[0].Y);
                Point p2 = new Point(p[4].X, p[4].Y);
                p1 = new Point(p1.X, p1.Y);
                p2 = new Point(p2.X, p2.Y);
                int xc = Round((double)(p1.X + p2.X) / 2);
                int yc = Round((double)(p1.Y + p2.Y) / 2);

                // Goi A(xa, ya) la giao diem cua 0x va ellipse
                int xa = p2.X;
                int ya = Round((double)(p1.Y + p2.Y) / 2);

                // Goi B(xb, yb) la giao diem cua 0y va ellipse
                int xb = Round((double)(p1.X + p2.X) / 2);
                int yb = p1.Y;

                // Tinh rx va ry
                double rx = Math.Sqrt(Math.Pow(xa - xc, 2) + Math.Pow(ya - yc, 2));
                double ry = Math.Sqrt(Math.Pow(xb - xc, 2) + Math.Pow(yb - yc, 2));

                Point pV; //Biến gán tạm mỗi đỉnh của Ellipse
                for (int alpha = 0; alpha < 360; alpha += 360 / totalSegments)
                {
                    //Tính theo radian
                    double alpha_rad = alpha * Math.PI / 180;
                    // Tinh x, y
                    pV = new Point(Round(xc + rx * Math.Cos(alpha_rad))
                        , Round(yc + ry * Math.Sin(alpha_rad)));
                    //Thêm vào danh sách các đỉnh của hình
                    pVertices.Add(pV);
                }
                return pVertices;
            }
        }

        // Class hình ngũ giác đều
        public class Pentagon : Object
        {
            private int R;
            private int oldR;
            private Point center;
            public override void setP(List<Point> p, float ratio)
            {
                R = (int)(oldR * ratio);
            }
            public Pentagon() { }
            //Hàm đặt điểm
            public override void updateP()
            {
                this.p.Clear();
                float grad = (float)((72 * 3.14) / 180);
                for (int i = 1; i < 5; i++)
                {
                    //Thực hiện phép xoay pixel
                    int x = (int)(-Math.Sin(i * grad) * R);
                    int y = (int)(Math.Cos(i * grad) * R);
                    p.Add(new Point(center.X + x, center.Y - y));
                }
                oldR = R;
            }
            public override void setPoint(Point start, Point end)
            {
                center.X = (start.X + end.X) / 2;
                center.Y = (start.Y + start.Y) / 2;
                if (Math.Abs(start.X - center.X) >= Math.Abs(start.Y - center.Y))
                    R = Math.Abs(start.X - center.X);
                else R = Math.Abs(start.Y - center.Y);
                oldR = R;

                float grad = (float)((72 * 3.14) / 180);

                p.Add(new Point(center.X, center.Y - R));
                for (int i = 1; i < 5; i++)
                {
                    //Thực hiện phép xoay pixel
                    int x = (int)(-Math.Sin(i * grad) * R);
                    int y = (int)(Math.Cos(i * grad) * R);
                    p.Add(new Point(center.X + x, center.Y - y));
                }
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
            private int oldR;
            private Point center;
            public override void setP(List<Point> p, float ration)
            {
                R = (int)(oldR * ration);
            }
            public Hexagon() { }
            public override void updateP()
            {
                this.p.Clear();
                float grad = (float)((60 * 3.14) / 180);
                for (int i = 1; i < 6; i++)
                {
                    //Thực hiện phép xoay pixel
                    int x = (int)(-Math.Sin(i * grad) * R);
                    int y = (int)(Math.Cos(i * grad) * R);
                    p.Add(new Point(center.X + x, center.Y - y));
                }
                oldR = R;

            }
            //Hàm đặt điểm
            public override void setPoint(Point start, Point end)
            {
                center.X = (start.X + end.X) / 2;
                center.Y = (start.Y + start.Y) / 2;
                if (Math.Abs(start.X - center.X) >= Math.Abs(start.Y - center.Y))
                    R = Math.Abs(start.X - center.X);
                else R = Math.Abs(start.Y - center.Y);
                oldR = R;
                float grad = (float)((60 * 3.14) / 180);
                p.Add(new Point(center.X, center.Y - R));
                for (int i = 1; i < 6; i++)
                {
                    //Thực hiện phép xoay pixel
                    int x = (int)(-Math.Sin(i * grad) * R);
                    int y = (int)(Math.Cos(i * grad) * R);
                    p.Add(new Point(center.X + x, center.Y - y));
                }
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
        private byte[] pixel;
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
            pkeogian.X = -1;
            pkeogian.Y = -1;
            indexnum = 0;

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

        //Nút tô màu hình
        private void bt_fill_MouseClick(object sender, MouseEventArgs e)
        {
            shShape = 7;
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
        Color GetColor(int x, int y, OpenGL gl)
        {
            Color cl = Color.FromArgb(0, (int)bitmap[y][x][0], (int)bitmap[y][x][1], (int)bitmap[y][x][2]);
            return cl;
        }
        private void tomaureal(Point p, Color color, Object[] arr, OpenGL gl)
        {
            List<Color> all_b_color = new List<Color>();
            for (int i = 0; i < numObj; i++)
            {
                int flag = 1;
                for (int j = 0; j < all_b_color.Count; j++)
                {
                    if (arr[j].lineColor == arr[i].lineColor)
                    {
                        flag = 0;
                        break;
                    }
                }
                if (flag == 1)
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
                bitmap[temp.Y][temp.X][0] = color.R;
                bitmap[temp.Y][temp.X][1] = color.G;
                bitmap[temp.Y][temp.X][2] = color.B;
                bitmap[temp.Y][temp.X][3] = 0;
                Point p1 = temp;
                Point p2 = temp;
                Point p3 = temp;
                Point p4 = temp;
                p1.X = temp.X - 1;
                p2.X = temp.X + 1;
                p3.Y = temp.Y - 1;
                p4.Y = temp.Y + 1;
                int flag1 = 1, flag2 = 1, flag3 = 1, flag4 = 1;
                if (p1.X < 0 || p1.Y < 0 || p1.X >= gl.RenderContextProvider.Width || p1.Y >= gl.RenderContextProvider.Height || diemdangxet.Contains(p1)) flag1 = 0;
                if (p2.X < 0 || p2.Y < 0 || p2.X >= gl.RenderContextProvider.Width || p2.Y >= gl.RenderContextProvider.Height || diemdangxet.Contains(p2)) flag2 = 0;
                if (p3.X < 0 || p3.Y < 0 || p3.X >= gl.RenderContextProvider.Width || p3.Y >= gl.RenderContextProvider.Height || diemdangxet.Contains(p3)) flag3 = 0;
                if (p4.X < 0 || p4.Y < 0 || p4.X >= gl.RenderContextProvider.Width || p4.Y >= gl.RenderContextProvider.Height || diemdangxet.Contains(p4)) flag4 = 0;
                Color color1 = Color.White, color2 = Color.White, color3 = Color.White, color4 = Color.White;
                if (flag1 == 1) color1 = GetColor(p1.X, p1.Y, gl);
                if (flag2 == 1) color2 = GetColor(p2.X, p2.Y, gl);
                if (flag3 == 1) color3 = GetColor(p3.X, p3.Y, gl);
                if (flag4 == 1) color4 = GetColor(p4.X, p4.Y, gl);

                for (int i = 0; i < all_b_color.Count; i++)
                {
                    if ((color1.R == all_b_color[i].R && color1.G == all_b_color[i].G) && (color1.B == all_b_color[i].B) ||
                        (color1.R == color.R && color1.G == color.G && color1.B == color.B)) flag1 = 0;
                    if ((color2.R == all_b_color[i].R && color2.G == all_b_color[i].G) && (color2.B == all_b_color[i].B) ||
                        (color2.R == color.R && color2.G == color.G && color2.B == color.B)) flag2 = 0;
                    if ((color3.R == all_b_color[i].R && color3.G == all_b_color[i].G) && (color3.B == all_b_color[i].B) ||
                        (color3.R == color.R && color3.G == color.G && color3.B == color.B)) flag3 = 0;
                    if ((color4.R == all_b_color[i].R && color4.G == all_b_color[i].G) && (color4.B == all_b_color[i].B) ||
                        (color4.R == color.R && color4.G == color.G && color4.B == color.B)) flag4 = 0;
                }

                if (flag1 == 1) diemdangxet.Enqueue(p1);
                if (flag2 == 1) diemdangxet.Enqueue(p2);
                if (flag3 == 1) diemdangxet.Enqueue(p3);
                if (flag4 == 1) diemdangxet.Enqueue(p4);
            }

            tatcavungto.Add(vt);
        }
        private void tomau(Point p, Color color, Object[] arr, OpenGL gl)
        {
            gl.ReadPixels(0, 0, gl.RenderContextProvider.Width
                    , gl.RenderContextProvider.Height, OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, pixel);
            int pixelindex = 0;
            for (int i = 0; i < bitmap.Count; i++)
            {
                for (int j = 0; j < bitmap[i].Count; j++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        bitmap[i][j][z] = pixel[pixelindex++];

                    }
                }
            }
            tomaureal(p, color, arr, gl);
            pixelindex = 0;
            for (int i = 0; i < bitmap.Count; i++)
            {
                for (int j = 0; j < bitmap[i].Count; j++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        pixel[pixelindex++] = (byte)bitmap[i][j][z];

                    }
                }
            }
            gl.DrawPixels(gl.RenderContextProvider.Width, gl.RenderContextProvider.Height, OpenGL.GL_UNSIGNED_BYTE, pixel);
        }

        #region THUAT TOAN SCANLINE
        /// <summary>
        /// Cấu trúc dữ liệu Active Edge List
        /// </summary>
        class AEL
        {
            /// <summary>
            /// Tọa độ y đỉnh cao của cạnh (đã được tinh chế)
            /// </summary>
            int yUpper;
            /// <summary>
            /// Tọa độ x của giao điểm đầu tiên của cạnh và dòng quét
            /// </summary>
            float xIntersect;
            /// <summary>
            /// Nghịch đảo hệ số góc (tính theo dữ liệu gốc)
            /// </summary>
            float reciSlope; //Reciprocal Slope
            public int GetYUpper()
            {
                return this.yUpper;
            }
            public float GetXInt()
            {
                return this.xIntersect;
            }
            public float GetReciSlope()
            {
                return this.reciSlope;
            }
            /// <summary>
            /// Hàm cập nhật lại giá trị của xIntersect sau khi thực hiện một dòng quét
            /// </summary>
            public void UpdateXInt()
            {
                this.xIntersect += this.reciSlope;
            }
            /// <summary>
            /// Hàm khởi tạo cấu trúc AEL
            /// </summary>
            /// <param name="y_upper">Tọa độ y đỉnh cao</param>
            /// <param name="x_int">Tọa độ x giao điểm đầu tiên</param>
            /// <param name="reci_slope">Nghịch đảo độ dốc</param>
            public AEL(int y_upper, float x_int, float reci_slope)
            {
                this.yUpper = y_upper;
                this.xIntersect = x_int;
                this.reciSlope = reci_slope;
            }
        }

        /// <summary>
        /// Class Edge Table phục vụ cho thuật toán tô màu Scanline
        /// </summary>
        class ET
        {
            Hashtable hTable = new Hashtable();
            /// <summary>
            /// Key nhỏ nhất mà có chứa ít nhất một cạnh trong linked list các cạnh
            /// </summary>
            int minKey = 99999; //Tạo số trước để tý so sánh
            /// <summary>
            /// Key lớn nhất mà có chứa ít nhất một cạnh trong linked list các cạnh
            /// </summary>
            int maxKey = -2; //Tạo số trước để tý so sánh
            /// <summary>
            /// Hàm kiểm tra giao điểm của 2 cạnh có phải cực trị
            /// </summary>
            /// <param name="p1">Điểm riêng cạnh 1</param>
            /// <param name="p2">Điểm chung 2 cạnh</param>
            /// <param name="p3">Điểm riêng cạnh 2</param>
            /// <returns></returns>
            private bool IsExtreme(Point p1, Point p2, Point p3)
            {
                if ((p2.Y >= p1.Y && p2.Y >= p3.Y) || (p2.Y <= p1.Y && p2.Y <= p3.Y))
                    return true;
                return false;
            }
            /// <summary>
            /// Hàm tính hệ số góc của một cạnh
            /// </summary>
            /// <param name="p1">Điểm đầu</param>
            /// <param name="p2">Điểm cuối</param>
            /// <returns></returns>
            private float CalcSlope(Point p1, Point p2)
            {
                if (p2.X == p1.X)
                {
                    return 0;
                }
                return ((float)(p2.Y - p1.Y) / (p2.X - p1.X));
            }
            /// <summary>
            /// Hàm lấy bảng băm trỏ đến các linkedlist chứa các cạnh
            /// </summary>
            /// <returns>Bảng băm (cũng chính là Edge Table)</returns>
            public Hashtable GetET()
            {
                return this.hTable;
            }
            /// <summary>
            /// Hàm lấy minKey
            /// </summary>
            /// <returns>minKey</returns>
            public int GetMinKey()
            {
                return this.minKey;
            }
            /// <summary>
            /// Hàm lấy maxKey
            /// </summary>
            /// <returns>maxKey</returns>
            public int GetMaxKey()
            {
                return this.maxKey;
            }
            /// <summary>
            /// Hàm duyệt qua các cạnh để lưu thông số (yUpper, xInt, reciSlope) vào bảng băm
            /// </summary>
            /// <param name="pList">Danh sách các đỉnh của hình</param>
            public void SetET(List<Point> pList)
            {
                for (int i = 0; i < pList.Count; i++)
                {
                    // Khởi tạo điểm trước, điểm hiện tại và điểm tiếp theo
                    Point prevPoint, curPoint, nextPoint;
                    //Set điểm trước
                    if (i == 0)
                    {
                        prevPoint = pList[pList.Count - 1];
                    }
                    else
                    {
                        prevPoint = pList[i - 1];
                    }
                    //Set điểm hiện tại
                    curPoint = pList[i];
                    //Set điểm tiếp theo
                    if (i == pList.Count - 1)
                    {
                        nextPoint = pList[0];
                    }
                    else
                    {
                        nextPoint = pList[i + 1];
                    }

                    //Set reciSlope để lưu vào ET
                    float slope = this.CalcSlope(curPoint, nextPoint);
                    float reci_slope;
                    if (slope == 0)
                    {
                        // Neu canh vuong goc Oy thi chay den vong lap tiep theo
                        if (curPoint.X != nextPoint.X)
                        {
                            continue;
                        }
                        else
                        {
                            reci_slope = 0;
                        }
                    }
                    else
                    {
                        reci_slope = 1 / slope;
                    }

                    //Set yUpper để lưu vào ET
                    int y_upper;

                    if (nextPoint.Y > curPoint.Y)
                    {
                        y_upper = nextPoint.Y;
                        Point nextNextPoint; // Điểm tiếp theo của điểm tiếp theo
                        if (i >= pList.Count - 2) // (i+2) la index cua diem lien sau nextPoint, neu no vuot ra khoi do dai danh sach
                        {
                            nextNextPoint = pList[i + 2 - pList.Count];// thi coi nhu no se tro ve dau danh sach
                        }
                        else
                        {
                            nextNextPoint = pList[i + 2];
                        }
                        //Làm ngắn cạnh không phải cực trị
                        if (this.IsExtreme(curPoint, nextPoint, nextNextPoint) == false)
                            y_upper--;
                    }
                    else //nextPoint.Y <= curPoint.Y
                    {
                        y_upper = curPoint.Y;
                        if (this.IsExtreme(prevPoint, curPoint, nextPoint) == false)// Kiem tra co phai la cuc tri
                            y_upper--;
                    }
                    if (y_upper - 1 > this.maxKey) // Neu thoa dieu kien nay thi cap nhat lai maxKey
                        maxKey = y_upper - 1;

                    //Set xIntersect để lưu vào ET
                    float x_int;
                    if (nextPoint.Y < curPoint.Y)// xet dinh dau va dinh cuoi cua canh, diem nao co y be hon thi x cua no se duoc chon lam x_intersect
                    {
                        x_int = nextPoint.X;
                    }
                    else //nextPoint.Y >= curPoint.Y
                    {
                        x_int = curPoint.X;
                    }

                    // Thực hiện lưu vào ET
                    AEL temp = new AEL(y_upper, x_int, reci_slope);
                    int y_lower;
                    if (nextPoint.Y < curPoint.Y)
                    {
                        y_lower = nextPoint.Y;
                    }
                    else
                    {
                        y_lower = curPoint.Y;
                    }
                    if (y_lower < this.minKey)
                    {
                        minKey = y_lower;
                    }

                    if (hTable.ContainsKey(y_lower))
                    {
                        ((LinkedList<AEL>)hTable[y_lower]).AddLast(temp);
                    }
                    else // Nếu chưa
                    {
                        LinkedList<AEL> eList = new LinkedList<AEL>();
                        eList.AddLast(temp);
                        hTable.Add(y_lower, eList);
                    }
                }
            }
        }

        /// <summary>
        /// Hàm thuật toán tô màu scanline color fill
        /// </summary>
        /// <param name="gl">OPENGL Controller</param>
        /// <param name="color">Màu tô đã chọn</param>
        /// <param name="pVertices">Danh sách các đỉnh của hình</param>
        /// <param name="oColor">Màu viền của hình</param>
        private void ScanLineColorFill(OpenGL gl, Color color, List<Point> pVertices, Color oColor)
        {
            //Tạo bảng băm lưu danh sách cấu trúc AEL
            ET edgeTable = new ET();
            edgeTable.SetET(pVertices);
            //Tạo hashtable temp để lưu bảng băm từ Edge Table ở trên
            Hashtable temp = new Hashtable();
            temp = edgeTable.GetET();
            //Tạo con trỏ kiểu cấu trúc AEL để trỏ vào danh sách các cạnh mà dòng quét đi qua
            LinkedList<AEL> begList = new LinkedList<AEL>();
            //Lần lướt gán biến start và biến end bằng MinKey và MaxKey của bảng băm
            int start = edgeTable.GetMinKey(), end = edgeTable.GetMaxKey();
            //Tạo vùng tô mới
            vungto vt = new vungto
            {
                cl = color
            };
            //Duyệt các dòng quét từ MIN_Y đến MAX_Y
            for (int y = start; y <= end; y++)
            {
                // Bước 1: Thêm cạnh ở vị trí y trong edge table vào danh sách begList (đóng vai trò là con trỏ)
                if (temp.ContainsKey(y))
                {
                    foreach (var e in (LinkedList<AEL>)temp[y])
                    {
                        begList.AddLast(e);
                    }
                }
                // Bước 2: Sắp xếp các cạnh theo thứ tự xIntersect tăng dần
                var sortedBegList = begList.OrderBy(AEL => AEL.GetXInt());

                // Bước 3: Lắp đầy các pixels giữa các cặp giao điểm lẻ-chẵn
                LinkedListNode<AEL> tempNode = begList.First;
                while (tempNode != null) //Duyệt đến cuối danh sách thì dừng
                {
                    var nextNode = tempNode.Next;
                    var nextNextNode = nextNode.Next;
                    Point p1 = new Point((int)tempNode.Value.GetXInt(), y);
                    Point p2 = new Point((int)nextNode.Value.GetXInt(), y);
                    int p_start, p_end;
                    //So sánh tọa độ X của 2 điểm liền kề
                    if (p1.X <= p2.X)
                    {
                        p_start = p1.X;
                        p_end = p2.X;
                    }
                    else //p1.X > p2.X
                    {
                        p_start = p2.X;
                        p_end = p1.X;
                    }
                    
                    for (int i = p_start; i <= p_end; i++)
                    {
                        //Kiểm tra nếu đụng biên thì chuyển sang không thêm vào vungto
                        Color pColor = GetColor(i, gl.RenderContextProvider.Height - y, gl);
                        if (pColor.R == oColor.R && pColor.G == oColor.G && pColor.B == oColor.B)
                        {
                            //do nothing
                        }
                        else
                        {
                            vt.p.Add(new Point(i, gl.RenderContextProvider.Height - y));
                            bitmap[gl.RenderContextProvider.Height - y][i][0] = color.R;
                            bitmap[gl.RenderContextProvider.Height - y][i][1] = color.G;
                            bitmap[gl.RenderContextProvider.Height - y][i][2] = color.B;
                            bitmap[gl.RenderContextProvider.Height - y][i][3] = 0;
                        }
                    }
                    tempNode = nextNextNode;// Bỏ qua cặp chẵn-lẻ
                }

                // Bước 4: Xóa các cạnh có yUppper = y
                var node = begList.First; // first element of begList
                while (node != null)
                {
                    var nextNode = node.Next;
                    if (node.Value.GetYUpper() == y)
                        begList.Remove(node);// remove nodes having yUpper = y
                    node = nextNode;
                }

                // Bước 5: Cập nhật xIntersect
                foreach (var e in begList)
                {
                    e.UpdateXInt();
                }
            }
            tatcavungto.Add(vt);
        }

        private void tomau2(Point p, Color color, Object[] arr, OpenGL gl, int outline)
        {
            List<Point> pList = new List<Point>();
            Color oColor = Color.White;
            for (int i = 0; i < numObj; i++)
            {
                if (arr[i].IsInside(p))
                {
                    pList = arr[i].GetVerticesForScanline();
                    oColor = arr[i].lineColor;
                    break;
                }
            }
            if (pList != null)
            {
                gl.ReadPixels(0, 0, gl.RenderContextProvider.Width
                    , gl.RenderContextProvider.Height, OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, pixel);
                int pixelindex = 0;
                for (int i = 0; i < bitmap.Count; i++)
                {
                    for (int j = 0; j < bitmap[i].Count; j++)
                    {
                        for (int z = 0; z < 4; z++)
                        {
                            bitmap[i][j][z] = pixel[pixelindex++];
                        }
                    }
                }
                ScanLineColorFill(gl, color, pList, oColor);
                pixelindex = 0;
                for (int i = 0; i < bitmap.Count; i++)
                {
                    for (int j = 0; j < bitmap[i].Count; j++)
                    {
                        for (int z = 0; z < 4; z++)
                        {
                            pixel[pixelindex++] = (byte)bitmap[i][j][z];
                        }
                    }
                }
                gl.DrawPixels(gl.RenderContextProvider.Width, gl.RenderContextProvider.Height, OpenGL.GL_UNSIGNED_BYTE, pixel);
            }
        }
        #endregion

        private void openGLControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {

            // Lấy object của OpenGL
            OpenGL gl = openGLControl.OpenGL;
            // Xoá màu và bộ nhớ
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //Tạo ra một thực thể hình mới
            switch (shShape)
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
            if (shShape < 7)
            {
                arrObj[numObj].setPoint(pStart, pEnd);
                arrObj[numObj].set(pStart, pEnd);
                arrObj[numObj].setLineColor(userLineColor);
            }

            if (numObj > 0)
                arrObj[numObj - 1].setLineSize(userLineSize);
            else arrObj[numObj].setLineSize(userLineSize);

            //Vẽ lại tất cả các hình
            for (int i = 0; i <= numObj; i++)
            {
                arrObj[i].drawObject(gl);
                //arrObj[i].colorObject(gl);
            }

            if (shShape == 7)
            {
                if (toadomau.X != -1 && toadomau.Y != -1)
                {
                    toadomau.Y = gl.RenderContextProvider.Height - toadomau.Y;
                    gl.PointSize(1);
                    gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
                    tomau(toadomau, userBgColor, arrObj, gl);
                    //tomau2(toadomau, userBgColor, arrObj, gl);
                }
                toadomau.X = -1;
                toadomau.Y = -1;
            }

            if (shShape == 10)
            {
                if (toadomau.X != -1 && toadomau.Y != -1)
                {
                    tomau2(toadomau, userBgColor, arrObj, gl, userLineSize);
                }
                toadomau.X = -1;
                toadomau.Y = -1;
            }
            gl.PointSize(1);
            gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
            foreach (var vt in tatcavungto)
            {
                gl.Color(vt.cl.R / 255.0, vt.cl.G / 255.0, vt.cl.B / 255.0, 0);
                gl.Begin(OpenGL.GL_POINTS);
                foreach (var p in vt.p)
                {
                    gl.Vertex(p.X, p.Y);
                }
                gl.End();
                gl.Flush();
            }
            if (shShape == 8)
            {
                int flag = 0;
                Point ptemp = pStart;
                int index = 0;
                int indexj = 0;
                if (ptemp.X != 0 && ptemp.Y != 0)
                {
                    for (int i = 0; i < numObj; i++)
                    {

                        List<Point> p = arrObj[i].GetVertices();
                        for (int j = 0; j < p.Count; j++)
                        {

                            if (Math.Sqrt((pStart.X - p[j].X) * (pStart.X - p[j].X) + (pStart.Y - (p[j].Y)) * (pStart.Y - (p[j].Y))) <= epsilon)
                            {
                                indexnum = i;
                                ptemp = p[i];
                                flag = 1;
                                index = i;
                                indexj = j;
                                break;

                            }
                        }
                        if (flag == 1) break;
                    }
                    if (flag == 1)
                    {
                        AffineTransform t = new AffineTransform();
                        t.CoGian(arrObj[index], arrObj[index].GetVertices()[indexj], pEnd);
                    }
                }
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
            height = gl.RenderContextProvider.Height;
            width = gl.RenderContextProvider.Width;
            pixel = new byte[4 * (gl.RenderContextProvider.Width) * (gl.RenderContextProvider.Height)];
            //Xóa màn hình, trả chế độ và load view
            gl.ClearColor(0, 0, 0, 0);
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            int h = gl.RenderContextProvider.Height;
            bitmap = new List<List<List<int>>>();
            for (int i = 0; i < h; i++)
            {
                List<List<int>> temp = new List<List<int>>();
                for (int j = 0; j < gl.RenderContextProvider.Width; j++)
                {
                    List<int> temp1 = new List<int>();
                    for (int z = 0; z < 4; z++)
                    {
                        int u = 0;
                        temp1.Add(u);

                    }
                    temp.Add(temp1);
                }
                bitmap.Add(temp);
            }
            /*   for(int i = 0; i < bitmap.Count; i++)
               {
                   for(int j = 0; j < bitmap[i].Count; j++)
                   {
                       bitmap[i][j] = new List<int>(3);
                   }
               }*/

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

        private void Keo_Gian_MouseDown(object sender, MouseEventArgs e)
        {
            shShape = 8;

        }

        private void bt_select_Click(object sender, EventArgs e)
        {
            shShape = 9;
        }

        private void bt_scanline_Click(object sender, EventArgs e)
        {
            shShape = 10;
        }

        //Khi di chuyển chuột trên màn hình
        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                pEnd = e.Location;
                pkeogian = e.Location;
                idViewPoint = numObj;
            }
        }

        //Khi thả chuột ra
        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (shShape < 7)
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
            if (shShape == 8)
            {
                arrObj[indexnum].updateP();
            }
        }

        //Khi nhấp chuột vào màn hình
        private void openGLControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (shShape == 7 || shShape == 10)
            {
                toadomau = pStart;
            }
            if (shShape == 9)
                for (int i = numObj - 1; i >= 0; i--)
                {//Kiểm tra điểm được chọn có nằm trong hình đã vẽ không
                    if (arrObj[i].IsInside(e.Location))
                    {
                        idViewPoint = i;
                        break;
                    }
                    if (i == 0) idViewPoint = -1; //Nếu không thì coi như không hiện viewPoint lên
                }
        }
    }
}