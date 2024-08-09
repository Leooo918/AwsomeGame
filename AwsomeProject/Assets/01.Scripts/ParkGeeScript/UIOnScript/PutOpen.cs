using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PutOpen : MonoBehaviour
{
    [SerializeField] private GameObject _interact;
    private Player _player;
    private PopUpPanel _popUpPanel;

    private void Start()
    {
        _popUpPanel = UIManager.Instance.panelDictionary[UIType.PopUp] as PopUpPanel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out _player))
        {
            _popUpPanel.SetText("포션 제작 [ F ]");
            UIManager.Instance.Open(UIType.PopUp);
            _player.PlayerInput.InteractPress += OnInteract;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out _player))
        {
            UIManager.Instance.Close(UIType.PopUp);
            _player.PlayerInput.InteractPress -= OnInteract;
        }
    }

    private void OnInteract()
    {
        UIManager.Instance.Open(UIType.PotionCraft);
        UIManager.Instance.Close(UIType.PopUp);
        _player.PlayerInput.InteractPress -= OnInteract;
    }
}
