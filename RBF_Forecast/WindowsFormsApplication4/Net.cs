using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
    class Net
    {
        public Form1 form;
        static int k;//кол-во нейронов в скрытом слое
        public double eps = 0.001;
        public Neiron[] net;
        public double[] X;
        public int window;
        public Net(int q, double n)
        {   ///создаем сеть
            k = q;
            net = new Neiron[k];

            int s = 0;
            ///создаем тип-т.е.считать
            for (int i = 0; i < k; i++)
            {
                if (s < 4)
                {
                    X = new double[s + 1];
                    s++;
                }
                else { s = 0; }
                X = Neiron.Norma(X);
                ///////пока ничего не поняла
                double[] qw = new double[window];
                for (int j = 0; j < window; j++)
                {
                    qw[j] = X[j];
                }
                net[i] = new Neiron(qw, k);
            }
        }
        // сравниваем погрешность с эпсилон
        public bool Sravn(Neiron c1, double[] c2)
        {
            bool flag = true;
            double buf = 0, max = 0;
            for (int i = 0; i < 9; i++)
            {
                buf = Math.Abs(c1[i] - c2[i]);
                if ((buf) >= eps)
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                max = 0;
                for (int i = 0; i < 9; i++)
                {

                    buf = Math.Abs(c1[i] - c2[i]);
                    if (buf > max)
                    {
                        max = buf;
                    }
                }
                /////////поставить лейбл
                form.label18.Text = max.ToString();
            }

            return flag;
        }



        ///алгоритм корректировки центров

        public void AlgorithmK(double[][] a, double nu, double nu1, int window)
        {

            Random rand = new Random();
            double[] temp = new double[window]; // хранит координаты центра нейрона победителя  

            int t = 0;
            int i_av = 0;
            int[] mini = new int[window * k]; //хранит историю, какие нейроны я/я нейронами-победителями
            do
            {
                double[] x = a[i_av];

                x = Neiron.Norma(x);
                i_av++;
                double sum = 0, buf = 10000;

                //определение нейрона-победителя
                for (int i = 0; i < k; i++)
                {
                    sum = Neiron.EvklidNorma(x, net[i], window);
                    sum = Math.Sqrt(sum);
                    if (sum < buf)
                    {
                        buf = sum;
                        mini[t] = i;
                    }
                }
                // хранит координаты центра нейрона победителя 
                for (int i = 0; i < window; i++)
                {
                    temp[i] = net[mini[t]][i];
                }
                //определение первого соседа
                int min1 = 0;
                buf = 10000;
                for (int i = 0; i < k; i++)
                {
                    if (i != mini[t])
                    {
                        sum = Neiron.EvklidNorma(x, net[i], window);
                        sum = Math.Sqrt(sum);
                        if (sum < buf)
                        {
                            buf = sum;
                            min1 = i;
                        }
                    }
                }
                //определение второго соседа
                int min2 = 0; buf = 10000;
                for (int i = 0; i < k; i++)
                {
                    if ((i != mini[t]) && (i != min1))
                    {
                        sum = Neiron.EvklidNorma(x, net[i], window);
                        sum = Math.Sqrt(sum);
                        if (sum < buf)
                        {
                            buf = sum;
                            min2 = i;
                        }
                    }
                }
                //определение третьего соседа min3
                int min3 = 0; buf = 10000;
                for (int i = 0; i < k; i++)
                {
                    if ((i != mini[t]) && (i != min1) && (i != min2))
                    {
                        sum = Neiron.EvklidNorma(x, net[i], window);
                        sum = Math.Sqrt(sum);
                        if (sum < buf)
                        {
                            buf = sum;
                            min3 = i;
                        }
                    }
                }
                //уточнение центра-победителя
                for (int i = 0; i < window; i++)
                {
                    net[mini[t]][i] = net[mini[t]][i] + nu * (x[i] - net[mini[t]][i]);
                }
                //уточнение соседей
                for (int i = 0; i < window; i++)
                {
                    net[min1][i] = net[min1][i] - nu1 * (x[i] - net[min1][i]);
                    net[min2][i] = net[min2][i] - nu1 * (x[i] - net[min2][i]);
                    net[min3][i] = net[min3][i] - nu1 * (x[i] - net[min3][i]);
                }
                t++;
            }
            while (t < window * k && (!Sravn(net[mini[t]], temp)));
            // от while
            if (t == window * k) { }
            int R = 3;//количество соседей
            double summ = 0;
            int min11 = 0, min22 = 0, min33 = 0;
            double buff = 10000;
            for (int i = 0; i < k; i++)//по всем нейронам
            {
                for (int j = 0; j < k; j++)
                {
                    if (i != j)
                    {
                        summ = Neiron.EvklidNormaNeiro(net[i], net[j], window);
                        summ = Math.Sqrt(summ);
                        if (summ < buff)
                        {
                            buff = summ;
                            min11 = j;
                        }
                    }

                }
                buff = 10000;
                for (int j = 0; j < k; j++)
                {
                    if ((i != j) && (j != min11))
                    {
                        summ = Neiron.EvklidNormaNeiro(net[i], net[j], window);
                        summ = Math.Sqrt(summ);
                        if (summ < buff)
                        {
                            buff = summ;
                            min22 = j;
                        }
                    }

                }
                buff = 10000;
                for (int j = 0; j < k; j++)
                {
                    if ((i != j) && (j != min11) && (j != min22))
                    {
                        summ = Neiron.EvklidNormaNeiro(net[i], net[j], window);
                        summ = Math.Sqrt(summ);
                        if (summ < buff)
                        {
                            buff = summ;
                            min33 = j;
                        }
                    }

                }
                //считаем дельты
                for (int j = 0; j < 9; j++)
                {
                    net[i].setDelta(j, Math.Sqrt((1.0 / R) * (Math.Pow((net[i][j] - net[min11][j]), 2) + Math.Pow((net[i][j] - net[min22][j]), 2) + Math.Pow((net[i][j] - net[min33][j]), 2))));
                }
            }
        }//от алгоритма 

        ///что делать с выходным значением??type[]
        public void AlgorithmW(double[][] a, Neiron w0, double nu2, double y, int window)
        {

            Random rand = new Random();
            double[,] temp = new double[k + 1, k]; // хранит частные производные

            int t = 0;
            int i_av = 0;
            double buff = 0;

            do
            {
                double[] x = a[i_av];
                x = Neiron.Norma(x);
                i_av++;


                //определение частных производных
                for (int q = 0; q < 4; q++)
                {
                    ///////вместо x.type должен быть выходной вектор temp[0, q] = Neiron.F(net, w0, x, k, q,window) - x.type[q];
                    temp[0, q] = Neiron.F(net, w0, x, k, q, window) - y;
                }

                for (int i = 0; i < k; i++)
                {
                    for (int q = 0; q < 4; q++)
                    {
                        temp[i + 1, q] = (Neiron.F(net, w0, x, k, q, window) - x.type[q]) * (Neiron.Fi(x, net[i], window));
                    }
                }

                //коррекция весов

                for (int q = 0; q < k; q++)
                {
                    w0.setW(q, w0.getW(q) - nu2 * temp[0, q]);
                }

                for (int i = 0; i < k; i++)
                {
                    for (int q = 0; q < k; q++)
                    {
                        net[i].setW(q, net[i].getW(q) - nu2 * temp[i + 1, q]);
                    }
                }
                t++;
                buff = Neiron.F(net, w0, x, k, 0, window) - x.type[0];
                for (int q = 0; q < 4; q++)
                {
                    if ((Neiron.F(net, w0, x, k, q, window) - x.type[q]) > buff)
                    { buff = Neiron.F(net, w0, x, k, q, window) - x.type[q]; }

                }
                double h = buff * buff / 2;
                //лейбл!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                form.label19.Text = h.ToString();
            }
            while (t < window * k && ((buff * buff / 2) > eps));
        }
    }
}
