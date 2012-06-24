using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KrdLab.Lisys.Visualizer
{
    /// <summary>
    /// MatrixビジュアライザのUIクラス
    /// </summary>
    public partial class MatrixVisualizerForm : Form
    {
        /// <summary>
        /// 公開コンストラクタ
        /// </summary>
        public MatrixVisualizerForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// csv形式のデータオブジェクトを設定する．
        /// </summary>
        /// <param name="data">ICsvインタフェースを実装したオブジェクト</param>
        public void SetCSV(KrdLab.Lisys.ICsv data)
        {
            this.DataGridView.Rows.Clear();
            this.DataGridView.Columns.Clear();
            
            if (data == null)
            {
                return;
            }

            string csv = data.ToCsv();
            string[] lines = csv.Split(new string[]{"\r\n"},StringSplitOptions.None);
            if (lines.Length < 1)
            {
                return;
            }

            String[] cols = lines[0].Split(',');
            for (int c = 0; c < cols.Length; ++c)
            {
                this.DataGridView.Columns.Add("c-" + c, c.ToString());
            }

            foreach (string line in lines)
            {
                this.DataGridView.Rows.Add(line.Split(','));
            }
        }
    }
}