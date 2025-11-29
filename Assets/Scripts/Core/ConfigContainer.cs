using UnityEngine;

public class ConfigContainer : MonoBehaviour
{
    [SerializeField] private WeaponConfig _weaponConfig;
    [SerializeField] private MovementConfig _movementConfig;

    public static WeaponConfig Weapon;
    public static MovementConfig Movement;

    private void Start()
    {
        Weapon = _weaponConfig;
        Movement = _movementConfig;
    }
}