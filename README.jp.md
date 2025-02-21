内視鏡シミュレーション映像データセット
=====


# 概要
本リポジトリは、カプセル内視鏡用のシミュレーション環境である[VR-Caps](https://github.com/CapsuleEndoscope/VirtualCapsuleEndoscopy)に基づいて、消化器内部のシミュレーション画像データを生成するインターフェースを提供するものである。
本レポジトリでは、VR-Capsに下記の２ステップを追加し画像生成を行う方法を実装した。
EMBC2024において発表した論文[View Synthesis of Endoscope Images by Monocular Depth Prediction and Gaussian Splatting](https://ieeexplore.ieee.org/abstract/document/10782148)で利用したデータセットは[ダウンロード](#ダウンロード)を参照。

・UnityのGUIを利用した任意カメラパスの作成  
・カメラパスに沿ったRGB画像またはDepth画像の生成  

# セットアップ

## 開発環境
・Unity version: 2019.3.3f1  
・Unity Hub  
・Anaconda  
・Python 3.10  

## レポジトリのクローン
```sh
git clone https://github.com/TakuyaTorii0806/VR-Caps_for_image_generation.git  
```  

## プロジェクトの起動  
VR-Caps-Unity / Assets / Scenes / Record_scene.unityを起動 


# カメラパスの生成  

## GUIを用いたカメラパスの記録
1. Hierarchy Window > Capsule > Cameraを選択  
2. Inspector Window > CameraMover のチェックボックスを有効  
3. Inspector Window > CameraPathSave のチェックボックスを有効  
4. Inspector Window > CameraPathSave > Save Path にcsvファイルのパスを指定  
   ![setting](readme_imgs/Unity_CameraPath_all.png)
5. 実行ボタンを押下すると、マウスとキー操作によるカメラ移動が可能  

   ・W : 前方向, S : 後方向, A : 左方向, D : 右方向  
   ・Q : 上昇, E : 下降  
   ・マウスのドラッグ : 任意回転  

6. スペースキー押下でカメラパスの記録開始  
7. 再度スペースキー押下でカメラパスの記録終了  
   -> 保存先にcsvファイルを生成  


# 画像データ生成

## 作成したカメラパスの指定方法  
### 1つのカメラパスから生成する場合
1. Hierarchy Window > Capsule > Cameraを選択  
2. Inspector Window > Depth Save > Load Camera Pose Path にcsvファイルのパスを指定  
3. Inspector Window > RGB Save > Load Camera Pose Path にcsvファイルのパスを指定  
   ![setting](readme_imgs/Unity_select_camerapath_all.png)

### 複数のカメラパスからまとめて生成する場合
1. VR-Caps-Unity / Assets / Resources に複数のカメラパスcsvファイルを置く
2. Hierarchy Window > Capsule > Cameraを選択  
3. Inspector Window > Depth Save > Load Camera Pose Path を空に指定 (何も記載しない)  
4. Inspector Window > RGB Save > Load Camera Pose Path を空に指定 (何も記載しない)  


## RGB画像の生成  
1. Hierarchy Window > Capsule > Cameraを選択  
2. Inspector Window > RGB Save のチェックボックスを有効  
3. Inspector Window > RGB Save > Save Folder Path に保存先のフォルダパスを指定  
   ![setting](readme_imgs/Unity_figure_RGB_all.png)
4. 実行ボタン(再生マーク)を押すと自動でカメラパスに沿って撮影を開始  
   -> 保存先にRGB画像(.png)を生成
   
## 深度画像の生成  
1. Edit Tab > Project Setting > HDRP Default Settings > After Post Process > DepthExampleを指定
   ![setting](readme_imgs/Unity_figure_Depth_edit_tab_all.png)
   ![setting](readme_imgs/Unity_figure_Depth_after_post_process_all.png)
2. Hierarchy Window > Capsule > Cameraを選択  
3. Inspector Window > Depth Save のチェックボックスを有効  
4. Inspector Window > Depth Save > Save Folder Path に保存先のフォルダパスを指定  
   ![setting](readme_imgs/Unity_Depth_all.png)  
5. 実行ボタンを押すと自動でカメラパスに沿って撮影を開始  
   -> 保存先に深度画像データ(.exr)を生成  

## 深度データの確認方法  

1. 下記モジュールをインストール  
・numpy  
・openEXR  
・matplotlib  
```sh
conda install numpy
conda install -c conda-forge openexr-python
pip install matplotlib
```  

2. exrファイルのパスをコピー  

3. コピーしたパスを VR-Caps-Unity/Assets/test_exr.py 内の filename 変数に指定  

4. コマンドプロンプト等でtest_exr.pyを実行  
![fig](readme_imgs/txt_exr.png)

# ダウンロード
これまでに生成したデータは、下記からダウンロード可能である。
```sh

```  

# Reference
```
@inproceedings{masuda2024view,
  title={View Synthesis of Endoscope Images by Monocular Depth Prediction and Gaussian Splatting},
  author={Masuda, Takeshi and Sagawa, Ryusuke and Furukawa, Ryo and Kawasaki, Hiroshi},
  booktitle={2024 46th Annual International Conference of the IEEE Engineering in Medicine and Biology Society (EMBC)},
  pages={1--6},
  year={2024},
  organization={IEEE}
}
```




