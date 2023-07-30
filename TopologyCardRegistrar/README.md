# トポロジーカード登録ソフト
このディレクトリにはトポロジー神経衰弱で使われるトポロジーカードの登録を目的とするソフトウェアが入っています。

## ファイル構造
```
TopologyCardRegistrar
│- TopologyCardRegistrar.sln
│
└───TopologyCardRegistrar
    │- Form1.Designer.cs
    │- Form1.cs
    │- Form1.resx
    │- JsonSaver.cs
    │- Program.cs
    │- TopologyCalculator.cs
    │- TopologyCardRegistrar.csproj
```

## ファイル説明
TopologyCardRegistrar.sln: ソリューションファイル。Visual Studio で開きます。
TopologyCardRegistrar.csproj: プロジェクト設定ファイル。
Form1.cs, Form1.Designer.cs, Form1.resx: メインフォームのソースコードと設定。
JsonSaver.cs: JSON 形式でデータを保存するためのクラスが定義されています。
Program.cs: アプリケーションのエントリーポイントです。メインフォームを起動します。
TopologyCalculator.cs: トポロジー計算のためのクラスが定義されています。

## ビルド方法
Visual Studioを使用してビルドしてください。ビルドはTopologyCardRegistrar.sln ファイルを開いてビルド/ソリューションのビルドを押下すると実行できます。

## 使用方法
ビルドして生成された実行可能ファイル（.exe）を実行すると以下の画面が表示されます。
![](images/description.png)
「入力された画像の保存先」には登録するカードの画像を保存するフォルダを指定してください
「穴の数の出力先ファイル」にはトポロジーカードの情報をまとめたjsonファイルを指定してください。既に存在するファイルを指定した場合は情報が追加される形で出力されます。
「画像を読み込む」ボタンを押下すると上画面に読み込んだ画像が表示されます。また、読み込んだ画像の穴の数が「入力された画像の要素ごとの穴の数」の部分に表示されます。
以上の3つのボタンを押すと「カードを保存する」ボタンが押せるようになります。
このボタンを押すと画像と図形の情報がjsonに保存されます。