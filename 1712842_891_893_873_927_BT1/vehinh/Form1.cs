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

namespace vehinh
{
    
    struct filled_area
    {
        public Color color;
        public List<Point> t;
    }
    public partial class Form1 : Form
    {
        Color color_user_color;//mau ve hinh
        int sh_shape;//che do ve, 0 = Line,1 = Circle, 2 = Hinh Chu nhat, 3 = Ellipse, 4 = Tam Giac Deu, 5 = Ngu Giac Deu, 6 = Luc Giac Deu
        Point pStart;
        Point pEnd;
        Point clicked;
        Color fill_color;
        int flag;
        int smallLineFlag;
        int mediumLineFlag;
        int bigLineFlag;
        //List<Point> RasterPoint;
        List<filled_area> filled;
        List<Shape> Hinh;
        public Form1()
        {
            InitializeComponent();
            color_user_color = Color.Black;//gan gia tri ban dau
            sh_shape = 0;
            flag = 1;
            fill_color = Color.White;
            filled = new List<filled_area>();
            Hinh = new List<Shape>();
            smallLineFlag = 1;
            mediumLineFlag = bigLineFlag = 0;

        }

        private void btBangMau_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                if (sh_shape != 7)
                    color_user_color = colorDialog1.Color;
                else
                {
                    fill_color = colorDialog1.Color;
                }
            }
            
        }

        private void openGLControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {

            OpenGL gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            if(smallLineFlag == 1)
            {
                gl.PointSize(1);
                gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
            }
            else if (mediumLineFlag == 1)
            {
                gl.PointSize(5);
                gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
            }
            else if(bigLineFlag == 1)
            {
                gl.PointSize(10);
                gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
            }
            
            for (int i = 0; i < Hinh.Count;i++)
            {
                gl.Begin(OpenGL.GL_POINTS);
                for (int j = 0; j < Hinh[i].Points_Size(); j++)
                {
                    gl.Vertex(Hinh[i].Point_at(j).X, gl.RenderContextProvider.Height - Hinh[i].Point_at(j).Y);
                }
                gl.End();
                gl.Flush();
            }


            for (int i = 0; i < filled.Count; i++)
            {
                gl.Color(filled[i].color.R / 255.0, filled[i].color.G / 255.0, filled[i].color.B / 255.0, 0);
                gl.Begin(OpenGL.GL_POINTS);
                for (int j = 0; j < filled[i].t.Count; j++)
                {
                    gl.Vertex(filled[i].t[j].X, gl.RenderContextProvider.Height - filled[i].t[j].Y);
                }
                gl.End();
                gl.Flush();
            }

            gl.Color(color_user_color.R/255.0, color_user_color.G / 255.0, color_user_color.B / 255.0,0); // Chọn màu đỏ
            if (sh_shape == 0)
            {
                veDuongThang(pStart, pEnd);
                //ve duong thang
            }
            else if (sh_shape == 1)
            {
                veHinhTron(pStart, pEnd);
                // ve duong tron
            }
            else if (sh_shape == 2)
            {
                veHinhChuNhat(pStart, pEnd);
                //chu nhat
            }
            else if (sh_shape == 3)
            {
                veHinhEllipse(pStart, pEnd);
                //ve ellipse
            }
            else if (sh_shape == 4)
            {
                veTamGiacDeu(pStart, pEnd);
                //ve tam giac deu
            }
            else if (sh_shape == 5)
            {
                //ve ngu giac
                veNguGiacDeu(pStart, pEnd);
            }
            else if (sh_shape == 6)
            {
                //ve 6 giac
                veLucGiacDeu(pStart, pEnd);
            }
            else if (sh_shape == 7)
            {
                if (clicked.X != -1 && clicked.Y != -1)
                    to_mau_fill(clicked, fill_color);
                clicked.X = -1;
                clicked.Y = -1;

            }
        }

        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            clicked.X = -1;
            clicked.Y = -1;
            // Set the clear color.
            gl.ClearColor(1, 1, 1, 0);
            // Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            // Load the identity.
            gl.LoadIdentity();
        }

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
        
        private void veDuongThang(Point pStart, Point pEnd)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Tính các thông số cơ bản
            //int x0 = pStart.X;
            //int y0 = pStart.Y;
            int x0, y0;
            x0 = y0 = 0;
            int delta_X = Math.Abs(pEnd.X - pStart.X);
            int delta_Y = Math.Abs(pEnd.Y - pStart.Y);
            int x2delta_X = 2 * delta_X;
            int x2delta_Y = 2 * delta_Y;

            //Giải thuật và vẽ
            OpenGL gl = openGLControl.OpenGL;
            gl.Begin(OpenGL.GL_POINTS);
            gl.Vertex(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
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
                        gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                    if (pStart.X > pEnd.X && pStart.Y <= pEnd.Y)
                        gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0)); //2
                    if (pStart.X > pEnd.X && pStart.Y > pEnd.Y)
                        gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0)); //3
                    if (pStart.X < pEnd.X && pStart.Y > pEnd.Y)
                        gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0)); //4

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
                        gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                    if (pStart.X > pEnd.X && pStart.Y < pEnd.Y)
                        gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0)); //2
                    if (pStart.X > pEnd.X && pStart.Y > pEnd.Y)
                        gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0)); //3
                    if (pStart.X <= pEnd.X && pStart.Y > pEnd.Y)
                        gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0)); //4

                    k++;
                }
            }

            gl.End();
            gl.Flush();

            sw.Stop();
            textBox.Text = sw.Elapsed.ToString();
        }
        private void veTamGiacDeu(Point pStart,Point pEnd)
        {
            Point test=pStart;
            Point test1 = pEnd;
            test = pEnd;
            test.Y = pStart.Y;
            veDuongThang(pStart, test);
            double delta = test.X - pStart.X;
            test.X = pStart.X + (int)Math.Round(delta / 2);
            test.Y = (int)Math.Round(pStart.Y + delta * Math.Sqrt(3) / 2);
            test1.Y = pStart.Y;
            veDuongThang(pStart, test);
            veDuongThang(test, test1);

        }
        private void veHinhTron(Point pStart,Point pEnd)
        {
            //Ham do thoi gian ve
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //GEt the OpenGL object
            OpenGL gl = openGLControl.OpenGL;
            gl.Begin(OpenGL.GL_POINTS);

            double radius = Math.Sqrt((pStart.X - pEnd.X) * (pStart.X - pEnd.X) + (pStart.Y - pEnd.Y) * (pStart.Y - pEnd.Y));
            //ve cac diem (0, r), (0, -r), (r, 0), (-r, 0)
            gl.Vertex(pStart.X, Math.Round(gl.RenderContextProvider.Height - Math.Round(pStart.Y + radius)));
            gl.Vertex(pStart.X, gl.RenderContextProvider.Height - Math.Round(pStart.Y - radius));
            gl.Vertex(pStart.X + Math.Round(radius), gl.RenderContextProvider.Height - pStart.Y);
            gl.Vertex(pStart.X - Math.Round(radius), gl.RenderContextProvider.Height - pStart.Y);

            //Diem bat dau (x0, y0) = (0, r)
            double x0 = 0;
            double y0 = Math.Round(radius);
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
                //Ve cac diem y = 0, x = 0, y = x, y = -x
                gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                gl.Vertex(pStart.X + y0, gl.RenderContextProvider.Height - (pStart.Y + x0));
                gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
                gl.Vertex(pStart.X + y0, gl.RenderContextProvider.Height - (pStart.Y - x0));

                gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                gl.Vertex(pStart.X - y0, gl.RenderContextProvider.Height - (pStart.Y + x0));
                gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
                gl.Vertex(pStart.X - y0, gl.RenderContextProvider.Height - (pStart.Y - x0));
                k = k + 1;
            }

            gl.End();
            gl.Flush();

            //Dung do thoi gian
            sw.Stop();
            textBox.Text = sw.Elapsed.ToString();
        }

        private void veHinhEllipse(Point pStart, Point pEnd)
        {
            //Ham do thoi gian ve
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //GEt the OpenGL object
            OpenGL gl = openGLControl.OpenGL;
            gl.Begin(OpenGL.GL_POINTS);

            double rx = Math.Abs(pEnd.X - pStart.X) / 2.0;
            double ry = Math.Abs(pEnd.Y - pStart.Y) / 2.0;
            double x0 = 0, y0 = ry;

            double p1 = (ry * ry) - (rx * rx * ry) + (0.25 * rx * rx);
            double dx = 2 * ry * ry * x0;
            double dy = 2 * rx * rx * y0;

            //ve cac diem (0, ry), (0, -ry), (rx, 0), (-rx, 0)
            gl.Vertex(pStart.X, gl.RenderContextProvider.Height - Math.Round(pStart.Y + ry));
            gl.Vertex(pStart.X, gl.RenderContextProvider.Height - Math.Round(pStart.Y - ry));
            gl.Vertex(pStart.X + rx, gl.RenderContextProvider.Height - pStart.Y);
            gl.Vertex(pStart.X - rx, gl.RenderContextProvider.Height - pStart.Y);

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
                gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
                gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
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
                gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
                gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
            }
            gl.End();
            gl.Flush();

            //Dung do thoi gian
            sw.Stop();
            textBox.Text = sw.Elapsed.ToString();
        }

        private void btLine_Click(object sender, EventArgs e)
        {
            sh_shape = 0;
        }

        private void btCircle_Click(object sender, EventArgs e)
        {
            sh_shape = 1;
            
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            pStart = e.Location;
            pEnd = pStart;
            if (sh_shape == 7)
            {
                clicked = pStart;
            }
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            pEnd = e.Location;
            if (sh_shape == 0)
            {
                Shape temp = new DuongThang(pStart,pEnd);
                temp.LuuHinh(pStart,pEnd);
                Hinh.Add(temp);
                //ve duong thang
            }
            else if (sh_shape == 1)
            {
                Shape temp = new HinhTron(pStart, pEnd);
                temp.LuuHinh(pStart, pEnd);
                Hinh.Add(temp);
                // ve duong tron
            }
            else if (sh_shape == 2)
            {
                Shape temp = new HinhChuNhat(pStart, pEnd);
                temp.LuuHinh(pStart, pEnd);
                Hinh.Add(temp);
                //chu nhat
            }
            else if (sh_shape == 3)
            {
                Shape temp = new Ellipse(pStart, pEnd);
                temp.LuuHinh(pStart, pEnd);
                Hinh.Add(temp);
                //ve ellipse
            }
            else if (sh_shape == 4)
            {
                Shape temp = new TamGiacDeu(pStart, pEnd);
                temp.LuuHinh(pStart, pEnd);
                Hinh.Add(temp);
                //ve tam giac deu
            }
            else if (sh_shape == 5)
            {
                //ve ngu giac
                Shape temp = new NguGiacDeu(pStart, pEnd);
                temp.LuuHinh(pStart, pEnd);
                Hinh.Add(temp);
            }
            else if (sh_shape == 6)
            {
                //ve 6 giac
                Shape temp = new LucGiacDeu(pStart, pEnd);
                temp.LuuHinh(pStart, pEnd);
                Hinh.Add(temp);
                // veLucGiacDeu(pStart, pEnd);
            }
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            pEnd = e.Location;
        }

        private void bt_hinh_chu_nhat_Click(object sender, EventArgs e)
        {

            sh_shape = 2;
        }

        private void btEllipse_Click(object sender, EventArgs e)
        {
            sh_shape = 3;
        }

        private void bt_tam_giac_deu_Click(object sender, EventArgs e)
        {
            sh_shape = 4;
        }

        private void bt_ngu_giac_deu_Click(object sender, EventArgs e)
        {
            sh_shape = 5;
        }

        private void bt_luc_giac_deu_Click(object sender, EventArgs e)
        {
            sh_shape = 6;
        }

        private void bt_fill_MouseClick(object sender, MouseEventArgs e)
        {
            sh_shape = 7;
        }

        private void to_mau_fill(Point Start, Color color)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            filled_area area;
            area.color = color;
            area.t = new List<Point>();
            OpenGL gl = openGLControl.OpenGL;
            gl.Color(color.R / 255.0, color.G / 255.0, color.B / 255.0, 0);
            List<Point> searched = new List<Point>();
            Queue<Point> frontier = new Queue<Point>();
            frontier.Enqueue(Start);
            Point temp = Start;
            gl.Begin(OpenGL.GL_POINTS);
            while (frontier.Count > 0)
            {
                temp = frontier.Dequeue();

                gl.Vertex(temp.X, gl.RenderContextProvider.Height - temp.Y);
                area.t.Add(temp);

                if (!inList(temp, searched)) searched.Add(temp);
                //phat sinh con 4 huong
                Point temp1 = temp;
                temp1.X = temp.X + 1;
                Point temp2 = temp;
                temp2.Y = temp.Y + 1;
                Point temp3 = temp;
                temp3.Y = temp.Y - 1;
                Point temp4 = temp;
                temp4.X = temp.X - 1;
                int flag1 = 0;
                int flag2 = 0;
                int flag3 = 0;
                int flag4 = 0;// !inList(temp1, RasterPoint),frontier.Enqueue(temp4);
                if ((temp1.X >= 0 && temp1.X <= gl.RenderContextProvider.Width) && (temp1.Y >= 0 && temp1.Y <= gl.RenderContextProvider.Height && !inList(temp1, searched)) && !frontier.Contains(temp1)) flag1 = 1;
                if ((temp2.X >= 0 && temp2.X <= gl.RenderContextProvider.Width) && (temp2.Y >= 0 && temp2.Y <= gl.RenderContextProvider.Height && !inList(temp2, searched)) && !frontier.Contains(temp2)) flag2 = 1;
                if ((temp3.X >= 0 && temp3.X <= gl.RenderContextProvider.Width) && (temp3.Y >= 0 && temp3.Y <= gl.RenderContextProvider.Height && !inList(temp3, searched)) && !frontier.Contains(temp3)) flag3 = 1;
                if ((temp4.X >= 0 && temp4.X <= gl.RenderContextProvider.Width) && (temp4.Y >= 0 && temp4.Y <= gl.RenderContextProvider.Height && !inList(temp4, searched)) && !frontier.Contains(temp4)) flag4 = 1;
                for(int i = 0; i < Hinh.Count; i++)
                {
                    for(int j = 0; j < Hinh[i].Points_Size(); j++)
                    {
                        if (inList(temp1, Hinh[i].getPoinsList()))
                        {
                            flag1 = 0;
                            break;
                        }
                    }
                    if (flag1 == 0) break;
                }
                if(flag1==1) frontier.Enqueue(temp1);
                for (int i = 0; i < Hinh.Count; i++)
                {
                    for (int j = 0; j < Hinh[i].Points_Size(); j++)
                    {
                        if (inList(temp2, Hinh[i].getPoinsList()))
                        {
                            flag2 = 0;
                            break;
                        }
                    }
                    if (flag2 == 0) break;
                }
                if (flag2 == 1) frontier.Enqueue(temp2);

                for (int i = 0; i < Hinh.Count; i++)
                {
                    for (int j = 0; j < Hinh[i].Points_Size(); j++)
                    {
                        if (inList(temp3, Hinh[i].getPoinsList()))
                        {
                            flag3 = 0;
                            break;
                        }
                    }
                    if (flag3 == 0) break;
                }
                if (flag3 == 1) frontier.Enqueue(temp3);

                for (int i = 0; i < Hinh.Count; i++)
                {
                    for (int j = 0; j < Hinh[i].Points_Size(); j++)
                    {
                        if (inList(temp4, Hinh[i].getPoinsList()))
                        {
                            flag4 = 0;
                            break;
                        }
                    }
                    if (flag4 == 0) break;
                }
                if (flag4 == 1) frontier.Enqueue(temp4);
            }
            gl.End();
            gl.Flush();
            sw.Stop();
            textBox.Text = sw.Elapsed.ToString();
            filled.Add(area);
        }
        private bool inList(Point t, List<Point> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].X == t.X && l[i].Y == t.Y) return true;
            }
            return false;
        }

        private void veHinhChuNhat(Point pStart, Point pEnd)
        {
            Point temp1 = pStart;
            temp1.Y = pEnd.Y;
            Point temp3 = pEnd;
            Point temp4 = pEnd;
            temp4.Y = pStart.Y;
            veDuongThang(pStart, temp1);
            veDuongThang(pStart, temp4);
            veDuongThang(temp4, temp3);
            veDuongThang(temp3, temp1);
        }

        private void veNguGiacDeu(Point pStart,Point pEnd)
        {
            Point tempEnd = pEnd;
            if (tempEnd.X >= pStart.X)
            {
                tempEnd.Y = pStart.Y;
                double a = Math.Abs(tempEnd.X - pStart.X);
                Point temp1 = pStart;
                double goc1 = 72 * Math.PI / 180.0;//goc 72 rad
                temp1.X = (int)Math.Round(pStart.X - a * Math.Cos(goc1));
                temp1.Y = (int)Math.Round(pStart.Y + a * Math.Sin(goc1));
                Point temp2 = pStart;
                temp2.X = (pStart.X + tempEnd.X) / 2;
                double goc2 = 54 * Math.PI / 180.0;//goc 72 rad
                temp2.Y = (int)Math.Round(pStart.Y + a * Math.Tan(goc2) / 2 + a / (2 * Math.Cos(goc2)));
                Point temp3 = tempEnd;
                temp3.X = (int)Math.Round(tempEnd.X + a * Math.Cos(goc1));
                temp3.Y = temp1.Y;
                veDuongThang(pStart, temp1);
                veDuongThang(temp1, temp2);
                veDuongThang(temp2, temp3);
                veDuongThang(temp3, tempEnd);
                veDuongThang(pStart, tempEnd);
            }
            else
            {
                tempEnd.Y = pStart.Y;
                double a = Math.Abs(tempEnd.X - pStart.X);
                Point temp1 = pStart;
                double goc1 = 72 * Math.PI / 180.0;//goc 72 rad
                temp1.X = (int)Math.Round(pStart.X + a * Math.Cos(goc1));
                temp1.Y = (int)Math.Round(pStart.Y + a * Math.Sin(goc1));
                Point temp2 = pStart;
                temp2.X = (pStart.X + tempEnd.X) / 2;
                double goc2 = 54 * Math.PI / 180.0;//goc 72 rad
                temp2.Y = (int)Math.Round(pStart.Y + a * Math.Tan(goc2) / 2 + a / (2 * Math.Cos(goc2)));
                Point temp3 = tempEnd;
                temp3.X = (int)Math.Round(tempEnd.X - a * Math.Cos(goc1));
                temp3.Y = temp1.Y;
                veDuongThang(pStart, temp1);
                veDuongThang(temp1, temp2);
                veDuongThang(temp2, temp3);
                veDuongThang(temp3, tempEnd);
                veDuongThang(pStart, tempEnd);



            }





        }

        private void veLucGiacDeu(Point point,Point pEnd)
        {
            Point tempEnd = pEnd;
            if (tempEnd.X >= pStart.X)
            {
                tempEnd.Y = pStart.Y;
                double a = Math.Abs(tempEnd.X - pStart.X);
                Point temp1 = pStart;
                double goc1 = 60 * Math.PI / 180.0;
                temp1.X = (int)Math.Round(pStart.X - a * Math.Cos(goc1));
                temp1.Y = (int)Math.Round(pStart.Y + a * Math.Sin(goc1));
                Point temp2 = pStart;
                temp2.Y = (int)Math.Round(pStart.Y + a * Math.Tan(goc1));
                Point temp3 = tempEnd;
                temp3.Y = temp2.Y;
                Point temp4 = tempEnd;
                temp4.X = (int)Math.Round(tempEnd.X + a * Math.Cos(goc1));
                temp4.Y = temp1.Y;
                veDuongThang(pStart, temp1);
                veDuongThang(temp1, temp2);
                veDuongThang(temp2, temp3);
                veDuongThang(temp3, temp4);
                veDuongThang(temp4, tempEnd);
                veDuongThang(pStart, tempEnd);

            }
            else
            {
                tempEnd.Y = pStart.Y;
                double a = Math.Abs(tempEnd.X - pStart.X);
                Point temp1 = pStart;
                double goc1 = 60 * Math.PI / 180.0;
                temp1.X = (int)Math.Round(pStart.X + a * Math.Cos(goc1));
                temp1.Y = (int)Math.Round(pStart.Y + a * Math.Sin(goc1));
                Point temp2 = pStart;
                temp2.Y = (int)Math.Round(pStart.Y + a * Math.Tan(goc1));
                Point temp3 = tempEnd;
                temp3.Y = temp2.Y;
                Point temp4 = tempEnd;
                temp4.X = (int)Math.Round(tempEnd.X - a * Math.Cos(goc1));
                temp4.Y = temp1.Y;
                veDuongThang(pStart, temp1);
                veDuongThang(temp1, temp2);
                veDuongThang(temp2, temp3);
                veDuongThang(temp3, temp4);
                veDuongThang(temp4, tempEnd);
                veDuongThang(pStart, tempEnd);
            }


            }
        private void bt_DoDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Begin(OpenGL.GL_POINTS);

            //GLfloat lineWidthRange[2] = { 0.0f, 0.0f };
            //glGetFloatv(GL_ALIASED_LINE_WIDTH_RANGE, lineWidthRange);

            if ((string)bt_DoDay.SelectedItem == "Small")
            {
                smallLineFlag = 1;
                bigLineFlag = mediumLineFlag = 0;
                /*gl.PointSize(1);
                gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);*/
            }
            else if ((string)bt_DoDay.SelectedItem == "Medium")
            {
                smallLineFlag = bigLineFlag = 0;
                mediumLineFlag = 1;
                //gl.PointSize(5);
                //gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
            }
            else if ((string)bt_DoDay.SelectedItem == "Big")
            {
                smallLineFlag = mediumLineFlag = 0;
                bigLineFlag = 1;
                //gl.PointSize(10);
                //gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
            }
            gl.End();
            gl.Flush();
        }
    }
}