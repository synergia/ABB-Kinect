   M S B A M L   `   `   `  ����   
ABB Kinect    ABB_Kinect.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     �Name$  ABB_Kinect_Main_Window�� � Title$ 
ABB Kinect��$	��200��$	��640�� � 
ResizeMode$ NoResize=� � Icon$ industrial_robot_sh.ico��.��� $
��Left=�$	��171��$	��Top=�$	��634��.+��� -    ��Name$ ListOfDevices��$
��Left=�$	��630��$��0,0,0,35q� J� RenderTransformOrigin$ -0.008,-0.544&� �� View ��  �� ColumnHeaderStringFormat$  �� �� ColumnHeaderContextMenu �� .P��� $��
IP Address��	 �� Width$		 100��
 �� DisplayMemberBinding
 �� *	IPAddress+�� $��ID��$	 50��
 �� *ID+�� $��Availability��$	 80��
 �� *Availability+�� $��Virtual��$	 60��
 �� *	IsVirtual+�� $��System name��$		 120��
 �� *
SystemName+�� $��
RW Version��$	 80��
 �� *	Version+�� $��Controller Name��$		 100��
 �� *ControllerName+�� -    ��Name$ ConnectButton��$��Connect��$
��Left=�$��468,141,0,0q�$	��Top=�$��76���� -   $ 
ExitButton��$
��Exit��$
��Left=�$��549,141,0,0q�$	��Top=�$��75��
���  ���� -    ��Name$ StatusTextBlock��$
��Left=�$��
10,145,0,0q�$
��Wrap=�$��Status��$	��Top=�$	��200��$
��Bold"��� -   $ DisconnectButton��$��
Disconnect��$
��Left=�$��388,141,0,0q�$	��Top=�$��75��