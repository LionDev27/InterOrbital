using UnityEngine;
using Cinemachine;
using InterOrbital.WorldSystem;
using InterOrbital.Player;

public class CameraController : MonoBehaviour
{
    public Transform target; // Referencia al transform del personaje que seguirá la cámara
    public float boundaryDistanceX = 14f; 
    public float boundaryDistanceY = 8f; 

    public CinemachineVirtualCamera virtualCamera;
    private float minX, maxX, minY, maxY;

    private void Start()
    {
        // Calcula los límites en los ejes X e Y del mapa
        minX = boundaryDistanceX;
        maxX = GridLogic.Instance.width - boundaryDistanceX;
        minY = boundaryDistanceY;
        maxY = GridLogic.Instance.height - boundaryDistanceY;
    }

    private void LateUpdate()
    {
        Vector3 playerPos = PlayerComponents.Instance.GetPlayerPosition();
        
        if(playerPos.x < minX || playerPos.x  >= maxX || playerPos.y >= minY || playerPos.y >= maxY) {
            
            Vector3 offset = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
            if(playerPos.x < minX)
            {
                offset.x = boundaryDistanceX - playerPos.x;
            }
            if (playerPos.x >= maxX)
            {
                offset.x = maxX - playerPos.x;
            }
            if(playerPos.y < minY)
            {
                offset.y = boundaryDistanceY - playerPos.y;
            }
            if (playerPos.y >= maxY)
            {
                offset.y = maxY - playerPos.y;
            }
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = offset;
        }
    }
}
