using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace KrdLab.Lisys
{
    /// <summary>
    /// �s�񂪎���x�N�g���R���N�V����
    /// </summary>
    public class ColumnCollection : IVectorCollection
    {
        private double[] _body = null;
        private int _rsize = 0;
        private int _csize = 0;

        internal ColumnCollection(double[] body, int rowSize, int columnSize)
        {
            this._body = body;
            this._rsize = rowSize;
            this._csize = columnSize;
        }

        #region IVectorCollection �����o

        /// <summary>
        /// ��R���N�V��������C�w�肵����̎擾�E�ݒ���s���D
        /// </summary>
        /// <param name="c">��index</param>
        /// <returns>��x�N�g��</returns>
        public IVector this[int c]
        {
            get
            {
                if (c < 0 || this.Count <= c)
                {
                    throw new IndexOutOfRangeException();
                }
                return new RefVector(this._body, c * this._rsize, 1, this._rsize);
            }
            set
            {
                CollectionImpl.Setter(this, c, value);
            }
        }

        /// <summary>
        /// �񐔂��擾����D
        /// </summary>
        public int Count
        {
            get { return this._csize; }
        }

        /// <summary>
        /// �R���N�V������ action ��K�p����D
        /// </summary>
        /// <param name="action"><see cref="VectorCollectionAction"/>�ŋK�肳���A�N�V����</param>
        /// <returns>�K�p��̎��g�ւ̎Q��</returns>
        public IVectorCollection ForEach(VectorCollectionAction action)
        {
            return CollectionImpl.ForEach(this, action);
        }

        /// <summary>
        /// �R���N�V������ action ��K�p����D
        /// </summary>
        /// <param name="action"><see cref="VectorCollectionActionWithIndex"/>�ŋK�肳���A�N�V����</param>
        /// <returns>�K�p��̎��g�ւ̎Q��</returns>
        public IVectorCollection ForEach(VectorCollectionActionWithIndex action)
        {
            return CollectionImpl.ForEach(this, action);
        }

        /// <summary>
        /// �����R���N�V������Ԃ��D
        /// </summary>
        /// <param name="startIndex">�J�nindex</param>
        /// <returns><paramref name="startIndex"/>����<c><see cref="IVectorCollection.Count"/>-1</c>�܂ł̕����R���N�V����</returns>
        public IVectorCollection Subcollection(int startIndex)
        {
            return CollectionImpl.Subcollection(this, CollectionType.Columns, startIndex);
        }

        /// <summary>
        /// �����R���N�V������Ԃ��D
        /// </summary>
        /// <param name="startIndex">�J�nindex</param>
        /// <param name="count">�擾����R���N�V�����̐�</param>
        /// <returns><paramref name="startIndex"/>����͂��܂钷��<paramref name="count"/>�̕����R���N�V����</returns>
        public IVectorCollection Subcollection(int startIndex, int count)
        {
            return CollectionImpl.Subcollection(this, CollectionType.Columns, startIndex, count);
        }

        /// <summary>
        /// �ێ����Ă���R���N�V��������C�V����<see cref="Matrix"/>�I�u�W�F�N�g���쐬����D
        /// </summary>
        /// <returns><see cref="Matrix"/>�I�u�W�F�N�g</returns>
        public Matrix ToMatrix()
        {
            return CollectionImpl.ColumnsToMatrix(this);
        }

        /// <summary>
        /// �w�肳�ꂽ������ւ���D
        /// </summary>
        /// <param name="index1">��Index</param>
        /// <param name="index2">��Index</param>
        /// <returns>����ւ���̎��g�ւ̎Q��</returns>
        public IVectorCollection Swap(int index1, int index2)
        {
            return CollectionImpl.Swap(this, index1, index2);
        }

        #endregion

        #region IEnumerable �����o

        IEnumerator IEnumerable.GetEnumerator()
        {
            return CollectionImpl.Enumerator(this);
        }

        #endregion

        #region IEnumerable<IVector> �����o

        /// <summary>
        /// �񋓎q���擾����D
        /// </summary>
        /// <returns>�񋓎q</returns>
        public IEnumerator<IVector> GetEnumerator()
        {
            return CollectionImpl.Enumerator(this);
        }

        #endregion

        /// <summary>
        /// ��R���N�V��������w�肳�ꂽ����擾����D
        /// </summary>
        /// <param name="indexes">�擾���������index�z��</param>
        /// <returns>�����Ŏw�肳�ꂽ��Z�b�g�̗�R���N�V����</returns>
        public IVectorCollection this[params int[] indexes]
        {
            get
            {
                CollectionImpl.CheckIndexes(this, indexes);
                return new SubCollection(this, indexes, CollectionType.Columns);
            }
        }

    }
}
