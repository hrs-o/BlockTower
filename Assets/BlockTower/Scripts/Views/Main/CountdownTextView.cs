using BlockTower.Presenters.Main;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

namespace BlockTower.Views.Main
{
    public class CountdownTextView : MonoBehaviour
    {
        [SerializeField] private string gameStartMessage = "START!";

        [Inject] private CountdownTextPresenter _presenter;

        private TextMeshProUGUI _countdownText;

        private void Start()
        {
            _countdownText = gameObject.GetComponent<TextMeshProUGUI>();
            Enabled(false);

            _presenter.CountdownSec
                .Subscribe(UpdateCountDownText)
                .AddTo(this);

            _presenter.IsEnabled
                .Subscribe(Enabled)
                .AddTo(this);
        }

        private void UpdateCountDownText(int sec)
        {
            if (sec <= 0)
            {
                _countdownText.text = gameStartMessage;
                return;
            }

            _countdownText.text = sec.ToString();
        }

        private void Enabled(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);
        }
    }
}
