using System;

namespace KrdLab.Lisys
{
    /// <summary>
    /// サイズを表す．<see cref="int"/> と区別するための定義．
    /// </summary>
    [Serializable]
    public struct Size
    {
        private readonly int value;

        /// <summary>
        /// int -> Size
        /// </summary>
        public Size(int size)
        {
            this.value = size;
        }

        /// <summary>
        /// 実際の値
        /// </summary>
        public int Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// </summary>
        public static implicit operator int(Size s)
        {
            return s.value;
        }
    }

    /// <summary>
    /// 行列のサイズを表す．
    /// </summary>
    [Serializable]
    public struct Sizes
    {
        private readonly int rows;
        private readonly int cols;

        /// <summary>
        /// RowSize -> ColumnSize -> Sizes
        /// </summary>
        public Sizes(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
        }

        /// <summary>
        /// 行サイズ
        /// </summary>
        public int Rows { get { return this.rows; } }

        /// <summary>
        /// 列サイズ
        /// </summary>
        public int Cols { get { return this.cols; } }

        /// <summary>
        /// </summary>
        public override int GetHashCode()
        {
            unchecked {
                int h = 17;
                h = h * 31 + rows.GetHashCode();
                h = h * 31 + cols.GetHashCode();
                return h;
            }
        }

        /// <summary>
        /// </summary>
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            return this == ((Sizes)obj);
        }

        /// <summary>
        /// </summary>
        public static bool operator ==(Sizes l, Sizes r)
        {
            if (l == null || r == null) { return false; }
            return l.rows == r.rows && l.cols == r.cols;
        }

        /// <summary>
        /// </summary>
        public static bool operator !=(Sizes l, Sizes r)
        {
            return !(l == r);
        }
    }
}
