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
    /// Matrix�r�W���A���C�U��UI�N���X
    /// </summary>
    public partial class MatrixVisualizerForm : Form
    {
        /// <summary>
        /// ���J�R���X�g���N�^
        /// </summary>
        public MatrixVisualizerForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// csv�`���̃f�[�^�I�u�W�F�N�g��ݒ肷��D
        /// </summary>
        /// <param name="data">ICsv�C���^�t�F�[�X�����������I�u�W�F�N�g</param>
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