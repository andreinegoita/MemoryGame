   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MemoryGame.ViewMemoryGame.View  5      %    MemoryGame.View.StartUpMenuView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:MemoryGame.View     5      .��5	      � .+�5
   
   �� $ ��/Assets/MemoryGameMenu.jpg��6   $
��Fill=�5      �� 5   
   $��Welcome  Enjoy this game!��6   $��40(�5      $
��Bold"�5      $��Javanese Text)�5      
���<��$��Center=�5      $	��Top=�5      $��85��5      $	��628��5      $��0,30,0,0q�5        J� RenderTransformOrigin$  0.865,0.965&�6&   �� 5   
   $
��Fill=�61   $	��409��6@   $	��277��6L   $
��Left=�5      $	��Top=�6+   $��638,326,0,0q�5      ��5      �� *SelectedImage+5      �� 5   
   $��<<��6   $
��Left=�5      $	��Top=�6,   $��56��5      $��30��6   $��700,660,0,0q�6(   $  	6.75,11.2&�6=   ��5   $   �� *PreviousImageCommand+5   =   �� 5    
   $��>>��6   $��Right=�5!      $	��Top=�6-   $��56��5"      $��30��6   $��0,655,630,0q�6(   ��5       �� *NextImageCommand+5"   (   �� 5$   
    �� Orientation$ 
Horizontal=�6   $
��Left=�6.   $	��Top=�6I   $��728,737,0,0q�6a   .+�5%      �� $��New User��6   $��80��6K   $��5q�6V   ��6(   �� *AddUserCommand+6V   �� 5&      $��Delete User��6   $��80��6�   $��5q�6�   ��6+   �� *DeleteUserCommand+{�6R   �� *SelectedUser+   NullToBooleanConverter �� 	Converter# [  6�   �� 5'      $
��Play��6   $��80��6D   $��5q�6O   ��6$   �� *PlayCommand+6O   �� 5(      $��Cancel��6   $��80��6H   $��5q�6S   ��6&   �� *CancelCommand+6S   5)      �� 5+   
    �� DisplayMemberPath$
 Name��5-   
   $
��Left=�5.   
   $��Center=�5/   
   
������ 
������ 
�������$��20(�53   
   $��Italic$�54   
   $
��Bold"�55   
   $	��200��56   
   $	��238��6   $��10,0,0,0q�57   
   ��5+      �� *Users+ �� SelectedItem 5,   
   �� *SelectedUser+ �� Mode$ TwoWay=�57   
   5>      5?      