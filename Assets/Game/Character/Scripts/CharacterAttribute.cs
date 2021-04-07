using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttribute : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _thisClientUserId;
    [SerializeField]
    private PlayersInMapInfoSO _playerInMapInfo;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private InputValueHolders _inputHolders;

    [Header("Config")]
    [SerializeField]
    private Transform _camFollow;
    [SerializeField]
    private Transform _camLook;
    [SerializeField]
    private Transform _aimLook;
    [SerializeField]
    private TargetableProjectileSpawner _arrowSpawner;
    [SerializeField]
    private CharacterController _chaCon;
    [SerializeField]
    private CharacterAnimController _animController;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Transform _arrnadeModelPos;
    [SerializeField]
    private Transform _daggerModelPos;

    private Vector3Variable _aimSpot;
    private Vector3Variable _joyStickDirection;
    private int _team = 0;
    private List<int> _pickedSkills = new List<int>();

    public string AssignedUserId { get; set; }
    public int Team
    {
        get
        {
            if (_team == 0)
            {
                _team = _roomInfo.GetTeam(AssignedUserId);
            }
            return _team;
        }
    }
    public List<int> PickedSkills
    {
        get
        {
            if (_pickedSkills.Count == 0)
            {
                _pickedSkills = _roomInfo.GetPickAtPos(_roomInfo.GetPlayerPos(AssignedUserId));
            }
            return _pickedSkills;
        }
    }
    public bool IsThisPlayer
    {
        get
        {
            var characterAtt = GetComponent<CharacterAttribute>();
            if (characterAtt == null)
            {
                return false;
            }

            if (!_thisClientUserId.Value.Equals(characterAtt.AssignedUserId))
            {
                return false;
            }
            return true;
        }
    }
    public Transform Follow
    {
        get
        {
            return _camFollow;
        }
    }
    public Transform Camlook
    {
        get
        {
            return _camLook;
        }
    }
    public Transform AimLook
    {
        get
        {
            return _aimLook;
        }

    }
    public CharacterController ChaCon
    {
        get
        {
            return _chaCon;
        }
    }
    public CharacterAnimController AnimController
    {
        get
        {
            return _animController;
        }
    }
    public Animator Animator
    {
        get
        {
            return _animator;
        }
    }
    public TargetableProjectileSpawner ArrowSpawner
    {
        get
        {
            return _arrowSpawner;
        }
    }
    public Vector3Variable AimSpot
    {
        get
        {
            if (_aimSpot == null)
            {
                _aimSpot = _inputHolders.GetInputValueHolder(_roomInfo.GetPlayerPos(AssignedUserId)).AimSpot;
            }
            return _aimSpot;
        }
    }

    public Vector3Variable JoyStickDirection
    {
        get
        {
            if (_joyStickDirection == null)
            {
                _joyStickDirection = _inputHolders.GetInputValueHolder(_roomInfo.GetPlayerPos(AssignedUserId)).JoyStickDirection;
            }
            return _joyStickDirection;
        }
    }

    private PlayerInMapInfo _inMapInfo;
    public PlayerInMapInfo InMapInfo
    {
        get
        {
            if (_inMapInfo == null)
            {
                _inMapInfo = _playerInMapInfo.GetPlayerInfo(_roomInfo.GetPlayerPos(AssignedUserId));
            }
            return _inMapInfo;
        }
    }

    public Transform ArrNadeModelTransform
    {
        get
        {
            return _arrnadeModelPos;
        }
    }

    public Transform DaggerModelTransform
    {
        get
        {
            return _daggerModelPos;
        }
    }

}
