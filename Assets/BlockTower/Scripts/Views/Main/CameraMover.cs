using BlockTower.Presenters.Main;
using UniRx;
using UnityEngine;
using VContainer;

namespace BlockTower.Views.Main
{
    public class CameraMover : MonoBehaviour
    {
        [Inject] private MoveCameraAreaPresenter _presenter;

        private Transform _transform;

        private void Start()
        {
            _transform = transform;
            _presenter.AddMoveCameraHeightOffsetAsObservable
                .Subscribe(MoveCamera)
                .AddTo(this);
        }

        private void MoveCamera(float y)
        {
            _transform.Translate(0, y, 0);
        }
    }
}
