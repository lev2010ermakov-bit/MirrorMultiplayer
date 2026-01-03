using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayerScripts;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button SaveButton;

    public static string PlayerName;

    private void Start()
    {
        SetButtonInteracteble(input.text);
        if (!string.IsNullOrWhiteSpace(PlayerData.PlayerName))
        {
            input.text = PlayerData.PlayerName;
        }   
    }

    public void SetPlayerName()
    {
        PlayerName = input.text;
        PlayerData.PlayerName = PlayerName;
    }

    public void SetButtonInteracteble(string value)
    {
        SaveButton.interactable = !string.IsNullOrWhiteSpace(input.text);
    }
}