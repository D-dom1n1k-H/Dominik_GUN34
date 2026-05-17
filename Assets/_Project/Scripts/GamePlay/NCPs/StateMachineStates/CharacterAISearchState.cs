using Korven.StateMachines;
using UnityEngine;

namespace Korven.GamePlay.NCPs.StateMachineStates
{
    public sealed class CharacterAISearchState : CharacterAIState
    {
        public CharacterAISearchState(CharacterAI characterAI, CharacterAIStateMachine stateMachine) : base(characterAI, stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Entering CharacterAISearchState");

            CharacterAI.StartPlayingSearchAnimation();

            CharacterAI.MoveToRandomPoint();
        }

        public override void Update()
        {
            if (CharacterAI.CanSeeTarget)
            {
                StateMachine.ChangeState(CharacterAI.CollectState);
                return;
            }

            if (CharacterAI.HasReachedDestination())
            {
                CharacterAI.MoveToRandomPoint();
            }
        }

        public override void Exit()
        {
            Debug.Log("Exiting CharacterAISearchState");

            CharacterAI.StopMovement();
        }
    }
}