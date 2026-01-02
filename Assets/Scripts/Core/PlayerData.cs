using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public string PlayerName;
    public static PlayerData Instance;

    private void Start()
    {
        if (Instance != null) Destroy(gameObject); else Instance = this; 
        DontDestroyOnLoad(gameObject);
    }

    public void CallPlayerName(string name) => PlayerName = name;
}
