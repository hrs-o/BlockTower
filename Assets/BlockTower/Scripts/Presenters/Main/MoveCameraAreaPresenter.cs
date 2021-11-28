using System;
using System.Threading;
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
    public class MoveCameraAreaPresenter : IStartable, IDisposable
    {
        private const float MoveCameraHeightOffset = 0.05f;

        [Inject] private IPublisher<GameState> _gameStatePublisher;
        [Inject] private ISubscriber<GameState> _onGameStateChanged;

        public IObservable<float> AddMoveCameraHeightOffsetAsObservable => _addMoveCameraHeightOffsetSubject;
        private readonly Subject<float> _addMoveCameraHeightOffsetSubject = new();

        private readonly CancellationTokenSource _cancelTokenSrc = new();
        private readonly CompositeDisposable _disposable = new();

        private float _currentCameraOffsetHeight;
        private bool _isCollisionArea;

        public void Start()
        {
            _addMoveCameraHeightOffsetSubject.AddTo(_disposable);

            _onGameStateChanged
                .AsObservable()
                .Where(state => state == GameState.Init)
                .Subscribe(_ =>
                {
                    _addMoveCameraHeightOffsetSubject.OnNext(-_currentCameraOffsetHeight);
                    _currentCameraOffsetHeight = 0;
                })
                .AddTo(_disposable);

            _onGameStateChanged
                .AsObservable()
                .Where(state => state == GameState.MovingCamera)
                .Subscribe(_ => MovingCameraAsync().Forget())
                .AddTo(_disposable);
        }

        public void MovingCamera()
        {
            _isCollisionArea = true;
        }

        public void StopCamera()
        {
            _isCollisionArea = false;
        }

        private async UniTaskVoid MovingCameraAsync()
        {
            while (_isCollisionArea)
            {
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _cancelTokenSrc.Token);

                _addMoveCameraHeightOffsetSubject.OnNext(MoveCameraHeightOffset);
                _currentCameraOffsetHeight += MoveCameraHeightOffset;
            }

            _gameStatePublisher.Publish(GameState.GeneratingBlock);
        }

        public void Dispose() => _disposable?.Dispose();
    }
}
