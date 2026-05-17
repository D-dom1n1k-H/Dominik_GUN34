using Korven.GamePlay.NCPs.StateMachineStates;

namespace Korven.StateMachines
    {
        public sealed class CharacterAIStateMachine
        {
            public CharacterAIState CurrentState {get; private set;}

            public void Initialize(CharacterAIState startingState)
            {
                CurrentState = startingState;
                CurrentState.Enter();
            }

            public void ChangeState(CharacterAIState newState)
            {
                CurrentState.Exit();
                CurrentState = newState;
                CurrentState.Enter();
            }

            public void Update()
            {
                CurrentState?.Update();
            }
        }
    }