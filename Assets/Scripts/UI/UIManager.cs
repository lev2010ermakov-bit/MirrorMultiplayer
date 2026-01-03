using UnityEngine;
using TMPro;
using PlayerScripts;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RedText;
    [SerializeField] private GameObject SniperPanel;
    [SerializeField] private GameObject StoreMenu;
    public static UIManager Instance;
    public Player player;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
                StoreMenuSet();
    }

    public void DiePanel()
    {
        
    }
    public void SnipeSet(bool v)
    {
        SniperPanel.SetActive(v);
    }
    public void StoreMenuSet()
    {
        Cursor.visible = !StoreMenu.activeSelf;
        Cursor.lockState = StoreMenu.activeSelf ? CursorLockMode.Locked : CursorLockMode.None;
        StoreMenu.SetActive(!StoreMenu.activeSelf);
    }

    public void BuyWeapon(string name)
    {
        if (name == nameof(WeaponType.AWP))
            player.BuyWeapon(WeaponType.AWP);
        if (name == nameof(WeaponType.AK))
            player.BuyWeapon(WeaponType.AK);
        if (name == nameof(WeaponType.GLOCK))
            player.BuyWeapon(WeaponType.GLOCK);
        if (name == nameof(WeaponType.KNIFE))
            player.BuyWeapon(WeaponType.KNIFE);
    }
}