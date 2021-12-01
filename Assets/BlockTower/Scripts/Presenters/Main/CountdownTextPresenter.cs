using System;
using System.Threading;
using BlockTower.Models.Main.Interfaces;
using BlockTower.Presenters.Main.Enums;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MessagePipe;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace BlockTower.Presenters.Main
{
    [UsedImplicitly]
    public class CountdownTextPresenter : IStartable, IDisposable
    {
        public IReadOnlyReactiveProperty<bool> IsEnabled => _isEnabled;
        private readonly BoolReactiveProperty _isEnabled = new();

        public IReadOnlyReactiveProperty<int> CountdownSec => _countdownSec;
        private readonly IntReactiveProperty _countdownSec = new();

        private readonly CancellationTokenSource _cancelTokenSrc = new();
        private readonly CompositeDisposable _disposable = new();

        [Inject] private IPublisher<GameState> _gameStatePublisher;
        [Inject] private ISubscriber<GameState> _onGameStateChanged;
        [Inject] private ICountdownText _model;

        public void Start()
        {
            _onGameStateChanged
                .AsObservable()
                .Where(state => state == GameState.Countdown)
                .Subscribe(_ =>
                {
                    _isEnabled.Value = true;
                    _model.CountDown();
                })
                .AddTo(_disposable);

            _model.CountdownSec
                .Subscribe(sec => UpdateCountdownSec(sec, _cancelTokenSrc.Token).Forget())
                .AddTo(_disposable);
        }

        private async UniTaskVoid UpdateCountdownSec(int sec, CancellationToken token)
        {
            _countdownSec.Value = sec;

            if (sec != 0) return;
            await UniTask.Delay(1000, cancellationToken: token);

            _isEnabled.Value = false;
            _gameStatePublisher.Publish(GameState.GeneratingBlock);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _cancelTokenSrc?.Cancel();
        }
    }
}
