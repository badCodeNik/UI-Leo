using System;
using Leopotam.EcsLite;
using Project._Scripts.ECS.Components;
using Project._Scripts.ECS.Systems;
using Project._Scripts.UI;
using Project._Scripts.UI.Configs;
using Project._Scripts.UI.Models;
using UnityEngine;
using UnityEngine.Serialization;
using Application = UnityEngine.Device.Application;

namespace Project._Scripts.Infrastructure
{
    public class Bootstrapper : MonoBehaviour 
    {
        [SerializeField] private GameUiManager _gameUiManager;
        [SerializeField] private GameConfig _gameConfig;
        [FormerlySerializedAs("_textConfig")] [SerializeField] private BusinessNameConfig businessNameConfig;
        private EcsSystems _systems;

        private void Awake()
        {
            InitEcsWorld();
        }
    
        private void InitEcsWorld()
        {
            var world = new EcsWorld();
            _systems = new EcsSystems(world, _gameUiManager);
            _systems.Add(new BusinessIncomeSystem());
            _systems.Add(new BusinessUpgradeSystem());
            _systems.Init();
            _gameUiManager.Initialize(_gameConfig,businessNameConfig, world);
        }

        private void Update()
        {
            _systems.Run();
        }

    }
}
