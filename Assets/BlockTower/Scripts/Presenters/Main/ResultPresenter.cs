using System;
using BlockTower.Presenters.Main.Enums;
using JetBrains.Annotations;
using MessagePipe;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace BlockTower.Presenters.Main
{
    [UsedImplicitly]
    public class ResultPresenter : IStartable, IDisposable
    {
        [Inject] private ISubscriber<GameState> _onGameStateChanged;

        public IObservable<bool> IsShowAsObservable => _isShowSubject;
        private readonly Subject<bool> _isShowSubject = new();

        private readonly CompositeDisposable _disposable = new();

        public void Start()
        {
            _onGameStateChanged
                .AsObservable()
                .Subscribe(state => _isShowSubject.OnNext(state == GameState.Result))
                .AddTo(_disposable);
        }

        public void Dispose() => _disposable?.Dispose();
    }
}
