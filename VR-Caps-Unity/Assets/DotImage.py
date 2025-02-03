#ImageDrawモジュールのインポート
from PIL import ImageDraw, Image
height=100
width=100
#新規で画像作成
#画像サイズ (height*10)*(width*10)
img=Image.new('RGBA',(height*10,width*10),'black')
#ImageDrawオブジェクトの生成
d=ImageDraw.Draw(img)
for i in range(height):
    for j in range(width):
        d.point((i*200,j*200),fill='white') #同じ行（列）の点との間隔
for i in range (height):
    for j in range (width):
        I=i*200
        J=j*200
        d.point((I+100,J+100),fill='white')#斜め先の点の点との縦横の距離
img.show()
img.save("DotImage1000_100.png")#画像サイズ_点の間隔