public class LocalPlayer 
{
    private static int _localPlayerIndex = 0;

    public static int LocalPlayerIndex 
    {
        get => _localPlayerIndex; 
        private set => _localPlayerIndex = value; 
    }
}
