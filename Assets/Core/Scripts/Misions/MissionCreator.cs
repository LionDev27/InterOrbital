using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InterOrbital.Item;
using InterOrbital.Utils;
using PixelCrushers.DialogueSystem;

namespace InterOrbital.Mission
{
    public class MissionCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _visuals;
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

        private void Start()
        {
            _visuals.SetActive(false);
        }

        private IEnumerator WaitForNextMission()
        {
            _missionText.color = Color.green;
            yield return new WaitForSeconds(2);
            _missionCompleted = false;
            if (_actualMission.nextConversationName != "")
                _tutorialManager.StartDialogue(_actualMission.nextConversationName);
        }
    
        public void CreateMission(MissionScriptableObject mission)
        {
            if (!_visuals.activeInHierarchy)
                _visuals.SetActive(true);
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

        public void UpdateMission(int amountGet, string name = null)
        {
            if (_missionCompleted || _actualMission == null) return;
            if (name != null && _actualMission is MissionItemScriptableObject childObject)
            {
                foreach (var item in childObject.itemsGoalList)
                {
                    if(item.itemName == name)
                        _actualProgress += amountGet;
                }
            }
            else if (name == null && _actualMission.typeMission == TypeMission.Hunt)
                _actualProgress += amountGet;

            if (_actualMission is MissionRecollectScriptableObject recollectMission)
            {
                _feedbackText.color = _actualProgress >= recollectMission.amountToReach ? Color.green : Color.red;
                _feedbackText.text = _actualProgress + "/" + recollectMission.amountToReach;
            }

            if (_actualMission.typeMission == TypeMission.None)
            {
                bool defaultMission = _actualMission is not MissionRecollectScriptableObject &&
                                      _actualMission is not MissionButtonScriptableObject;
                if (defaultMission && name != _actualMission.typeMission.ToString())
                    amountGet = 0;
            }
            
            CheckEndMission(amountGet);
        }

        private void CheckEndMission(int amountGet)
        {
            bool ended;
            if (_actualMission is MissionRecollectScriptableObject recollectMission)
                ended = _actualProgress >= recollectMission.amountToReach;
            else
                ended = amountGet > 0;

            if (!ended) return;
            _missionCompleted = true;
            StartCoroutine(WaitForNextMission());
        }
    }
}

