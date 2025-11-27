using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Game-Configs/WeaponConfig", order = 0)]
public class WeaponConfig : ScriptableObject
{
    public List<WeaponSetting> WeaponSetts; 
}

[Serializable]
public struct WeaponSetting
{
    public WaeponType Type;
    public bool SniperLike;
    public GameObject PhysicVersion;
    public GameObject HandedVersion;

    public int DamagePerBullet;
    [Range(1f, 8f)]public float HeadMultipler;
    
    public float StandSpray;
    public float CroshiredSpray;
    public float MoveSpray;

    public float OutForce;

    public float StrikeToStrikeTime;
}
