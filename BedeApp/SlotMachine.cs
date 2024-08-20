namespace BedeApp
{
    class SlotMachineGame
    {
        private static readonly Random rnd = new Random();
        private static readonly char[] symbols = { 'A', 'B', 'P', '*' };
        private static readonly double[] coefficients = { 0.4, 0.6, 0.8, 0 };
        private static readonly double[] probabilities = { 0.45, 0.35, 0.15, 0.05 };

        public void Spin()
        {
            double currentBalance = EnterDeposit();

            while (currentBalance > 0)
            {
                double betAmount = EnterBet(currentBalance);
                if (betAmount <= 0)
                {
                    Console.WriteLine("The bet amount must be over 0.");
                    continue;
                }

                char[,] slotView = SpinReels();

                double winnings = CalculateWinnings(slotView, betAmount);

                currentBalance = Math.Round((currentBalance - betAmount) + winnings, 2);

                Console.WriteLine("You won: " + winnings);
                Console.WriteLine("Remaining Balance: " + currentBalance);
            }

            Console.WriteLine("You're out of money. Game over!");
        }

        private static double EnterDeposit()
        {
            Console.Write("Please enter the amount to deposit: ");
            double deposit = double.Parse(Console.ReadLine());

            while (deposit <= 0)
            {
                Console.WriteLine("The deposit amount must be over 0.");
                Console.Write("Please enter the amount to deposit: ");
                deposit = double.Parse(Console.ReadLine());
            }

            return deposit;
        }
        
        private static double EnterBet(double balance)
        {
            Console.Write("Please enter the amount to bet: ");
            double bet = double.Parse(Console.ReadLine());

            while (bet > balance)
            {
                Console.WriteLine("The bet exceeds your current balance. Please enter a smaller amount.");
                Console.Write("Please enter the amount to bet: ");
                bet = double.Parse(Console.ReadLine());
            }

            return bet;
        }

        private static char[,] SpinReels()
        {
            //generate reels
            char[,] reels = new char[4, 3];
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    reels[row, col] = GetRandomSymbol();
                }
            }

            //print reels to screen
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Console.Write(reels[row, col] + " ");
                }
                Console.WriteLine();
            }
            return reels;
        }

        private static char GetRandomSymbol()
        {
            double randValue = rnd.NextDouble();
            double cumulativeProb = 0.0;

            for (int index = 0; index < symbols.Length; index++)
            {
                cumulativeProb += probabilities[index];
                if (randValue < cumulativeProb)
                {
                    return symbols[index];
                }
            }

            return symbols[0];
        }

        private static double CalculateWinnings(char[,] reels, double bet)
        {
            double totalCoefficient = 0.0;

            for (int row = 0; row < 4; row++)
            {
                if (IsWinningLine(reels, row))
                {
                    for (int col = 0; col < 3; col++)
                    {
                        totalCoefficient += GetPayoutForSymbol(reels[row, col]);
                    }
                }
            }

            return Math.Round(totalCoefficient * bet, 2);
        }

        private static bool IsWinningLine(char[,] reels, int row)
        {
            if (reels[row, 0] == reels[row, 1] && reels[row, 1] == reels[row, 2])
                return true;
            else if (reels[row, 0] == '*' && reels[row, 1] == reels[row, 2])
                return true;
            else if (reels[row, 1] == '*' && reels[row, 0] == reels[row, 2])
                return true;
            else if (reels[row, 2] == '*' && reels[row, 0] == reels[row, 1])
                return true;
            else return false;
        }

        private static double GetPayoutForSymbol(char symbol)
        {
            for (int i = 0; i < symbols.Length; i++)
            {
                if (symbols[i] == symbol)
                {
                    return coefficients[i];
                }
            }

            return 0.0;
        }

        static void Main()
        {
            var slotMachine = new SlotMachineGame();
            slotMachine.Spin();
        }
    }
}