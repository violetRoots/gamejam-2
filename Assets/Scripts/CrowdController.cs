using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CrowdController : SingletonMonoBehaviourBase<CrowdController>
{
    public bool IsWin { get; set; }
    public bool IsGameOver { get; set; }

    public int DiedCount { get; private set; }
    public int KilledCount { get; private set; }
    public int SavedCount => _savedHumans.Count;

    [SerializeField] private float movementSpeed = 100.0f;
    [SerializeField] private float startShooterRadius = 1.0f;
    [SerializeField] private float gunOffset = 1.0f;
    [Min(1)]
    [SerializeField] private int startShootersCount;
    [Min(1)]
    [SerializeField] private int startIlluminatorsCount;

    [Space]
    [SerializeField] private int maxIlluminatorsCount = 10;
    [MinMaxSlider(0.0f, 10.0f)]
    [SerializeField] private Vector2 vignetteBounds;

    [Space]
    [SerializeField] private Transform humansContainer;
    [SerializeField] private Transform shootersDestinationPoint;
    [SerializeField] private Transform illuminatorsDestinationPoint;

    [SerializeField] private GameObject gameplayCamera;
    [SerializeField] private GameObject endCamera;

    [Space]
    [SerializeField] private Shooter shooter;
    [SerializeField] private Illuminator illuminator;
    [SerializeField] Volume volume;

    private readonly List<Human> _humans = new();

    private List<Shooter> _shooters => _humans.Where(h => h is Shooter).Cast<Shooter>().ToList();
    private List<Illuminator> _illuminators => _humans.Where(h => h is Illuminator).Cast<Illuminator>().ToList();
    private List<Human> _savedHumans => _humans.Where(h => h.InSafe).ToList();

    private void Start()
    {
        SpawnUnits(shooter, startShootersCount);
        SpawnUnits(illuminator, startIlluminatorsCount);
    }

    private void Update()
    {
        UpdateShooterDestinationPointByInput();
        UpdateShootersRotationByInput();
        UpdatePosByInput();

        CheckGameOver();

        UpdatePostProcess();
        UpdateCameras();
    }

    private void SpawnUnits(Human human, int count)
    {
        for (var i = 0; i < count; i++)
            SpawnUnit(human);
    }

    private void SpawnUnit(Human human)
    {
        var newHuman = Instantiate(human, ((Vector2) transform.position) + Random.insideUnitCircle * 5.0f, Quaternion.identity);
        AddHuman(newHuman);
    }

    public void AddHuman(Human newHuman)
    {
        newHuman.transform.SetParent(humansContainer);

        newHuman.AddInCrowd();
        newHuman.SetDestinationPoint(GetHumanDestinationPoint(newHuman));

        _humans.Add(newHuman);
    }

    private void UpdateShooterDestinationPointByInput()
    {
        var oldPos = shootersDestinationPoint.position;
        var newPos = (InputManager.Instance.WorldMousePos - transform.position).normalized * startShooterRadius;
        newPos.z = oldPos.z;
        shootersDestinationPoint.localPosition = newPos;
    }

    private void UpdateShootersRotationByInput()
    {
        for(var i = 0; i < _shooters.Count; i++)
        {
            var shooter = _shooters[i];

            var pos = InputManager.Instance.WorldMousePos;
            pos.z = shooter.transform.position.z;

            var angle = Vector3.SignedAngle(shooter.transform.position - pos, Vector3.right, Vector3.forward);
            pos += Quaternion.Euler(0.0f, 0.0f, angle) * Vector2.up * gunOffset * i;
            angle = Vector3.SignedAngle(shooter.transform.position - pos, Vector3.right, Vector3.forward);

            var newRot = shooter.transform.rotation.eulerAngles;
            newRot.z = -(angle + 180.0f);
            shooter.SetBodyRotation(newRot);
        }
    }

    private void UpdatePosByInput()
    {
        transform.position += InputManager.Instance.MovementInputVector * movementSpeed * Time.deltaTime;
    }

    private Transform GetHumanDestinationPoint(Human human)
    {
        if (human is Shooter)
            return shootersDestinationPoint;
        else
            return illuminatorsDestinationPoint;
    }

    public bool TryKill(Human human, bool killedByHuman)
    {
        if (!_humans.Contains(human)) return false;

        _humans.Remove(human);
        human.Die();

        DiedCount++;
        if (killedByHuman)
            KilledCount++;

        return true;
    }

    private void CheckGameOver()
    {
        if (_humans.Count > 0 || IsGameOver) return;

        IsGameOver = true;

        UIManager.Instance.SetVisibleGameOver(true);
    }

    private void UpdatePostProcess()
    {
        if (!IsWin)
        {
            var newIntensity = Mathf.Lerp(vignetteBounds.y, vignetteBounds.x, (float)_illuminators.Count / maxIlluminatorsCount);
            volume.profile.TryGet(out Vignette vignette);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, newIntensity, Time.deltaTime);
        }
        else
        {
            volume.profile.TryGet(out Vignette vignette);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.5f, Time.deltaTime);
        }
    }

    public void SetNewDestinationForAll(Transform destination)
    {
        foreach (var human in _humans)
            human.SetDestinationPoint(destination);
    }

    public bool SaveHuman(Human human)
    {
        human.InSafe = true;
        human.gameObject.SetActive(false);

        IsWin = _savedHumans.Count >= _humans.Count;

        return IsWin;
    }

    private void UpdateCameras()
    {
        gameplayCamera.SetActive(!IsWin);
        endCamera.SetActive(IsWin);
    }
}
