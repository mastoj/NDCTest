using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDCTest;
using NUnit.Framework;

namespace ScoreBoard
{
    public abstract class gameBase : Specification<Bla, GameResult>
    {
        protected override Bla Given()
        {
            return new Bla();
        }
    }

    public class a_home_win_hockey_game : gameBase
    {

        public override Command<GameResult> When(Bla context)
        {
            return new BlaParserCommand(SUT, "H,3,2");
        }

        [Test]
        public void should_print_W_3_2()
        {

            Assert.AreEqual("W 3-2", Result.ToString());

        }
    }

    public class a_away_win_hockey_game : Specification<Bla, GameResult>
    {
        protected override Bla Given()
        {
            return new Bla();
        }

        public override Command<GameResult> When(Bla context)
        {
            return new BlaParserCommand(SUT, "H,3,4");
        }

        [Test]
        public void should_print_L_3_4()
        {
            Assert.AreEqual("L 3-4", Result.ToString());

        }
    }

    public class a_tied_hockey_game : Specification<Bla, GameResult>
    {
        protected override Bla Given()
        {
            return new Bla();
        }

        public override Command<GameResult> When(Bla context)
        {
            return new BlaParserCommand(SUT, "H,3,3");
        }

        [Test]
        public void should_print_I()
        {
            Assert.AreEqual("I", Result.ToString());

        }
    }

    public class a_soccer_game : Specification<Bla, GameResult>
    {
        protected override Bla Given()
        {
            return new Bla();
        }

        public override Command<GameResult> When(Bla context)
        {
            return new BlaParserCommand(SUT, "S,3,4");
        }

        [Test]
        public void should_print_L_3_4()
        {
            Assert.AreEqual("L 3-4", Result.ToString());
        }
    }

    public class a_tied_soccer_game : Specification<Bla, GameResult>
    {
        protected override Bla Given()
        {
            return new Bla();
        }

        public override Command<GameResult> When(Bla context)
        {
            return new BlaParserCommand(SUT, "S,3,3");
        }

        [Test]
        public void should_print_T_3_3()
        {
            Assert.AreEqual("T 3-3", Result.ToString());
        }
    }

    public class GameResult
    {
        enum Outcome
        {
            Win, Loss, Tie, Invalid
        }

        public readonly GameType GameType;
        public readonly int Score1;
        public readonly int Score2;

        public GameResult(GameType gameType, int score1, int score2)
        {
            GameType = gameType;
            Score1 = score1;
            Score2 = score2;
        }

        public override string ToString()
        {
            var outcome = GetOutcome();
            var resultString = GetResult();
            return string.Format("{0} {1}", outcome, resultString).Trim();
        }

        private string GetResult()
        {
            if (GameType == GameType.Hockey && Score1 == Score2) return "";
            else return string.Format("{0}-{1}", Score1, Score2);
        }

        private string GetOutcome()
        {
            if (Score1 == Score2)
            {
                switch (GameType)
                {
                    case GameType.Hockey:
                        return "I";
                        break;
                    default:
                        return "T";
                }
            }
            else if (Score1 > Score2) return "W";
            else return "L";
        }

    }

    public class BlaParserCommand : Command<GameResult>
    {
        private readonly Bla _sut;
        private readonly string _s;
        private GameResult _gameResult;

        public BlaParserCommand(Bla sut, string s)
        {
            _sut = sut;
            _s = s;
        }

        public override GameResult Execute()
        {
            _gameResult = _sut.Parse(_s);
            return _gameResult;
        }

        public override string ToString()
        {
            return "a " + _gameResult.GameType + " game with score " + _gameResult.Score1 + " - " + _gameResult.Score2;
        }
    }

    public class Bla
    {
        public GameResult Parse(string s)
        {
            var stringContents = s.Split(new char[] { ',' });
            var gametype = stringContents.First().ToGameType();
            var Score1 = Convert.ToInt32(stringContents.Skip(1).First());
            var Score2 = Convert.ToInt32(stringContents.Skip(2).First());

            return new GameResult(gametype, Score1, Score2);
        }
        public override string ToString()
        {
            return "A game";
        }
    }
    public enum GameType
    {
        Hockey, Soccer
    }

    public static class StringExtensions
    {
        public static GameType ToGameType(this string elem)
        {
            switch (elem)
            {
                case "H":
                    return GameType.Hockey;
                    break;
                case "S":
                    return GameType.Soccer;
                    break;
                default: throw new ArgumentException(elem);
            }
        }
    }
}
