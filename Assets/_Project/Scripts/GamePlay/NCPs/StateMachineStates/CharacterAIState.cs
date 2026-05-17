using Korven.StateMachines;

namespace Korven.GamePlay.NCPs.StateMachineStates
{
    public abstract class CharacterAIState
    {
        protected CharacterAI CharacterAI;
        protected CharacterAIStateMachine  StateMachine;

        protected CharacterAIState(CharacterAI characterAI, CharacterAIStateMachine stateMachine)
        {
            this.CharacterAI = characterAI;
            this.StateMachine = stateMachine;
        }
        
        public virtual void Enter() {}
        public virtual void Update() {}
        public virtual void Exit() {}
    }
}