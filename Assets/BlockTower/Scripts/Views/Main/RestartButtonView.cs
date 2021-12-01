using BlockTower.Presenters.Main.Enums;
using MessagePipe;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace BlockTower.Views.Main
{
    [RequireComponent(typeof(Button))]
    public class RestartButtonView : MonoBehaviour
    {
        [Inject] private IPublisher<GameState> _gameStatePublisher;

        private Button _restartButton;

        private void Start()
        {
            _restartButton = GetComponent<Button>();
            _restartButton.OnClickAsObservable()
                .Subscribe(_ => _gameStatePublisher.Publish(GameState.Init))
                .AddTo(this);
        }
    }
}
