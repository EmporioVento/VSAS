   M S B A M L   `   `   `  ����   labs    labs.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     SCustom@http://schemas.microsoft.com/winfx/2006/xaml/presentation/ribbon     -     �Name$  mainWnd�� � Title$ ПЗКС Сюр А.В.��$	��670��$	��924�� � WindowStartupLocation$ CenterScreen=�.��� -    �Name$ grid1��$	��914��$
��Left=�$	��638��$	��Top=�.+�o� $
��Left=�$	��Top=�$	��963�� K� ZIndex$ 10��.@�m� -   $&��   Граф задачі / КС  ��m� -   $��  Моделювання ��m� $��  Статистика  ��m� $��  Допомога    ��m� -   $��  Вихід   ��� -   $ 
GraphPanel��
�������$
��Left=�$	��Top=�$	��189��$
��1000��.+��� -    ��Name$ btnAdd��$��Нова вершина��$
��Left=�$	��Top=�$	��142��$��	23,62,0,0q�$��30���� -   $ btnAddR��$��Нова дуга��$
��Left=�$��
23,144,0,0q�$	��Top=�$	��142��$��30���� -	   $ btnDeleteNode��$%��Видалити вершину��$
��Left=�$��	23,92,0,0q�$	��Top=�$	��142��$��30���� -
   $��Запис у файл��$
��Left=�$��
23,445,0,0q�$	��Top=�$	��142��$��30���� -   $.��(Завантаження із файлу��$
��Left=�$��
23,475,0,0q�$	��Top=�$	��142�� J� RenderTransformOrigin$ 0.5,0.5&�$��30���� -   $ 0.5,0.5&�$��Очистити��$
��Left=�$��
23,572,0,0q�$	��Top=�$	��142��$��30���� -   $ btnDeleteEdge��$��Видалити дугу��$
��Left=�$	��Top=�$	��142��$��
23,174,0,0q�$��30���� -   $ 	Connected��$1��+Перевірка на зв'язність��$
��Left=�$��
23,226,0,0q�$	��Top=�$	��142��$��30���� -   $ 	Algorithm��$��Алгоритми��$
��Left=�$��
23,256,0,0q�$	��Top=�$	��142��$��30���� -    ��Name$ tabCntrl��$��
189,21,0,0q�$ 20��.@��� -    ��Name$ tabTask������ $��3q��� $�Граф задачі��.���� -   	 ��Name$	 canvas��$
��Left=�$	��579��$	��Top=�$	��713���� -   $ tabCS������ $��3q��� $�Граф системи��.���� -   $	 canvasCS��$
��Left=�$	��579��$	��Top=�$	��713��� -   $ 
ModelPanel��
�������$
��Left=�$	��Top=�$	��189��$
��1000��$y�Hidden=�.+��� $0��*Параметри процесорів...��$
��Left=�$��	22,77,0,0q�$	��Top=��� $$��Діаграма Ганта...��$
��Left=�$��
22,244,0,0q�$	��Top=�$	��142��$��30���� $��Статистика...��$
��Left=�$��
22,440,0,0q�$	��Top=�$	��142��$��30��