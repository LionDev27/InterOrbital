using System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace InterOrbital.Mission
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private bool _playTutorial;
        private DialogueSystemTrigger _trigger;

        private void Awake()
        {
            _trigger = GetComponent<DialogueSystemTrigger>();
        }

        private void Start()
        {
            if (!_playTutorial) return;
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

        public void StartDialogue(string conversation)
        {
            _trigger.conversation = conversation;
            _trigger.OnUse();
        }
    }
}