using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HumanGroup : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float respawnTime = 5.0f;
    [SerializeField] private float spawnRadius = 5.0f;

    [MinMaxSlider(0.0f, 20.0f)]
    [SerializeField] private Vector2 timerIconScaleBounds = Vector2.up;

    [Space(10)]
    [SerializeField] private Transform humanContainer;
    [SerializeField] private Transform timerIcon;

    [ReadOnly]
    [SerializeField] private CircleCollider2D _circleCollider;

    [Header("Spawn Info")]
    [SerializeField] private List<Human> humanPrefabs = new List<Human>();

    private CrowdController _crowdController;
    private List<Human> _humans = new List<Human>();

    private float _lastInteractionTime;
    private bool _humanSpawned;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
    }
#endif

    private void Start()
    {
        _crowdController = CrowdController.Instance;

        SpawnHumans();
    }

    private void Update()
    {
        var minSize = new Vector2(timerIconScaleBounds.x, timerIconScaleBounds.x);
        var maxSize = new Vector2(timerIconScaleBounds.y, timerIconScaleBounds.y);
        timerIcon.localScale = _humanSpawned ? maxSize : Vector2.Lerp(minSize, maxSize, (Time.time - _lastInteractionTime) / respawnTime);

        if (Time.time - _lastInteractionTime >= respawnTime && !_humanSpawned)
        {
            SpawnHumans();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out HumanCollectArea _)) return;

        if (!_humanSpawned) return;

        AddHumans();
    }

    private void AddHumans()
    {
        foreach (var human in _humans)
        {
            _crowdController.AddHuman(human);
        }

        _humans.Clear();

        ResetValues();
    }

    private void ResetValues()
    {
        _humanSpawned = false;
        _lastInteractionTime = Time.time;
    }

    private void SpawnHumans()
    {
        foreach(var humanPrefab in humanPrefabs)
        {
            var newPos = transform.position + new Vector3(Random.value, Random.value) * spawnRadius;
            var human = Instantiate(humanPrefab, newPos, Quaternion.identity, humanContainer);
            _humans.Add(human);
        }

        _humanSpawned = true;
        CheckTriggerByCast();
    }

    private void CheckTriggerByCast()
    {
        var hits = Physics2D.CircleCastAll(transform.position, _circleCollider.radius, Vector2.zero);
        foreach (var hit in hits)
        {
            if (!hit.collider.TryGetComponent(out HumanCollectArea _)) continue;

            AddHumans();
        }
    }
}
