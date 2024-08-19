using System;
using System.Collections.Generic;
using System.Linq;
using static MemoryGameLogic.Board;

namespace MemoryGameLogic
{
    public class GameLogic
    {
        private List<Player> m_PlayerList;
        private Board m_Board;
        private int m_Turn;
        private int m_NumOfPairsFound;
        private int m_TotalNumPairs;
        private bool m_IsGameOver;
        private ComPlayer m_CpuTurnGenerator ;
        public class ComPlayer
        {
            private List<Board.Cell> m_RevealdCells ;

            public ComPlayer()
            {
                m_RevealdCells = new List<Board.Cell>();
            }
            public List<Board.Cell> RevealdCells
            {
                get { return m_RevealdCells; }
            }
                        public void GeneraeteComCellPick(ref Board i_Board, out Board.Index o_FirstCellIndex, out Board.Index o_SecondCellIndex)
            {
                int numOfChosenCell = 0;
                o_FirstCellIndex = i_Board.GameBoard[0, 0].Index;
                o_SecondCellIndex = i_Board.GameBoard[0, 0].Index;

                if (m_RevealdCells.Count >= 2)
                {
                    m_RevealdCells.Sort((x, y) => x.Note.CompareTo(y.Note));
                    for (int i = 0; i < m_RevealdCells.Count - 1; i++)
                    {
                        if (m_RevealdCells[i].Note == m_RevealdCells[i + 1].Note)
                        {
                            o_FirstCellIndex = m_RevealdCells[i].Index;
                            o_SecondCellIndex = m_RevealdCells[i + 1].Index;
                            m_RevealdCells.Remove(m_RevealdCells[i]);
                            m_RevealdCells.Remove(m_RevealdCells[i]);
                            numOfChosenCell = 2;
                            break;
                        }
                    }
                }

                if (numOfChosenCell < 2)
                {
                    List<Index> invisibleCellsIndex = new List<Index>(i_Board.GameBoard.Length);
                    foreach (Board.Cell cell in i_Board.GameBoard)
                    {
                        if (!cell.Visible)
                        {
                            invisibleCellsIndex.Add(new Index { RowIndex = cell.Index.RowIndex, ColIndex = cell.Index.ColIndex });
                        }
                    }
                    Random rand = new Random();
                    invisibleCellsIndex = invisibleCellsIndex.OrderBy(x => rand.Next()).ToList();
                    o_FirstCellIndex = invisibleCellsIndex[0];
                    o_SecondCellIndex = invisibleCellsIndex[1];
                }
            }
        }


        public GameLogic()
        {
            m_Turn = 0;
            m_IsGameOver = false;
            m_CpuTurnGenerator = new ComPlayer();
        }
        public ComPlayer CpuTurnGenerator
        {
            get { return m_CpuTurnGenerator; }
        }
        public ref List<Player> PlayerList
        {
            get { return ref m_PlayerList; }
        }
        public int Turn
        {
            get { return m_Turn; }
        }
        public ref Board Board
        {
            get { return ref m_Board; }
        }
        public bool IsGameOver
        {
            get { return m_IsGameOver; }
        }

        // $G$ DSN-002 (-10) You should not make UI calls from your logic classes. 

        public bool IsNumOfCellsEqual(int i_NumOfRows, int i_NumOfCols)
        {
            bool isValid = true;

            if ((i_NumOfRows * i_NumOfCols) % 2 != 0)
            {
                isValid = false;
                Console.WriteLine("The number of cells is not equal!");
            }

            return isValid;
        }
        public void InitializeBoard(int i_NumOfRows, int i_NumOfCols)
        {
            m_Board = new Board(i_NumOfRows, i_NumOfCols);
            m_TotalNumPairs = m_Board.NumOfCols * m_Board.NumOfRows / 2;
        }
        public void SetPlayerList(int i_PlayersAmount)
        {
            m_PlayerList = new List<Player>(i_PlayersAmount);
        }
        public int GetNumOfPlayers()
        {
            return m_PlayerList.Capacity;
        }
        public string GetPlayerName(int i_Turn)
        {
            return m_PlayerList[i_Turn].Name;
        }
        public void AddPlayer(string i_Name, bool i_IsCom) 
        {
            m_PlayerList.Add(new Player(i_Name, i_IsCom));
        }
        public void UpdateCellVisible(Board.Index i_Index, bool i_Visability)
        {
            m_Board.UpdateCellVisibility(i_Index, i_Visability);
        }
        public void CheckPairAndUpdateSystem(Board.Index i_CellIndexOne, Board.Index i_CellIndexTwo)
        {
            if (m_Board.GetCellNote(i_CellIndexOne) == m_Board.GetCellNote(i_CellIndexTwo))
            {
                CorrectPair();
                DeleteIfInRevealdCells(i_CellIndexOne);
                DeleteIfInRevealdCells(i_CellIndexTwo);
            }
            else
            {
                InCorrectPair(i_CellIndexOne, i_CellIndexTwo);
                AddIfNotInRevealdCells(i_CellIndexOne);
                AddIfNotInRevealdCells(i_CellIndexTwo);
            }
        }


        public void AddIfNotInRevealdCells( Board.Index i_CellIndex)
        {
            bool isFound = false;

            for (int i = 0; i < m_CpuTurnGenerator.RevealdCells.Count; i++)
            {
                if (m_CpuTurnGenerator.RevealdCells[i].Index.ColIndex == i_CellIndex.ColIndex && m_CpuTurnGenerator.RevealdCells[i].Index.RowIndex == i_CellIndex.RowIndex)
                {
                    isFound = true;
                    break;
                }
            }
            if(!isFound)
            {
                Board.Cell cellToAdd = new Board.Cell(m_Board.GetCellNote(i_CellIndex), i_CellIndex.RowIndex, i_CellIndex.ColIndex);

                m_CpuTurnGenerator.RevealdCells.Add(cellToAdd);
            }
        }

        public void DeleteIfInRevealdCells(Board.Index i_CellIndex)
        {
            for (int i = 0; i < m_CpuTurnGenerator.RevealdCells.Count; i++)
            {
                if (m_CpuTurnGenerator.RevealdCells[i].Index.ColIndex == i_CellIndex.ColIndex && m_CpuTurnGenerator.RevealdCells[i].Index.RowIndex == i_CellIndex.RowIndex)
                {
                    m_CpuTurnGenerator.RevealdCells.Remove(m_CpuTurnGenerator.RevealdCells[i]);
                }
            }
        }
        public void CorrectPair()
        {
            m_PlayerList[m_Turn].PlayerScore++;
            m_NumOfPairsFound++;
            if (m_NumOfPairsFound == m_TotalNumPairs)
            {
                m_IsGameOver = true;
            }
        }
        public void InCorrectPair(Board.Index i_CellIndexOne, Board.Index i_CellIndexTwo)
        {
            m_Board.UpdateCellVisibility(i_CellIndexOne, false);
            m_Board.UpdateCellVisibility(i_CellIndexTwo, false);
            ForwardTurn();
            System.Threading.Thread.Sleep(2000);
        }
        public void ForwardTurn()
        {
            m_Turn = (m_Turn + 1) % m_PlayerList.Count;
        }
        public bool ShowCell(Board.Index i_CellIndex)
        {
            bool isLegalIndex = true;

            isLegalIndex = IsIndexValid(i_CellIndex);
            if (isLegalIndex)
            {
                m_Board.UpdateCellVisibility(i_CellIndex, true);
            }

            return isLegalIndex;
        }

        public bool IsIndexValid(Board.Index i_CellIndex)
        {
            bool isValidCellIndex = true;
            
            if (i_CellIndex.ColIndex >= m_Board.NumOfCols || i_CellIndex.RowIndex >= m_Board.NumOfRows)
            {
                isValidCellIndex = false;
            }
            else if (m_Board.GetCellVisibility(i_CellIndex))
            {
                isValidCellIndex = false;
            }

            return isValidCellIndex;
        }

    }
}

