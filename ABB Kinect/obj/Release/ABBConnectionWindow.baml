   M S B A M L   `   `   `  ����   
ABB Kinect$    ABB_Kinect.ABBConnectionWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  ABBConnectionWindow��$	��480��$	��640�� � 
ResizeMode$ NoResize=�.��� .+�� $
��Left=�$	��431��$��	10,10,0,0q�$	��Top=�$	��614��.+��� -    ��Name$ 	ABB_Image��$
��Left=�$	��231��$��
442,10,0,0q�$	��Top=�$	��162��$#��Resources\handling-robots.jpg���� $
��Left=�$��58��$��
10,363,0,0q�$	��Top=�$	��255��$��Resources\Logo_big.png���� -    ��Name$ ABBWindowTitleLabel��$
��Left=�$
��Wrap=�$	��Top=�$��27��$	��427��$��10,0,0,0q�$��20(�$
��Bold"���  � Language$ en-gb�$�Connected to ���� -   $ ABBWindowNameLabel��$
��Left=�$��	10,32,0,0q�$
��Wrap=�$	��Top=�$	��427���� $ en-gb�$�Controller Name���� -    ��Name$ ResetPositionButton��$��Reset Position��$
��Left=�$��	10,53,0,0q�$	��Top=�$��90��$��Italic$����	#FF087A03�� -    ��Name$ 
TabControl��$
��Left=�$	��285��$��	10,73,0,0q�$	��Top=�$	��432��.@��� $��Manual Mode��$
��-2,0q� J� RenderTransformOrigin$ 1.746,0.545&�.��� ���	#FFE5E5E5.+��� $��Joint 1��$
��Left=�$��	10,10,0,0q�$	��Top=�$ -0.289,-0.346&��� $��Joint 2��$
��Left=�$��	10,36,0,0q�$	��Top=��� $��Joint 3��$
��Left=�$��	10,62,0,0q�$	��Top=��� $��Joint 4��$
��Left=�$��	10,88,0,0q�$	��Top=��� $��Joint 5��$
��Left=�$��
10,114,0,0q�$	��Top=��� $��Joint 6��$
��Left=�$��
10,140,0,0q�$	��Top=��� -    ��Name$ Joint1Slider��$
��Left=�$��	55,13,0,0q�$	��Top=�$	��306���� -   $ Joint2Slider��$
��Left=�$��	55,41,0,0q�$	��Top=�$ -11.75,-0.833&�$	��306���� -	   $ Joint3Slider��$
��Left=�$��	55,67,0,0q�$	��Top=�$	��306���� -
   $ Joint4Slider��$
��Left=�$��	55,93,0,0q�$	��Top=�$	��306���� -   $ Joint5Slider��$
��Left=�$��
55,119,0,0q�$	��Top=�$	��306���� -   $ Joint6Slider��$
��Left=�$��
55,145,0,0q�$	��Top=�$	��306���� -   $ Joint1TextBlock��$
��Left=�$��
366,13,0,0q�$
��Wrap=�$��	TextBlock��$	��Top=��� -   $ Joint2TextBlock��$
��Left=�$��
366,41,0,0q�$
��Wrap=�$��	TextBlock��$	��Top=��� -   $ Joint3TextBlock��$
��Left=�$��
366,67,0,0q�$
��Wrap=�$��	TextBlock��$	��Top=��� -   $ Joint4TextBlock��$
��Left=�$��
366,93,0,0q�$
��Wrap=�$��	TextBlock��$	��Top=��� -   $ Joint5TextBlock��$
��Left=�$��366,119,0,0q�$
��Wrap=�$��	TextBlock��$	��Top=��� -   $ Joint6TextBlock��$
��Left=�$��366,145,0,0q�$
��Wrap=�$��	TextBlock��$	��Top=��� -   	 ��Name$	 DynamicModeCheckBox��$��Dynamic Mode��$
��Left=�$��
55,174,0,0q�$	��Top=�{�.  �� $��Kinect Mode��.��� ���	#FFE5E5E5
 � ColumnDefinitions
 �� $	��55* ��� $
��371* �.+��� -   $ KinectImageControl��$
��Left=�$	��237��$��	10,10,0,0q�$	��Top=�$	��316��$��2���� -   $ TryAgainButton��$��Connect��$��1��$
��Left=�$��286,227,0,0q�$	��Top=�$��75��{�.  �� -   $ KinectNotFoundTextBlock��$��1��$
��Left=�$��286,209,0,0q�$
��Wrap=�$	��Top=�$��8(�
���  ���� $ en-gb�$�Kinect disconnected���� $	��Arm��$��1��$
��Left=�$��
271,10,0,0q�$	��Top=�$ -3.105,0.577&�$
��Bold"��� -   $ ArmAngleTextBlock��$��1��$
��Left=�$��
336,11,0,0q�$
��Wrap=�$��0��$	��Top=�$��35��$ 3,-1.378&�$��16(��� $
��Body��$��1��$
��Left=�$��
271,36,0,0q�$	��Top=�$ -3.105,0.577&�$
��Bold"��� -   $ BodyAngleTextBlock��$��1��$
��Left=�$��
336,37,0,0q�$
��Wrap=�$��0��$	��Top=�$��35��$ 3,-1.378&�$��16(��� $��Elbow��$��1��$
��Left=�$��
271,62,0,0q�$	��Top=�$ -3.105,0.577&�$
��Bold"��� -   $ ElbowAngleTextBlock��$��1��$
��Left=�$��
336,63,0,0q�$
��Wrap=�$��0��$	��Top=�$��35��$ 3,-1.378&�$��16(��� $��Rotation��$��1��$
��Left=�$��
271,88,0,0q�$	��Top=�$ -3.105,0.577&�$
��Bold"��� -   $ ElbowRotTextBlock��$��1��$
��Left=�$��
336,89,0,0q�$
��Wrap=�$��0��$	��Top=�$��35��$ 3,-1.378&�$��16(�A� -    A�Name$ GoLED��$��1��
���  ��$
��Left=�$��48��$��304,156,0,0q�
���   �$	��Top=�$��48��$ 0.6,1.35&��� $	��GO?��$��1��$
��Left=�$��304,122,0,0q�$	��Top=�$��20(�$
��Bold"��� $
��Left=�$	��112��$��491,246,0,0q�$	��Top=�$	��113��$��Resources\logo_wmech.gif��$ 0.923,0.448&��� $
��Left=�$��58��$��329,363,0,0q�$	��Top=�$	��275��$��Resources\Pwr-logo.png�� �� BitmapScalingMode$ HighQuality=�$��1���� -    ��Name$ ReadyLED��
���   �
���  ��$��1q�$
��Left=�$��20��$��
417,73,0,0q�$	��Top=�$��20���� $��Created by Marcin Twardak��$
��Left=�$��
243,72,0,0q�$	��Top=�$	��169��$
��Bold"��� -    ��Name$ SpeedTxtControl��$
��Left=�$��25��$��
339,42,0,0q� �� TextWrapping$
 Wrap=�$��25��$	��Top=�$��32��$ -0.213,0.348&��� $��Speed (0 - 100)��$
��Left=�$��
243,42,0,0q�$	��Top=�$ 0.473,0.577&�