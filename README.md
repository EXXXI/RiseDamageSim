# RiseDamageSim

## 概要

モンハンライズ用ダメージ比較ツールα

## 注意点

- 自分用に作っただけなので大剣しか計算できません
- 自分用に作っただけなのでクッソ使いづらいです
- 異常系作ってないので変な入力したら変な結果が出たり落ちたりします
- Windowsでしか動きません(たぶんWin7以上ならOK)
- .Netのインストールが必要です(無ければ起動時に案内されるはず)
- ファイル入出力を行うので環境によってはウィルス対策ソフトが文句を言ってきます

## 使い方

- 装備登録画面で比較したい装備を登録
- 肉質・モーション画面で攻撃する肉質と使うモーションを設定
  - 抜刀力や抜刀技の対象は対象になる確率を0~100で記載
  - 怒り倍率や空棲等の属性も手動入力
- 戦い方画面で各種バフ・スキルの発動率などを設定
  - 不屈1を80%、不屈2を50%にすると、1死30%2死50%で計算
  - 粉塵ホムラチョウビルドアップは独立に計算したうえで同時発動した場合は優先度の高いものだけが効果を発揮

## ライセンス

MIT License

### ↑このライセンスって何？

こういう使い方までならOKだよ、ってのを定める取り決め

今回のは大体こんな感じ

- 誰でもどんな目的でも好きに使ってOK
- でもこれのせいで何か起きても開発者は責任取らんよ
- 改変や再配布するときはよく調べてルールに従ってね

## 使わせていただいたOSS(+必要であればライセンス)

### System.Text.Json

プロジェクト：<https://dot.net/>

ライセンス：<https://github.com/dotnet/runtime/blob/main/LICENSE.TXT>

### Prism.Wpf

プロジェクト：<https://github.com/PrismLibrary/Prism>

ライセンス：<https://www.nuget.org/packages/Prism.Wpf/8.1.97/license>

### ReactiveProperty

プロジェクト：<https://github.com/runceel/ReactiveProperty>

ライセンス：<https://github.com/runceel/ReactiveProperty/blob/main/LICENSE.txt>