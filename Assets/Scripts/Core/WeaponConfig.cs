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
    public GameObject FPCPrefab;
    public GameObject SkinPrefab;

    public int DamagePerBullet;
    public AudioClip ShootSound;
    public AudioClip AimSound;
}
