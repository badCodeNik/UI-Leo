using Leopotam.EcsLite;
using Project._Scripts.ECS.Components;
using Project._Scripts.UI;
using UnityEngine;

namespace Project._Scripts.ECS.Systems
{
    public class BusinessUpgradeSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _upgradeFilter;
        private EcsPool<BusinessComponents.BusinessComponent> _businessPool;
        private EcsPool<BusinessRequests.LevelUpRequest> _levelUpPool;
        private EcsPool<BusinessRequests.BusinessUpgradeRequest> _upgradeRequestPool;
        private GameUiManager _gameUiManager;

        public void Init(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            _gameUiManager = systems.GetShared<GameUiManager>();
            _upgradeFilter = ecsWorld
                .Filter<BusinessComponents.BusinessComponent>()
                .End();
            _businessPool = ecsWorld.GetPool<BusinessComponents.BusinessComponent>();
            _upgradeRequestPool = ecsWorld.GetPool<BusinessRequests.BusinessUpgradeRequest>();
            _levelUpPool = ecsWorld.GetPool<BusinessRequests.LevelUpRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _upgradeFilter)
            {
                ref var business = ref _businessPool.Get(entity);
                if (_levelUpPool.Has(entity))
                {
                    _levelUpPool.Del(entity);
                    TryLevelUp(business.businessId);
                    SyncEcsWithUI(ref business);
                }

                if (_upgradeRequestPool.Has(entity))
                {
                    var request = _upgradeRequestPool.Get(entity);
                    _upgradeRequestPool.Del(entity);
                    TryBuyUpgrade(business.businessId, request.upgradeIndex);
                    SyncEcsWithUI(ref business);
                }
            }
        }

        private void TryBuyUpgrade(int businessId, int upgradeIndex)
        {
            _gameUiManager.TryBuyUpgrade(businessId, upgradeIndex);
        }

        private void SyncEcsWithUI(ref BusinessComponents.BusinessComponent business)
        {
            if (_gameUiManager.TryGetPresenterById(business.businessId, out var presenter))
            {
                presenter.SyncWithECS(ref business);
            }
        }

        private void TryLevelUp(int businessBusinessId)
        {
            _gameUiManager.TryLevelUp(businessBusinessId);
        }
    }
}