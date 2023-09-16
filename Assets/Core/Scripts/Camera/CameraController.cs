using UnityEngine;
using Cinemachine;
using InterOrbital.WorldSystem;
using InterOrbital.Player;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private float _maxDistance = 3f;
    public float boundaryDistanceX = 14f; 
    public float boundaryDistanceY = 8f; 

    public CinemachineVirtualCamera virtualCamera;
    private float minX, maxX, minY, maxY;

    private void Start()
    {
        // Calcula los l�mites en los ejes X e Y del mapa
        minX = boundaryDistanceX;
        maxX = GridLogic.Instance.width - boundaryDistanceX;
        minY = boundaryDistanceY;
        maxY = GridLogic.Instance.height - boundaryDistanceY;
    }

    private void LateUpdate()
    {
        Vector3 playerPos = PlayerComponents.Instance.GetPlayerPosition();
        
        AimBetweenPlayerAndCursor(playerPos);
        CheckBoundaries(playerPos);
    }

    private void CheckBoundaries(Vector3 playerPos)
    {
        if(playerPos.x < minX || playerPos.x  >= maxX || playerPos.y < minY || playerPos.y >= maxY) {
            
            Vector3 offset = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
            if(playerPos.x < minX)
                offset.x = boundaryDistanceX - playerPos.x;
            if (playerPos.x >= maxX)
                offset.x = maxX - playerPos.x;
            if(playerPos.y < minY)
                offset.y = boundaryDistanceY - playerPos.y;
            if (playerPos.y >= maxY)
                offset.y = maxY - playerPos.y;
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = offset;
        }
    }

    private void AimBetweenPlayerAndCursor(Vector3 playerPos)
    {
        _followTarget.position = UpdateTargetPos(playerPos);
    }

    private Vector2 GetMousePos()
    {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition); //raw mouse pos
        ret *= 2; 
        ret -= Vector2.one; //set (0,0) of mouse to middle of screen
        float max = 0.9f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
            ret = ret.normalized; //helps smooth near edges of screen
        return ret;
    }

    private Vector2 UpdateTargetPos(Vector3 playerPos)
    {
        Vector3 mouseOffset = GetMousePos() * _maxDistance; //mult mouse vector by distance scalar 
        Vector3 ret = playerPos + mouseOffset; //find position as it relates to the player
        return ret;
    }
}
