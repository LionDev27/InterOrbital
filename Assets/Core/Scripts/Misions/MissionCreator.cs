using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InterOrbital.Item;

namespace InterOrbital.Mission
{
    public class MissionCreator : MonoBehaviour
    {
        [SerializeField] private Image _missionImage;
        [SerializeField] private TextMeshProUGUI _missionText;
        [SerializeField] private TextMeshProUGUI _feedbackText;
        private int _actualProgress = 0;
        private MissionScriptableObject _actualMission;

        private IEnumerator WaitForNextMission()
        {
            yield return new WaitForSeconds(1);
            MissionManager.Instance.NextMission();
        }
    
        public void CreateMission(MissionScriptableObject mission)
        {
            _actualMission = mission;
            _missionImage.sprite = mission.imageMission;
            _missionText.text = mission.missionDescription;
            _feedbackText.color = Color.red;
            _feedbackText.text = "0/" +mission.amountToReach.ToString();
            _actualProgress = 0;
        }

        public void UpdateMission(int amountGet, ItemScriptableObject item = null)
        {

            if (item != null && _actualMission is MissionItemScriptableObject childObject)
            {
               if(childObject.itemGoal == item) {
                    _actualProgress += amountGet;
                }
            }
            else if (item == null)
            {
                _actualProgress += amountGet;
            }
            _feedbackText.color = _actualProgress >= _actualMission.amountToReach ? Color.green : Color.red;
            _feedbackText.text = _actualProgress.ToString() + "/" + _actualMission.amountToReach.ToString();
            if (_actualProgress >= _actualMission.amountToReach)
            {
                StartCoroutine(WaitForNextMission());
            }
        }
    }
}

