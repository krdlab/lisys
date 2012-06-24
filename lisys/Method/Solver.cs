using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// A * X = Bを解く（Xを求める）．
    /// </summary>
    public class Solver
    {
        private Matrix _x;

        /// <summary>
        /// [A]{X}={B}の解{X}を取得する．
        /// </summary>
        public Matrix X
        {
            get { return this._x; }
        }

        /// <summary>
        /// 一般正方行列 A をもつ連立一次方程式  A * X = B を解く（Xを求める）．
        /// </summary>
        /// <param name="A">係数行列（正方行列）（書き換えられることはない）</param>
        /// <param name="B">定数項行列（書き換えられることはない）</param>
        /// <exception cref="Exception.NotSquareMatrixException">
        /// <paramref name="A"/> が正方行列でない場合に throw される．
        /// </exception>
        public Solver(Matrix A, Matrix B)
        {
            MatrixChecker.IsSquare(A);

            Matrix a = new Matrix(A);
            Matrix b = new Matrix(B);
            
            this._x = new Matrix();

            krdlab.law.func.dgesv(ref this._x._body, ref this._x._rsize, ref this._x._csize,
                                    a._body, a._rsize, a._csize, b._body, b._rsize, b._csize);
        }
    }
}
