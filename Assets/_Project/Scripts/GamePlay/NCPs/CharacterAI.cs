using System;
using System.Collections;
using Korven.GamePlay.NCPs.StateMachineStates;
using Korven.StateMachines;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Korven.GamePlay.NCPs
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class CharacterAI : MonoBehaviour
    {
        public event Action OnTargetSphereCollectedEvent;

        [SerializeField, Header("===== Audio Settings =====")]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip footstepSound;
        [SerializeField]
        private AudioClip onLandSound;

        [field: SerializeField, Header("===== NavMeshAgent Settings =====")]
        public float RandomMovementRadius { get; private set; } = 20f;
        
        [field: SerializeField, Header("===== Gizmos / FOV Settings =====")]
        public Transform TargetTransform { get; private set; }
        [SerializeField]
        private Transform rayOrigin;
        [SerializeField]
        private LayerMask obstacleMask;
        [SerializeField]
        private float ableToSeeRadius = 8f;
        [SerializeField, Tooltip("Not recomended to change")]
        private float ableToCollectRadius = 1f;

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        private CharacterAIStateMachine _stateMachine;
        public CharacterAIIdleState IdleState { get; private set; }
        public CharacterAISearchState SearchState { get; private set; }
        public CharacterAICollectState CollectState { get; private set; }

        private bool _canSeeTarget;
        private bool _canCollectTarget;
        private Coroutine _fovCoroutine;

        public bool CanSeeTarget => _canSeeTarget;
        public bool CanCollectTarget => _canCollectTarget;


        private void Awake()
        {
            _stateMachine = new CharacterAIStateMachine();
            IdleState = new CharacterAIIdleState(this, _stateMachine);
            SearchState = new CharacterAISearchState(this, _stateMachine);
            CollectState = new CharacterAICollectState(this, _stateMachine);

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _stateMachine.Initialize(SearchState);
            StartCheckFieldOfViewCoroutine();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, ableToSeeRadius);

            if (_canSeeTarget)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, TargetTransform.position);
            }
        }


        #region PublicAPI

        public void MoveTo(Vector3 position)
            => _navMeshAgent.SetDestination(position);

        public void StopMovement() =>
            _navMeshAgent.ResetPath();

        public void MoveToRandomPoint()
        {
            Vector3 destination = GetRandomPoint(transform.position, RandomMovementRadius);
            _navMeshAgent.SetDestination(destination);
        }

        public bool HasReachedDestination()
            => !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= 0.5f;

        public void StartPlayingIdleAnimation()
            => _animator.SetTrigger("StartIdling");


        public void StartPlayingSearchAnimation()
            => _animator.SetTrigger("StartSearching");


        public void PlayCollectAnimation() =>
            _animator.SetTrigger("Collect");

        public void StartCheckFieldOfViewCoroutine()
        {
            if (_fovCoroutine == null) _fovCoroutine = StartCoroutine(CheckFieldOfViewCoroutine());
        }

        #endregion

        #region PrivateLogic

        private Vector3 GetRandomPoint(Vector3 center, float radius)
        {
            float randomX = Random.Range(-radius, radius);
            float randomZ = Random.Range(-radius, radius);

            Vector3 point = new Vector3(center.x + randomX, center.y, center.z + randomZ);

            NavMeshHit hit;
            NavMesh.SamplePosition(point, out hit, radius, NavMesh.AllAreas);

            return hit.position;
        }

        private void CheckForTarget()
        {
            
            Vector3 directionToTarget = (TargetTransform.position - rayOrigin.position).normalized;
            float distanceToTarget = Vector3.Distance(rayOrigin.position, TargetTransform.position);

            if (distanceToTarget > ableToSeeRadius)
            {
                _canSeeTarget = false;
            }
            else
            {
                if (!Physics.Raycast(rayOrigin.position, directionToTarget, out RaycastHit hit, distanceToTarget,
                        obstacleMask))
                {
                    _canSeeTarget = true;
                }
                else
                {
                    _canSeeTarget = false;
                }
            }

            if (distanceToTarget > ableToCollectRadius)
            {
                _canCollectTarget = false;
            }
            else
            {
                _canCollectTarget = true;
            }
        }

        #endregion

        #region Coroutines

        private IEnumerator CheckFieldOfViewCoroutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (_navMeshAgent.enabled)
            {
                CheckForTarget();
                yield return wait;
            }
        }

        public void StopCheckFieldOfViewCoroutine()
        {
            if (_fovCoroutine != null)
            {
                StopCoroutine(_fovCoroutine);
                _fovCoroutine = null;
            }
        }

        #endregion

        #region Events

        public void CallOnTargetSphereCollectedEvent()
        {
            OnTargetSphereCollectedEvent?.Invoke();
        }
        
        # endregion

        #region AnimationEvents

        // Systems events

        public void OnFootstep()
        {
            audioSource.PlayOneShot(footstepSound);
        }

        public void OnLand()
        {
            audioSource.PlayOneShot(onLandSound);
        }

        #endregion
    }
}