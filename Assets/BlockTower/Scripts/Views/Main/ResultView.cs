using BlockTower.Presenters.Main;
using UniRx;
using UnityEngine;
using VContainer;

namespace BlockTower.Views.Main
{
    public class ResultView : MonoBehaviour
    {
        [Inject] private ResultPresenter _presenter;

        private void Start()
        {
            gameObject.SetActive(false);

            _presenter.IsShowAsObservable
                .Subscribe(gameObject.SetActive)
                .AddTo(this);
        }
    }
}
