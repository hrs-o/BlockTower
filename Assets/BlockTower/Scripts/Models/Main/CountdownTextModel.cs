using System.Threading;
using BlockTower.Models.Main.Interfaces;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UniRx;

namespace BlockTower.Models.Main
{
    [UsedImplicitly]
    public class CountdownTextModel : ICountdownText
    {
        private const int DefaultCountdownSec = 3;

        public IReadOnlyReactiveProperty<int> CountdownSec => _countdownSec;

        private readonly IntReactiveProperty _countdownSec = new(DefaultCountdownSec);

        public void CountDown()
        {
            CountDownAsync(new CancellationToken()).Forget();
        }

        private async UniTaskVoid CountDownAsync(CancellationToken token)
        {
            _countdownSec.Value = DefaultCountdownSec;
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(1000, cancellationToken: token);
                _countdownSec.Value--;

                if (_countdownSec.Value > 0) continue;

                await UniTask.Delay(1000, cancellationToken: token);
                return;
            }
        }
    }
}
