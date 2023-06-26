using System;
using InterOrbital.WorldSystem;
using UnityEngine;
using NavMeshPlus.Components;

namespace InterOrbital.Combat.IA
{
    public class NavMeshController : MonoBehaviour
    {
        private NavMeshSurface _navigationSurface;

        private void Awake()
        {
            _navigationSurface = GetComponent<NavMeshSurface>();
            _navigationSurface.hideEditorLogs = true;
        }

        private void Start()
        {
            GridLogic.Instance.OnTilemapFilled += BakeNavMesh;
        }

        private void BakeNavMesh()
        {
            _navigationSurface.BuildNavMeshAsync();
        }

        private void OnDisable()
        {
            GridLogic.Instance.OnTilemapFilled -= BakeNavMesh;
        }
    }
}
