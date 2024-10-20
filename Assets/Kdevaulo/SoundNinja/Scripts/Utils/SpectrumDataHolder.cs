namespace Kdevaulo.SoundNinja.Utils
{
    public static class SpectrumDataHolder
    {
        private static ISoundDataContainer _dataContainer;

        public static void CacheData(ISoundDataContainer dataContainer)
        {
            _dataContainer = dataContainer;
        }

        public static ISoundDataContainer GetDataContainer()
        {
            return _dataContainer;
        }
    }
}