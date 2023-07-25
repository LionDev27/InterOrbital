using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Events;

namespace InterOrbital.Events
{
    public class TotalDark_Event : EventBase
    {
        public override void StartEvent()
        {
            base.StartEvent();
            Debug.Log("EMPEZAMOS EVENTO NOCHE TOTAL");
        }

        public override void EndEvent()
        {
            base.EndEvent();
            Debug.Log("EMPEZAMOS EVENTO NOCHE TOTAL");
        }

    }

}
