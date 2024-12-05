namespace AoC2022
{
    public class Day2 : AoC.DayBase
    {
        enum Outcome
        {
            Win = 6,
            Draw = 3,
            Loss = 0,
        }

        enum RockPaperScissors
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3,
        }

        private static Outcome EvalWin(RockPaperScissors theirs, RockPaperScissors mine)
        {
            if( theirs == RockPaperScissors.Rock)
            {
                if (mine == RockPaperScissors.Paper) return Outcome.Win;
                if (mine == RockPaperScissors.Rock) return Outcome.Draw;
                if (mine == RockPaperScissors.Scissors) return Outcome.Loss;
                throw new InvalidOperationException();
            }
            if (theirs == RockPaperScissors.Paper)
            {
                if (mine == RockPaperScissors.Paper) return Outcome.Draw;
                if (mine == RockPaperScissors.Rock) return Outcome.Loss;
                if (mine == RockPaperScissors.Scissors) return Outcome.Win;
                throw new InvalidOperationException();
            }
            if (theirs == RockPaperScissors.Scissors)
            {
                if (mine == RockPaperScissors.Paper) return Outcome.Loss;
                if (mine == RockPaperScissors.Rock) return Outcome.Win;
                if (mine == RockPaperScissors.Scissors) return Outcome.Draw;
                throw new InvalidOperationException();
            }
            throw new InvalidOperationException();
        }

        private static int Eval(RockPaperScissors theirs, RockPaperScissors mine)
        {
            return (int)EvalWin(theirs, mine) + (int)mine;
        }

        private RockPaperScissors Decode(string code)
        {
            switch(code[0])
            {
                case 'A':
                case 'X':
                    return RockPaperScissors.Rock;
                case 'B':
                case 'Y':
                    return RockPaperScissors.Paper;
                case 'C':
                case 'Z':
                    return RockPaperScissors.Scissors;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override object Solve1(string filename)
        {
            var lines = File.ReadAllLines(filename).Select(line => line.Split(' ').Select(Decode)).Select(a => new { theirs = a.First(), mine = a.Last() });

            return lines.Sum(g => Eval(g.theirs, g.mine));
        }

        private Outcome DecodeOutcome(string code)
        {
            switch (code[0])
            {
                case 'X':
                    return Outcome.Loss;
                case 'Y':
                    return Outcome.Draw;
                case 'Z':
                    return Outcome.Win;
                default:
                    throw new NotImplementedException();
            }
        }

        private static RockPaperScissors DecideAction(RockPaperScissors theirs, Outcome outcome)
        {
            switch (outcome)
            {
            case Outcome.Win:
                switch (theirs)
                {
                    case RockPaperScissors.Rock: return RockPaperScissors.Paper;
                    case RockPaperScissors.Paper: return RockPaperScissors.Scissors;
                    case RockPaperScissors.Scissors: return RockPaperScissors.Rock;
                }
                throw new NotImplementedException();
            case Outcome.Draw:
                return theirs;
            case Outcome.Loss:
                switch (theirs)
                {
                    case RockPaperScissors.Rock: return RockPaperScissors.Scissors;
                    case RockPaperScissors.Paper: return RockPaperScissors.Rock;
                    case RockPaperScissors.Scissors: return RockPaperScissors.Paper;
                }
                throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }

        protected override object Solve2(string filename)
        {
            var lines = File.ReadAllLines(filename).Select(line => line.Split(' ')).Select(a => new { theirs = Decode(a.First()), outcome = DecodeOutcome(a.Last()) });

            return lines.Sum(game => (int)DecideAction(game.theirs, game.outcome) + (int)game.outcome);
        }

        public override object SolutionExample1 => 15;
        public override object SolutionPuzzle1 => 15422;
        public override object SolutionExample2 => 12;
        public override object SolutionPuzzle2 => 15442;
    }
}
