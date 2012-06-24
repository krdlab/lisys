using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 分散の種類
    /// </summary>
    public enum VarianceType
    {
        /// <summary>
        /// 分散（平均からの差の2乗和をNで割った値）
        /// </summary>
        DivN,

        /// <summary>
        /// 標本分散（推定量：N-1で割った値）
        /// </summary>
        Sample,
    }

    /// <summary>
    /// 処理の対象
    /// </summary>
    public enum Target
    {
        /// <summary>
        /// 行単位を対象とする．
        /// </summary>
        EachRow,

        /// <summary>
        /// 列単位を対象とする．
        /// </summary>
        EachColumn,
    }

    /// <summary>
    /// 処理の方向
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// 行方向に処理を行う．
        /// </summary>
        Row,

        /// <summary>
        /// 列方向に処理を行う．
        /// </summary>
        Column,
    }

    /// <summary>
    /// ベクトルの種類
    /// </summary>
    public enum VectorType
    {
        /// <summary>
        /// 行ベクトル
        /// </summary>
        RowVector,

        /// <summary>
        /// 列ベクトル
        /// </summary>
        ColumnVector,
    }

    /// <summary>
    /// コレクションの種類
    /// </summary>
    public enum CollectionType
    {
        /// <summary>
        /// 行コレクション
        /// </summary>
        Rows,

        /// <summary>
        /// 列コレクション
        /// </summary>
        Columns,
    }
}
