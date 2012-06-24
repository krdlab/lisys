using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KrdLab.Lisys.IO
{
    /// <summary>
    /// データの格納形式を示す列挙子
    /// </summary>
    public enum XsvDataFormatType
    {
        /// <summary>
        /// 1行に1つの行列データが格納されている．
        /// </summary>
        Line,

        /// <summary>
        /// 1つの表に1つの行列データが格納されている．
        /// </summary>
        Table,
    }

    /// <summary>
    /// XSVが扱うストリームデータのフォーマットを設定する．
    /// </summary>
    public sealed class XsvFormat
    {
        private Encoding encoding = Encoding.UTF8;
        private char separator = ',';
        private string lineSeparator = LisysConfig.LineSeparator;
        private bool hasHeader = false;
        private bool hasIndexes = false;

        private XsvDataFormatType dataFormat = XsvDataFormatType.Table;
        private int columnSize = 0;

        /// <summary>
        /// デフォルト設定のオブジェクトを生成する．
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///     <listheader>
        ///         <term>property</term>
        ///         <description>default value</description>
        ///     </listheader>
        ///     <item>
        ///         <term>Encoding</term>
        ///         <description>UTF-8</description>
        ///     </item>
        ///     <item>
        ///         <term>Separator</term>
        ///         <description>,</description>
        ///     </item>
        ///     <item>
        ///         <term>LineSeparator</term>
        ///         <description>LisysConfig.LineSeparatorと同じ</description>
        ///     </item>
        ///     <item>
        ///         <term>HasHeader</term>
        ///         <description>false</description>
        ///     </item>
        ///     <item>
        ///         <term>HasIndexes</term>
        ///         <description>false</description>
        ///     </item>
        ///     <item>
        ///         <term>DataFormat</term>
        ///         <description>XsvDataFormatType.Table</description>
        ///     </item>
        ///     <item>
        ///         <term>ColumnSize</term>
        ///         <description>0</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public XsvFormat()
        { }

        /// <summary>
        /// 文字エンコーディングを設定・取得する．
        /// </summary>
        public Encoding Encoding
        {
            set { this.encoding = value; }
            get { return this.encoding; }
        }

        /// <summary>
        /// 区切り文字を設定・取得する．
        /// </summary>
        public char Separator
        {
            set { this.separator = value; }
            get { return this.separator; }
        }

        /// <summary>
        /// 行区切りを設定・取得する．
        /// </summary>
        public string LineSeparator
        {
            set { this.lineSeparator = value; }
            get { return this.lineSeparator; }
        }

        /// <summary>
        /// 入力データがヘッダを持っているかどうかを設定・取得する．
        /// （表データの一行目がヘッダである場合にtrueを設定する）
        /// </summary>
        public bool HasHeader
        {
            set { this.hasHeader = value; }
            get { return this.hasHeader; }
        }

        /// <summary>
        /// 入力データがインデックスを持っているかどうかを設定・取得する．
        /// （表データの一列目が行番号であるときにtrueを設定する）
        /// </summary>
        public bool HasIndexes
        {
            set { this.hasIndexes = value; }
            get { return this.hasIndexes; }
        }

        /// <summary>
        /// 行列データの格納形式を設定・取得する．
        /// </summary>
        public XsvDataFormatType DataFormat
        {
            set { this.dataFormat = value; }
            get { return this.dataFormat; }
        }

        /// <summary>
        /// DataFormatとして XsvDataFormatType.Line が指定されている場合のカラムサイズを設定・取得する．
        /// Count = 1行分のデータ数; としたとき，Count mod ColumnSize == 0 でなければならない．
        /// </summary>
        public int ColumnSize
        {
            set { this.columnSize = value; }
            get { return this.columnSize; }
        }
    }
}
