using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace KrdLab.Lisys.Visualizer
{
    // TODO: SomeType のインスタンスをデバッグするときに、このビジュアライザを表示するために SomeType の定義に次のコードを追加します:
    // 
    //  [DebuggerVisualizer(typeof(MatrixVisualizer))]
    //  [Serializable]
    //  public class SomeType
    //  {
    //   ...
    //  }
    // 
    /// <summary>
    /// Matrix のビジュアライザ
    /// </summary>
    public class MatrixVisualizer : DialogDebuggerVisualizer
    {
        /// <summary>
        /// ビジュアライザのUIを表示する．
        /// </summary>
        /// <param name="windowService"></param>
        /// <param name="objectProvider"></param>
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            // TODO: ビジュアライザを表示する目的のオブジェクトを取得します。
            //       objectProvider.GetObject() の結果を視覚化 
            //       されるオブジェクトの型にキャストします。
            object data = (object)objectProvider.GetObject();

            // TODO: オブジェクトのビューを表示します。
            //       displayForm をユーザー独自のカスタム フォームまたはコントロールで置き換えます。
            using (MatrixVisualizerForm displayForm = new MatrixVisualizerForm())
            {
                // 型チェックをしていない
                KrdLab.Lisys.ICsv content = (KrdLab.Lisys.ICsv)data;
                displayForm.SetCSV(content);

                windowService.ShowDialog(displayForm);
            }
        }

        // TODO: ビジュアライザをテストするために、次のコードをユーザーのコードに追加します:
        // 
        //    MatrixVisualizer.TestShowVisualizer(new SomeType());
        // 
        /// <summary>
        /// デバッガの外部にホストすることにより、ビジュアライザをテストします。
        /// </summary>
        /// <param name="objectToVisualize">ビジュアライザに表示するオブジェクトです。</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(MatrixVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}