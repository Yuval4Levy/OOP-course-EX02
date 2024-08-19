using MemoryGameLogic;
using Ex02.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace MemoryGameUI
{

    public class GameUI
    {
        const int k_MaxNumOfRowsAndCols = 6;
        const int k_MinNumOfRowsAndCols = 4;
        const int k_FirstRowAndColIndex = 0;
        const string k_TerminatedChar = "Q";

        public void PrintPlayersScoreboard(ref List<Player> i_PlayerList)
        {
            Console.WriteLine("The players scoreboard for the last game is:");
            foreach (Player player in i_PlayerList)
            {
                Console.WriteLine($"{player.Name} has {player.PlayerScore} points");
            }
        }
        
        public void PrintCurrentGameBoard(ref MemoryGameLogic.Board i_Board, ref Player i_CurrentPlayer)
        {
            eCols startCol = eCols.A;
            eCols endCol = (eCols)i_Board.NumOfCols;

            Screen.Clear();
            Console.Write($"This is {i_CurrentPlayer.Name}'s Turn.");
            Console.WriteLine(i_CurrentPlayer.IsCom ? " (Computer)\n\n" : " (Human)\n\n");
            Console.Write("  ");
            for (int i = (int)startCol; i < (int)endCol; i++)
            {
                Console.Write("   ");
                Console.Write((eCols)i);
            }

            Console.WriteLine();
            Console.Write("   ");
            Console.WriteLine(new string('=', 4 * i_Board.NumOfCols + 1));
            for (int row = 0; row < i_Board.NumOfRows; row++)
            {
                Console.Write($"{row + 1,2} |");
                for (int col = 0; col < i_Board.NumOfCols; col++)
                {
                    MemoryGameLogic.Board.Index currentCellIndex = new MemoryGameLogic.Board.Index(row, col);
                    char note = i_Board.GetCellNote(currentCellIndex);
                    bool isCellVisible = i_Board.GetCellVisibility(currentCellIndex);
                    Console.Write(isCellVisible ? " " + note.ToString() + " |" : "   |");
                }

                Console.WriteLine("");
                Console.Write("   ");
                Console.WriteLine(new string('=', 4 * i_Board.NumOfCols + 1));
            }
        }
        public int GetNumOfPlayers()
        {
            int numberOfPlayers = 0;

            do
            {
                Console.WriteLine("Hi,Please enter the desired number of players (greater than zero).");
                numberOfPlayers = int.Parse(Console.ReadLine());
                if (numberOfPlayers <= 0)
                {
                    Console.WriteLine("Invalid input! Please enter a number greater that zero.");
                }

            } while(numberOfPlayers <= 0);

            return numberOfPlayers;
        }
        bool CheckBoolInput(string i_Input)
        {
            bool isBoolInputValid = true;

            if (i_Input != "False" && i_Input != "True")
            {
                isBoolInputValid = false;
                Console.Write("Invalid input!");
            }

            return isBoolInputValid;
        }
        public void GetplayersDetails(ref GameLogic i_GameLogic)
        {
            string currPlayerName, PlayerIsComInput;
            bool isCurrPlayerCom;

            Console.WriteLine("Please enter the details about each player:");
            for (int i = 0; i < i_GameLogic.GetNumOfPlayers(); i++)
            {
                Console.WriteLine($"Player #{i + 1} Name:");
                currPlayerName = Console.ReadLine();
                do
                {
                    Console.WriteLine($"Is {currPlayerName} a computer? (True - yes, False - No):");
                    PlayerIsComInput = Console.ReadLine();
                } while (!CheckBoolInput(PlayerIsComInput));

                isCurrPlayerCom = bool.Parse(PlayerIsComInput);
                i_GameLogic.AddPlayer(currPlayerName, isCurrPlayerCom);
            }
        }


        public bool ShouldStartAnotherGame()
        {
            bool shouldStartAnotherGame;
            string userInput;

            do
            {
                Console.WriteLine("Should we start another game? (True - yes, False - No):");
                userInput = Console.ReadLine();
            } while (!CheckBoolInput(userInput));

            shouldStartAnotherGame = bool.Parse(userInput);

            return shouldStartAnotherGame;
        }

        public void GetBoardDimension(out int o_RowsDimension, out int o_ColsDimension)
        {
            bool isValidDimension;

            Console.WriteLine("Hi, welcome to the memory game!");
            do
            {
                Console.WriteLine("Please enter the number of rows and cols for the board seperated by whitespace.");
                Console.WriteLine("Please note that the number of cells must be equal and number of rows and cols must be at least 4 and not bigger that 6.");
                string input = Console.ReadLine();

                if (IsValidFormat(input))
                {
                    string[] inputs = input.Split(' ');

                    o_RowsDimension = int.Parse(inputs[0]);
                    o_ColsDimension = int.Parse(inputs[1]);
                    isValidDimension = IsValidDimension(o_RowsDimension, o_ColsDimension);
                    if (!isValidDimension)
                    {
                        Console.WriteLine("Invalid input! Please enter a valid number of row and cols!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Format! Please enter a valid format.");
                    isValidDimension = false;
                    o_RowsDimension = -1;
                    o_ColsDimension = -1;
                }

            }
            while (!isValidDimension);
        }
        
        public void GetCellFromUser(out int o_CellRow, out MemoryGameUI.eCols o_CellCol, ref Board i_Board, ref eGameState i_eGameState)
        {
            bool isValidCellPick = false;

            do
            {
                Console.WriteLine("Please enter the desired cell to open in a digit letter format, seperated by a white space, or Q to terminate the game.");

                string input = Console.ReadLine();
                if (input.Length == 1 && input == k_TerminatedChar)
                {
                    i_eGameState = eGameState.Terminated;
                    o_CellRow = -1;
                    o_CellCol = eCols.None;
                    break;
                }
                else if (IsValidFormat(input))
                {
                    string[] inputs = input.Split(' ');

                    o_CellRow = int.Parse(inputs[0]) - 1;
                    Enum.TryParse<MemoryGameUI.eCols>(inputs[1], true, out o_CellCol);

                    isValidCellPick = IsValidCell(o_CellRow, (int)(o_CellCol), ref i_Board);
                    if (!isValidCellPick)
                    {
                        Console.WriteLine("Invalid input! Please enter a valid cell row and col!");
                    }
                }
                else
                {
                    isValidCellPick = false;
                    Console.WriteLine("Invalid Format! Please enter a valid format.");
                    o_CellRow = -1;
                    o_CellCol = eCols.None;
                }
            }
            while (!isValidCellPick);
        }
        public static bool IsValidFormat(string i_Input)
        {
            bool isValidFormat = true;

            if (i_Input.Length < 3)
            {
                isValidFormat = false;
            }

            if (char.IsWhiteSpace(i_Input[0]) || char.IsWhiteSpace(i_Input[i_Input.Length - 1]))
            {
                isValidFormat = false;
            }

            if (i_Input.Length == 3 && !char.IsWhiteSpace(i_Input[1]))
            {
                isValidFormat = false;
            }

            if (!char.IsLetterOrDigit(i_Input[0]) || !char.IsLetterOrDigit(i_Input[i_Input.Length - 1]))
            {
                isValidFormat = false;
            }

            return isValidFormat;
        }
        public bool IsValidCell(int i_CellRows, int i_CellCols, ref MemoryGameLogic.Board i_Board)
        {
            Board.Index index = new Board.Index(i_CellRows, i_CellCols);
            bool isValid = false;

            if (i_CellRows >= k_FirstRowAndColIndex && i_CellRows <= i_Board.NumOfRows - 1
                && i_CellCols >= k_FirstRowAndColIndex && i_CellCols <= i_Board.NumOfCols - 1 && !i_Board.GetCellVisibility(index))
            {
                isValid = true;
            }

            return isValid;
        }


        private bool IsValidDimension(int i_RowsDimension, int i_ColsDimension)
        {
            bool isValid = false;

            if (i_RowsDimension >= k_MinNumOfRowsAndCols && i_RowsDimension <= k_MaxNumOfRowsAndCols 
                && i_ColsDimension >= k_MinNumOfRowsAndCols && i_ColsDimension <= k_MaxNumOfRowsAndCols)
            {
                isValid = true;
            }

            return isValid;
        }
    }
    



}
