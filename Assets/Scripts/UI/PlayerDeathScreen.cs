using UnityEngine.UI;
using UnityEngine;

public class PlayerDeathScreen : MonoBehaviour
{
    [SerializeField] private EventBus _eventBus;
    [SerializeField] private GameObject _holder;
    [SerializeField] private Button _retryButton;

    void Start()
    {
        _holder.SetActive(false);
        _eventBus.Subscribe<PlayerDeathSignal>(OnPlayerDeath);
        _retryButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        _retryButton.onClick.RemoveListener(OnButtonClick);
        _eventBus.Unsubscribe<PlayerDeathSignal>(OnPlayerDeath);
    }

    private void OnButtonClick() 
    {
        HideScreen();

        _eventBus.Invoke(new RestartLevelClickedSignal());
    }

    private void OnPlayerDeath(PlayerDeathSignal signal) 
    {
        GameStateManager.SetState(GameStateManager.GameState.DeathScreen);
        ShowScreen();
    }

    private void ShowScreen() 
    {
        _holder.SetActive(true);
    }

    private void HideScreen()
    {
        _holder.SetActive(false);
    }
}
