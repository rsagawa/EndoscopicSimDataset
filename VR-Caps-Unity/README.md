RGB画像と深度データを生成するための機能追加
=====


## 概要
・VR-Capsの3Dモデルから自動でカメラパスに沿ってRGB画像またはDepthデータの取得を可能にした  

・カメラパスを任意に作成も可能  (デフォルトでは事前に作成したカメラパスを使用)  

## 開発環境
・Unity (version: 2019.3.3f1)  
・Unity Hub  
・Anaconda  
・Python 3.10  

## Getting Started

### 1. Installation
#### Clone the Repository
```sh
git clone https://github.com/CapsuleEndoscope/VirtualCapsuleEndoscopy/our_project.git
```

### 2. 画像生成  

#### RGB画像の生成  

1. RGBSave.csにチェックして有効を確認  

2. 保存先のパスを記入  

3. 実行ボタン(再生マーク)を押すと自動でカメラパスに沿って撮影を開始  

   指定先のパスにRGB画像(.png)が保存される  


#### 深度画像の生成  

1. Project SettingからポストエフェクトにDepthExampleを指定  

2. DepthSave.csにチェックして有効を確認  

3. 保存先のパスを記入  

4. 実行ボタン(再生マーク)を押すと自動でカメラパスに沿って撮影を開始  

   指定先のパスに深度画像データ(.exr)が保存される  


### 3. パターンの投影  

1. RGBSave.csにチェックして有効を確認  
   
2. Patternのチェックボックスを有効  

3. Camera PreViewで投影を確認  

##### パターン投影の詳細設定  

1. Hierarchy Window>Capsule>Camera>SpotLightを選択 
 
2. Inspector Window>Light>General>ModeでBakedを指定  

3. Inspector Window>Light>Shape>Coneを指定  

4. Radiusで光源のサイズ調整  

5. Inspector Window>Light>Emission>Colorで色指定  

6. Inspector Window>Light>Emission>CookieでAssetsの中のパターン画像を指定  


### 深度データの確認方法  

1. 下記モジュールをインストール  
・numpy  
・openEXR  
・matplotlib  
```sh
conda install numpy
conda install -c conda-forge openexr-python
pip install matplotlib
```  

2. 確認に使うexr画像ファイルの上でShift+右クリックしパスのコピーを選択  

3. コピーしたパスをtest_exr.py内のfilename変数に指定  

4. コマンドプロンプト等でtest_exr.pyを実行  







