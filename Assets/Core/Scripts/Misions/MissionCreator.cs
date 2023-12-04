using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InterOrbital.Item;
using PixelCrushers.DialogueSystem;

namespace InterOrbital.Mission
{
    public class MissionCreator : MonoBehaviour
    {
        [SerializeField] private Image _missionImage;
        [SerializeField] private TextMeshProUGUI _missionText;
        [SerializeField] private TextMeshProUGUI _feedbackText;
        [SerializeField] private TutorialManager _tutorialManager;
        private int _actualProgress = 0;
        private MissionScriptableObject _actualMission;
        private bool _missionCompleted;
        private ButtonMissionController _buttonMissions;

        private void Awake()
        {
            _buttonMissions = GetComponent<ButtonMissionController>();
        }

        private IEnumerator WaitForNextMission()
        {
            _missionText.color = Color.green;
            yield return new WaitForSeconds(2);
            _missionCompleted = false;
            _tutorialManager.StartDialogue(_actualMission.nextConversationName);
        }
    
        public void CreateMission(MissionScriptableObject mission)
        {
            _actualMission = mission;
            if (mission.imageMission != null)
            {
                _missionImage.gameObject.SetActive(true);
                _missionImage.sprite = mission.imageMission;
            }
            else
                _missionImage.gameObject.SetActive(false);
            _missionText.text = mission.missionDescription;
            _missionText.color = Color.white;
            if (mission is MissionRecollectScriptableObject recollectMission)
            {
                _feedbackText.gameObject.SetActive(true);
                _feedbackText.text = "0/" + recollectMission.amountToReach;
                _feedbackText.color = Color.red;
            }
            else
                _feedbackText.gameObject.SetActive(false);
            if (mission is MissionButtonScriptableObject buttonMission)
                _buttonMissions.Initialize(buttonMission);

            _actualProgress = 0;
        }

        public void UpdateMission(int amountGet, string name = null, string nameEnemie = null)
        {
            if (_missionCompleted) return;
            if (name != null && _actualMission is MissionItemScriptableObject childObject)
            {
                foreach (var item in childObject.itemsGoalList)
                {
                    if(item.itemName == name)
                    {
                        _actualProgress += amountGet;
                    }
                }
            }
            else if (name == null && _actualMission.typeMission == Utils.TypeMission.Hunt)
                _actualProgress += amountGet;

            if (_actualMission is MissionRecollectScriptableObject recollectMission)
            {
                _feedbackText.color = _actualProgress >= recollectMission.amountToReach ? Color.green : Color.red;
                _feedbackText.text = _actualProgress + "/" + recollectMission.amountToReach;
            }
            
            CheckEndMission(amountGet);
        }

        private void CheckEndMission(int amountGet)
        {
            var ended = false;
            switch (_actualMission)
            {
                case MissionRecollectScriptableObject recollectMission:
                    ended = _actualProgress >= recollectMission.amountToReach;
                    break;
                case MissionButtonScriptableObject:
                    ended = amountGet > 0;
                    break;
            }

            if (!ended) return;
            _missionCompleted = true;
            StartCoroutine(WaitForNextMission());
        }
    }
}

