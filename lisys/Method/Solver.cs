using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// A * X = B�������iX�����߂�j�D
    /// </summary>
    public class Solver
    {
        private Matrix _x;

        /// <summary>
        /// [A]{X}={B}�̉�{X}���擾����D
        /// </summary>
        public Matrix X
        {
            get { return this._x; }
        }

        /// <summary>
        /// ��ʐ����s�� A �����A���ꎟ������  A * X = B �������iX�����߂�j�D
        /// </summary>
        /// <param name="A">�W���s��i�����s��j�i�����������邱�Ƃ͂Ȃ��j</param>
        /// <param name="B">�萔���s��i�����������邱�Ƃ͂Ȃ��j</param>
        /// <exception cref="Exception.NotSquareMatrixException">
        /// <paramref name="A"/> �������s��łȂ��ꍇ�� throw �����D
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
