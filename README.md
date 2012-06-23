# Lisys (Japanese)

## What is it?
  C# 向けに LAPACK をラップしたライブラリです．
  単純なラップだけではなく，Vector/Matrix を通して操作できる I/F を定義しています．

## Features
  * Vector, RowVector, ColumnVector
  * Matrix
  * Eigenvalues/vectors
  * Singular Value Decomposition
  * LU Decomposition
  * Solver
  * Linear Discriminant Analysis
  * ...etc

## Usage
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

  5.  Windows のルールに従って DLL を配置

      <http://msdn.microsoft.com/en-us/library/7d83bc18%28v=vs.100%29.aspx>

      * BLAS.dll
      * LAPACK.dll
      * libgcc_s_dw2-1.dll
      * libgfortran-3.dll


## Samples
  `sample` フォルダを参照してください．

## License
  MIT License

  Copyright (C) 2007- KrdLab All Rights Reserved.

