using InterOrbital.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private bool _minimapOpen;
    private void Update()
    {
        UpdateCameraPosition();
    }
    private void UpdateCameraPosition()
    {
        if (!_minimapOpen)
        {
            Vector3 playerPos = PlayerComponents.Instance.transform.position;
            Vector3 newPos = new Vector3(playerPos.x, playerPos.y, -20);

            transform.position = newPos;
        }
    }

    public void OpenMinimap()
    {
        _minimapOpen = true;
    }

    public void CloseMinimap()
    {
        _minimapOpen = false;
    }
}
