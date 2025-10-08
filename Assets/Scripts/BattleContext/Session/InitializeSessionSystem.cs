using Unity.Entities;

public partial struct InitializeSessionSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (sessionConfig, entity) in
                 SystemAPI.Query<RefRO<SessionConfig>>()
                 .WithEntityAccess())
        {
            switch (sessionConfig.ValueRO.mode)
            {
                case SessionMode.OneVsOne:
                    {
                        // player1
                        var player1 = state.EntityManager.Instantiate(sessionConfig.ValueRO.playerPrefab);
                        ecb.AddComponent(player1, new PlayerIdentifier() { playerId = 0 });

                        // player2
                        var player2 = state.EntityManager.Instantiate(sessionConfig.ValueRO.playerPrefab);
                        ecb.AddComponent(player2, new PlayerIdentifier() { playerId = 1 });

                        break;
                    }
            }

            ecb.RemoveComponent<SessionConfig>(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
