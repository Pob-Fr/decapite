using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls {

    public static PlayerControls player1Control = BuildPlayer1Controls();
    public static PlayerControls player2Control = BuildPlayer2Controls();

    public string name = "Player X";
    public PlayerSlot playerSlot;


    public ControlType controlType {
        get { return CONTROL_TYPE; }
        set {
            CONTROL_TYPE = value;
            ResetDefault();
        }
    }
    public ControlType CONTROL_TYPE;

    public KeyCode attackKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode upKey;
    public KeyCode downKey;

    public string horizontalAxisName = "HorizontalC";
    public string verticalAxisName = "VerticalC";

    private const float deadzone = 0.5f;

    public bool GetAttack() {
        return Input.GetKey(attackKey);
    }

    public Vector2 GetAxisRaw() {
        Vector2 output;
        if (controlType == ControlType.Controller) {
            output = new Vector2(Input.GetAxisRaw(horizontalAxisName), Input.GetAxisRaw(verticalAxisName));
            if (output.magnitude < deadzone)
                output = Vector2.zero;
        } else {
            output = new Vector2();
            bool left, right, up, down;
            left = Input.GetKey(leftKey);
            right = Input.GetKey(rightKey);
            up = Input.GetKey(upKey);
            down = Input.GetKey(downKey);
            if (left && !right)
                output.x = -1;
            else if (right && !left)
                output.x = 1;
            if (up && !down)
                output.y = 1;
            else if (down && !up)
                output.y = -1;
        }
        return output;
    }

    public void ResetDefault() {
        if (CONTROL_TYPE == ControlType.Controller) {
            if (playerSlot == PlayerSlot.Player1) {
                attackKey = KeyCode.Joystick1Button0;
                horizontalAxisName = "Horizontal1";
                verticalAxisName = "Vertical1";
            } else {
                attackKey = KeyCode.Joystick2Button0;
                horizontalAxisName = "Horizontal2";
                verticalAxisName = "Vertical2";
            }
        } else {
            if (playerSlot == PlayerSlot.Player1) {
                attackKey = KeyCode.Mouse0;
                leftKey = KeyCode.LeftArrow;
                rightKey = KeyCode.RightArrow;
                upKey = KeyCode.UpArrow;
                downKey = KeyCode.DownArrow;
            } else {
                attackKey = KeyCode.Space;
                leftKey = KeyCode.Q;
                rightKey = KeyCode.D;
                upKey = KeyCode.Z;
                downKey = KeyCode.S;
            }
        }
    }

    public static PlayerControls BuildPlayer1Controls() {
        PlayerControls controls = new PlayerControls();
        controls.playerSlot = PlayerSlot.Player1;
        controls.name = "Player 1";
        controls.ResetDefault();
        return controls;
    }

    public static PlayerControls BuildPlayer2Controls() {
        PlayerControls controls = new PlayerControls();
        controls.playerSlot = PlayerSlot.Player2;
        controls.name = "Player 2";
        controls.ResetDefault();
        return controls;
    }

}
