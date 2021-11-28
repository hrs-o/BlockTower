using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BlockTower.Views.Main.Block
{
    public class BlockRotator : MonoBehaviour
    {
        [SerializeField] private float rotateAngle = 30f;
        [SerializeField] private Button rotateLeftButton;
        [SerializeField] private Button rotateRightButton;

        private InputAction _inputRotateLeft;
        private InputAction _inputRotateRight;

        private float _startY;
        private Rigidbody2D _rigidbody;

        private readonly CompositeDisposable _disposables = new();

        private void Start()
        {
            var playerInput = GetComponent<PlayerInput>();
            var actionMap = playerInput.currentActionMap;
            _inputRotateLeft = actionMap["RotateLeft"];
            _inputRotateRight = actionMap["RotateRight"];

            _rigidbody = GetComponent<Rigidbody2D>();
            _startY = _rigidbody.position.y;

            this.ObserveEveryValueChanged(_ => _inputRotateLeft.WasPressedThisFrame())
                .Where(wasPressed => wasPressed)
                .Subscribe(_ => RotateLeft())
                .AddTo(this)
                .AddTo(_disposables);

            this.ObserveEveryValueChanged(_ => _inputRotateRight.WasPressedThisFrame())
                .Where(wasPressed => wasPressed)
                .Subscribe(_ => RotateRight())
                .AddTo(this)
                .AddTo(_disposables);

            rotateLeftButton.OnClickAsObservable()
                .Subscribe(_ => RotateLeft())
                .AddTo(this)
                .AddTo(_disposables);

            rotateRightButton.OnClickAsObservable()
                .Subscribe(_ => RotateRight())
                .AddTo(this)
                .AddTo(_disposables);

            this.ObserveEveryValueChanged(_ => _rigidbody.position.y)
                .Where(posY => posY < _startY)
                .Subscribe(_ => _disposables.Dispose())
                .AddTo(this)
                .AddTo(_disposables);
        }

        private void RotateLeft()
        {
            transform.Rotate(0, 0, rotateAngle);
        }

        private void RotateRight()
        {
            transform.Rotate(0, 0, -rotateAngle);
        }
    }
}
