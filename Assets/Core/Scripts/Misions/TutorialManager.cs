using System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace InterOrbital.Mission
{
    public class TutorialManager : MonoBehaviour
    {
        private DialogueSystemTrigger _trigger;

        private void Awake()
        {
            _trigger = GetComponent<DialogueSystemTrigger>();
        }

        private void Start()
        {
            Invoke(nameof(StartDialogue), 4f);
        }

        private void StartDialogue()
        {
            _trigger.OnUse();
        }

        public void OnConversationStart()
        {
            Time.timeScale = 0f;
        }

        public void OnConversationEnd()
        {
            Time.timeScale = 1f;
        }
    }
}