using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using kcp2k;
using System;

namespace Mirror.Examples.Chat
{
    public class ChatUI : NetworkBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] TextMeshProUGUI chatText;
        [SerializeField] Scrollbar scrollbar;
        [SerializeField] TMP_InputField chatMessage;
        [SerializeField] Button sendButton;

        // This is only set on client to the name of the local player
        public static string localPlayerName;

        // Server-only cross-reference of connections to player names

        public override void OnStartServer()
        {
            
        }

        private void Start()
        {
            ToggleButton(chatMessage.text);
        }

        public override void OnStartClient()
        {
            chatText.text = "";
        }

        [Command(requiresAuthority = false)]
        void CmdSend(string message, string senderName)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                RpcReceive(senderName, message.Trim());

                if (isServer)
                {
                    string LogMessage = senderName + ": " + message;
                    AppendMessageToLog(LogMessage);
                }
            }
        }

        [ClientRpc]
        void RpcReceive(string senderName, string message)
        {
            string prettyMessage = senderName == localPlayerName ?
                $"<color=#CF27F5>{"You"}:</color> {message}" :
                $"<color=#27F56C>{senderName}:</color> {message}";
            AppendMessage(prettyMessage);
        }

        void AppendMessage(string message)
        {
            StartCoroutine(AppendAndScroll(message));
        }

        [Server]
        private void AppendMessageToLog(string Log)
        {
            try
            {
                // Append the content to the file. Use Environment.NewLine for a new line.
                File.AppendAllText($"/home/{Environment.UserName}/Documents/Chat.txt", Log + "\n");
                Debug.Log("Message added to chat history file");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error writing to file: " + e.Message);
            }
        }

        IEnumerator AppendAndScroll(string message)
        {
            chatText.text += message + "\n";


            // it takes 2 frames for the UI to update ?!?!
            yield return null;
            yield return null;

            // slam the scrollbar down
            scrollbar.value = 0;
        }

        // Called by UI element ExitButton.OnClick
        public void ExitButtonOnClick()
        {
            // StopHost calls both StopClient and StopServer
            // StopServer does nothing on remote clients
            NetworkManager.singleton.StopHost();
        }

        // Called by UI element MessageField.OnValueChanged
        public void ToggleButton(string input)
        {
            sendButton.interactable = !string.IsNullOrWhiteSpace(chatMessage.text);
        }

        // Called by UI element MessageField.OnEndEdit
        public void OnEndEdit(string input)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Submit"))
                SendMessage();
        }

        // Called by OnEndEdit above and UI element SendButton.OnClick
        public void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(chatMessage.text))
            {
                CmdSend(chatMessage.text.Trim(), localPlayerName);
                chatMessage.text = string.Empty;
                chatMessage.ActivateInputField();
            }
        }
    }
}
