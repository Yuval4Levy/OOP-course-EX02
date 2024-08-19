using MemoryGameLogic;



namespace MemoryGameUI
{
    internal class Program
    {
        eGameState m_eGameState;
        GameLogic m_GameLogic;
        GameUI m_GameUI;
        private int m_NumOfRows;
        private int m_NumOfCols;



        public void MemoryGame()
        {
            bool StartAnotherGame;
            eGameState lastGameFinishedStatus;

            while (m_eGameState != eGameState.Terminated)
            {
                lastGameFinishedStatus = NewGame();
                if (lastGameFinishedStatus == eGameState.Finished)
                {
                    m_GameUI.PrintPlayersScoreboard(ref m_GameLogic.PlayerList);
                    StartAnotherGame = m_GameUI.ShouldStartAnotherGame();
                    if (StartAnotherGame == true)
                    {
                        m_eGameState = eGameState.StartNewGame;
                    }
                    else
                    {
                        m_eGameState = eGameState.Terminated;
                    }
                }
            }
        }
        public eGameState NewGame()
        {
            InitializeGame();
            while (m_eGameState == eGameState.Running)
            {
                Turn();
            }

            return m_eGameState;
        }
        

        public void Turn()
        {
            Board.Index firstCellIndex = new Board.Index(), secondCellIndex = new Board.Index();
            Player currentPlayer = m_GameLogic.PlayerList[m_GameLogic.Turn]; 

            if (m_GameLogic.PlayerList[m_GameLogic.Turn].IsCom)
            {
                m_GameLogic.CpuTurnGenerator.GeneraeteComCellPick(ref m_GameLogic.Board, out firstCellIndex, out secondCellIndex);
                m_GameLogic.Board.UpdateCellVisibility(firstCellIndex, true);
                m_GameLogic.Board.UpdateCellVisibility(secondCellIndex, true);
                m_GameUI.PrintCurrentGameBoard(ref  m_GameLogic.Board, ref currentPlayer);
            }
            else
            {
                GetCellUserPick(ref firstCellIndex, ref m_GameLogic.Board, ref currentPlayer);
                if (m_eGameState == eGameState.Running)
                {
                    GetCellUserPick(ref secondCellIndex, ref m_GameLogic.Board, ref currentPlayer);
                    m_GameUI.PrintCurrentGameBoard(ref m_GameLogic.Board, ref currentPlayer);
                }
            }

            if (m_eGameState == eGameState.Running)
            {
                m_GameLogic.CheckPairAndUpdateSystem(firstCellIndex, secondCellIndex);
                m_GameUI.PrintCurrentGameBoard(ref m_GameLogic.Board, ref currentPlayer);
            }

            if (m_GameLogic.IsGameOver)
            {
                m_eGameState = eGameState.Finished;
            }
        }

        public void GetCellUserPick(ref Board.Index o_CellIndex, ref Board i_Board, ref Player i_CurrentPlayer)
        {
            int rowIndex;
            eCols colIndex;

            m_GameUI.PrintCurrentGameBoard(ref i_Board, ref i_CurrentPlayer);
            m_GameUI.GetCellFromUser(out rowIndex, out colIndex, ref i_Board, ref m_eGameState);
            if (m_eGameState == eGameState.Running) 
            {
                o_CellIndex = new Board.Index(rowIndex, (int)colIndex);
                i_Board.UpdateCellVisibility(o_CellIndex, true);
            }
        }

        public void InitializeGame()
        {
            m_GameUI = new GameUI();
            m_GameLogic = new GameLogic();
            int numOfPlayers;

            do
            {
                m_GameUI.GetBoardDimension(out m_NumOfRows, out m_NumOfCols);
            }
            while (!m_GameLogic.IsNumOfCellsEqual(m_NumOfRows, m_NumOfCols));

            m_GameLogic.InitializeBoard(m_NumOfRows, m_NumOfCols);
            numOfPlayers = m_GameUI.GetNumOfPlayers();
            m_GameLogic.SetPlayerList(numOfPlayers);
            m_GameUI.GetplayersDetails(ref m_GameLogic);
            m_eGameState = eGameState.Running;
        }

        public static void Main()
        {
            Program program = new Program();
            
            program.MemoryGame();
        }

    }
}
