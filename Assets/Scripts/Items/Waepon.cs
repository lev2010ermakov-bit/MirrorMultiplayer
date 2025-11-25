using System;
using Mirror;
using UnityEngine;

public class Waepon : NetworkBehaviour
{
    public WaeponType Type;
    public GameObject PhysicsVersion;
}
[Serializable]
public enum WaeponType
{
    AK, // Yes
    Gagil, // Yes
    M4A4, // Yes
    M4A1, // Yes
    XM, // Yes
    USP, // 
    GLOCK, // Yes
    AWP, // Yes
    SSG, // 
    Negenev, // 
    Desert_Eagle // Yes
}