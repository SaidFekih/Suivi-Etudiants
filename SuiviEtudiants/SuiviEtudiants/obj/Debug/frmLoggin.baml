   M S B A M L   `   `   `  ����I  ESuiviEtudiants, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SuiviEtudiantsSuiviEtudiants  5   	       SuiviEtudiants.frmLoggin   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:SuiviEtudiants     5   	   -   5          � Title$  Accès��5   	   $	��171��6   $	��507��6$    � WindowStartupLocation$ CenterScreen=�60    � 
ResizeMode$ NoResize=�6U   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��10 �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��50 �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5!      $��* �6   .+�5$   
   �� $2��,/SuiviEtudiants;component/Resources/Clé.png��6   $��1��6E   $��0��6U   $��6��6b   �� 5%   
   $��_Nom d'utilisateur :��6   $��2��6/   $��1��6?   $��Right=�6L   $	��0,2q�6h    �� Target 6u   ��  �� ElementName$ txtUtilisateur���� 5&   
   -   5         ��Name$ txtUtilisateur��5&      $��3��6*   $��1��6:   $	��150��6G   $	��0,2q�6S   $��Center=�6`   6{   �� 5'   
   $��_Mot de passe :��6   $��2��6+   $��2��6;   $��Right=�6H   $	��0,2q�6d    6q   �� $ txtMotPasse��G� 5(   
   -   5         G�Name$ txtMotPasse��5(      $��3��6+   $��2��6;   $	��150��6H   $	��0,2q�6T   $��Center=�6a   6|   � 5)   
   $��3��6   $��4��6$   .+�5*      �� -   5        	 ��Name$	 btnOk��5*      $��2��6$   $��4��64   $	��_OK��6A   $��50��6O   $	��0,2q�6Z   
 �� 	IsDefault
 . {�.  6�   �� 5+      -   5        $	 
btnAnnuler��5+      $��3��6)   $��4��69   $��_Annuler��6F   $��50��6Y   $��50,2,0,2q�6d    �� IsCancel . 6�   5,      5-      5.      