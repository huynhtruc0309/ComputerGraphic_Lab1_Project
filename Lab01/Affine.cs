using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab01
{
    
    class Mat
    {
        private List<List<float>> matrix;
        public Mat(int rows, int cols)
        {
            List<List<float>> result = new List<List<float>>(rows);
            for(int i = 0; i < rows; i++)
            {
                result[i] = new List<float>(cols);
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
        public void set(int i, int j , float c)
        {
            matrix[i][j] = c;
        }
        public Mat multiply(Mat a, Mat b)
        {
            Mat c = new Mat(a.getRows(), b.getCols());
            for(int i = 0;i< a.getRows(); i++)
            {
                for(int j = 0; j < b.getCols(); j++)
                {
                    float sum = 0;
                    for(int z = 0;z < a.getCols(); z++)
                    {
                        sum += a.at(i, z) + b.at(z, j);
                    }
                    c.set(i,j, sum);
                }
            }
            return c;
        }


    }

    class AffineTransform {
        Mat _matrixTransform;
        public AffineTransform()
        {
            _matrixTransform = new Mat(3, 3);
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    _matrixTransform.set(i, j, 0);
                }
            }
            for (int i = 0; i < 3; i++) _matrixTransform.set(i, i, 1);
        }
        public void Translate(float dx, float dy)
        {
            Mat temp = new Mat(3, 3);
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
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
            temp.set(0, 0,(float)Math.Cos(degree));
            temp.set(0, 1, -(float)Math.Sin(degree));
            temp.set(1, 0, (float)Math.Sin(degree));
            temp.set(1, 1, (float)Math.Cos(degree));
            temp.set(2, 2,1);
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
        public void TransformPoint(ref float x,ref float y)
        {
            Mat temp = new Mat(3,1);
            temp.set(0, 0, x);
            temp.set(1, 0, y);
            temp.set(2, 0, 1);
            temp = _matrixTransform.multiply(_matrixTransform, temp);
            x = temp.at(0, 0);
            y = temp.at(1, 0);
        }


    }


}
