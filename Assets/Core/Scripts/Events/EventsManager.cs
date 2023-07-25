using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Events
{
    public class EventsManager : MonoBehaviour
    {
        public List<EventBase> eventsPool;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                eventsPool[0].StartEvent();
            }
        }


    }
}
   
