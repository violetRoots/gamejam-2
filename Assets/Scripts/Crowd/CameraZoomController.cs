using Cinemachine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    [MinMaxSlider(0, 50)]
    [SerializeField] private Vector2Int humansCountBounds = Vector2Int.up;
    [MinMaxSlider(0.0f, 20.0f)]
    [SerializeField] private Vector2 zoomBounds = Vector2.up;
    [SerializeField] protected float zoomChangeSpeed = 1.0f;

    private CrowdController _crowdController;
    private CinemachineVirtualCamera _vCam;

    private void Start()
    {
        _crowdController = CrowdController.Instance;

        _vCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        var humansCount = _crowdController.GetHumansCount();
        var humansValue = (float) (Mathf.Clamp(humansCount, humansCountBounds.x, humansCountBounds.y) - humansCountBounds.x) / humansCountBounds.y;
        var zoomValue = Mathf.Lerp(zoomBounds.x, zoomBounds.y, humansValue);
        _vCam.m_Lens.OrthographicSize = Mathf.Lerp(_vCam.m_Lens.OrthographicSize, zoomValue, zoomChangeSpeed * Time.smoothDeltaTime);
    }
}
