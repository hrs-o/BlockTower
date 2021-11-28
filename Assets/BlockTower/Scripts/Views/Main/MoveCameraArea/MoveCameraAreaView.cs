using BlockTower.Presenters.Main;
using UnityEngine;
using VContainer;

namespace BlockTower.Views.Main.MoveCameraArea
{
    public class MoveCameraAreaView : MonoBehaviour
    {
        [Inject] private MoveCameraAreaPresenter _presenter;

        private void OnTriggerEnter2D(Collider2D other)
        {
            _presenter.MovingCamera();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _presenter.StopCamera();
        }
    }
}
