using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI Wkey;
    [SerializeField] private TextMeshProUGUI Akey;
    [SerializeField] private TextMeshProUGUI Dkey;
    [SerializeField] private TextMeshProUGUI Skey;
    [SerializeField] private TextMeshProUGUI INTERACTE;
    [SerializeField] private TextMeshProUGUI ALT;
    [SerializeField] private TextMeshProUGUI Pause;
    [SerializeField] private TextMeshProUGUI GamePadInteract;
    [SerializeField] private TextMeshProUGUI GamePadInteractALT;
    [SerializeField] private TextMeshProUGUI GamePadPause;


    public static TutorialUI Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of TutorialUI found!");

            return;
        }
        Instance = this;
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        KitchenGameManager.Instance.StartGame();
        Hide(); // Gọi hàm ẩn UI hướng dẫn
    }


    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartAcitve())
        {
            KitchenGameManager.Instance.StartGame();
            Hide();
        }
    }

    private void UpdateVisual()
    {
        Wkey.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_UP);
        Akey.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_LEFT);
        Dkey.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_RIGHT);
        Skey.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_DOWN);
        INTERACTE.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        ALT.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        Pause.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        GamePadInteract.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        GamePadInteractALT.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        GamePadPause.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
