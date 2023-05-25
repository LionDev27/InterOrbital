using InterOrbital.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private void Update()
    {
        UpdateCameraPosition();
    }
    private void UpdateCameraPosition()
    {
        Vector3 playerPos = PlayerComponents.Instance.transform.position;
        Vector3 newPos = new Vector3(playerPos.x, playerPos.y, -20);

        transform.position = newPos;
    }
}
