using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class TeleportActivator : SkillActivator
{
    [Header("Runtime Reference")]
    [SerializeField]
    private CharacterController _ownerTransform;

    [Header("Config")]
    [SerializeField]
    private TargetableProjectileSpawner _daggerSpawner;
    [SerializeField]
    private ObjectPool _teleportEffectPool;

    private GameObject _dagger;
    private BlinkDagger _daggerComp;

    public override void Setup(params object[] args)
    {
        if (!(args[0] is Vector3Variable aimSpot))
        {
            Debug.LogError("invalid aimSpot", this);
            return;
        }
        _daggerSpawner.SetAimSpotInput(aimSpot);

        if (!(args[1] is CharacterController ownerTransform))
        {
            Debug.LogError("invalid CharacterController", this);
            return;
        }
        _ownerTransform = ownerTransform;

        _teleportEffectPool.SpawnPos = _ownerTransform.transform;
    }

    public override void FirstState()
    {
        _dagger = _daggerSpawner.FireArrowToAimSpot();
        _daggerComp = _dagger.GetComponent<BlinkDagger>();
        _daggerComp.Used = false;
    }

    public override void SecondState()
    {
        _daggerComp.Teleport(_ownerTransform);
    }
}
