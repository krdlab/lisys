using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sample
{
    using KrdLab.Lisys;
    using KrdLab.Lisys.Visualizer;

    class Program
    {
        static void Main(string[] args)
        {
            Sample1();
            Sample2();
        }

        static void Sample1()
        {
            const int C = 3;
            var data = new Matrix[C];
            for (int i = 0; i < C; ++i)
            {
                data[i] =
                    File.ReadAllLines(String.Format("data/iris/data_{0}.csv", i), Encoding.UTF8)
                        .Select(line =>
                            line.Split(',').Select(s => Double.Parse(s)).ToRow())
                        .ToMatrix();
            }
            
            Lda result = Func.Lda(data);

            // Coefficients of linear discriminants
            var ldc1 = new ColumnVector(result.Eigenvectors[0]);
            var ldc2 = new ColumnVector(result.Eigenvectors[1]);
            var coef = new Matrix(ldc1, ldc2);

            for (int i = 0; i < C; ++i)
            {
                var m = data[i] * coef;
                File.WriteAllText(String.Format("result_{0}.csv", i), Matrix.ToCsv(m), Encoding.UTF8);
            }
        }

        static void Sample2()
        {
            var m = new Matrix(new[,] {
                                {86.0, 67},
                                {71, 78},
                                {42, 39},
                                {62, 98},
                                {96, 61} });

            // correlation matrix
            var cor1 = Func.Correlate(m, Func.Target.EachColumn);
            MatrixVisualizer.TestShowVisualizer(cor1);

            // normalization -> correlation matrix
            var n = new Matrix(m);
            n.Columns.ForEach((ci, cv) => {
                var avg = cv.Average;
                var std = Math.Sqrt(cv.UnbiasedVariance);
                cv.Apply((i, val) => (val - avg) / std);
            });

            var cor2 = Matrix.T(n) * n / (n.RowSize - 1);
            MatrixVisualizer.TestShowVisualizer(cor2);
        }
    }
}
