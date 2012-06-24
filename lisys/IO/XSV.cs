using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KrdLab.Lisys.IO
{
    /// <summary>
    /// Matrixオブジェクトとストリームの間の変換を行う．
    /// ある文字で区切られた（CSVやTSV等）形式を扱う．
    /// </summary>
    public class XSV : IDisposable
    {
        private Stream stream = null;
        private XsvFormat format = new XsvFormat();

        private string leftTopCell = null;
        private string[] header = null;
        private string[] indexes = null;


        #region プロパティの定義

        /// <summary>
        /// XSVから読み込んだ左上のセル値を設定・取得する．
        /// ヘッダとインデックスの両方を持っているときに使用可能となる．
        /// ToMatrix，ToMatricesメソッドが呼び出されないと有効な値が格納されない．
        /// </summary>
        public string LeftTopCell
        {
            set { this.leftTopCell = value; }
            get { return this.leftTopCell; }
        }

        /// <summary>
        /// XSVから読み込んだヘッダを設定・取得する．
        /// ToMatrix，ToMatricesメソッドが呼び出されないと有効な値が格納されない．
        /// </summary>
        public string[] Header
        {
            set { this.header = value; }
            get { return this.header; }
        }

        /// <summary>
        /// XSVから読み込んだインデックスを設定・取得する．
        /// ToMatrix，ToMatricesメソッドが呼び出されないと有効な値が格納されない．
        /// </summary>
        public string[] Indexes
        {
            set { this.indexes = value; }
            get { return this.indexes; }
        }

        /// <summary>
        /// 読み込み可能であるかどうかを取得する．
        /// </summary>
        public bool CanRead
        {
            get { return this.stream.CanRead; }
        }

        /// <summary>
        /// 書き込み可能であるかどうかを取得する．
        /// </summary>
        public bool CanWrite
        {
            get { return this.stream.CanWrite; }
        }

        #endregion


        /// <summary>
        /// XSVオブジェクトを構築する．
        /// </summary>
        /// <param name="stream">Read/Write対象となるStreamオブジェクト</param>
        /// <param name="format">データの扱いを決定するフォーマットオブジェクト</param>
        public XSV(Stream stream, XsvFormat format)
        {
            _XSV(stream, format);
        }


        private void _XSV(Stream stream, XsvFormat format)
        {
            this.stream = stream;
            this.format = format;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~XSV()
        {
            Dispose(false); // numanaged only
        }

        /// <summary>
        /// このオブジェクトが保持する managed および unmanaged オブジェクトを解放する．
        /// unmanaged オブジェクトは，引数にかかわらず解放される．
        /// </summary>
        /// <param name="disposing">managedオブジェクトを解放するときはtrueを，それ以外はfalseを指定する．</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // managed code
                if (this.stream != null)
                {
                    this.stream.Close();
                    this.stream = null;
                }
                this.format = null;

                this.leftTopCell = null;
                this.header = null;
                this.indexes = null;
            }
            // unmanaged code
        }

        #region IDisposable メンバ

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 補助メソッド

        private string[] GetHeader(List<List<string>> data)
        {
            return data[0].ToArray();
        }

        private void RemoveHeader(List<List<string>> data)
        {
            data.RemoveAt(0);
        }

        private string[] GetIndexes(List<List<string>> data)
        {
            List<string> rets = new List<string>();
            foreach (List<string> line in data)
            {
                rets.Add(line[0]);
            }
            return rets.ToArray();
        }

        private void RemoveIndexes(List<List<string>> data)
        {
            foreach (List<string> line in data)
            {
                line.RemoveAt(0);
            }
        }

        /// <summary>
        /// 引数からヘッダとインディックスを抽出する．
        /// その際，ヘッダとインディックスは引数から削除され，フィールドに保持される．
        /// </summary>
        /// <param name="data">元データ</param>
        private void ExtractHeaderAndIndexes(List<List<string>> data)
        {
            if (this.format.HasHeader)
            {
                List<string> hd = new List<string>(GetHeader(data));
                RemoveHeader(data);

                // Indexesも持っている場合は，表の左上要素をセットする
                if (this.format.HasIndexes)
                {
                    this.leftTopCell = hd[0];
                    hd.RemoveAt(0);
                }
                this.header = hd.ToArray();
            }

            if (this.format.HasIndexes)
            {
                this.indexes = GetIndexes(data);
                RemoveIndexes(data);
            }
        }
        
        /// <summary>
        /// ストリームからデータを読み出し，表データとして取得する．
        /// </summary>
        /// <param name="stream">データを読み出すストリーム</param>
        /// <returns>1行分のデータを1つのリストに格納したもののリスト</returns>
        private List<List<string>> ToValues(Stream stream)
        {
            byte[] buffer = new byte[this.stream.Length];
            this.stream.Read(buffer, 0, buffer.Length);

            String[] lines = this.format.Encoding.GetString(buffer).Split(
                                                new string[] { this.format.LineSeparator },
                                                StringSplitOptions.RemoveEmptyEntries);

            List<List<string>> data = new List<List<string>>();
            foreach (string line in lines)
            {
                data.Add(new List<string>(line.Split(this.format.Separator)));
            }

            return data;
        }

        /// <summary>
        /// <see cref="string"/>の塊から，<see cref="double"/>型のテーブルを作成する．
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private double[,] ToArray(List<List<string>> data)
        {
            double[,] ret = new double[data.Count, data[0].Count];
            for (int r = 0; r < ret.GetLength(0); ++r)
            {
                for (int c = 0; c < ret.GetLength(1); ++c)
                {
                    ret[r, c] = double.Parse(data[r][c]);
                }
            }
            return ret;
        }

        private void WriteString(String value)
        {
            byte[] buffer = this.format.Encoding.GetBytes(value);
            this.stream.Write(buffer, 0, buffer.Length);
        }

        private string GetOutputString(string xsv)
        {
            StringBuilder sb = new StringBuilder();

            if (this.format.HasHeader
                && this.format.HasIndexes
                && this.leftTopCell != null)
            {
                sb.Append(this.leftTopCell + this.format.Separator);
            }
            if (this.format.HasHeader && this.header != null)
            {
                sb.Append(string.Join(this.format.Separator.ToString(), this.header) + this.format.LineSeparator);
            }
            if (this.format.HasIndexes && this.indexes != null)
            {
                string[] lines = xsv.Split(new string[] { this.format.LineSeparator },
                                                    StringSplitOptions.RemoveEmptyEntries);
                for (int r = 0; r < lines.Length; ++r)
                {
                    sb.Append(this.indexes[r] + this.format.Separator + lines[r] + this.format.LineSeparator);
                }
            }
            else
            {
                sb.Append(xsv);
            }

            return sb.ToString();
        }
        
        #endregion


        /// <summary>
        /// ストリームから取得したデータを，1つのMatrixオブジェクトとして出力する．
        /// </summary>
        /// <returns>Matrixオブジェクト</returns>
        public Matrix ToMatrix()
        {
            // エラーチェック
            if (!this.CanRead)
            {
                throw new Exception.NotReadableStreamException("Cannot read the current stream.");
            }
            if (this.format.DataFormat != XsvDataFormatType.Table)
            {
                throw new Exception.IllegalXsvFormatException("Data format is not Table.");
            }

            // StreamからListListへ
            List<List<string>> data = this.ToValues(this.stream);

            if (data.Count < 1)
            {
                // ファイルが空，もしくは正しくSplitされなかったとき
                return new Matrix();
            }

            // ヘッダやインデックスがあれば，それらをデータから取り除く（所定のメンバに参照させる）
            this.ExtractHeaderAndIndexes(data);

            // 数値データからMatrixを生成する
            return new Matrix(this.ToArray(data));
        }

        /// <summary>
        /// ストリームから取得したデータを，1行につき1つのMatrixオブジェクトとして出力する．
        /// </summary>
        /// <returns>Matrixオブジェクトの配列</returns>
        public Matrix[] ToMatrices()
        {
            // エラーチェック
            if (!this.CanRead)
            {
                throw new Exception.NotReadableStreamException("Cannot read the current stream.");
            }
            if (this.format.DataFormat != XsvDataFormatType.Line)
            {
                throw new Exception.IllegalXsvFormatException("Data format is not Line.");
            }

            // StreamからListListへ
            List<List<string>> data = this.ToValues(this.stream);

            if (data.Count < 1)
            {
                // ファイルが空，もしくは正しくSplitされなかったとき
                return new Matrix[] { };
            }

            // ヘッダやインデックスがあれば，それらをデータから取り除く（所定のメンバに参照させる）
            this.ExtractHeaderAndIndexes(data);

            // 数値データからMatrixオブジェクトを生成する
            int colSize = this.format.ColumnSize;
            Matrix[] ms = new Matrix[data.Count];
            for (int i = 0; i < ms.Length; ++i)
            {
                string[] cells = data[i].ToArray();
                if (cells.Length % colSize != 0)
                {
                    throw new Exception.IllegalXsvFormatException("Illegal ColumnSize.");
                }

                int rowSize = cells.Length / colSize;
                Matrix m = new Matrix(rowSize, colSize);
                for (int r = 0; r < rowSize; ++r)
                {
                    for (int c = 0; c < colSize; ++c)
                    {
                        m[r, c] = double.Parse(cells[r * colSize + c]);
                    }
                }

                ms[i] = m;
            }

            return ms;
        }


        /// <summary>
        /// ストリームにMatrixオブジェクトを書き込む．
        /// </summary>
        /// <param name="matrix">Matrixオブジェクト</param>
        public void Write(Matrix matrix)
        {
            if (!this.CanWrite)
            {
                throw new Exception.NotWritableStreamException("Cannot write the current stream.");
            }
            if (this.format.DataFormat != XsvDataFormatType.Table)
            {
                throw new Exception.IllegalXsvFormatException("Data format is not Table.");
            }
            if (this.format.HasHeader)
            {
                if (this.header == null || this.header.Length != matrix.ColumnSize)
                {
                    throw new Exception.IllegalXsvStateException("XSV header is illegal.");
                }
            }
            if (this.format.HasIndexes)
            {
                if (this.indexes == null || this.indexes.Length != matrix.RowSize)
                {
                    throw new Exception.IllegalXsvStateException("XSV indexes are illegal.");
                }
            }
            if (this.format.HasHeader && this.format.HasIndexes)
            {
                if (this.leftTopCell == null)
                {
                    throw new Exception.IllegalXsvStateException("XSV left-top-cell is illegal.");
                }
            }

            string xsv = matrix.ToCsv();
            if (!this.format.Separator.Equals(','))
            {
                // 出力として ',' 区切りが指定されていないとき
                xsv = xsv.Replace(',', this.format.Separator);
            }

            // 書き込み
            this.WriteString(this.GetOutputString(xsv));
        }

        /// <summary>
        /// ストリームにMatrixオブジェクトの配列を書き込む．
        /// 1行に1つのMatrixオブジェクトが書き込まれる．
        /// </summary>
        /// <param name="matrices">Matrixオブジェクト配列</param>
        public void Write(Matrix[] matrices)
        {
            if (!this.CanWrite)
            {
                throw new Exception.NotWritableStreamException("Cannot write the current stream.");
            }
            if (this.format.DataFormat != XsvDataFormatType.Line)
            {
                throw new Exception.IllegalXsvFormatException("Data format is not Line.");
            }
            if (this.format.HasHeader)
            {
                if (this.header == null || this.header.Length != (matrices[0].RowSize * matrices[0].ColumnSize))
                {
                    throw new Exception.IllegalXsvStateException("XSV header is illegal.");
                }
            }
            if (this.format.HasIndexes)
            {
                if (this.indexes == null || this.indexes.Length != matrices.Length)
                {
                    throw new Exception.IllegalXsvStateException("XSV indexes are illegal.");
                }
            }
            if (this.format.HasHeader && this.format.HasIndexes)
            {
                if (this.leftTopCell == null)
                {
                    throw new Exception.IllegalXsvStateException("XSV left-top-cell is illegal.");
                }
            }

            char sep = this.format.Separator;
            String ls = this.format.LineSeparator;

            StringBuilder sb = new StringBuilder();            
            foreach (Matrix m in matrices)
            {
                sb.Append(m.ToCsv().Replace(',', sep).Replace(ls, sep.ToString()) + ls);
            }
            string xsv = sb.ToString();

            // 書き込み
            this.WriteString(this.GetOutputString(xsv));
        }
    }
}
