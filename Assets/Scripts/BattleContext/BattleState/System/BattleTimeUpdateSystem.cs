using Unity.Entities;

public partial struct BattleTimeUpdateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.TryGetSingleton<BattleState>(out var battleState))
        {
            var deltaTime = UnityEngine.Time.deltaTime;
            switch (battleState.State)
            {
                case BattleStateType.None:
                    {
                        battleState.State = BattleStateType.Prepare;
                        break;
                    }

                case BattleStateType.Prepare:
                    {
                        battleState.timeInState += deltaTime;
                        if (battleState.timeInState >= BattleConfiguration.InitializationDurationSec)
                        {
                            battleState.State = BattleStateType.Active;
                            battleState.timeInState = 0;
                        }
                        break;
                    }

                case BattleStateType.Active:
                    {
                        battleState.timeInState += deltaTime;
                        if (battleState.timeInState >= BattleConfiguration.ActivePlayDurationSec)
                        {
                            battleState.State = BattleStateType.Ended;
                            battleState.timeInState = 0;
                        }
                        break;
                    }

                case BattleStateType.Ended:
                    {
                        break;
                    }
            }

            SystemAPI.SetSingleton(battleState);
        }
    }
}
