using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrowdController : SingletonMonoBehaviourBase<CrowdController>
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float lerpMoveSpeed = 1.0f;
    [SerializeField] private float lerpTranslation = 1.0f;

    [MinMaxSlider(1.0f, 50.0f)]
    [SerializeField] private Vector2 radiusBounds;
    [MinMaxSlider(1, 200)]
    [SerializeField] private Vector2Int positionPointsCountBounds;
    [SerializeField] private RotatablePositionPoint rotatablePositionPointPrefab;
    [SerializeField] private StaticPositionPoint staticPositionPointPrefab;
    [SerializeField] private Transform rotatablePositionPointsContainer;
    [SerializeField] private Transform staticPositionPointsContainer;
    [SerializeField] private Transform moveContainer;
    [SerializeField] private Transform humansContainer;

    private InputManager _inputManager;

    private List<CrowdHumanInfo> _rotatableHumanInofos => _humanInfos.Where(info => info.human is RotatableHuman).ToList();
    private List<CrowdHumanInfo> _staticHumanInofos => _humanInfos.Where(info => info.human is StaticHuman).ToList();
    private List<CrowdHumanInfo> _humanInfos = new List<CrowdHumanInfo>();
    private List<RotatablePositionPoint> _rotatablePositionPoints = new List<RotatablePositionPoint>();
    private List<StaticPositionPoint> _staticPositionPoints = new List<StaticPositionPoint>();

    private Vector3 _translation;

    public int RotatablePositionPointsCount
    {
        get => _postionPointsCount;
        set => _postionPointsCount = Mathf.Clamp(value, positionPointsCountBounds.x, positionPointsCountBounds.y);
    }
    private int _postionPointsCount;
    private float _circleOffset = 0.1f;
    private float _colliderRadius = 1.0f;
    private int _circlesCount;

    private void Start()
    {
        _inputManager = InputManager.Instance;
    }

    private void FixedUpdate()
    {
        MoveCrowd();
        RotateCrowd();
        UpdateRotatablePositionPointsCount();

        MoveRotatableHumans();
        MoveStaticHumans();
    }

    private void MoveCrowd()
    {
        _translation = moveContainer.position + (Vector3) _inputManager.MoveDirection * moveSpeed;
        moveContainer.position = Vector3.Lerp(moveContainer.position, _translation, lerpMoveSpeed * Time.fixedDeltaTime);
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
            var destinationPos = _staticPositionPoints[i].transform.position;
            staticHumanInfos[i].human.SetDestinationPosition(destinationPos);
        }
    }

    private void MoveRotatableHumans()
    {
        var rotatableHumanInfos = _humanInfos.Where(humanInfo => humanInfo.human is RotatableHuman).ToArray();
        for (var i = 0; i < rotatableHumanInfos.Length; i++)
        {
            var destinationPos = _rotatablePositionPoints[i].transform.position;
            rotatableHumanInfos[i].human.SetDestinationPosition(destinationPos);
        }
    }

    private void UpdateRotatablePositionPointsCount()
    {
        //for (var i = 0; i < _rotatablePositionPoints.Count; i++)
        //{
        //    Destroy(_rotatablePositionPoints[i]);
        //}
        //_rotatablePositionPoints.Clear();

        //RotatablePositionPointsCount = (int)(2 * Mathf.PI * Radius * RadiusToPositionPointMultiplier);

        //for (var i = 0; i < RotatablePositionPointsCount; i++)
        //{
        //    var positionPoint = Instantiate(rotatablePositionPointPrefab, rotatablePositionPointsContainer);
        //    _rotatablePositionPoints.Add(positionPoint);
        //}

        //var angle = Mathf.Rad2Deg * Mathf.PI * 2.0f / RotatablePositionPointsCount;
        //for (int i = 0; i < RotatablePositionPointsCount; i++)
        //{
        //    var index = i % 2 == 0 ? RotatablePositionPointsCount - 1 - i : i;
        //    _rotatablePositionPoints[i].transform.localPosition = Quaternion.Euler(0, 0, angle * index) * Vector3.right * Radius;
        //}
    }

    private void UpdateStaticPositionPointsCount()
    {
        for (var i = 0; i < _staticPositionPoints.Count; i++)
        {
            Destroy(_staticPositionPoints[i]);
        }
        _staticPositionPoints.Clear();

        for (var i = 0; i < _rotatablePositionPoints.Count; i++)
        {
            Destroy(_rotatablePositionPoints[i]);
        }
        _rotatablePositionPoints.Clear();

        _circlesCount = 0;

        while (_staticPositionPoints.Count < _staticHumanInofos.Count)
        {
            SpawnStaticCircle(_circlesCount);
            _circlesCount++;
        }

        while (_rotatablePositionPoints.Count < _rotatableHumanInofos.Count)
        {
            SpawnRotatableCircle(_circlesCount);
            _circlesCount++;
        }
    }

    private void SpawnStaticCircle(int circleIndex)
    {
        var radius = 0.5f + circleIndex + _circleOffset * (circleIndex + 1) * _colliderRadius;
        var length = 2.0f * Mathf.PI * radius;
        var markersCount = (int)(length / _colliderRadius);
        var angle = Mathf.Rad2Deg * 2.0f * Mathf.PI / markersCount;
        for (var j = 0; j < markersCount; j++)
        {
            var localPosition = Quaternion.Euler(0, 0, angle * j) * Vector2.right * radius;
            var staticPositionPoint = Instantiate(staticPositionPointPrefab, staticPositionPointsContainer);
            staticPositionPoint.transform.localPosition = localPosition;
            _staticPositionPoints.Add(staticPositionPoint);
            staticPositionPoint.SetText(_staticPositionPoints.FindIndex(point => point == staticPositionPoint).ToString());
        }
    }

    private void SpawnRotatableCircle(int circleIndex)
    {
        var radius = 0.5f + circleIndex + _circleOffset * (circleIndex + 1) * _colliderRadius;
        var length = 2.0f * Mathf.PI * radius;
        var markersCount = (int)(length / _colliderRadius);
        var angle = Mathf.Rad2Deg * 2.0f * Mathf.PI / markersCount;
        for (var j = 0; j < markersCount * 2; j += 1 )
        {
            var index = j % 2 == 0 ? markersCount * 2 - 1 - j : j;
            SpawnRotatablePositionPoint(angle * index * 0.5f, radius);
        }
    }

    private void SpawnRotatablePositionPoint(float angle, float radius)
    {
        var localPosition = Quaternion.Euler(0, 0, angle) * Vector2.right * radius;
        var rotatablePositionPoint = Instantiate(rotatablePositionPointPrefab, rotatablePositionPointsContainer);
        rotatablePositionPoint.transform.localPosition = localPosition;
        _rotatablePositionPoints.Add(rotatablePositionPoint);
    }

    public bool HasHuman(Human human)
    {
        return _humanInfos.Any(info => info.human == human);
    }

    public void AddHuman(Human human)
    {
        var humanInfo = new CrowdHumanInfo()
        {
            human = human
        };

        _humanInfos.Add(humanInfo);

        human.transform.SetParent(humansContainer);

        UpdateStaticPositionPointsCount();
    }

    public void RemoveHuman(Human human)
    {
        var humanInfo = _humanInfos.Where(info => info.human == human).FirstOrDefault();

        if (humanInfo == null) return;

        _humanInfos.Remove(humanInfo);

        UpdateStaticPositionPointsCount();
    }
}

public class CrowdHumanInfo
{
    public Human human;
}
