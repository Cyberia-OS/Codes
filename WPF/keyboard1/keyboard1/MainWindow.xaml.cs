using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KeyboardTrainer
{
    public partial class MainWindow : Window
    {
        private string _target = "";
        private int _currentIndex = 0;
        private bool _isRunning = false;

        private List<Border> _charBorders = new List<Border>();

        private Dictionary<Key, char> _keyChar = new Dictionary<Key, char>();
        private Dictionary<Key, Border> _keyBorder = new Dictionary<Key, Border>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuildKeyMap();
        }

        private void BuildKeyMap()
        {
            AddKey(Key.Q, 'q', key_q);
            AddKey(Key.W, 'w', key_w);
            AddKey(Key.E, 'e', key_e);
            AddKey(Key.R, 'r', key_r);
            AddKey(Key.T, 't', key_t);
            AddKey(Key.Y, 'y', key_y);
            AddKey(Key.U, 'u', key_u);
            AddKey(Key.I, 'i', key_i);
            AddKey(Key.O, 'o', key_o);
            AddKey(Key.P, 'p', key_p);
            AddKey(Key.A, 'a', key_a);
            AddKey(Key.S, 's', key_s);
            AddKey(Key.D, 'd', key_d);
            AddKey(Key.F, 'f', key_f);
            AddKey(Key.G, 'g', key_g);
            AddKey(Key.H, 'h', key_h);
            AddKey(Key.J, 'j', key_j);
            AddKey(Key.K, 'k', key_k);
            AddKey(Key.L, 'l', key_l);
            AddKey(Key.Z, 'z', key_z);
            AddKey(Key.X, 'x', key_x);
            AddKey(Key.C, 'c', key_c);
            AddKey(Key.V, 'v', key_v);
            AddKey(Key.B, 'b', key_b);
            AddKey(Key.N, 'n', key_n);
            AddKey(Key.M, 'm', key_m);

            AddKey(Key.D1, '1', key_1);
            AddKey(Key.D2, '2', key_2);
            AddKey(Key.D3, '3', key_3);
            AddKey(Key.D4, '4', key_4);
            AddKey(Key.D5, '5', key_5);
            AddKey(Key.D6, '6', key_6);
            AddKey(Key.D7, '7', key_7);
            AddKey(Key.D8, '8', key_8);
            AddKey(Key.D9, '9', key_9);
            AddKey(Key.D0, '0', key_0);

            _keyBorder[Key.Space] = key_space;
        }

        private void AddKey(Key key, char ch, Border border)
        {
            _keyChar[key] = ch;
            _keyBorder[key] = border;
        }

        private void sliderLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbSliderValue != null)
                tbSliderValue.Text = ((int)sliderLength.Value).ToString();
        }

        //start
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            _target = GenerateTarget();
            _currentIndex = 0;

            BuildCharBorders();

            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            sliderLength.IsEnabled = false;

            _isRunning = true;
        }

        //Stop 
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            StopSession();
        }

        private void StopSession()
        {
            _isRunning = false;
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            sliderLength.IsEnabled = true;
        }

        //генерация строки
        private string GenerateTarget()
        {
            string pool = "qwertyuiopasdfghjklzxcvbnm1234567890";
            int length = (int)sliderLength.Value;
            var rnd = new Random();
            var sb = new StringBuilder();

            for (int i = 0; i < length; i++)
                sb.Append(pool[rnd.Next(pool.Length)]);

            return sb.ToString();
        }

        //построение визуальных индикаторов символов 
        private void BuildCharBorders()
        {
            wpChars.Children.Clear();
            _charBorders.Clear();

            foreach (char c in _target)
            {
                var tb = new TextBlock
                {
                    Text = c.ToString(),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                var border = new Border
                {
                    Width = 26,
                    Height = 30,
                    Margin = new Thickness(2),
                    Background = Brushes.LightGray,
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(3),
                    Child = tb
                };

                _charBorders.Add(border);
                wpChars.Children.Add(border);
            }

            if (_charBorders.Count > 0)
                _charBorders[0].Background = Brushes.Yellow;
        }

        //нажатие клавиши
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            HighlightKey(e.Key, true);

            if (!_isRunning) return;
            if (_currentIndex >= _target.Length) return;
            if (!_keyChar.ContainsKey(e.Key)) return;

            char typed = char.ToLowerInvariant(_keyChar[e.Key]); // сравнение без учёта регистра
            e.Handled = true;

            if (typed == _target[_currentIndex])
            {
                //правильно
                _charBorders[_currentIndex].Background = Brushes.LightGreen;
                _currentIndex++;

                if (_currentIndex < _charBorders.Count)
                    _charBorders[_currentIndex].Background = Brushes.Yellow;

                if (_currentIndex >= _target.Length)
                {
                    MessageBox.Show("Готово!", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                    StopSession();
                }
            }
            else
            {
                //Ошибка
                _charBorders[_currentIndex].Background = Brushes.LightCoral;
            }
        }

        //отпускание клавиши
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            HighlightKey(e.Key, false);
        }

        //подсветка клавиши на экране
        private void HighlightKey(Key key, bool pressed)
        {
            if (!_keyBorder.ContainsKey(key)) return;

            _keyBorder[key].Background = pressed
                ? new SolidColorBrush(Color.FromRgb(255, 200, 50))
                : new SolidColorBrush(Color.FromRgb(240, 240, 240));
        }
    }
}