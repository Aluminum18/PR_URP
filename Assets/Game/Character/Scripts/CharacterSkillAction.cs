using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CharacterSkillAction : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _thisUserId;
    [SerializeField]
    private IntegerListVariable _thisPickedSkills;
    [SerializeField]
    private SkillListSO _skillList;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onActivateSkill;

    [Header("Config")]
    [SerializeField]
    private Transform _skillObjTransform;
    [SerializeField]
    private CharacterAction _characterAction;

    private CharacterAttribute _attribute;
    private Dictionary<int, GameObject> _skillObjectMap = new Dictionary<int, GameObject>();

    public void SetUpSkills()
    {
        _skillObjectMap.Clear();

        List<int> pickedSkills = _attribute.PickedSkills;

        if (pickedSkills == null)
        {
            return;
        }

        for (int i = 0; i < pickedSkills.Count; i++)
        {
            SkillId skillId = (SkillId)pickedSkills[i];
            var skillObj = _skillList.GetSkillActivationObject(skillId);
            if (skillObj == null)
            {
                continue;
            }

            var skill = Instantiate(skillObj, _skillObjTransform);
            _skillObjectMap[(int)skillId] = skill;

            if (_skillList.GetSkillType(skillId).Equals(SkillType.Passive))
            {
                ActivateSkill(skillId, SkillState.First, null);
            }

            SetupSkill(skillId, skill);
        }
    }

    private void SetupSkill(SkillId skillId, GameObject skillObj)
    {
        var activator = skillObj.GetComponent<SkillActivator>();
        if (activator == null)
        {
            return;
        }
        activator.Owner = _attribute.AssignedUserId;
        activator.Team = _attribute.Team;

        switch (skillId)
        {
            case SkillId.ArrNade:
                {
                    activator.Setup(_attribute.AimSpot, _skillList.GetSkillDamage(skillId));

                    var skillAnimControl = skillObj.GetComponent<SkillAnimControl>();
                    skillAnimControl.Setup(_attribute.AnimController);
                    skillAnimControl.AttachModel(_attribute.ArrNadeModelTransform);
                    break;
                }
            case SkillId.Teleport:
                {
                    activator.Setup(_attribute.AimSpot, _attribute.ChaCon);

                    var skillAnimControl = skillObj.GetComponent<SkillAnimControl>();
                    skillAnimControl.Setup(_attribute.AnimController);
                    skillAnimControl.AttachModel(_attribute.DaggerModelTransform);
                    break;
                }
            case SkillId.PowerShot:
                {
                    // delay 1 frame to make sure the weapon is latest;
                    Observable.TimerFrame(1).Subscribe(_ =>
                    {
                        skillObj.GetComponent<SkillAnimControl>()?.Setup(_attribute.AnimController);
                        skillObj.GetComponent<SkillActivator>()?.Setup( _attribute.AimSpot, 
                                                                        _characterAction.UsingWeapon.AttackDelay, 
                                                                        _skillList.GetSkillDamage(skillId));
                    });
                    break;
                }
            case SkillId.MulShot:
                {
                    // delay 1 frame to make sure the weapon is latest;
                    Observable.TimerFrame(1).Subscribe(_ =>
                    {
                        skillObj.GetComponent<SkillAnimControl>()?.Setup(_attribute.AnimController);
                        skillObj.GetComponent<SkillActivator>()?.Setup( _attribute.AimSpot, 
                                                                        _characterAction.UsingWeapon.AttackDelay, 
                                                                        _skillList.GetSkillDamage(skillId));
                    });
                    break;
                }
            default:
                {
                    return;
                }
        }
    }

    private void ActivateSkill(SkillId skillId, SkillState skillState, object[] skillData)
    {
        var skillSO = _skillList.GetSkill((int)skillId);

        if (skillSO.SkillType.Equals(SkillType.Passive))
        {
            HandlePassiveSkill(skillId);
            return;
        }

        if (skillSO.SkillUsageType.Equals(SkillUsageType.SingleState))
        {
            HandleSingleStateSkill(skillId);
            return;
        }

        HandleDoubleStateSkill(skillId, skillState);
    }

    private void HandlePassiveSkill(SkillId skillId)
    {
        // refactor this
        var skillObject = _skillObjectMap[(int)skillId];
        if (skillObject == null)
        {
            return;
        }
        var weapon = skillObject.GetComponent<RangeTargetableWeapon>();
        if (weapon == null)
        {
            return;
        }

        weapon.SetTeamAndOwner(_attribute.Team, _attribute.AssignedUserId);
        weapon.SetTarget(_attribute.AimSpot);

        _characterAction.SetWeapon(weapon);

        var crossbowAnimControl = skillObject.GetComponent<CrossbowControlCharacterAnim>();
        if (crossbowAnimControl == null)
        {
            return;
        }
        crossbowAnimControl.SetAnimator(_attribute.Animator);
        crossbowAnimControl.SetAnimControl(_attribute.AnimController);
        crossbowAnimControl.ActiveWeaponLayer(true);
    }

    private void HandleSingleStateSkill(SkillId skillId)
    {
        _skillObjectMap.TryGetValue((int)skillId, out var skillObj);
        if (skillObj == null)
        {
            return;
        }

        skillObj.GetComponent<SkillActivator>()?.ActiveFirstState();
    }

    private void HandleDoubleStateSkill(SkillId skillId, SkillState skillState)
    {
        _skillObjectMap.TryGetValue((int)skillId, out var skillObj);
        if (skillObj == null)
        {
            return;
        }

        if (skillState.Equals(SkillState.Second))
        {
            skillObj.GetComponent<SkillActivator>()?.ActiveSecondState();
        }
        else
        {
            skillObj.GetComponent<SkillActivator>()?.ActiveFirstState();
        }
    }

    private void HandleActivateSkill(object[] eventParam)
    {
        if (!(eventParam[0] is string userId))
        {
            userId = "";
        }

        if (!userId.Equals(_attribute.AssignedUserId))
        {
            return;
        }

        if (!(eventParam[1] is SkillId skillId))
        {
            return;
        }
        
        if (!(eventParam[2] is SkillState skillState))
        {
            return;
        }

        if (!(eventParam[3] is object[] skillData))
        {
            skillData = null;
        }

        ActivateSkill(skillId, skillState, skillData);
    }

    private void OnEnable()
    {
        _attribute = GetComponent<CharacterAttribute>();
        _onActivateSkill.Subcribe(HandleActivateSkill);
    }

    private void OnDisable()
    {
        _onActivateSkill.Unsubcribe(HandleActivateSkill);
    }

    
}
