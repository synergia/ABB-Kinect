   M S B A M L   `   `   `  ����  
ABB Kinect    ABB_Kinect.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          �Name$  ABB_Kinect_Main_Window��5   	    � Title$ 
ABB Kinect��5   	   $	��200��6   $	��640��6)    � 
ResizeMode$ NoResize=�65    � Icon$ industrial_robot_sh.ico��6K   .��5      � $
��Left=�6   $	��171��6#   $	��Top=�60   $	��634��6H   .+�5      �� -   5         ��Name$ ListOfDevices��5      $
��Left=�6$   $	��630��6?   $��0,0,0,35q�6K    J� RenderTransformOrigin$ -0.008,-0.544&�6]    �� View 5      �� 5       �� ColumnHeaderStringFormat$  ��6    �� ColumnHeaderContextMenu 5	      �� 5
      .P�5      �� $��
IP Address��6   	 �� Width$		 100��6U   
 �� DisplayMemberBinding
 6*   �� *	IPAddress+6U   �� 5      $��ID��6   $	 50��6F   
 6"   �� *ID+6F   �� 5      $��Availability��6   $	 80��6Z   
 6,   �� *Availability+6Z   �� 5      $��Virtual��6   $	 60��6R   
 6'   �� *	IsVirtual+6R   �� 5      $��System name��6   $		 120��6W   
 6+   �� *
SystemName+6W   �� 5      $��
RW Version��6   $	 80��6S   
 6*   �� *	Version+6S   �� 5      $��Controller Name��6   $		 100��6_   
 6/   �� *ControllerName+6_   5      5      �� 5      -   5         ��Name$ ConnectButton��5      $��Connect��6"   $
��Left=�64   $��468,141,0,0q�6O   $	��Top=�6d   $��76��6|   6�   �� 5      -   5        $ 
ExitButton��5      $
��Exit��6   $
��Left=�6.   $��549,141,0,0q�6I   $	��Top=�6^   $��75��6v   
���  ��6�   �� 5      -   5         ��Name$ StatusTextBlock��5      $
��Left=�6'   $��
10,145,0,0q�6B   $
��Wrap=�6V   $��Status��6j   $	��Top=�6x   $	��200��6�   $
��Bold"�6�   �� 5      -   5        $ DisconnectButton��5      $��
Disconnect��6%   $
��Left=�6:   $��388,141,0,0q�6U   $	��Top=�6j   $��75��6�   6�   5      5      