import numpy as np
import OpenEXR
import Imath
import array
import matplotlib.pyplot as plt
from mpl_toolkits import mplot3d


##filename = 'VR-Caps-Unity/Assets/SavedScreen.exr'
filename = 'C:\\Users\\juranmar7993\\Desktop\\VirtualCapsuleEndoscopy\\VR-Caps-Unity\\Assets\\ScreenShot_EXR\\EXR20230308_colon2_00000.exr'
#filename ='C:\\Users\\juranmar7993\\github\\pushtest\\VirtualCapsuleEndoscopy\\VR-Caps-Unity\\Assets\\ss0.exr'
img_exr = OpenEXR.InputFile(filename)

# Compute the size
dw = img_exr.header()['dataWindow']
sz = (dw.max.x - dw.min.x + 1, dw.max.y - dw.min.y + 1)

# Read the three color channels as 32-bit floats
FLOAT = Imath.PixelType(Imath.PixelType.FLOAT)
# (R,G,B) = [array.array('f', img_exr.channel(Chan, FLOAT)).tolist() for Chan in ("R", "G", "B") ]
(R,G,B) = [np.array(array.array('f', img_exr.channel(Chan, FLOAT))).reshape(sz) for Chan in ("R", "G", "B") ]

# print(sz)

if 0:
    plt.imshow(R)
else:
    fig = plt.figure(figsize=(8,6))
    ax3d = plt.axes(projection="3d")
    x = np.arange(sz[0])
    y = np.arange(sz[1])
    X,Y = np.meshgrid(x,y)
    #print(X.shape, Y.shape, R.shape)
    ax3d.plot_surface(X,Y,R)

plt.show()