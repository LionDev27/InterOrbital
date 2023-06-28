using System;
using InterOrbital.WorldSystem;
using UnityEngine;
using NavMeshPlus.Components;

namespace InterOrbital.Combat.IA
{
    public class NavMeshController : MonoBehaviour
    {
        [Tooltip("Rate at which the navigation mesh is to be updated")]
        [SerializeField] private float _navMeshRefreshRate = 2;
        private NavMeshSurface _navigationSurface;

        private void Awake()
        {
            _navigationSurface = GetComponent<NavMeshSurface>();
            _navigationSurface.hideEditorLogs = true;
        }

        private void Start()
        {
            GridLogic.Instance.OnTilemapFilled += BakeNavMesh;
            InvokeRepeating(nameof(UpdateNavMesh), _navMeshRefreshRate, _navMeshRefreshRate);
        }

        private void BakeNavMesh()
        {
            _navigationSurface.BuildNavMeshAsync();
        }
        
        private void OnDisable()
        {
            GridLogic.Instance.OnTilemapFilled -= BakeNavMesh;
        }
        
        public void UpdateNavMesh()
        {
            _navigationSurface.UpdateNavMesh(_navigationSurface.navMeshData);
        }
    }
}
