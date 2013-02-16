using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Millioner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string FolderName = "music\\";
        private Button _rightButton;
        private object _selectedButton;
        private readonly Questions _questions = new Questions();
        private Button[] _buttons;
        private bool _isRightAnswer;
        private readonly int[] _a = new[] { 0, 100, 200, 300, 500, 1000, 2000, 4000, 8000, 16000, 32000, 64000, 125000, 250000, 500000, 1000000 };
        private int _i;
        private readonly MediaPlayer _player;
        private bool _isPlaying;

        private const int N = 3, M = 5;


        public MainWindow()
        {
            InitializeComponent();
            _player = new MediaPlayer();
            _player.MediaOpened += (sender, args) => _isPlaying = true;
            _player.MediaEnded += (sender, args) => _isPlaying = false;
        }


        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            QuestionBorder.MaxWidth = dockpanel.RenderSize.Width;
            //_prop = RenderSize.Width / RenderSize.Height;
            InitializeButtons();
            SetLevelsScore();
            //_formLoaded = true;
            NewgameClick(null, null);
        }


        private void InitializeButtons()
        {
            const int n = 4; //Количество кнопочек (не думаю, что будет другое число, но на всякий случай)
            _buttons = new Button[n];
            for (int i = 0; i < n; i++)
            {
                var button = new Button {Style = (Style) FindResource("ButtonStyle"), Tag = (char) ('A' + i) + ":"};
                button.Click += ButtonClick;
                Grid.SetRow(button, i / 2);
                Grid.SetColumn(button, i % 2);
                ButtonGrid.Children.Add(button);
                _buttons[i] = button;
            }
        }


        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (Equals(sender, _selectedButton))
                ChechAnswer(sender);
            else
            {
                Play("Answer");
                _selectedButton = sender;
            }
        }


        private void Play(string s)
        {
            _player.Open(new Uri(FolderName + s + ".mp3", UriKind.Relative));
            _player.Play();
        }


        private void ChechAnswer(object sender)
        {
            ColorAnimation animation = BlinkAmination;
            _isRightAnswer = Equals(sender, _rightButton);
            Play(_isRightAnswer ? _i % M == 0 ? "EpicWin" : "Win" : "Lose");
            animation.Completed += ShowScore;
            _rightButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }


        private void ShowScore(object sender, EventArgs eventArgs)
        {
            if (!_isRightAnswer)
                _i -= _i % M;
            var form = new ScoreWindow(_a[_i], () => _isPlaying, !_isRightAnswer) { Owner = this };
            form.ShowDialog();
            if (_isRightAnswer)
                AskNewQuestion();
            else
            {
                QuestionTextBlock.Text = "Конец игры";
                ButtonGrid.IsEnabled = false;
            }
        }


        private ColorAnimation BlinkAmination
        {
            get
            {
                var color = _rightButton.IsFocused ? Colors.Orange : Colors.Black;
                _rightButton.Background = new SolidColorBrush();
                return new ColorAnimation(color, Colors.Green, new Duration(TimeSpan.FromMilliseconds(200)))
                    {AutoReverse = true, RepeatBehavior = new RepeatBehavior(N + 0.5)};
            }
        }


        private void NewgameClick(object sender, RoutedEventArgs e)
        {
            _i = 0;
            UnmarkListBox();
            ButtonGrid.IsEnabled = true;
            _questions.NewGame();
            AskNewQuestion();
        }


        private void UnmarkListBox()
        {
            foreach (ListBoxItem item in listbox.Items)
                item.TabIndex = 0;
        }


        private void AskNewQuestion()
        {
            ClearValues();
            var q = _questions.Next;
            QuestionTextBlock.Text = q.Text;
            for (int i = 0; i < 4; i++)
                _buttons[i].Content = q.Answers[i];
            _rightButton = _buttons[q.RightId];
            MarkListBox(_i++);
        }

        private ListBoxItem GetListBoxItem(int i)
        {
            if (i < 0 || i > M*N - 1) return null;
            return ((ListBoxItem) listbox.Items[14-i]);
        }

        private void MarkListBox(int i)
        {
            var x = GetListBoxItem(i);
            if (x != null)
                x.IsSelected = true;
            x = GetListBoxItem(i - 1);
            if (x != null)
                x.TabIndex = 1;
        }


        private void ClearValues()
        {
            Unfocus();
            if (_rightButton != null)
                _rightButton.ClearValue(BackgroundProperty);
            _selectedButton = _rightButton = null;
        }


        private void SetLevelsScore()
        {
            for (int i = 15, j = 1000000; i > 0; i--, j /= 2)
            {
                var item = new ListBoxItem {Content = string.Format("{0,8}\t{1,-13}", i.ToString(), _a[i])};
                if (i % M == 0)
                    item.Tag = "CheckPoint";
                listbox.Items.Add(item);
            }
        }


        private void Unfocus()
        {
            UnfocusElement.Focus();
        }

        private void QuitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = MessageBox.Show("Вы уверены что хотите выйти?",
                                       "Внимание",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question) == MessageBoxResult.No;
        }


        //private double _prop;
        //private bool _formLoaded;

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
            if (!_formLoaded) return;
            if (e.HeightChanged)
                Width = e.NewSize.Height * _prop;
            else
                Height = e.NewSize.Width / _prop;
            e.Handled = true;*/
        }
    }
}