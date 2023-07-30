
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TMPro.TextMeshProUGUI _buttonText;
    [SerializeField] private GameObject _menuHolder;

    private string _pauseText = "Pause";
    private string _unPauseText = "Unpause";

    void Awake()
    {
        SetButtonText();
        _pauseButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        _pauseButton.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked() 
    {
        bool isActive = _menuHolder.activeInHierarchy;
        GameStateManager.GameState state = !isActive 
            ? GameStateManager.GameState.Paused
            : GameStateManager.GameState.GamePlay;

        GameStateManager.SetState(state);

        _menuHolder.SetActive(!isActive);
        SetButtonText();
    }
    private void SetButtonText() 
    {
        _buttonText.text = (_menuHolder.activeInHierarchy) ? _unPauseText : _pauseText;
    }

}
