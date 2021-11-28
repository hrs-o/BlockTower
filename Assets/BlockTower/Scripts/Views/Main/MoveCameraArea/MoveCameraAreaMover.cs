using BlockTower.Presenters.Main;
using UniRx;
using UnityEngine;
using VContainer;

namespace BlockTower.Views.Main.MoveCameraArea
{
    public class MoveCameraAreaMover : MonoBehaviour
    {
        [Inject] private MoveCameraAreaPresenter _presenter;

        private Transform _transform;

        private void Start()
        {
            _transform = transform;
            _presenter.AddMoveCameraHeightOffsetAsObservable
                .Subscribe(MoveArea)
                .AddTo(this);
        }

        private void MoveArea(float y)
        {
            _transform.Translate(0, y, 0);
        }
    }
}
