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
        List<Point> RasterPoint;
        List<filled_area> filled;
        public Form1()
        {
            InitializeComponent();
            color_user_color = Color.Black;//gan gia tri ban dau
            sh_shape = 0;
            flag = 1;
            fill_color = Color.White;
            filled = new List<filled_area>();
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

            gl.Begin(OpenGL.GL_POINTS);
            for (int i = 0; i < RasterPoint.Count; i++)
            {
                gl.Vertex(RasterPoint[i].X, gl.RenderContextProvider.Height - RasterPoint[i].Y);
            }
            gl.End();
            gl.Flush();

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
                //chu nhat
            }
            else if (sh_shape == 3)
            {
                veHinhEllipse(pStart, pEnd);
                //ve ellipse
            }
            else if (sh_shape == 4)
            {
                //ve tam giac deu
            }
            else if (sh_shape == 5)
            {
                //ve ngu giac
            }
            else if (sh_shape == 6)
            {
                //ve 6 giac
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
            RasterPoint = new List<Point>();
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

        /// <summary>
        /// Hàm vẽ đường thẳng
        /// </summary>
        /// <param name="pStart">Điểm bắt đầu</param>
        /// <param name="pEnd">Điểm kết thúc</param>
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

                    if (pStart.X < pEnd.X && pStart.Y < pEnd.Y)
                        gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                    if (pStart.X > pEnd.X && pStart.Y < pEnd.Y)
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
                    if (pStart.X < pEnd.X && pStart.Y < pEnd.Y)
                        gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
                    if (pStart.X > pEnd.X && pStart.Y < pEnd.Y)
                        gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0)); //2
                    if (pStart.X > pEnd.X && pStart.Y > pEnd.Y)
                        gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0)); //3
                    if (pStart.X < pEnd.X && pStart.Y > pEnd.Y)
                        gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0)); //4

                    k++;
                }
            }

            gl.End();
            gl.Flush();

            sw.Stop();
            textBox.Text = sw.Elapsed.ToString();
        }

        /// <summary>
        /// Hàm vẽ hình tròn
        /// </summary>
        /// <param name="pStart">Điểm bắt đầu</param>
        /// <param name="pEnd">Điểm kết thúc</param>
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
        //{ //nen ve bang mid point
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    // double radius = Math.Sqrt((pStart.X - pEnd.X) * (pStart.X - pEnd.X) + (pStart.Y - pEnd.Y) * (pStart.Y - pEnd.Y));
        //    // double k = 1 / radius;
        //    //double alpha = 0;
        //    //OpenGL gl = openGLControl.OpenGL;
        //    //gl.Begin(OpenGL.GL_POINTS);
        //    //while (alpha < Math.PI * 2)
        //    //{
        //    //    gl.Vertex(pStart.X + Math.Round(radius*Math.Cos(alpha)), gl.RenderContextProvider.Height - pStart.Y + Math.Round(radius * Math.Sin(alpha)));
        //    //    alpha += k;
        //    //}
        //    //gl.End();
        //    //gl.Flush();
        //    double radius = Math.Sqrt((pStart.X - pEnd.X) * (pStart.X - pEnd.X) + (pStart.Y - pEnd.Y) * (pStart.Y - pEnd.Y));
        //    OpenGL gl = openGLControl.OpenGL;
        //    gl.Begin(OpenGL.GL_POINTS);
        //    gl.Vertex(pStart.X, gl.RenderContextProvider.Height - Math.Round(pStart.Y + radius));
        //    gl.Vertex(pStart.X, gl.RenderContextProvider.Height - Math.Round(pStart.Y - radius));
        //    gl.Vertex(pStart.X + Math.Round(radius), gl.RenderContextProvider.Height - pStart.Y);
        //    gl.Vertex(pStart.X - Math.Round(radius), gl.RenderContextProvider.Height - pStart.Y);
        //    //ve tu tam roi tinh tien do do phuong trinh se la x^2 + y^2 = r
        //    int x0 = 0;
        //    int y0 = (int)Math.Round(radius);
        //    double po = 5 / 4.0 - radius;
        //    //int k = 0;
        //    while (x0 < y0)
        //    {
        //        if (po < 0)
        //        {
        //            x0 = x0 + 1;
        //            po = po + 2 * x0 + 1;
        //        }
        //        else
        //        {
        //            x0 = x0 + 1;
        //            y0 = y0 - 1;
        //            po = po + 2 * x0 - 2 * y0 + 1;
        //        }
        //        gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
        //        gl.Vertex(pStart.X + y0, gl.RenderContextProvider.Height - (pStart.Y + x0));
        //        gl.Vertex(pStart.X + x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
        //        gl.Vertex(pStart.X + y0, gl.RenderContextProvider.Height - (pStart.Y - x0));

        //        gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y + y0));
        //        gl.Vertex(pStart.X - y0, gl.RenderContextProvider.Height - (pStart.Y + x0));
        //        gl.Vertex(pStart.X - x0, gl.RenderContextProvider.Height - (pStart.Y - y0));
        //        gl.Vertex(pStart.X - y0, gl.RenderContextProvider.Height - (pStart.Y - x0));
        //       // k = k + 1;

        //    }

        //    gl.End();
        //    gl.Flush();

        //    sw.Stop();
        //    textBox.Text = sw.Elapsed.ToString();

        //}

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
            if (sh_shape == 1)
            {
                double radius = Math.Sqrt((pStart.X - pEnd.X) * (pStart.X - pEnd.X) + (pStart.Y - pEnd.Y) * (pStart.Y - pEnd.Y));
                OpenGL gl = openGLControl.OpenGL;
                Point temp = pStart;
                temp.X = pStart.X; temp.Y = (int)(Math.Round(pStart.Y + radius));
                RasterPoint.Add(temp);
                temp.X = pStart.X; temp.Y = (int)(Math.Round(pStart.Y - radius));
                RasterPoint.Add(temp);
                temp.X = (int)(pStart.X + Math.Round(radius)); temp.Y = pStart.Y;
                RasterPoint.Add(temp);
                temp.X = (int)(pStart.X - Math.Round(radius)); temp.Y = pStart.Y;
                RasterPoint.Add(temp);
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
                    RasterPoint.Add(temp);
                    temp.X = pStart.X + y0; temp.Y = (pStart.Y + x0);
                    RasterPoint.Add(temp);
                    temp.X = pStart.X + x0; temp.Y = (pStart.Y - y0);
                    RasterPoint.Add(temp);
                    temp.X = pStart.X + y0; temp.Y = (pStart.Y - x0);
                    RasterPoint.Add(temp);
                    temp.X = pStart.X - x0; temp.Y = (pStart.Y + y0);
                    RasterPoint.Add(temp);
                    temp.X = pStart.X - y0; temp.Y = (pStart.Y + x0);
                    RasterPoint.Add(temp);
                    temp.X = pStart.X - x0; temp.Y = (pStart.Y - y0);
                    RasterPoint.Add(temp);
                    temp.X = pStart.X - y0; temp.Y = (pStart.Y - x0);
                    RasterPoint.Add(temp);
                    k = k + 1;

                }
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
            //    Stopwatch sw = new Stopwatch();
            //    sw.Start();
            //    filled_area area;
            //    area.color = color;
            //    area.t = new List<Point>();
            //    OpenGL gl = openGLControl.OpenGL;
            //    gl.Color(color.R / 255.0, color.G / 255.0, color.B / 255.0, 0);
            //    List<Point> searched = new List<Point>();
            //    Queue<Point> frontier= new Queue<Point>();
            //    frontier.Enqueue(Start);
            //    Point temp=Start;
            //    gl.Begin(OpenGL.GL_POINTS);
            //    while (frontier.Count > 0)
            //    {
            //        temp = frontier.Dequeue();

            //        gl.Vertex(temp.X, gl.RenderContextProvider.Height - temp.Y);
            //        area.t.Add(temp);

            //        if (!inList(temp, searched)) searched.Add(temp);
            //        //phat sinh con 4 huong
            //        Point temp1 = temp;
            //        temp1.X = temp.X + 1;
            //        Point temp2 = temp;
            //        temp2.Y = temp.Y + 1;
            //        Point temp3 = temp;
            //        temp3.Y = temp.Y - 1;
            //        Point temp4 = temp;
            //        temp4.X = temp.X - 1;
            //        if ((temp1.X >= 0 && temp1.X <= gl.RenderContextProvider.Width) && (temp1.Y >= 0 && temp1.Y <= gl.RenderContextProvider.Height && !inList(temp1,searched)) && !inList(temp1,RasterPoint) && !frontier.Contains(temp1)) frontier.Enqueue(temp1);
            //        if ((temp2.X >= 0 && temp2.X <= gl.RenderContextProvider.Width) && (temp2.Y >= 0 && temp2.Y <= gl.RenderContextProvider.Height && !inList(temp2, searched)) && !inList(temp2, RasterPoint)&& !frontier.Contains(temp2)) frontier.Enqueue(temp2);
            //        if ((temp3.X >= 0 && temp3.X <= gl.RenderContextProvider.Width) && (temp3.Y >= 0 && temp3.Y <= gl.RenderContextProvider.Height && !inList(temp3, searched)) && !inList(temp3, RasterPoint) && !frontier.Contains(temp3)) frontier.Enqueue(temp3);
            //        if ((temp4.X >= 0 && temp4.X <= gl.RenderContextProvider.Width) && (temp4.Y >= 0 && temp4.Y <= gl.RenderContextProvider.Height && !inList(temp4, searched)) && !inList(temp4, RasterPoint)&& !frontier.Contains(temp4)) frontier.Enqueue(temp4);

            //    }
            //    gl.End();
            //    gl.Flush();
            //    sw.Stop();
            //    textBox.Text = sw.Elapsed.ToString();
            //    filled.Add(area);
            //}
            //private bool inList(Point t,List<Point> l)
            //{
            //    for(int i = 0; i < l.Count; i++)
            //    {
            //        if(l[i].X == t.X && l[i].Y == t.Y)return true;
            //    }
            //    return false;
            //}

            //private void bt_DoDay_SelectedIndexChanged(object sender, EventArgs e)
            //{
            //    OpenGL gl = openGLControl.OpenGL;
            //    gl.Begin(OpenGL.GL_POINTS);

            //    //GLfloat lineWidthRange[2] = { 0.0f, 0.0f };
            //    //glGetFloatv(GL_ALIASED_LINE_WIDTH_RANGE, lineWidthRange);

            //    if ((string)bt_DoDay.SelectedItem == "Small")
            //    {
            //        gl.PointSize(1);
            //        gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
            //    }
            //    else if ((string)bt_DoDay.SelectedItem == "Medium")
            //    {
            //        gl.PointSize(5);
            //        gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
            //    }
            //    else if ((string)bt_DoDay.SelectedItem == "Big")
            //    {
            //        gl.PointSize(10);
            //        gl.Enable(OpenGL.GL_VERTEX_PROGRAM_POINT_SIZE);
            //    }
            //    gl.End();
            //    gl.Flush();
        }
    }
}