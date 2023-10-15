using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.InputSystem;

#region RequireComponent
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
#endregion
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public CinemachineVirtualCamera playerFollowCamera;
    public Transform center;

    #region Event
    public UnityEvent onChangeNearestInteractor = new UnityEvent();
    public UnityEvent onTryInteract = new UnityEvent();
    public UnityEvent onStartAuto = new UnityEvent();
    public UnityEvent onStop = new UnityEvent();
    #endregion

    #region StartComponent
    private PlayerInput _playerInput;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    #endregion

    #region AnimationString
    private readonly string ANI_FLOAT_SPEED = "Speed";
    private readonly string ANI_TRIGGER_STOP= "Stop";
    #endregion

    #region Input & Move
    private enum MoveState
    {
        IDLE,
        INTERACT_TRY,
        INTERACT,
    }
    private MoveState moveState;
    private bool autoMode;
    public bool IsAutoMode()
    {
        return autoMode;
    }
    public bool IsInputMoveable()
    {
        if (moveState == MoveState.IDLE)
            return true;
        return false;
    }
    private void InputMove()
    {
        if (!IsInputMoveable())
            return;
        Vector3 targetDestiantion = transform.position + new Vector3(VirtualJoystick.input.x, 0f, VirtualJoystick.input.z);
        _navMeshAgent.SetDestination(targetDestiantion);
    }
    #endregion

    #region Interaction
    [SerializeField]
    private float interactSearchRadius = 3f;

    private IInteractor nearestInteractor;
    private IInteractor curInteractor;
    public IInteractor GetNearestInteractor() { return nearestInteractor; }

    public bool IsInteractable()
    {
        if (moveState == MoveState.IDLE)
            return true;
        else if (moveState == MoveState.INTERACT_TRY)
            return true;
        return false;
    }
    private void SearchInteractors()
    {
        List<IInteractor> nearInteractors = new List<IInteractor>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactSearchRadius, 1 << 6);
        colliders = colliders.OrderBy(collider => (transform.position - collider.transform.position).sqrMagnitude).ToArray();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IInteractor>(out IInteractor interactor))
                nearInteractors.Add(interactor);
        }
        IInteractor newNearestInteractor = nearInteractors.Count > 0 ? nearInteractors[0] : null;
        if (nearestInteractor != newNearestInteractor)
        {
            nearestInteractor = newNearestInteractor;
            onChangeNearestInteractor.Invoke();
        }
    }
    public void TryInteract()
    {
        TryInteract(nearestInteractor);
    }
    public void TryInteract(IInteractor interactor)
    {
        onTryInteract.Invoke();
        curInteractor = interactor;
        if (!curInteractor.IsInteractAbleDistance(transform.position))
        {
            moveState = MoveState.INTERACT_TRY;
            _navMeshAgent.SetDestination(curInteractor.position);
        }
        else
            Interact();
    }
    public void Interact()
    {
        if (!IsInteractable())
            return;

        moveState = MoveState.INTERACT;
        _navMeshAgent.ResetPath();
        curInteractor.Interact();
    }
    #endregion

    #region Gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactSearchRadius);

        //Gizmos.color = moveState == MoveState.INPUT ? Color.green : moveState == MoveState.INTERACT_TRY ? Color.yellow : moveState == MoveState.INTERACT_ING ? Color.red : Color.magenta;
        Gizmos.DrawSphere(transform.position + Vector3.up * 2f, 0.1f);
    }
    #endregion

    #region ParticleSystem
    [SerializeField]
    private ParticleSystem particleSystem_itemPickUp;
    #endregion

    void Start()
    {
        instance = this;
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        VirtualJoystick.onInput.AddListener(InputMove);
        playerFollowCamera = GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<CinemachineVirtualCamera>();
    }
    void Update()
    {
        _animator.SetFloat(ANI_FLOAT_SPEED, _navMeshAgent.velocity.magnitude);
        SearchInteractors();


        if(moveState == MoveState.INTERACT_TRY && curInteractor.IsInteractAbleDistance(transform.position))
        {
            Interact();
        }
    }

    public void EndCollection()
    {
        if (autoMode)
        {
            moveState = MoveState.IDLE;
            _animator.SetTrigger(ANI_TRIGGER_STOP);
        }
        else
            Stop();
    }
    public void Stop()
    {
        moveState = MoveState.IDLE;
        _navMeshAgent.ResetPath();
        _animator.SetTrigger(ANI_TRIGGER_STOP);
        if (curInteractor != null)
            curInteractor.StopInteract();
        curInteractor = null;
        autoMode = false;
        onStop.Invoke();
    }

    public void StartAuto()
    {
        autoMode = true;
        onStartAuto.Invoke();
    }



    public void ParticleSystemPlay_ItemPickUp()
    {
        particleSystem_itemPickUp.Play();
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

}
