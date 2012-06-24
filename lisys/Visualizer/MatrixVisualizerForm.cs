using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace KrdLab.Lisys.Visualizer
{
    /// <summary>
    /// Visualizer の Form 定義．
    /// </summary>
    public partial class MatrixVisualizerForm : Form
    {
        /// <summary>
        /// </summary>
        public MatrixVisualizerForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 表示対象の行列を設定する．
        /// </summary>
        /// <param name="m"></param>
        public void SetMatrix(KrdLab.Lisys.Matrix m)
        {
            this.DataGridView.Rows.Clear();
            this.DataGridView.Columns.Clear();
            if (m == null)
            {
                return;
            }

            for (int c = 0; c < m.ColumnSize; ++c)
            {
                var gridCol = new DataGridViewColumn();
                gridCol.HeaderText = String.Format("[ , {0}]", c);
                gridCol.ReadOnly = true;
                gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.DataGridView.Columns.Add(gridCol);
            }

            for (int r = 0; r < m.RowSize; ++r)
            {
                var gridRow = new DataGridViewRow();
                gridRow.HeaderCell.Value = String.Format("[{0}, ]", r);
                gridRow.ReadOnly = true;
                var row = m.Rows[r];
                foreach (var v in row)
                {
                    var cell = new DataGridViewTextBoxCell();
                    cell.Value = v.ToString();
                    gridRow.Cells.Add(cell);
                }
                this.DataGridView.Rows.Add(gridRow);
            }
            this.DataGridView.RowHeadersWidth = 100;
        }
    }
}