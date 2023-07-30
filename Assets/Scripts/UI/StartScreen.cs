using System.Threading;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private EventBus _eventBus;
    [SerializeField] private GameObject _holder;
    [SerializeField] private TMPro.TextMeshProUGUI _text;
    

    private CancellationTokenSource _startCountDownCancelToken;

    void Awake()
    {
        _holder.SetActive(false);
        _eventBus.Subscribe<StartLevelSignal>(ShowScreen);
        _startCountDownCancelToken = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<StartLevelSignal>(ShowScreen);
    }

    private void ShowScreen(StartLevelSignal signal) 
    {
        _holder.SetActive(true);
        StartCountDown(_startCountDownCancelToken.Token);
    }

    private async void StartCountDown(CancellationToken token)
    {
        if (token.IsCancellationRequested) return;

        _text.text = "3...";
        await System.Threading.Tasks.Task.Delay(1 * 1000);
        if (token.IsCancellationRequested) return;
        _text.text = "2...";
        await System.Threading.Tasks.Task.Delay(1 * 1000);
        if (token.IsCancellationRequested) return;
        _text.text = "1...";
        await System.Threading.Tasks.Task.Delay(1 * 1000);
        if (token.IsCancellationRequested) return;
        _text.text = "GO!";
        await System.Threading.Tasks.Task.Delay((int)(0.3f * 1000));
        _holder.SetActive(false);
        _eventBus.Invoke(new StartCountDownFinishedSignal());
    }
}
