using UnityEngine;

namespace PlayerScripts
{
    public class PlayerData : MonoBehaviour
    {
        public static string PlayerName;
        public static PlayerData Instance;

        private void Start()
        {
            if (Instance != null) 
                Destroy(gameObject);
            else
            {  
                Instance = this; 
                DontDestroyOnLoad(gameObject);
            } 
        }
    }
}
