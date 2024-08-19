namespace MemoryGameLogic
{
    public class Player
    {
        private readonly string r_Name;
        private readonly bool r_IsCom;
        private int m_PlayerScore;

        public Player(string i_Name, bool i_IsCom)
        {
            r_Name = i_Name;
            r_IsCom = i_IsCom;
            m_PlayerScore = 0;
        }
        public string Name
        {
            get { return r_Name; }
        }
        public bool IsCom
        {
            get { return r_IsCom; }
        }
        public int PlayerScore
        {
            get { return m_PlayerScore; }
            set { m_PlayerScore = value; }
        }
    }
}
