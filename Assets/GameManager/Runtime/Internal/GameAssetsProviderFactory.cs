namespace Game.GameManager
{
    internal class GameAssetsProviderFactory
    {
        public IGameAssetsProvider Create(IBundleHolder holder)
        {
            return new GameAssetsProvider(holder);
        }
    }
}