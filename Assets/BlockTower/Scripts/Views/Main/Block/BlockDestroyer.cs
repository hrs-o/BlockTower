using BlockTower.Presenters.Main.Enums;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

namespace BlockTower.Views.Main.Block
{
    public class BlockDestroyer : MonoBehaviour
    {
        private const float GameOverPosition = -10f;

        [Inject] private IPublisher<GameState> _gameStatePublisher;

        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            this.ObserveEveryValueChanged(_ => _rigidbody.position.y)
                .FirstOrDefault(posY => posY < GameOverPosition)
                .Subscribe(_ =>
                {
                    _gameStatePublisher.Publish(GameState.Result);
                    Destroy(gameObject);
                })
                .AddTo(this);
        }
    }
}
