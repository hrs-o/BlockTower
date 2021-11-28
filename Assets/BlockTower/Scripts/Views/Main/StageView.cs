using System.Collections.Generic;
using System.Threading;
using BlockTower.Presenters.Main;
using BlockTower.Presenters.Main.Enums;
using BlockTower.Views.Main.Block;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BlockTower.Views.Main
{
    public class StageView : MonoBehaviour
    {
        [SerializeField] private BlockView blockTemplate;
        [Inject] private IObjectResolver _resolver;
        [Inject] private StagePresenter _presenter;

        [Inject] private IPublisher<GameState> _gameStatePublisher;

        private readonly CancellationTokenSource _cancelTokenSrc = new();
        private readonly List<BlockView> _blocks = new();

        private void Start()
        {
            _presenter.InitBlockAsObservable
                .Subscribe(_ => Init().Forget())
                .AddTo(this);

            _presenter.GenerateBlockAsObservable
                .Subscribe(GenerateBlock)
                .AddTo(this);

            _gameStatePublisher.Publish(GameState.Init);
        }

        private void GenerateBlock(float generateHeight)
        {
            var block = _resolver.Instantiate(blockTemplate);
            block.Init(generateHeight);
            _blocks.Add(block);
            _gameStatePublisher.Publish(GameState.GeneratedBlock);
        }

        private async UniTaskVoid Init()
        {
            for (var idx = 0; idx < _blocks.Count; idx++)
            {
                if (_blocks[idx] == null) continue;
                Destroy(_blocks[idx].gameObject);
            }

            _blocks.Clear();

            await UniTask.Delay(1000, cancellationToken: _cancelTokenSrc.Token);
            _gameStatePublisher.Publish(GameState.Countdown);
        }

        private void OnDestroy()
        {
            _cancelTokenSrc?.Cancel();
        }
    }
}
