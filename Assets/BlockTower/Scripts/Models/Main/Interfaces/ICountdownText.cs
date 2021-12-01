using UniRx;

namespace BlockTower.Models.Main.Interfaces
{
    public interface ICountdownText
    {
        public IReadOnlyReactiveProperty<int> CountdownSec { get; }

        public void CountDown();
    }
}
