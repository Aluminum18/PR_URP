using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportActivator : SkillActivator
{
    [Header("Runtime Reference")]
    [SerializeField]
    private Transform _ownerTransform;

    [Header("Config")]
    [SerializeField]
    private TargetableProjectileSpawner _daggerSpawner;

    private GameObject _dagger;

    public override void Setup(params object[] args)
    {
        if (!(args[0] is Vector3Variable aimSpot))
        {
            Debug.LogError("invalid aimSpot", this);
            return;
        }
        _daggerSpawner.SetAimSpotInput(aimSpot);

        if (!(args[1] is Transform ownerTransform))
        {
            Debug.LogError("invalid Transform", this);
            return;
        }
        _ownerTransform = ownerTransform;
    }

    public override void FirstState()
    {
        _dagger = _daggerSpawner.FireArrowToAimSpot();
    }

    public override void SecondState()
    {
        _ownerTransform.position = _dagger.transform.position;
    }
}
