using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class BlinkDagger : MonoBehaviour
{
    [Header("Inspec")]
    [SerializeField]
    private bool _used;
    public bool Used
    {
        get
        {
            return _used;
        }
        set
        {
            _used = value;
        }
    }

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onRequestActivateSkill;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onTeleport;

    public void Activate()
    {
        _onRequestActivateSkill.Raise(SkillId.Teleport, SkillState.Second);
    }

    public void Teleport(CharacterController chaConToTeleport)
    {
        if (Used)
        {
            return;
        }

        _onTeleport.Invoke();

        Used = true;

        chaConToTeleport.enabled = false;
        chaConToTeleport.transform.position = transform.position - transform.forward * 1.5f;


        // Delay 3 frames for transform is applied
        Observable.TimerFrame(3).Subscribe(_ =>
        {
            chaConToTeleport.enabled = true;
            chaConToTeleport.SimpleMove(Vector3.zero);
        });
    }
}
