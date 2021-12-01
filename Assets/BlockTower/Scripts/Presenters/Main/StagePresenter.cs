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
    public class StagePresenter : IStartable, IDisposable
    {
        private const float DefaultGenerateHeight = 3f;

        [Inject] private MoveCameraAreaPresenter _moveCameraAreaPresenter;
        [Inject] private ISubscriber<GameState> _onGameStateChanged;

        private float _currentGenerateHeightOffset;

        public IObservable<float> GenerateBlockAsObservable => _generateBlockSubject;
        private readonly Subject<float> _generateBlockSubject = new();

        public IObservable<Unit> InitBlockAsObservable => _initBlockSubject;
        private readonly Subject<Unit> _initBlockSubject = new();

        private readonly CompositeDisposable _disposable = new();

        public void Start()
        {
            _generateBlockSubject.AddTo(_disposable);
            _initBlockSubject.AddTo(_disposable);
            
            _moveCameraAreaPresenter.AddMoveCameraHeightOffsetAsObservable
                .Subscribe(offset => _currentGenerateHeightOffset += offset)
                .AddTo(_disposable);

            _onGameStateChanged
                .AsObservable()
                .Where(state => state == GameState.Init)
                .Subscribe(_ => _initBlockSubject.OnNext(Unit.Default))
                .AddTo(_disposable);

            _onGameStateChanged
                .AsObservable()
                .Where(state => state == GameState.GeneratingBlock)
                .Subscribe(_ =>
                    _generateBlockSubject.OnNext(DefaultGenerateHeight + _currentGenerateHeightOffset)
                )
                .AddTo(_disposable);
        }

        public void Dispose() => _disposable?.Dispose();
    }
}
