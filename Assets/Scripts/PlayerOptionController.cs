using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOptionController : MonoBehaviour {

    public PlayerControls playerController;

    public PlayerSlot player;
    public InputField userNameInputField;
    public Text controlTypeText;
    public Text attackText;
    public Button attackButton;
    public Text leftText;
    public Button leftButton;
    public Text rightText;
    public Button rightButton;
    public Text upText;
    public Button upButton;
    public Text downText;
    public Button downButton;

    public Image waitForInputPanel;
    public Text waitForInputText;

    void Start() {
        switch (player) {
            case PlayerSlot.Player2:
                playerController = PlayerControls.player2Control;
                break;
            default:
                playerController = PlayerControls.player1Control;
                break;
        }
        userNameInputField.text = playerController.name;
        UpdateInputText();
    }

    public void UpdateInputText() {
        if (controlType == ControlType.Controller) {
            controlTypeText.text = playerController.controlType.ToString();
            attackText.text = playerController.attackKey.ToString();
            leftText.text = "(Stick)";
            leftButton.interactable = false;
            rightText.text = "(Stick)";
            rightButton.interactable = false;
            upText.text = "(Stick)";
            upButton.interactable = false;
            downText.text = "(Stick)";
            downButton.interactable = false;
        } else {
            controlTypeText.text = playerController.controlType.ToString();
            attackText.text = playerController.attackKey.ToString();
            leftText.text = playerController.leftKey.ToString();
            leftButton.interactable = true;
            rightText.text = playerController.rightKey.ToString();
            rightButton.interactable = true;
            upText.text = playerController.upKey.ToString();
            upButton.interactable = true;
            downText.text = playerController.downKey.ToString();
            downButton.interactable = true;
        }
    }

    public ControlType controlType {
        get { return playerController.controlType; }
        set {
            playerController.controlType = value;
            playerController.ResetDefault();
            UpdateInputText();
        }
    }

    public void ChangeName(string name) {
        playerController.name = name;
    }

    public void ChangeControlType() {
        if (controlType == ControlType.Controller) {
            controlType = ControlType.Keyboard;
        } else {
            controlType = ControlType.Controller;
        }
    }

    public void ChangeAttackInput() {
        StartCoroutine(WaitForInput(InputButton.Attack));
    }

    public void ChangeLeftInput() {
        StartCoroutine(WaitForInput(InputButton.Left));
    }

    public void ChangeRightInput() {
        StartCoroutine(WaitForInput(InputButton.Right));
    }

    public void ChangeUpInput() {
        StartCoroutine(WaitForInput(InputButton.Up));
    }

    public void ChangeDownInput() {
        StartCoroutine(WaitForInput(InputButton.Down));
    }

    public void ShowWaitForInput(InputButton input) {
        waitForInputPanel.gameObject.SetActive(true);
        waitForInputText.gameObject.SetActive(true);
        waitForInputText.text = "Press a button to assign to the " + input.ToString() + " command.\nPress Escape to cancel.";
    }

    public void HideWaitForInput() {
        waitForInputPanel.gameObject.SetActive(false);
        waitForInputText.gameObject.SetActive(false);
    }

    public IEnumerator WaitForInput(InputButton inputToChange) {
        ShowWaitForInput(inputToChange);
        yield return new WaitUntil(() => Input.anyKeyDown);
        if (!Input.GetKeyDown(KeyCode.Escape)) { // if not canceled, save
            KeyCode k = GetKeyDown();
            Debug.Log(k.ToString());
            if (k != KeyCode.None) {
                switch (inputToChange) {
                    case InputButton.Attack:
                        playerController.attackKey = k;
                        break;
                    case InputButton.Left:
                        playerController.leftKey = k;
                        break;
                    case InputButton.Right:
                        playerController.rightKey = k;
                        break;
                    case InputButton.Up:
                        playerController.upKey = k;
                        break;
                    case InputButton.Down:
                        playerController.downKey = k;
                        break;
                    default: // error
                        Debug.LogError(inputToChange.ToString() + " is not a valid input button.");
                        break;
                }
                UpdateInputText();
            }
        }
        HideWaitForInput();
    }

    public static KeyCode GetButtonDown() {
        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode))) {
            if (k >= KeyCode.JoystickButton0 && Input.GetKeyDown(k))
                return k;
        }
        return KeyCode.None;
    }

    public static KeyCode GetKeyDown() {
        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode))) {
            if (k < KeyCode.JoystickButton0 && Input.GetKeyDown(k))
                return k;
        }
        return KeyCode.None;
    }

}
