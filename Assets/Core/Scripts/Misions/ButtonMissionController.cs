using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InterOrbital.Mission
{
    public class ButtonMissionController : MonoBehaviour
    {
        private MissionCreator _creator;
        private List<KeyCode> _buttons = new();
        private bool _started;

        private void Awake()
        {
            _creator = GetComponent<MissionCreator>();
        }

        private void Update()
        {
            if (_started)
                CheckButtons();
        }

        public void Initialize(MissionButtonScriptableObject buttonMission)
        {
            _buttons.Clear();
            _buttons = buttonMission.buttons.ToList();
            _started = true;
        }

        private void CheckButtons()
        {
            if (_buttons.Count > 0)
            {
                foreach (var key in _buttons.Where(Input.GetKeyDown))
                {
                    _buttons.Remove(key);
                    break;
                }
            }
            else
            {
                _started = false;
                _creator.UpdateMission(1);
            }
        }
    }
}