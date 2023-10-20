using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Human : MonoBehaviour
{
    public bool InSafe { get; set; }

    [SerializeField] protected float speed;
    [SerializeField] private float stepAnimationValue = 0.02f;
    [SerializeField] private GameObject bloodEffect;

    [Space]
    [SerializeField] private Animator walkAnimator;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip walkAnimation;

    protected Rigidbody2D _rigidbody2D;
    protected CircleCollider2D _circleCollider;
    protected Transform _distinationPoint;
    protected bool _inCrowd;

    private Vector3 _previousPos;
    private int _walkSwitchCount;
    private int _idleSwitchCount;

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (!_inCrowd) return;

        MoveToDestinationPoint();
    }

    protected virtual void Update()
    {
        UpdateAnimation();

        _previousPos = transform.position;
    }

    public virtual void AddInCrowd()
    {
        _inCrowd = true;
    }

    public virtual void Die()
    {
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SetDestinationPoint(Transform destinationPoint)
    {
        _distinationPoint = destinationPoint;
    }

    private void MoveToDestinationPoint()
    {
        var hit = Physics2D.Raycast(transform.position, _distinationPoint.position - transform.position, _circleCollider.radius + speed * Time.fixedDeltaTime);
        if (hit.collider.TryGetComponent(out Human human) && human != this) return;

        transform.position = Vector3.MoveTowards(transform.position, _distinationPoint.position, speed * Time.fixedDeltaTime);
    }

    private void UpdateAnimation()
    {
        _walkSwitchCount = IsCanWalkOrIdleAnimation().Item1 ? _walkSwitchCount + 1 : 0;
        _idleSwitchCount = IsCanWalkOrIdleAnimation().Item2 ? _idleSwitchCount + 1 : 0;

        if (_walkSwitchCount > 10)
        {
            walkAnimator.Play(walkAnimation.name);
        }
        else if(_idleSwitchCount > 10)
        {
            walkAnimator.Play(idleAnimation.name);
        }
    }

    private (bool, bool) IsCanWalkOrIdleAnimation()
    {
        var canWalk = Vector2.Distance(transform.position, _previousPos) >= stepAnimationValue * Time.deltaTime;
        var isOnIdle = walkAnimator.GetCurrentAnimatorStateInfo(0).IsName(idleAnimation.name);
        var isOnWalk = walkAnimator.GetCurrentAnimatorStateInfo(0).IsName(walkAnimation.name);

        return (canWalk && !isOnWalk && isOnIdle, !canWalk && isOnWalk && !isOnIdle);
    }
}
