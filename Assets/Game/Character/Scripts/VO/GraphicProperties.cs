using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterGraphicProps", menuName = "NFQ/Character/Create GraphicProps")]
[System.Serializable]
public class GraphicProperties : ScriptableObject
{
    public Texture2D MainTex;

    public Color TeamColor;
    public Color TeamColorEmission;

    public Material DefaultMat;
    public Material DissolveMat;
}
