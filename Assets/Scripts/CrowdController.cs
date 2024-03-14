using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrowdController : SingletonMonoBehaviourBase<CrowdController>
{
    public Transform MoveContainer => moveRigidbody.transform;

    [Header("General")]
    [SerializeField] private float crowdMoveSpeed = 1.0f;
    [SerializeField] private float dampMoveMultiplier = 1.0f;

    [Space(10)]
    [SerializeField] private Rigidbody2D moveRigidbody;
    [SerializeField] private Transform staticHumansContainer;
    [SerializeField] private Transform rotatableHumansContainer;

    [Header("Position Points")]
    [SerializeField] private float distanceBetweenCircles = 0.1f;
    [SerializeField] private float colliderRadius = 1.0f;
    [SerializeField] private float circlePartsCount = 2.0f;

    [Space(10)]
    [SerializeField] private RotatablePositionPoint rotatablePositionPointPrefab;
    [SerializeField] private StaticPositionPoint staticPositionPointPrefab;
    [SerializeField] private Transform rotatablePositionPointsContainer;
    [SerializeField] private Transform staticPositionPointsContainer;

    private InputManager _inputManager;

    private List<CrowdHumanInfo> _rotatableHumanInofos => _humanInfos.Where(info => info.human is RotatableHuman).ToList();
    private List<CrowdHumanInfo> _staticHumanInofos => _humanInfos.Where(info => info.human is StaticHuman).ToList();

    private List<CrowdHumanInfo> _humanInfos = new List<CrowdHumanInfo>();

    private List<RotatablePositionPoint> _rotatablePositionPoints = new List<RotatablePositionPoint>();
    private List<StaticPositionPoint> _staticPositionPoints = new List<StaticPositionPoint>();

    private int _circlesCount;

    private Vector3 _currentVelocity;
    private Vector3 _targetVelocity;
    private Vector3 _dampVelocity;

    private void Start()
    {
        _inputManager = InputManager.Instance;
    }

    private void FixedUpdate()
    {
        MoveCrowd();
        RotateCrowd();

        MoveRotatableHumans();
        MoveStaticHumans();
    }

    private void MoveCrowd()
    {
        _targetVelocity = _inputManager.MoveDirection * crowdMoveSpeed;
        _currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _dampVelocity, dampMoveMultiplier * Time.fixedDeltaTime);
        moveRigidbody.velocity = _currentVelocity;
    }

    private void RotateCrowd()
    {
        if (_inputManager.RotateDirection.magnitude <= 0) return;

        var angle = -Vector2.SignedAngle(_inputManager.RotateDirection, Vector3.right);
        rotatablePositionPointsContainer.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void MoveStaticHumans()
    {
        var staticHumanInfos = _humanInfos.Where(humanInfo => humanInfo.human is StaticHuman).ToArray();
        for (var i = 0; i < staticHumanInfos.Length; i++)
        {
            var destinationPoint = _staticPositionPoints[i];

            if (staticHumanInfos[i].human == null) continue;

            staticHumanInfos[i].human.SetDestinationPosition(destinationPoint.transform.position);
            staticHumanInfos[i].human.SetAngleOffset(destinationPoint.AngleOffset);
        }
    }

    private void MoveRotatableHumans()
    {
        var rotatableHumanInfos = _humanInfos.Where(humanInfo => humanInfo.human is RotatableHuman).ToArray();
        var usedHumanInfos = new List<CrowdHumanInfo>();
        for (var i  = 0; i < _rotatablePositionPoints.Count && i < rotatableHumanInfos.Length; i++)
        {
            var destinationPoint = _rotatablePositionPoints[i];
            var info = rotatableHumanInfos.Where(info => !usedHumanInfos.Contains(info))
                .OrderBy(info => Vector2.Distance(destinationPoint.transform.position, info.human.transform.position))
                .FirstOrDefault();
            usedHumanInfos.Add(info);

            if (info.human == null) continue;

            info.human.SetDestinationPosition(destinationPoint.transform.position);
            info.human.SetAngleOffset(destinationPoint.AngleOffset);
        }
    }

    private void UpdatePositionPoints()
    {
        _circlesCount = 0;

        UpdateStaticPositionPointsCount();
        UpdateRotatablePositionPointsCount();
    }

    private void UpdateRotatablePositionPointsCount()
    {
        for (var i = 0; i < _rotatablePositionPoints.Count; i++)
        {
            Destroy(_rotatablePositionPoints[i]);
        }
        _rotatablePositionPoints.Clear();



        while (_rotatablePositionPoints.Count < _rotatableHumanInofos.Count)
        {
            _circlesCount++;
            SpawnRotatableCircle(_circlesCount);
        }
    }

    private void UpdateStaticPositionPointsCount()
    {
        for (var i = 0; i < _staticPositionPoints.Count; i++)
        {
            Destroy(_staticPositionPoints[i]);
        }
        _staticPositionPoints.Clear();

        while (_staticPositionPoints.Count < _staticHumanInofos.Count)
        {
            SpawnStaticCircle(_circlesCount);
            _circlesCount++;
        }
    }

    private void SpawnStaticCircle(int circleIndex)
    {
        var radius = 0.5f + circleIndex + distanceBetweenCircles * (circleIndex + 1) * colliderRadius;
        var length = 2.0f * Mathf.PI * radius;
        var markersCount = (int)(length / colliderRadius);
        var angle = Mathf.Rad2Deg * 2.0f * Mathf.PI / markersCount;
        for (var j = 0; j < markersCount; j++)
        {
            var localPosition = Quaternion.Euler(0, 0, angle * j) * Vector2.right * radius;
            var staticPositionPoint = Instantiate(staticPositionPointPrefab, staticPositionPointsContainer);
            staticPositionPoint.transform.localPosition = localPosition;
            staticPositionPoint.SetAngleOffset(angle);
            _staticPositionPoints.Add(staticPositionPoint);
            staticPositionPoint.SetText(_staticPositionPoints.FindIndex(point => point == staticPositionPoint).ToString());
        }
    }

    private void SpawnRotatableCircle(int circleIndex)
    {
        var radius = 0.5f + circleIndex + distanceBetweenCircles * (circleIndex + 1) * colliderRadius;
        var length = 2.0f * Mathf.PI * radius;
        var markersCount = (int)(length / colliderRadius);
        var angle = Mathf.Rad2Deg * 2.0f * Mathf.PI / markersCount;
        var half = (int)(markersCount / circlePartsCount);
        for (var j = 0; j < half; j += 1 )
        {
            int center = (int)(markersCount * 0.5f);
            int index = j % 2 == 0 ? - 1 - j : j;
            SpawnRotatablePositionPoint(angle * index * 0.5f, radius);
        }
    }

    private void SpawnRotatablePositionPoint(float angle, float radius)
    {
        var localPosition = Quaternion.Euler(0, 0, angle) * Vector2.right * radius;
        var rotatablePositionPoint = Instantiate(rotatablePositionPointPrefab, rotatablePositionPointsContainer);
        rotatablePositionPoint.transform.localPosition = localPosition;
        rotatablePositionPoint.SetAngleOffset(angle);
        _rotatablePositionPoints.Add(rotatablePositionPoint);
    }

    public bool HasHuman(Human human)
    {
        return _humanInfos.Any(info => info.human == human);
    }

    public void AddHuman(Human human)
    {
        if(human == null || !human.CanCollect()) return;

        if (_humanInfos.Where(info => info.human == human).ToArray().Length >= 1) return;

        var humanInfo = new CrowdHumanInfo()
        {
            human = human
        };

        _humanInfos.Add(humanInfo);

        var container = staticHumansContainer;
        if (human is StaticHuman)
            container = staticHumansContainer;
        else if(human is RotatableHuman)
            container = rotatableHumansContainer;
        human.transform.SetParent(container);

        UpdatePositionPoints();
    }

    public void RemoveHuman(Human human)
    {
        var humanInfo = _humanInfos.Where(info => info.human == human).FirstOrDefault();

        if (humanInfo == null) return;

        _humanInfos.Remove(humanInfo);

        UpdatePositionPoints();

        if(_humanInfos.Count <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerable<CrowdHumanInfo> GetHumanInfos()
    {
        return _humanInfos;
    }
}

public class CrowdHumanInfo
{
    public Human human;
}
