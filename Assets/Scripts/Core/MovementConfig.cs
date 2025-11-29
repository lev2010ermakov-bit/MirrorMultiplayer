using UnityEngine;

[CreateAssetMenu(fileName = "MovementConfig", menuName = "Game-Configs/MovementConfig")]
public class MovementConfig : ScriptableObject
{
    [Range(1, 20)] public int WalkSpeed; 
    [Range(1, 30)] public int RunSpeed;
    [Range(1, 10)] public float LookSence;
    [Range(1, 30)] public int SpeedChangeRate;
    [Range(0, 10)] public float JumpHeight;
    [Range(0, 50)] public float Gravity;

    [Space(30)]
    [Range(0, 70)] public int DropWeaponForce;
    [Range(0, 100)] public int TrowWeaponForce; 
}
