using System;

class Program
{
    static char[] board;
    static char player = 'X';
    static char computer = 'O';

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Escolha a versão do Jogo da Velha:");
            Console.WriteLine("1 - Jogo com Minimax");
            Console.WriteLine("2 - Jogo com Sistema de Pontuação");
            Console.WriteLine("3 - Sair");
            Console.Write("Opção: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                PlayMinimaxGame();
            }
            else if (choice == "2")
            {
                PlayScoringGame();
            }
            else if (choice == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Escolha inválida. Tente novamente.");
            }
        }
    }

    static void PlayMinimaxGame()
    {
        // Inicializa o tabuleiro para uma nova partida
        InitializeBoard();

        while (true)
        {
            PrintBoard();
            PlayerMove();
            if (IsGameOver())
            {
                break;
            }

            ComputerMoveMinimax();
            if (IsGameOver())
            {
                break;
            }
        }
    }

    static void PlayScoringGame()
    {
        // Inicializa o tabuleiro para uma nova partida
        InitializeBoard();

        while (true)
        {
            PrintBoard();
            PlayerMove();
            if (IsGameOver())
            {
                break;
            }

            ComputerMoveBasedOnScoring();
            if (IsGameOver())
            {
                break;
            }
        }
    }

    static void InitializeBoard()
    {
        board = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    }

    // Os métodos PlayerMove, PrintBoard, IsGameOver, etc. permanecem os mesmos.

    static void PlayerMove()
    {
        int move;
        while (true)
        {
            Console.WriteLine("Escolha uma posição (1-9): ");
            move = int.Parse(Console.ReadLine()) - 1;
            if (move >= 0 && move <= 8 && board[move] != player && board[move] != computer)
            {
                board[move] = player;
                break;
            }
            Console.WriteLine("Movimento inválido, tente novamente.");
        }
    }

    static void PrintBoard()
    {
        Console.Clear();
        Console.WriteLine(" {0} | {1} | {2} ", board[0], board[1], board[2]);
        Console.WriteLine("---|---|---");
        Console.WriteLine(" {0} | {1} | {2} ", board[3], board[4], board[5]);
        Console.WriteLine("---|---|---");
        Console.WriteLine(" {0} | {1} | {2} ", board[6], board[7], board[8]);
    }

    static bool IsGameOver()
    {
        int score = EvaluateBoard();
        if (score == 10)
        {
            PrintBoard();
            Console.WriteLine("Computador venceu!");
            return true;
        }
        else if (score == -10)
        {
            PrintBoard();
            Console.WriteLine("Você venceu!");
            return true;
        }
        else if (!IsMovesLeft())
        {
            PrintBoard();
            Console.WriteLine("Empate!");
            return true;
        }
        return false;
    }

    static bool IsMovesLeft()
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != player && board[i] != computer)
            {
                return true;
            }
        }
        return false;
    }

    static int EvaluateBoard()
    {
        int[,] winningPositions = new int[,]
        {
            { 0, 1, 2 },
            { 3, 4, 5 },
            { 6, 7, 8 },
            { 0, 3, 6 },
            { 1, 4, 7 },
            { 2, 5, 8 },
            { 0, 4, 8 },
            { 2, 4, 6 }
        };

        for (int i = 0; i < 8; i++)
        {
            int a = winningPositions[i, 0];
            int b = winningPositions[i, 1];
            int c = winningPositions[i, 2];

            if (board[a] == board[b] && board[b] == board[c])
            {
                if (board[a] == computer)
                    return 10;
                else if (board[a] == player)
                    return -10;
            }
        }
        return 0;
    }

    // Implementação do Minimax e Sistema de Pontuação
    static void ComputerMoveMinimax()
    {
        int bestMove = -1;
        int bestValue = int.MinValue;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != player && board[i] != computer)
            {
                char original = board[i];
                board[i] = computer;

                int moveValue = Minimax(board, false);

                board[i] = original;

                if (moveValue > bestValue)
                {
                    bestValue = moveValue;
                    bestMove = i;
                }
            }
        }

        board[bestMove] = computer;
    }

    static int Minimax(char[] board, bool isMaximizing)
    {
        int score = EvaluateBoard();
        if (score != 0)
            return score;

        if (IsMovesLeft() == false)
            return 0;

        if (isMaximizing)
        {
            int best = int.MinValue;

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != player && board[i] != computer)
                {
                    char original = board[i];
                    board[i] = computer;

                    best = Math.Max(best, Minimax(board, false));

                    board[i] = original;
                }
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != player && board[i] != computer)
                {
                    char original = board[i];
                    board[i] = player;

                    best = Math.Min(best, Minimax(board, true));

                    board[i] = original;
                }
            }
            return best;
        }
    }

    static void ComputerMoveBasedOnScoring()
    {
        int bestMove = -1;
        int bestScore = int.MinValue;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != player && board[i] != computer)
            {
                int score = CalculateScore(i);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        board[bestMove] = computer;
    }

    static int CalculateScore(int position)
    {
        int score = 0;

        // +2 pontos para a posição central
        if (position == 4)
        {
            score += 2;
        }

        // +1 ponto para os cantos
        if (position == 0 || position == 2 || position == 6 || position == 8)
        {
            score += 1;
        }

        // -2 pontos se houver uma peça do adversário na linha, coluna ou diagonal
        if (IsOpponentInSameLine(position))
        {
            score -= 2;
        }

        // +4 pontos se a posição levar a uma vitória
        if (WillMoveWin(position, computer))
        {
            score += 4;
        }

        // +4 pontos se a posição impedir a vitória do adversário
        if (WillMoveWin(position, player))
        {
            score += 4;
        }

        return score;
    }

    static bool IsOpponentInSameLine(int position)
    {
        int[,] winningPositions = new int[,]
        {
        { 0, 1, 2 },
        { 3, 4, 5 },
        { 6, 7, 8 },
        { 0, 3, 6 },
        { 1, 4, 7 },
        { 2, 5, 8 },
        { 0, 4, 8 },
        { 2, 4, 6 }
        };

        for (int i = 0; i < 8; i++)
        {
            int a = winningPositions[i, 0];
            int b = winningPositions[i, 1];
            int c = winningPositions[i, 2];

            if (position == a || position == b || position == c)
            {
                if ((board[a] == player || board[b] == player || board[c] == player) &&
                    (board[a] != computer && board[b] != computer && board[c] != computer))
                {
                    return true;
                }
            }
        }
        return false;
    }

    static bool WillMoveWin(int position, char symbol)
    {
        char original = board[position];
        board[position] = symbol;

        bool isWin = EvaluateBoard() == (symbol == computer ? 10 : -10);

        board[position] = original;
        return isWin;
    }
}
