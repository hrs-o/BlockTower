using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlockTower.Views.Main.Block
{
    public class BlockMover : MonoBehaviour
    {
        private Camera _camera;
        private Rigidbody2D _rigidbody;

        private InputAction _inputPress;
        private InputAction _inputMove;

        private IDisposable _moveDisposable;

        private void Start()
        {
            Debug.Assert(Camera.main != null, "Camera.main != null");
            _camera = Camera.main;

            var playerInput = gameObject.GetComponent<PlayerInput>();
            var actionMap = playerInput.currentActionMap;
            _inputPress = actionMap["Press"];
            _inputMove = actionMap["Move"];

            _rigidbody = gameObject.GetComponent<Rigidbody2D>();

            _moveDisposable = this.UpdateAsObservable()
                .Where(_ => _inputPress.IsPressed())
                .Select(_ => _inputMove.ReadValue<Vector2>())
                .Subscribe(MoveBlock)
                .AddTo(this);

            this.ObserveEveryValueChanged(_ => _rigidbody.isKinematic)
                .FirstOrDefault(x => x == false)
                .Subscribe(_ => _moveDisposable?.Dispose())
                .AddTo(this);
        }

        private void MoveBlock(Vector2 inputPos)
        {
            var inputX = _camera.ScreenToWorldPoint(inputPos).x;
            _rigidbody.position = new Vector2(inputX, _rigidbody.position.y);
        }
    }
}
