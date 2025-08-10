using Leopotam.EcsLite;
using Project._Scripts.ECS.Components;
using Project._Scripts.UI;
using UnityEngine;

namespace Project._Scripts.ECS.Systems
{
    public class BusinessIncomeSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _businessFilter;
        private EcsPool<BusinessComponents.BusinessComponent> _businessPool;
        private GameUiManager _gameUiManager;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _gameUiManager = systems.GetShared<GameUiManager>();
            _businessFilter = world.Filter<BusinessComponents.BusinessComponent>().End();
            _businessPool = world.GetPool<BusinessComponents.BusinessComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _businessFilter)
            {
                ref var businessComponent = ref _businessPool.Get(entity);
                if (businessComponent.level > 0)
                {
                    businessComponent.incomeTimer += Time.deltaTime;
                    var progress = businessComponent.incomeTimer / businessComponent.incomeDelay;
                    progress = Mathf.Clamp01(progress);
                    UpgradeBusinessProgress(businessComponent.businessId, progress);
                    if (businessComponent.incomeTimer >= businessComponent.incomeDelay)
                    {
                        businessComponent.incomeTimer -= businessComponent.incomeDelay;
                        _gameUiManager.AddIncome(businessComponent.income);
                    }
                }
            }
        }

        private void UpgradeBusinessProgress(int businessId, float progress)
        {
            if (_gameUiManager.TryGetPresenterById(businessId, out var presenter))
            {
                presenter?.UpdateProgress(progress);
            }
        }
    }
}