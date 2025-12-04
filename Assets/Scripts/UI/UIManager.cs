using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RedText;
    [SerializeField] private GameObject SniperPanel;
    public static UIManager Instance;

    private void Start()
    {
        Instance = this;
    }

    public void DiePanel()
    {
        
    }
    public void SnipeSet(bool v)
    {
        SniperPanel.SetActive(v);
    }
}