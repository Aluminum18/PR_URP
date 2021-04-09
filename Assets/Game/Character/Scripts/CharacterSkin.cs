using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CharacterSkin : MonoBehaviour
{
    [SerializeField]
    private Material _defaultMat;
    [SerializeField]
    private Material _dessolveMat;
    [SerializeField]
    private float _dissolveTime;
    [SerializeField]
    private List<SkinnedMeshRenderer> _meshes;
    [SerializeField]
    private GraphicProperties _team1Props;
    [SerializeField]
    private GraphicProperties _team2Props;

    private GraphicProperties _props;

    public void SetUniform(int team)
    {
        _props = team == 1 ? _team1Props : _team2Props;

        for (int i = 0; i < _meshes.Count; i++)
        {
            _meshes[i].material.mainTexture = _props.MainTex;
        }
    }

    public void ActiveDissolve(bool appear)
    {
        MainThreadDispatcher.StartUpdateMicroCoroutine(IE_Appear());
    }

    private IEnumerator IE_Appear()
    {
        SwitchMat(_dessolveMat);

        float dissolveTime = _dissolveTime;

        SetMatProps("_MainTex", _props.MainTex);
        while (dissolveTime > 0f)
        {
            SetMatProps("InitDissolvePercent", dissolveTime / _dissolveTime);
            dissolveTime -= Time.deltaTime;

            yield return null;
        }

        ToDefaultMat();
    }

    public void SwitchMat(Material mat)
    {
        for (int i = 0; i < _meshes.Count; i++)
        {
            _meshes[i].material = mat;
        }
    }

    public void ToDefaultMat()
    {
        for (int i = 0; i < _meshes.Count; i++)
        {
            _meshes[i].material = _defaultMat;
            _meshes[i].material.mainTexture = _props.MainTex;
        }
    }


    public void SetMatProps(string props, float value)
    {
        for (int i = 0; i < _meshes.Count; i++)
        {
            _meshes[i].material.SetFloat(props, value);
        }
    }

    public void SetMatProps(string props, Texture value)
    {
        for (int i = 0; i < _meshes.Count; i++)
        {
            _meshes[i].material.SetTexture(props, value);
        }
    }
}
