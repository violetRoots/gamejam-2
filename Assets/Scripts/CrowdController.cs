using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrowdController : MonoBehaviour
{
    private const float RadiusToPositionPointMultiplier = 2.5f;

    [SerializeField] private float moveSpeed = 1.0f;

    [MinMaxSlider(1.0f, 50.0f)]
    [SerializeField] private Vector2 radiusBounds;
    [MinMaxSlider(1, 200)]
    [SerializeField] private Vector2Int positionPointsCountBounds;
    [SerializeField] private RotatablePositionPoint rotatablePositionPointPrefab;
    [SerializeField] private StaticPositionPoint staticPositionPointPrefab;
    [SerializeField] private Transform rotatablePositionPointsContainer;
    [SerializeField] private Transform staticPositionPointsContainer;
    [SerializeField] private Transform moveContainer;

    private InputManager _inputManager;

    private List<CrowdHumanInfo> _humanInfos = new List<CrowdHumanInfo>();
    private List<RotatablePositionPoint> _rotatablePositionPoints = new List<RotatablePositionPoint>();
    private List<StaticPositionPoint> _staticPositionPoints = new List<StaticPositionPoint>();

    private float Radius 
    {
        get => _radius;
        set => _radius = Mathf.Clamp(value, radiusBounds.x, radiusBounds.y);
    }
    private float _radius;

    public int RotatablePositionPointsCount
    {
        get => _postionPointsCount;
        set => _postionPointsCount = Mathf.Clamp(value, positionPointsCountBounds.x, positionPointsCountBounds.y);
    }
    private int _postionPointsCount;

    private void Start()
    {
        _inputManager = InputManager.Instance;

        InitHumans();
    }

    private void Update()
    {
        MoveCrowd();
        RotateCrowd();
        UpdateRadiusByHumanCount();
        UpdateRotatablePositionPointsCount();
        UpdateStaticPositionPointsCount();
        MoveRotatableHumans();
        MoveStaticHumans();
    }

    private void MoveCrowd()
    {
        moveContainer.Translate(_inputManager.MoveDirection * moveSpeed * Time.deltaTime);
    }

    private void RotateCrowd()
    {
        var angle = -Vector2.SignedAngle(_inputManager.RotateDirection, Vector3.right);
        rotatablePositionPointsContainer.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void MoveStaticHumans()
    {
        var staticHumanInfos = _humanInfos.Where(humanInfo => humanInfo.human is StaticHuman).ToArray();
        for (var i = 0; i < staticHumanInfos.Length; i++)
        {
            var destinationPos = moveContainer.transform.position;
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

    private void InitHumans()
    {
        var humans = GetComponentsInChildren<Human>();
        for (var i = 0; i < humans.Length; i++)
        {
            AddHuman(humans[i]);
        }
    }

    private void UpdateRadiusByHumanCount()
    {
        Radius = Mathf.Sqrt(((float)_humanInfos.Count) / Mathf.PI);
    }

    private void UpdateRotatablePositionPointsCount()
    {
        for (var i = 0; i < _rotatablePositionPoints.Count; i++)
        {
            Destroy(_rotatablePositionPoints[i]);
        }
        _rotatablePositionPoints.Clear();

        RotatablePositionPointsCount = (int)(2 * Mathf.PI * Radius * RadiusToPositionPointMultiplier);

        for (var i = 0; i < RotatablePositionPointsCount; i++)
        {
            var positionPoint = Instantiate(rotatablePositionPointPrefab, rotatablePositionPointsContainer);
            _rotatablePositionPoints.Add(positionPoint);
        }

        var angle = Mathf.Rad2Deg * Mathf.PI * 2.0f / RotatablePositionPointsCount;
        for (int i = 0; i < RotatablePositionPointsCount; i++)
        {
            var index = i % 2 == 0 ? RotatablePositionPointsCount - 1 - i : i;
            _rotatablePositionPoints[i].transform.localPosition = Quaternion.Euler(0, 0, angle * index) * Vector3.right * Radius;
        }
    }

    private void UpdateStaticPositionPointsCount()
    {
        for (var i = 0; i < _staticPositionPoints.Count; i++)
        {
            Destroy(_staticPositionPoints[i]);
        }
        _staticPositionPoints.Clear();

        var firstPositionPoint = Instantiate(staticPositionPointPrefab, staticPositionPointsContainer);
        _staticPositionPoints.Add(firstPositionPoint);

        var test = 2;
        for (var i = 1; i < (int) (Radius * test); i++)
        {
            var pointsCount = (int)(2 * Mathf.PI * (Radius / test) * i * RadiusToPositionPointMultiplier);

            var angle = Mathf.Rad2Deg * Mathf.PI * 2.0f / pointsCount;
            for (var j = 0; j < pointsCount; j++)
            {
                var index = j % 2 == 0 ? pointsCount - 1 - j : j;
                var positionPoint = Instantiate(staticPositionPointPrefab, staticPositionPointsContainer);
                positionPoint.transform.localPosition = Quaternion.Euler(0, 0, angle * index) * Vector3.right * (Radius / test) * i;
                _staticPositionPoints.Add(positionPoint);
            }
        }
    }

    private void AddHuman(Human human)
    {
        var humanInfo = new CrowdHumanInfo()
        {
            human = human
        };

        _humanInfos.Add(humanInfo);
    }
}

public class CrowdHumanInfo
{
    public Human human;
}
