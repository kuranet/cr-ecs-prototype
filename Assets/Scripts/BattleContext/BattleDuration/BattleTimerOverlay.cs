using System;
using TMPro;
using Unity.Entities;
using UnityEngine;

public class BattleTimerOverlay : Overlay
{
    [SerializeField] private GameObject _timerBlock;
    [SerializeField] private TextMeshProUGUI _timerText;

    EntityManager _em;
    EntityQuery _singletonQuery;

    void Awake()
    {
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;
        _singletonQuery = _em.CreateEntityQuery(ComponentType.ReadOnly<BattleState>());
    }

    private void LateUpdate()
    {
        if (_singletonQuery.TryGetSingleton<BattleState>(out var state))
        {
            var shouldShowTimer = state.State == BattleStateType.Active;
            _timerBlock.gameObject.SetActive(shouldShowTimer);

            if (shouldShowTimer)
            {
                var secondsLeft = BattleConfiguration.ActivePlayDurationSec - state.timeInState;
                var timeSpan = TimeSpan.FromSeconds(secondsLeft);
                _timerText.text = $"{timeSpan.Minutes}:{timeSpan.Seconds}";
            }
        }
    }
}
