using System;
using BlockTower.Presenters.Main.Enums;
using MessagePipe;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace BlockTower.Views.Main.Block
{
    public class BlockFaller : MonoBehaviour
    {
        private const float StopWaitSec = 1f;

        [Inject] private IPublisher<GameState> _gameStatePublisher;

        private InputAction _inputPress;
        private Rigidbody2D _rigidbody;

        private float _startY;
        private float _stopElapsedSec;

        private IDisposable _stoppedDisposable;

        private void Start()
        {
            var playerInput = gameObject.GetComponent<PlayerInput>();
            var actionMap = playerInput.currentActionMap;
            _inputPress = actionMap["Press"];

            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _startY = _rigidbody.position.y;

            this.UpdateAsObservable()
                .FirstOrDefault(_ => _inputPress.WasReleasedThisFrame())
                .Subscribe(_ => FallBlock())
                .AddTo(this);

            _stoppedDisposable = this.UpdateAsObservable()
                .Where(_ => _rigidbody.position.y < _startY)
                .Subscribe(_ => Falling())
                .AddTo(this);
        }

        private void Falling()
        {
            if (!_rigidbody.IsSleeping())
            {
                _stopElapsedSec = 0;
                return;
            }

            _stopElapsedSec += Time.deltaTime;
            if (!(_stopElapsedSec >= StopWaitSec)) return;
            _stoppedDisposable?.Dispose();
            _stopElapsedSec = 0;
            _gameStatePublisher.Publish(GameState.MovingCamera);
        }

        private void FallBlock()
        {
            _rigidbody.isKinematic = false;
        }
    }
}
