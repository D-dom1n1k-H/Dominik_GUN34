using Korven.StateMachines;
using UnityEngine;

namespace Korven.GamePlay.NCPs.StateMachineStates
{
    public sealed class CharacterAICollectState : CharacterAIState
    {
        private bool _isCollecting;
        private float _timer;
        private Vector3 _targetPosition;

        public CharacterAICollectState(CharacterAI characterAI, CharacterAIStateMachine stateMachine) : base(
            characterAI, stateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("Entering Collect State");

            CharacterAI.StartPlayingSearchAnimation();

            _isCollecting = false;
            _targetPosition = CharacterAI.TargetTransform.position;
            CharacterAI.MoveTo(_targetPosition);
        }

        public override void Update()
        {
            if (!CharacterAI.CanSeeTarget)
            {
                StateMachine.ChangeState(CharacterAI.SearchState);
                return;
            }

            if (!_isCollecting)
            {
                if (CharacterAI.CanCollectTarget)
                {
                    _isCollecting = true;

                    CharacterAI.StopMovement();
                    CharacterAI.PlayCollectAnimation();

                    _timer = 0.8f;
                    return;
                }

                if (Vector3.Distance(_targetPosition, CharacterAI.TargetTransform.position) > 0.5f)
                {
                    _targetPosition = CharacterAI.TargetTransform.position;
                    CharacterAI.MoveTo(_targetPosition);
                }
            }
            else
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0f)
                {
                    CharacterAI.CallOnTargetSphereCollectedEvent();
                    StateMachine.ChangeState(CharacterAI.IdleState);
                }
            }
        }

        public override void Exit()
        {
            Debug.Log("Exiting Collect State");

            CharacterAI.StopMovement();
        }
    }
}