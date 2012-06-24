using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KrdLab.Lisys.IO
{
    /// <summary>
    /// Matrix�I�u�W�F�N�g�ƃX�g���[���̊Ԃ̕ϊ����s���D
    /// ���镶���ŋ�؂�ꂽ�iCSV��TSV���j�`���������D
    /// </summary>
    public class XSV : IDisposable
    {
        private Stream stream = null;
        private XsvFormat format = new XsvFormat();

        private string leftTopCell = null;
        private string[] header = null;
        private string[] indexes = null;


        #region �v���p�e�B�̒�`

        /// <summary>
        /// XSV����ǂݍ��񂾍���̃Z���l��ݒ�E�擾����D
        /// �w�b�_�ƃC���f�b�N�X�̗����������Ă���Ƃ��Ɏg�p�\�ƂȂ�D
        /// ToMatrix�CToMatrices���\�b�h���Ăяo����Ȃ��ƗL���Ȓl���i�[����Ȃ��D
        /// </summary>
        public string LeftTopCell
        {
            set { this.leftTopCell = value; }
            get { return this.leftTopCell; }
        }

        /// <summary>
        /// XSV����ǂݍ��񂾃w�b�_��ݒ�E�擾����D
        /// ToMatrix�CToMatrices���\�b�h���Ăяo����Ȃ��ƗL���Ȓl���i�[����Ȃ��D
        /// </summary>
        public string[] Header
        {
            set { this.header = value; }
            get { return this.header; }
        }

        /// <summary>
        /// XSV����ǂݍ��񂾃C���f�b�N�X��ݒ�E�擾����D
        /// ToMatrix�CToMatrices���\�b�h���Ăяo����Ȃ��ƗL���Ȓl���i�[����Ȃ��D
        /// </summary>
        public string[] Indexes
        {
            set { this.indexes = value; }
            get { return this.indexes; }
        }

        /// <summary>
        /// �ǂݍ��݉\�ł��邩�ǂ������擾����D
        /// </summary>
        public bool CanRead
        {
            get { return this.stream.CanRead; }
        }

        /// <summary>
        /// �������݉\�ł��邩�ǂ������擾����D
        /// </summary>
        public bool CanWrite
        {
            get { return this.stream.CanWrite; }
        }

        #endregion


        /// <summary>
        /// XSV�I�u�W�F�N�g���\�z����D
        /// </summary>
        /// <param name="stream">Read/Write�ΏۂƂȂ�Stream�I�u�W�F�N�g</param>
        /// <param name="format">�f�[�^�̈��������肷��t�H�[�}�b�g�I�u�W�F�N�g</param>
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
        /// �f�X�g���N�^
        /// </summary>
        ~XSV()
        {
            Dispose(false); // numanaged only
        }

        /// <summary>
        /// ���̃I�u�W�F�N�g���ێ����� managed ����� unmanaged �I�u�W�F�N�g���������D
        /// unmanaged �I�u�W�F�N�g�́C�����ɂ�����炸��������D
        /// </summary>
        /// <param name="disposing">managed�I�u�W�F�N�g���������Ƃ���true���C����ȊO��false���w�肷��D</param>
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

        #region IDisposable �����o

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region �⏕���\�b�h

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
        /// ��������w�b�_�ƃC���f�B�b�N�X�𒊏o����D
        /// ���̍ہC�w�b�_�ƃC���f�B�b�N�X�͈�������폜����C�t�B�[���h�ɕێ������D
        /// </summary>
        /// <param name="data">���f�[�^</param>
        private void ExtractHeaderAndIndexes(List<List<string>> data)
        {
            if (this.format.HasHeader)
            {
                List<string> hd = new List<string>(GetHeader(data));
                RemoveHeader(data);

                // Indexes�������Ă���ꍇ�́C�\�̍���v�f���Z�b�g����
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
        /// �X�g���[������f�[�^��ǂݏo���C�\�f�[�^�Ƃ��Ď擾����D
        /// </summary>
        /// <param name="stream">�f�[�^��ǂݏo���X�g���[��</param>
        /// <returns>1�s���̃f�[�^��1�̃��X�g�Ɋi�[�������̂̃��X�g</returns>
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
        /// <see cref="string"/>�̉򂩂�C<see cref="double"/>�^�̃e�[�u�����쐬����D
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
        /// �X�g���[������擾�����f�[�^���C1��Matrix�I�u�W�F�N�g�Ƃ��ďo�͂���D
        /// </summary>
        /// <returns>Matrix�I�u�W�F�N�g</returns>
        public Matrix ToMatrix()
        {
            // �G���[�`�F�b�N
            if (!this.CanRead)
            {
                throw new Exception.NotReadableStreamException("Cannot read the current stream.");
            }
            if (this.format.DataFormat != XsvDataFormatType.Table)
            {
                throw new Exception.IllegalXsvFormatException("Data format is not Table.");
            }

            // Stream����ListList��
            List<List<string>> data = this.ToValues(this.stream);

            if (data.Count < 1)
            {
                // �t�@�C������C�������͐�����Split����Ȃ������Ƃ�
                return new Matrix();
            }

            // �w�b�_��C���f�b�N�X������΁C�������f�[�^�����菜���i����̃����o�ɎQ�Ƃ�����j
            this.ExtractHeaderAndIndexes(data);

            // ���l�f�[�^����Matrix�𐶐�����
            return new Matrix(this.ToArray(data));
        }

        /// <summary>
        /// �X�g���[������擾�����f�[�^���C1�s�ɂ�1��Matrix�I�u�W�F�N�g�Ƃ��ďo�͂���D
        /// </summary>
        /// <returns>Matrix�I�u�W�F�N�g�̔z��</returns>
        public Matrix[] ToMatrices()
        {
            // �G���[�`�F�b�N
            if (!this.CanRead)
            {
                throw new Exception.NotReadableStreamException("Cannot read the current stream.");
            }
            if (this.format.DataFormat != XsvDataFormatType.Line)
            {
                throw new Exception.IllegalXsvFormatException("Data format is not Line.");
            }

            // Stream����ListList��
            List<List<string>> data = this.ToValues(this.stream);

            if (data.Count < 1)
            {
                // �t�@�C������C�������͐�����Split����Ȃ������Ƃ�
                return new Matrix[] { };
            }

            // �w�b�_��C���f�b�N�X������΁C�������f�[�^�����菜���i����̃����o�ɎQ�Ƃ�����j
            this.ExtractHeaderAndIndexes(data);

            // ���l�f�[�^����Matrix�I�u�W�F�N�g�𐶐�����
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
        /// �X�g���[����Matrix�I�u�W�F�N�g���������ށD
        /// </summary>
        /// <param name="matrix">Matrix�I�u�W�F�N�g</param>
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
                // �o�͂Ƃ��� ',' ��؂肪�w�肳��Ă��Ȃ��Ƃ�
                xsv = xsv.Replace(',', this.format.Separator);
            }

            // ��������
            this.WriteString(this.GetOutputString(xsv));
        }

        /// <summary>
        /// �X�g���[����Matrix�I�u�W�F�N�g�̔z����������ށD
        /// 1�s��1��Matrix�I�u�W�F�N�g���������܂��D
        /// </summary>
        /// <param name="matrices">Matrix�I�u�W�F�N�g�z��</param>
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

            // ��������
            this.WriteString(this.GetOutputString(xsv));
        }
    }
}
