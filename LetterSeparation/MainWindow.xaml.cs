using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string letters, symbols;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Возвращает строку уникальных символов из text
        private string SymbolsSeparation(string text)
        {
            text = new string(text.ToLower().Distinct().ToArray());

            /*text = text.ToLower();
             
            char[] allLettets = new char[300];
            int n = 0;

            for (int i = 0; i < text.Length; i++)
            {
                bool hit = false;
                for (int j = 0; j < n; j++)
                {
                    if (allLettets[j] == text[i])
                    {
                        hit = true;
                        break;
                    }
                }
                if (hit) continue;
                allLettets[n] = text[i];
                n++;
            }

            string allLettersString = new string(allLettets,0,n);*/
            return text;
        }

        // Возвращает те символы из text, которых нет в letters
        private string LettersSeparation(string text, string letters)
        {
            text = text.ToLower();

            char[] symbols = new char[text.Length];

            int n = 0;

            for (int i = 0; i < text.Length; i++)
            {
                bool hit = false;
                for (int j = 0; j < letters.Length; j++)
                {
                    if (letters[j] == text[i])
                    {
                        hit = true;
                        break;
                    }
                }
                if (hit) continue;
                symbols[n] = text[i];
                n++;
            }

            string symbolsString = new string(symbols, 0, n);
            return symbolsString;
        }

        private void TextBoxInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxSymbols.Text = LettersSeparation(SymbolsSeparation(TextBoxInput.Text), letters);
        }

        private void TextBoxLetters_TextChanged(object sender, TextChangedEventArgs e)
        {
            letters = SymbolsSeparation(TextBoxLetters.Text);
            TextBoxLetters.Text = letters;
            TextBoxSymbols.Text = LettersSeparation(SymbolsSeparation(TextBoxSymbols.Text), letters);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string allText = TextBoxInput.Text.ToLower();
            int n = letters.Length;
            int[,] table = new int[n, n];
            int[] sumL = new int[n];
            int[] sumR = new int[n];
            int[] mapp = new int[n];
            int mark = 0; // Указывает на первую согласную


            char a, b;
            // Строим таблицу повторений определенной пары букв
            for (int i = 0; i < allText.Length - 1; i++)
            {
                a = allText[i];
                b = allText[i + 1];
                for (int j = 0; j < n; j++)
                    if (letters[j] == a)
                        for (int k = 0; k < n; k++)
                            if (letters[k] == b)
                            {
                                table[j, k]++;
                                table[k, j]++;
                            }
            }
            // Исключаем главную диагональ
            for (int j = 0; j < n; j++)
            {
                table[j, j] = 0;
                mapp[j] = j;
            }
            do
            {
                // Суммируем числа в строках
                for (int i = mark; i < n; i++)
                {
                    sumL[i] = 0;
                    sumR[i] = 0;
                    for (int j = 0; j < mark; j++)
                        sumL[i] = sumL[i] + table[mapp[i], mapp[j]];
                    for (int j = mark; j < n; j++)
                        sumR[i] = sumR[i] + table[mapp[i], mapp[j]];
                }
                // Ищем максимальное
                int bufer, maxi = mark;
                for (int i = mark; i < n; i++)
                    if ((sumR[maxi] - sumL[maxi]) < (sumR[i] - sumL[i]))
                        maxi = i;
                if ((sumR[maxi] - sumL[maxi]) < 0) break; // При отрицательной наибольшей решающей разнице заканчиваем

                // Меняем символы местами
                bufer = mapp[mark];
                mapp[mark] = mapp[maxi];
                mapp[maxi] = bufer;
                mark++;
            } while (mark<n);

            string type1="", type2="";
            for (int i = 0; i < mark; i++)
                type1 += letters[mapp[i]];
            for (int i = mark; i < n; i++)
                type2 += letters[mapp[i]];
            LabelType1count.Content = type1.Length;
            LabelType2count.Content = type2.Length;
            LabelType1.Content = type1;
            LabelType2.Content = type2;


        }
    }
}
