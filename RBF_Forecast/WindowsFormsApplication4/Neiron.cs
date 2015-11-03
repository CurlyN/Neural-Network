using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
    class Neiron
    {
        double[] w;
        double[] c;
        double[] delta;
        public Neiron(int ch, int number_of_neurons)
        {
            c = new double[number_of_neurons];
            delta = new double[number_of_neurons];
            w = new double[ch];
            Random rand = new Random();
            for (int q = 0; q < ch; q++)
            {
                setW(q, (rand.NextDouble()));
            }
            Random r = new Random();
            for (int i = 0; i < number_of_neurons; i++)
            {
                c[i] = r.NextDouble() * 10 + 20;
            }

        }
        public Neiron(double[] cent, int numer_of_neuron)
        {
            w = new double[numer_of_neuron];
            Random rand = new Random();
            for (int q = 0; q < numer_of_neuron; q++)
            {
                setW(q, (rand.NextDouble()));
            }

            for (int i = 0; i < numer_of_neuron; i++)
            {
                c[i] = cent[i];
            }
        }
        public double getW(int index)
        {
            return w[index];
        }
        public void setW(int index, double val)
        {
            w[index] = val;
        }

        public double this[int x]
        {
            get
            {
                return c[x];
            }
            set
            {
                c[x] = value;
            }
        }
        public double getDelta(int index)
        {
            return delta[index];
        }
        public void setDelta(int index, double val)
        {
            delta[index] = val;
        }
        //////////////////////////////////////////////////
        public static double EvklidNorma(double[] x, Neiron net, int window)
        {
            double sum = 0;
            for (int j = 0; j < window; j++)
            {
                sum += Math.Pow((x[j] - net[j]), 2);
            }
            return sum;

        }
        public static double EvklidNormaNeiro(Neiron x, Neiron net, int window)
        {
            double sum = 0;
            for (int j = 0; j < window; j++)
            {
                sum += Math.Pow((x[j] - net[j]), 2);
            }
            return sum;

        }

        public static double Fi(double[] x, Neiron neiro, int window)
        {
            double sum = 0;
            for (int j = 0; j < window; j++)
            {
                sum += Math.Pow((x[j] - neiro[j]), 2) / Math.Pow(neiro.getDelta(j), 2);
            }
            return Math.Exp(-sum / 2);
        }

        public static double F(Neiron[] net, Neiron w0, double[] x, int k, int q, int window)// это y
        {
            double sum = 0;
            for (int i = 0; i < k; i++)
            {
                sum += net[i].getW(q) * Fi(x, net[i], window);
                //q++;
            }

            sum += w0.getW(q);
            return sum;
        }

        public static double[] NormaX(double[] x)
        {
            double sum = 0;

            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i] * x[i];
            }
            sum = Math.Sqrt(sum);
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = x[i] / sum;
            }
            return x;
        }

        public static double[] Norma(double[] c)
        {
            double sum = 0;
            for (int i = 0; i < c.Length; i++)
            {
                sum += c[i] * c[i];
            }
            sum = Math.Sqrt(sum);
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = c[i] / sum;
            }
            return c;
        }

    }

}
