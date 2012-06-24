using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace KrdLab.Lisys.Visualizer
{
    // 利用方法
    //
    // [DebuggerVisualizer(typeof(MatrixVisualizer))]
    // [Serializable]
    // public class Matrix
    // {
    //  ...
    // }

    /// <summary>
    /// Matrix のビジュアライザ
    /// </summary>
    public class MatrixVisualizer : DialogDebuggerVisualizer
    {
        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="windowService"></param>
        /// <param name="objectProvider"></param>
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            using (MatrixVisualizerForm displayForm = new MatrixVisualizerForm())
            {
                displayForm.SetMatrix(objectProvider.GetObject() as KrdLab.Lisys.Matrix);
                windowService.ShowDialog(displayForm);
            }
        }

        /// <summary>
        /// test
        /// </summary>
        /// <param name="objectToVisualize"></param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(MatrixVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}