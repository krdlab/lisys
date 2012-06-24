using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// <see cref="IVector.ForEach(ElementActionByVal)"/>のためのデリゲート
    /// </summary>
    /// <remarks>
    /// <code>
    /// foreach element in elements:
    ///     action(element)
    /// </code>
    /// </remarks>
    /// <param name="val"><see cref="IVector"/>が保持する要素の値</param>
    public delegate void ElementActionByVal(double val);

    /// <summary>
    /// <see cref="IVector.ForEach(ElementActionByRef)"/>のためのデリゲート
    /// </summary>
    /// <remarks>
    /// <code>
    /// foreach element in elements:
    ///     action(ref element)
    /// </code>
    /// </remarks>
    /// <param name="val"><see cref="IVector"/>が保持する要素の値</param>
    public delegate void ElementActionByRef(ref double val);

    /// <summary>
    /// <see cref="IVector.ForEach(ElementActionByValWithIndex)"/>のためのデリゲート
    /// </summary>
    /// <remarks>
    /// <code>
    /// for i in [0..Size-1]:
    ///     action(i, v[i])
    /// </code>
    /// </remarks>
    /// <param name="i"><see cref="IVector"/>が保持する要素のindex</param>
    /// <param name="val"><see cref="IVector"/>が保持する要素の値</param>
    public delegate void ElementActionByValWithIndex(int i, double val);

    /// <summary>
    /// <see cref="IVector.ForEach(ElementActionByRefWithIndex)"/>のためのデリゲート
    /// </summary>
    /// <remarks>
    /// <code>
    /// for i in [0..Size-1]:
    ///     action(i, ref v[i])
    /// </code>
    /// </remarks>
    /// <param name="i"><see cref="IVector"/>が保持する要素のindex</param>
    /// <param name="val"><see cref="IVector"/>が保持する要素の値</param>
    public delegate void ElementActionByRefWithIndex(int i, ref double val);

    /// <summary>
    /// ベクトルインタフェース
    /// </summary>
    public interface IVector : IEnumerable<double>
    {
        /// <summary>
        /// 要素の設定・取得を行う．
        /// </summary>
        /// <param name="i">要素のindex</param>
        /// <returns>指定されたindexを持つ要素</returns>
        double this[int i]
        {
            set;
            get;
        }

        /// <summary>
        /// 要素数を取得する．
        /// </summary>
        int Size
        {
            get;
        }

        /// <summary>
        /// ノルムを取得する．
        /// </summary>
        double Norm
        {
            get;
        }

        /// <summary>
        /// 要素の合計を取得する．
        /// </summary>
        double Sum
        {
            get;
        }

        /// <summary>
        /// 要素の二乗和を取得する．
        /// </summary>
        double SumSq
        {
            get;
        }

        /// <summary>
        /// 要素の平均値を取得する．
        /// </summary>
        double Average
        {
            get;
        }

        /// <summary>
        /// 要素の散布値を取得する．
        /// </summary>
        /// <remarks>
        /// <code>
        /// val = 0.0;
        /// avg = this.Average;
        /// foreach(e in this)
        /// {
        ///     val += ((e - avg) * (e - avg));
        /// }
        /// </code>
        /// </remarks>
        double Scatter
        {
            get;
        }

        /// <summary>
        /// 標本分散値を取得する．
        /// </summary>
        double Variance
        {
            get;
        }

        /// <summary>
        /// 各要素の符号を反転させる．
        /// </summary>
        /// <returns>自身の参照</returns>
        IVector Flip();

        /// <summary>
        /// 各要素の値を0にする．
        /// </summary>
        /// <returns>自身への参照</returns>
        IVector Zero();

        //IVector Clone();

        /// <summary>
        /// このオブジェクトの要素からなる配列を取得する．
        /// </summary>
        /// <returns>要素値の配列</returns>
        double[] ToArray();

        // 以下のメソッドは，RefVectorで実現できない
        //Clear
        //Resize

        /// <summary>
        /// <see cref="ElementActionByVal"/>により規定されたデリゲートを各要素に適用する．
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByVal"/>により規定されたデリゲート
        /// </param>
        /// <returns>メソッド適用後の自身への参照</returns>
        IVector ForEach(ElementActionByVal action);

        /// <summary>
        /// <see cref="ElementActionByRef"/>により規定されたデリゲートを各要素に適用する．
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByRef"/>により規定されたデリゲート
        /// </param>
        /// <returns>メソッド適用後の自身への参照</returns>
        IVector ForEach(ElementActionByRef action);

        /// <summary>
        /// <see cref="ElementActionByValWithIndex"/>により規定されたデリゲートを各要素に適用する．
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByValWithIndex"/>により規定されたデリゲート
        /// </param>
        /// <returns>メソッド適用後の自身への参照</returns>
        IVector ForEach(ElementActionByValWithIndex action);

        /// <summary>
        /// <see cref="ElementActionByRefWithIndex"/>により規定されたデリゲートを各要素に適用する．
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByRefWithIndex"/>により規定されたデリゲート
        /// </param>
        /// <returns>メソッド適用後の自身への参照</returns>
        IVector ForEach(ElementActionByRefWithIndex action);

        /// <summary>
        /// 部分ベクトルを返す．
        /// </summary>
        /// <param name="startIndex">部分ベクトルの開始位置</param>
        /// <returns><paramref name="startIndex"/>から終端までの部分ベクトル</returns>
        /// <remarks>
        /// このベクトルが保持する要素の部分集合である部分ベクトルを返す．
        /// 部分ベクトルが保持する要素は，このベクトルの要素と同じものである．
        /// </remarks>
        IVector Subvector(int startIndex);

        /// <summary>
        /// 部分ベクトルを返す．
        /// </summary>
        /// <param name="startIndex">部分ベクトルの開始位置</param>
        /// <param name="length">開始位置からの長さ</param>
        /// <returns>
        /// <paramref name="startIndex"/>から，長さ<paramref name="length"/>の部分ベクトルを返す．
        /// <paramref name="length"/>が<see cref="IVector.Size"/>を超える場合は，
        /// 終端まで（[<paramref name="startIndex"/>, <see cref="IVector.Size"/>)）の部分ベクトルを返す．
        /// </returns>
        /// <remarks>
        /// このベクトルが保持する要素の部分集合である部分ベクトルを返す．
        /// 部分ベクトルが保持する要素は，このベクトルの要素と同じものである．
        /// </remarks>
        IVector Subvector(int startIndex, int length);


        /// <summary>
        /// 各要素に<paramref name="value"/>を加算する．
        /// </summary>
        /// <param name="value">スカラ値</param>
        /// <returns>自身への参照</returns>
        IVector Add(double value);

        /// <summary>
        /// このベクトルオブジェクトに<paramref name="v"/>を加算する．
        /// </summary>
        /// <param name="v">ベクトルオブジェクト</param>
        /// <returns>自身への参照</returns>
        IVector Add(IVector v);

        /// <summary>
        /// このベクトルオブジェクトから<paramref name="v"/>を減算する．
        /// </summary>
        /// <param name="v">ベクトルオブジェクト</param>
        /// <returns>自身への参照</returns>
        IVector Sub(IVector v);

        /// <summary>
        /// 各要素に<paramref name="value"/>を乗算する．
        /// </summary>
        /// <param name="value">スカラ値</param>
        /// <returns>自身への参照</returns>
        IVector Mul(double value);

        /// <summary>
        /// 各要素に<paramref name="value"/>を除算する．
        /// </summary>
        /// <param name="value">スカラ値</param>
        /// <returns>自身への参照</returns>
        IVector Div(double value);
    }
}
