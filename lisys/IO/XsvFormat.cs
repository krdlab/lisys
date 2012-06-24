using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KrdLab.Lisys.IO
{
    /// <summary>
    /// �f�[�^�̊i�[�`���������񋓎q
    /// </summary>
    public enum XsvDataFormatType
    {
        /// <summary>
        /// 1�s��1�̍s��f�[�^���i�[����Ă���D
        /// </summary>
        Line,

        /// <summary>
        /// 1�̕\��1�̍s��f�[�^���i�[����Ă���D
        /// </summary>
        Table,
    }

    /// <summary>
    /// XSV�������X�g���[���f�[�^�̃t�H�[�}�b�g��ݒ肷��D
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
        /// �f�t�H���g�ݒ�̃I�u�W�F�N�g�𐶐�����D
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
        ///         <description>LisysConfig.LineSeparator�Ɠ���</description>
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
        /// �����G���R�[�f�B���O��ݒ�E�擾����D
        /// </summary>
        public Encoding Encoding
        {
            set { this.encoding = value; }
            get { return this.encoding; }
        }

        /// <summary>
        /// ��؂蕶����ݒ�E�擾����D
        /// </summary>
        public char Separator
        {
            set { this.separator = value; }
            get { return this.separator; }
        }

        /// <summary>
        /// �s��؂��ݒ�E�擾����D
        /// </summary>
        public string LineSeparator
        {
            set { this.lineSeparator = value; }
            get { return this.lineSeparator; }
        }

        /// <summary>
        /// ���̓f�[�^���w�b�_�������Ă��邩�ǂ�����ݒ�E�擾����D
        /// �i�\�f�[�^�̈�s�ڂ��w�b�_�ł���ꍇ��true��ݒ肷��j
        /// </summary>
        public bool HasHeader
        {
            set { this.hasHeader = value; }
            get { return this.hasHeader; }
        }

        /// <summary>
        /// ���̓f�[�^���C���f�b�N�X�������Ă��邩�ǂ�����ݒ�E�擾����D
        /// �i�\�f�[�^�̈��ڂ��s�ԍ��ł���Ƃ���true��ݒ肷��j
        /// </summary>
        public bool HasIndexes
        {
            set { this.hasIndexes = value; }
            get { return this.hasIndexes; }
        }

        /// <summary>
        /// �s��f�[�^�̊i�[�`����ݒ�E�擾����D
        /// </summary>
        public XsvDataFormatType DataFormat
        {
            set { this.dataFormat = value; }
            get { return this.dataFormat; }
        }

        /// <summary>
        /// DataFormat�Ƃ��� XsvDataFormatType.Line ���w�肳��Ă���ꍇ�̃J�����T�C�Y��ݒ�E�擾����D
        /// Count = 1�s���̃f�[�^��; �Ƃ����Ƃ��CCount mod ColumnSize == 0 �łȂ���΂Ȃ�Ȃ��D
        /// </summary>
        public int ColumnSize
        {
            set { this.columnSize = value; }
            get { return this.columnSize; }
        }
    }
}
