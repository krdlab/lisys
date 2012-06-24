# Lisys (Japanese)

## What is it?

Lisys = LAPACK wrapper for C# + Matrix/Vector + Some well-known statistical techniques

## Features

* Vector, RowVector, ColumnVector
* Matrix
* Eigenvalues/vectors
* Singular Value Decomposition
* LU Decomposition
* Solver
* Linear Discriminant Analysis
* ...etc

## Build & Run

1.  LAPACK と Mingw の lib/dll を取得

    次のサイトを参考に Windows 用の lib/dll をダウンロード

    <http://icl.cs.utk.edu/lapack-for-windows/lapack/#librairies>

    (Section: "Prebuilt dynamic libraries using Mingw")

2.  "law" プロジェクトを以下のように設定

    <http://icl.cs.utk.edu/lapack-for-windows/lapack/#librairies>

    Section: "Prebuilt dynamic libraries using Mingw" の Instructions に従って設定する．

3.  "law" -> "lisys" の順番でプロジェクトをビルド

4.  自身のプロジェクトにおける「参照の追加」で以下のファイルを設定

    * lisys.dll
    * law.dll

    一緒に `*.xml` がコピーされ，Visual Studio 上でパラメータヒントが出るようになります．

5.  Windows のルールに従って LAPACK 関連の DLL を配置

    * liblapack.dll
    * libblas.dll
    * libgcc_s_dw2-1.dll
    * libgfortran-3.dll
    * libquadmath-0.dll

## Test

NUnit を利用しています．

1. NUnit をインストール
2. test プロジェクトをビルド
3. test/data フォルダを test/bin/Debug 以下にコピー
4. NUnit から test/bin/Debug/test.dll を実行

## Sample

### sample 1

    const int C = 3;
    var data = new Matrix[C];

    // input
    for (int i = 0; i < C; ++i)
    {
        data[i] =
            File.ReadAllLines(String.Format("data/iris/data_{0}.csv", i), Encoding.UTF8)
                .Select(line =>
                    line.Split(',').Select(s => Double.Parse(s)).ToRow())
                .ToMatrix();
    }

    Lda result = Func.Lda(data);

    // Coefficients of linear discriminants
    var ldc1 = new ColumnVector(result.Eigenvectors[0]);
    var ldc2 = new ColumnVector(result.Eigenvectors[1]);
    var coef = new Matrix(ldc1, ldc2);

    for (int i = 0; i < C; ++i)
    {
        var m = data[i] * coef;   // calculate scores

        // output
        File.WriteAllText(String.Format("result_{0}.csv", i), Matrix.ToCsv(m), Encoding.UTF8);
    }

### sample 2

    var m = new Matrix(new[,] {
                        {86.0, 67.0},
                        
                        ...
                        
                        {96.0, 61.0} });

    // correlation matrix
    var cor1 = Func.Correlate(m, Func.Target.EachColumn);

    MatrixVisualizer.TestShowVisualizer(cor1);  // test method of DebuggerVisualizer

    // normalization -> correlation matrix
    var n = new Matrix(m);
    n.Columns.ForEach((ci, cv) => {
        var avg = cv.Average;
        var std = Math.Sqrt(cv.UnbiasedVariance);
        cv.Apply((i, val) => (val - avg) / std);
    });

    var cor2 = Matrix.T(n) * n / (n.RowSize - 1);

    MatrixVisualizer.TestShowVisualizer(cor2);

### other

  その他細かな利用方法は `test` や `sample` プロジェクトを参照してください．

## License

MIT License

Copyright (C) 2007- KrdLab All Rights Reserved.

## TODO

* 64bit 対応

