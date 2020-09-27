using System;
using System.Collections.Generic;

namespace MagnetGame
{
    public class StateMachine
    {
        public void StartLevel(Level level)
        {
            Level = level;
            RestartLevel();
        }

        public void RestartLevel()
        {
            CurrentState = Level.ToState();
        }

        public IEnumerable<Action> LegalMoves()
        {
            return LegalMoves(CurrentState);
        }

        public IEnumerable<Action> LegalMoves(GameState state)
        {
            var actions = new List<Action>();
            foreach(Action move in Enum.GetValues(typeof(Action)))
            {
                if(MoveLegal(move))
                {
                    actions.Add(move);
                }
            }
            return actions;
        }

        public bool MoveLegal(Action move)
        {
            // TODO: Implement function
            return false;
        }

        public StateTransition MakeMove(Action move)
        {
            var transition = NextState(move);
            CurrentState = transition.NewState;
            return transition;
        }

        public StateTransition NextState(Action move)
        {
            return NextState(CurrentState, move);
        }

        public StateTransition NextState(GameState state, Action Move)
        {
            // TODO: Implement function
            return new StateTransition();
        }

        public GameState CurrentState;
        public Level Level;
    }
}