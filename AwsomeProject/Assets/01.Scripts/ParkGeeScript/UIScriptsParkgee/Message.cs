using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private string message;
    [SerializeField] private TextMeshProUGUI text;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            MessageOn();
        }
    }

    public void MessageOn()
    {
        if (text.text == null)
        {
            text.text = message;
        }
        else
            text.text = null;
    }
}
