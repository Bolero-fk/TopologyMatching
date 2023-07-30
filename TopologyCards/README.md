#トポロジー神経衰弱 データセット
このディレクトリにはトポロジー神経衰弱ゲーム用の画像データとjsonファイルが含まれています。

##ディレクトリ構成
以下のようなディレクトリ構成となっています。

```
./
│
├── images/
│    ├── image1.svg
│    ├── image2.svg
│    ├── image3.svg
│    ...
│
├── cards.json
│
└── README.md

```

##ファイル説明
###images/
神経衰弱ゲームに使用する画像データが格納されています。
画像には
1. [デジタル庁のイラストレーション・アイコン素材](https://www.digital.go.jp/policies/servicedesign/designsystem/Illustration_Icons/)
2. [Google Fonts Noto Sans](https://fonts.google.com/noto/fonts?query=Noto+Sans)をsvg画像に変換したもの

が使用されています。

ライセンス情報
デジタル庁のイラストレーション・アイコン素材: [イラストレーション・アイコン素材利用規約](https://www.digital.go.jp/policies/servicedesign/designsystem/Illustration_Icons/terms_of_use/)
Google Fonts Noto Sans: [SIL Open Font License (OFL)](https://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=OFL)
ライセンスの詳細は各ライセンス提供元のウェブサイトやドキュメンテーションを参照してください。

### cards.json
cards.jsonファイルはゲームの進行に必要な情報を記載しています。各画像のファイル名、画像に含まれる図形の穴の数を記載しています。

