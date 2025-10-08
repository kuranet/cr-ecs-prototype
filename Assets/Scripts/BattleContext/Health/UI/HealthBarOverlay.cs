using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class HealthBarOverlay : Overlay
{
    [SerializeField] private HealthBar _healthBarPrefab;

    private List<HealthBar> _activeHealthBars = new List<HealthBar>();

    private void LateUpdate()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery healthQuery = entityManager.CreateEntityQuery(typeof(Health));
        NativeArray<Entity> entities = healthQuery.ToEntityArray(Allocator.Temp);
        foreach (var entity in entities)
        {
            var assignedBar = _activeHealthBars.FirstOrDefault(bar => bar.Entity == entity);
            if (assignedBar != null)
            {
                continue;
            }

            assignedBar = Instantiate(_healthBarPrefab, transform);
            _activeHealthBars.Add(assignedBar);

            assignedBar.Entity = entity;
        }

        for (var i = _activeHealthBars.Count - 1; i >= 0; i--)
        {
            var connectedEntityIsValid = entities.Contains(_activeHealthBars[i].Entity);
            if (connectedEntityIsValid)
            {
                continue;
            }

            Destroy(_activeHealthBars[i].gameObject);
            _activeHealthBars.RemoveAt(i);
        }

        entities.Dispose(); 

        foreach(var healthBar in _activeHealthBars)
        {
            healthBar.OnUpdate();
        }
    }
}
