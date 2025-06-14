using System;
using System.Collections.Generic;
using _01.Script.Players;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Script.Manager
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private ScriptFinderSO playerFinder;
        [SerializeField] private ScriptFinderSO uiManagerFinder;
        [SerializeField] private float clampRange = 10f;
        [SerializeField] private float fogMentalDown;
        [SerializeField] private List<Transform> walls;
        [SerializeField] private float updateInterval = 0.5f;
        private Dictionary<Transform, Collider> _wallColliders = new Dictionary<Transform, Collider>();
        private float _updateTimer = 0f;
        private HashSet<Transform> _playerInRangeWalls = new HashSet<Transform>();

        private Player _player;
        private UIManager _uiManager;

        private void Awake()
        {
            _player = playerFinder.GetTarget<Player>();
            _uiManager = uiManagerFinder.GetTarget<UIManager>();
            foreach (var wall in walls)
            {
                if (wall == null) continue;
                var wallCollider = wall.GetComponent<BoxCollider>();
                if (wallCollider != null)
                {
                    _wallColliders.Add(wall, wallCollider);
                }
                else
                {
                    Debug.LogWarning($"[MapManager] Wall {wall.name} does not have a BoxCollider component.");
                }
            }
        }

        private void Update()
        {
            _updateTimer += Time.deltaTime;
            if (_updateTimer >= updateInterval)
            {
                _updateTimer = 0f;
                var playerPos = playerFinder.GetTargetTransform().position;
                playerPos.y = 0;
                foreach (Transform wall in walls)
                {
                    if (wall == null) continue;
                    var wallCollider = _wallColliders[wall];
                    if (wallCollider == null) continue;
                    Vector3 closestPoint = wallCollider.ClosestPoint(playerPos);
                    closestPoint.y = 0;
                    float sqrDistance = (playerPos - closestPoint).sqrMagnitude;
                    bool shouldBeIn = sqrDistance <= clampRange * clampRange;
                    if (shouldBeIn)
                    {
                        _playerInRangeWalls.Add(wall);
                    }
                    else
                    {
                        _playerInRangeWalls.Remove(wall);
                    }
                }
                if (_playerInRangeWalls.Count > 0)
                {
                    _player.ScMental.IsInFogChange(true);
                    _player.ScSnow.SetSnowEffect(1);
                    _uiManager.SetFogText(true);
                }
                else
                {
                    _player.ScSnow.SetSnowEffect(0);
                    _player.ScMental.IsInFogChange(false);
                    _uiManager.SetFogText(false);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, clampRange);
        }
    }
}
