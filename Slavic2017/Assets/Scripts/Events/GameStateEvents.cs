using System.Collections.Generic;

namespace Events
{
    public class WinGamePossibleEvent : GameEvent
    {
        public int CollectedPresents { get; private set; }
        public bool CanWin { get; set; }

        public WinGamePossibleEvent(int collectedPresents, bool canWin)
        {
            CanWin = canWin;
            CollectedPresents = collectedPresents;
            args = new List<object> { CollectedPresents };
        }
    }

    public class CloseExitWindowEvent : GameEvent
    {
        public CloseExitWindowEvent()
        {
        }
    }

    public class ConfirmExitEvent : GameEvent
    {
        public ConfirmExitEvent()
        {
        }
    }

    public class GameOverEvent : GameEvent
    {
        public bool IsGameWon { get; set; }
        //public float FinishLevelTime { get; set; }

        public GameOverEvent(bool isGameWon)
        {
            IsGameWon = isGameWon;
            //FinishLevelTime = finishLevelTime;
            args = new List<object> { IsGameWon };
        }
    }

    public class QuitGameEvent : GameEvent
    {
        public QuitGameEvent()
        {
            
        }
    }
}
