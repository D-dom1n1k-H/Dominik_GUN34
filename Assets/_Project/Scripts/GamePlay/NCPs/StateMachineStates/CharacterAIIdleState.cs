using Korven.StateMachines;
using UnityEngine;

namespace Korven.GamePlay.NCPs.StateMachineStates
{
    public sealed class CharacterAIIdleState : CharacterAIState
    {
        private const float IdleDuration = 5f;

        private float _idleTimer;

        public CharacterAIIdleState(CharacterAI characterAI, CharacterAIStateMachine stateMachine) : base(characterAI, stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Entering CharacterAIIdleState");

            CharacterAI.StartPlayingIdleAnimation();

            _idleTimer = IdleDuration;
        }

        public override void Update()
        {
            _idleTimer -= Time.deltaTime;

            if (_idleTimer <= 0f)
            {
                StateMachine.ChangeState(CharacterAI.SearchState);
            }
        }

        public override void Exit()
        {
            Debug.Log("Exiting CharacterAIIdleState");
        }
    }
}