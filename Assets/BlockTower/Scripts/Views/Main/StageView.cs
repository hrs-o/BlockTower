using System.Threading;
using BlockTower.Presenters.Main.Enums;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace BlockTower.Views.Main
{
    public class StageView : MonoBehaviour
    {
        [Inject] private IPublisher<GameState> _gameStatePublisher;

        private readonly CancellationTokenSource _cancelTokenSrc = new();

        private void Start()
        {
            _gameStatePublisher.Publish(GameState.Countdown);
        }

        private void OnDestroy()
        {
            _cancelTokenSrc?.Cancel();
        }
    }
}
