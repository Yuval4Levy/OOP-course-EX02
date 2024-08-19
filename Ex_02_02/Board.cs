using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoryGameLogic
{
    public class Board
    {
        public static void Main()
        {
           
        }
        private readonly int r_NumOfRows;
        private readonly int r_NumOfCols;
        private Cell[,] m_GameBoard;
        public Board(int i_NumOfRows, int i_NumOfColumns)
        {
            r_NumOfRows = i_NumOfRows;
            r_NumOfCols = i_NumOfColumns;
            m_GameBoard = new Cell[r_NumOfRows, r_NumOfCols];

            BoardInitilizer(ref m_GameBoard);
        }
        public Cell GetCell(Index i_CellIndex)
        {
            return m_GameBoard[i_CellIndex.RowIndex, i_CellIndex.ColIndex];
        }
        public ref Cell[,] GameBoard
        {
            get { return ref m_GameBoard; }
        }
        public int NumOfRows
        {
            get { return r_NumOfRows; }
        }
        public int NumOfCols
        {
            get { return r_NumOfCols; }
        }
        
        public char GetCellNote(Index i_CellIndex)
        {
            return m_GameBoard[i_CellIndex.RowIndex, i_CellIndex.ColIndex].Note;
        }
        public bool GetCellVisibility(Index i_CellIndex)
        {
            return m_GameBoard[i_CellIndex.RowIndex, i_CellIndex.ColIndex].Visible;
        }
        public void UpdateCellVisibility(Index i_CellIndex, bool i_Visibility)
        {
           m_GameBoard[i_CellIndex.RowIndex, i_CellIndex.ColIndex].Visible = i_Visibility;
        }
        public void BoardInitilizer(ref Cell[,] o_GameBoard)
        {
            List<Index> cellPairs = new List<Index>(r_NumOfCols * r_NumOfRows);

            RandomPairsGenerator(ref cellPairs);
            SetNotesToCells(ref o_GameBoard, ref cellPairs);
        }
        public void SetNotesToCells(ref Cell[,] io_GameBoard, ref List<Index> i_CellPairs)
        {
            char currentNote = 'A';

            for (int i = 0; i < this.r_NumOfRows * r_NumOfCols; i += 2)
            {
                io_GameBoard[i_CellPairs[i].RowIndex, i_CellPairs[i].ColIndex] = new Cell(currentNote, i_CellPairs[i].RowIndex, i_CellPairs[i].ColIndex);
                io_GameBoard[i_CellPairs[i + 1].RowIndex, i_CellPairs[i + 1].ColIndex] = new Cell(currentNote, i_CellPairs[i + 1].RowIndex, i_CellPairs[i + 1].ColIndex);
                currentNote++;
            }
        }


        public void RandomPairsGenerator(ref List<Index> io_CellPairs)
        {
            Random rand = new Random();

            for (int i = 0; i < this.r_NumOfRows; i++)
            {
                for (int j = 0; j < this.r_NumOfCols; j++)
                {
                    io_CellPairs.Add(new Index{RowIndex = i, ColIndex = j});
                }
            }

            io_CellPairs = io_CellPairs.OrderBy(x => rand.Next()).ToList();
        }
        public struct Index
        {
            private int m_RowIndex;
            private int m_ColIndex;

            public Index(int i_RowIndex, int i_ColIndex)
            {
                m_RowIndex = i_RowIndex;
                m_ColIndex = i_ColIndex;
               
            }
            public int RowIndex
            {
                get { return m_RowIndex; }
                set { m_RowIndex = value; }
            }
            public int ColIndex
            {
                get { return m_ColIndex; }
                set { m_ColIndex = value; }
            }
        }
        public class Cell
        {
            private char m_Note;
            private bool m_Visible;
            private Index m_Index;

            public Cell(char i_Note, int i_RowIndex, int i_ColIndex)
            {
                this.m_Note = i_Note;
                this.m_Visible = false;
                m_Index = new Index(i_RowIndex, i_ColIndex);
            }
            public Index Index
            {
                get { return m_Index; }
                set { m_Index = value; }
            }
            public bool Visible
            {
                get { return m_Visible; }
                set { m_Visible = value; }
            }
            public char Note
            {
                get { return m_Note; }
                set { m_Note = value; }
            }
        }
    }
}
