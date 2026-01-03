using System;
using System.Collections.Generic;
using UnityEngine;
using PlayerScripts;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Game-Configs/WeaponConfig", order = 0)]
public class WeaponConfig : ScriptableObject
{
    public float GunRayDistance;
    public float KnifeRayDistance;
    public float KnifeRayThick;
    public List<WeaponSetting> WeaponSetts; 
}

[Serializable]
public struct WeaponSetting
{
    public WeaponType Type;
    public GameObject FPCPrefab;
    public GameObject SkinPrefab;

    public int DamagePerBullet;
    public AudioClip ShootSound;
    public AudioClip AimSound;
}
