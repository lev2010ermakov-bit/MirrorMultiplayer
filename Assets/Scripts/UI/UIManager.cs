using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RedText;
    public static UIManager Instance;

    private void Start()
    {
        Instance = this;
    }

    public void DiePanel()
    {
        
    }
}