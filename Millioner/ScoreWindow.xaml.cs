using System.Windows;

namespace Millioner
{
    /// <summary>
    /// Логика взаимодействия для ScoreWindow.xaml
    /// </summary>
    public partial class ScoreWindow
    {
        public delegate bool IsPlaying();
        private readonly IsPlaying _isPlaying; 

        public ScoreWindow(int score, IsPlaying isPlaying, bool lose)
        {
            InitializeComponent();
            Textblock.Text = score.ToString();
            _isPlaying = isPlaying;
            if (lose)
            {
                Button.Content = "Закончить";
                SumTextBlock.Text = "Выигрыш";
            }
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            if (!_isPlaying())
                Close();
        }
    }
}
