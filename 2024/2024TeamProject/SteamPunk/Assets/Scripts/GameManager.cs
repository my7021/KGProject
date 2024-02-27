public class GameManager
{
    private static GameManager _inst;
    public static GameManager Inst
    {
        get 
        { 
            if (_inst == null)
                _inst = new GameManager();
            return _inst; 
        }
    }

    public GameScene m_GameScene = null;
}
