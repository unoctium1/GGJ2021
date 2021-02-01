using System;

namespace GameJamCat
{
    public enum State
    {
        Pregame,
        Play,
        Dialogue,
        EndGame
    }
    
    public interface IStateManager
    {
        event Action<State> OnStateChanged;
        
        State GetState();

        void SetState(State state);
    }
    
    /// <summary>
    /// The 'Singleton' class
    /// </summary>
    public sealed class StateManager : IStateManager
    {
        private static readonly StateManager _instance = new StateManager();
        
        private State _gameState = State.Pregame;
        public event Action<State> OnStateChanged;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        // https://csharpindepth.com/articles/singleton#cctor
        static StateManager()
        {
        }

        private StateManager()
        {
        }

        public static StateManager Instance => _instance;
        
        /// <summary>
        /// Sets the state of the application
        /// </summary>
        /// <returns></returns>
        public State GetState()
        {
            return _gameState;
        }

        public void SetState(State state)
        {
            _gameState = state;
            OnStateChanged?.Invoke(state);
        }
    }
}