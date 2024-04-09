using SkyCrush.WSGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionProgressPanel : View
{
    [SerializeField] private Image filler;

    private Generator _generator;

    public override void OnCreate()
    {
        base.OnCreate();

        _generator = Generator.Instance;
    }

    private void Update()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        filler.transform.localScale = new Vector3(_generator.StageProcess / 100.0f, 1.0f, 1.0f);
    }
}
