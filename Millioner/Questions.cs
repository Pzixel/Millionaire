using System;
using System.Collections.Generic;
using System.Linq;

namespace Millioner
{
    [Serializable]
    public class Questions
    {
        private readonly List<Question> _questions;
        private readonly Random _random = new Random();
        private int _current;


        public Questions()
        {
            NewGame();
            _questions = new List<Question>();
            _questions.Add(new Question(1, "Смотрю в книгу вижу", new[] {"Яблоко", "Финик", "Фигу", "Апельсин"}, 2));
            _questions.Add(new Question(1, "Что обязан пропустить водитель при выезде с обочины на полосу движения?", 
                                        new[] { "Дорожную пыль", "Движущийся транспорт", "Стелющийся туман", "Стаканчик виски" }, 1));
            _questions.Add(new Question(2, "Как заканчивается песня \"Мне не дорог твой подарок, дорога твоя\"",
                                        new[] { "Квартира", "Машина", "Сберкнижка", "Любовь" }, 3));
            _questions.Add(new Question(3,
                            "Что должен сказать джентельмен, нечаянно наступив на ногу даме?",
                            new[] { "Какой размер?", "Благодарю!", "Извините!", "Чей туфля?" },
                            2));
            _questions.Add(new Question(4, "Как называют невнятное произношение?",
                                        new[] { "Борщ в голове", "Каша во рту", "Лапша на ушах", "Хрен на языке" }, 1));
            _questions.Add(new Question(5, "В каком году началась вторая мировая война?",
                                        new[] {"1939", "1940", "1941", "1945"}, 0));
            _questions.Add(new Question(5, "Как называется маленький микрофон?",
                                        new[] { "Эполетик", "Погончик", "Петличка", "Орденок" }, 2));
            _questions.Add(new Question(6, "В каком заливе обновят ласты аквалангисты, купившие тур в Дубай?",
                            new[] { "В Бисикайском", "В Суэцком", "В Персидском", "В бенгальском" }, 2));
        }


        public Question Current
        {
            get { return GetQuestion(false); }
        }


        public Question Next
        {
            get { return GetQuestion(true); }
        }       

        private Question GetQuestion(bool increment)
        {
            var q = _questions.Where(x => x.Level == _current).ToArray();
            if (increment)
                _current++;
            return q[_random.Next(0,q.Length)];
        }

        public void NewGame()
        {
            _current = 1;
        }
    }


    public struct Question
    {
        public readonly int Level;
        public readonly string Text;
        public readonly string[] Answers;
        public readonly int RightId;


        public Question(int level, string text, string[] answers, int rightId)
        {
            Level = level;
            Text = text;
            Answers = answers;
            RightId = rightId;
        }
    }
}