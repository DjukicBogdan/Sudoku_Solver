   M S B A M L   `   `   `  ���� A  =Sudoku, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:SudokuSudoku      Sudoku.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     &localclr-namespace:Sudoku       � Title$  Sudoku��$	��689��$
��1016�� � Icon$ 	smile.ico�� � 	Resources    StyleStackPanel �� 
TargetType �� Property �� Value  StyleTextBox  StyleButton��%  &	        &	 �     &	 �    �� " ��.���  �@��Orientation$ 
Horizontal=���  � ��$	 180����  � ��$	 180����  � ��
 �������  � ��$
 Left=���  � ��$	 Top=���  � ��$ 5q��� " ��.���  � ��$ 58����  � ��$ 58����  � ��$ 30(���  � ��$
 Bold"���  �@��TextAlignment$ Center=���  � ��
 �������  � ��$
 Left=���  � ��$	 Top=���  � ��$ 1,0,1,0q���  � ��$ 0q��� " ��.���  � ��
 �������  � ��$ 90����  � ��$ 90����  � ��$ 1,1,1,1q���  � ��$ 30(���  � ��$
 Bold"�.��� -    �Name$ MainGrid��
�������$
��1000��$	��650��$	��Top=�$
��Left=�$��0q�.+��� 
���   �$
��Left=�$	��Top=�$��650,40,10,10q�.+��� -    ��Name$ TimeTextBlock��$��00:00��
�������$��Stretch=� �� TextAlignment$ Center=�$	��Top=�$��5,5,5,5q�$��30(�$
��Bold"�$	��100���� -   	 ��Name$	 ButtonNewGame��$��New game��$
��Bold"�$
��Left=�$	��Top=�$��
836,40,0,0q�
���/���$��36��$��91���� -   $	 ButtonClearTable��$��Clear��$
��Bold"�$
��Left=�$	��Top=�
���/���$��36��$��64��$��
765,40,0,0q��� -   $	 ButtonSolve��$��
Show hints��
���   �$��5q�$	��Top=�$
��Left=�$��650,563,0,0q�$��30(�$
��Bold"�$	��285���� -   $	 ButtonSolveAlgoritam��$��Solve��$y�Visible=�
���   �$��5q�$	��Top=�$
��Left=�$��650,503,0,0q�$��30(�$
��Bold"�$	��285���� $��20q�
���   �$��Stretch=�$��Stretch=�
 K� ZIndex$
 -1���� -    ��Name$ MainStackPanel�� �� Orientation$ 
Horizontal=�
�������$
 -1��$	��570��$	��575��$
��Left=�$	��Top=�$��40q�.+��� -   $ StackPanelField1��$ Vertical=��� -	   $ StackPanelField2��$ Vertical=��� -
   $ StackPanelField3��$ Vertical=��� -   $ KeyBoardStackPanel��$ Vertical=�
�������$	��276��$	��368��$
��Left=�$	��Top=�$��650,115,0,0q�.+��� -   $ StackPanelKey1��$ 
Horizontal=��� -   $ StackPanelKey2��$ 
Horizontal=��� -   $ StackPanelKey3��$ 
Horizontal=��� -   $ StackPanelKeyDel��$ 
Horizontal=�$��Stretch=�� -   $ 
GridRecord��
����  �$y�Hidden=�$
 1��.+��� -   $	 ButtonClose��$��Close��$��Center=�$��	0,0,0,115q�$��Bottom=�$��76��&   System.Windows.Controls.DataGrid  -     Name$ DataGridRecord��$��5,5,5,5q�   HeadersVisibility$ Column=�   
IsReadOnly . $��22(�$
��Bold"�   AutoGenerateColumns .   �� AlternationCount$ 2��   AlternatingRowBackground
 �����$��Stretch=�$	��381��$��10,10,10,30q�$��Stretch=����� *NewlistRecord+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$  ��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Number+  $
 Name��$ *  �� *Name+  $
 Time��$ *  �� *Time+   RowStyle �� )  #System.Windows.Controls.DataGridRow"   �� Triggers P�  P� Property �@ 
IsSelected P� Value . . ���  � ��
 �  ����  � ��
 �  ��  �� Color��%�   (��       * SystemColors.HighlightBrushKey+)(��      *1/SystemColors.InactiveSelectionHighlightBrushKey+)�� $	 Red���� $	 Red��