using InterOrbital.Player;
using InterOrbital.WorldSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float cameraSize = 20; 

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
            float posX = Mathf.Clamp(playerPos.x, cameraSize, GridLogic.Instance.width - cameraSize);
            float posY = Mathf.Clamp(playerPos.y, cameraSize, GridLogic.Instance.width - cameraSize);
            Vector3 newPos = new Vector3(posX, posY, -cameraSize);

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
