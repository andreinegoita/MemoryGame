   M S B A M L   `   `   `  ����  
MemoryGame#    MemoryGame.View.GameBoardView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          >�Name$
  root��5       � 	Resources 5
         TileButtonStyle �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   Color   	Direction   ShadowDepth   Opacity	 �� 
TargetType
 ��Name J� Opacity J� IsHitTestVisible �� Triggers �� Value �� Binding �� 
TargetName��%3  &	        �� 5   
   " ��6(   .�5      ��  � ��$	 280��6&   �� 5       � ��$	 180��6'   �� 5       � ��$ 2q�6'   �� 5       � ��
 ���� 6+   �� 5       � �� �	#FF5555556,   �� 5       � ��$ 2q�60   �� 5       �@�Cursor$
 Hand��6'   �� 5       �@J�Effect 5        5      $ Black��6'   $	 270Q�65   $ 5Q�6E   $	 0.5Q�6U   5      �� 5       � �� 5      �� 5      "	 ��6&   .R�5      �� #��z��6!   #��z��5   !   #��z��5   !   ._�5      � .+�5   "   �� $
 	TileImage��6(   $��Uniform=�5!   (   $ 0Q�5"   (   $ False��5#   (   ��5    (   �� *	ImagePath+5#   (   5$      5%       5&      �� 5(      $
 True��6H    6*   �� *	IsFlipped+.a�5)   "   �� $ 	TileImage��6)    �@J�Opacity$ 1Q�6S   5*      �� 5,      $
 True��6H    6*   �� *	IsMatched+.a�5-   "   ��  � {�$ False��6>   �� 5/   "   $ 	TileImage��6)    �@J�Opacity$ 1Q�6S   50      52      54      55      56      .��58      � .+�59   
   �� ��6   �� *Tiles+��5:      �� 5;      .R�5<      A�  A� Rows 6"   �� *Rows+ A� Columns 68   �� *	Columns+5=      ��5?      �� 5@      .R�5A      �� #��[  5C      ��5A      �� *DataContext.TileClickCommand+ �� ElementName$
 root����5B      �� 5C      5D      5F      5G      5H      