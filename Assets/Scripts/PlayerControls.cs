using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls {

    private string verticalControllerInput;
    private string verticalKeyboardInput;
    private string horizontalControllerInput;
    private string horizontalKeyboardInput;
    private string attackControllerInput;
    private string attackKeyboardInput;

    private float deadzone = 0.2f;

    public Vector2 moveInput;
    public bool attackInput;

    public void UpdateControls()
    {
        float moveXController = Input.GetAxisRaw(horizontalControllerInput);
        float moveXKeyboard = Input.GetAxisRaw(horizontalKeyboardInput);
        float moveYController = Input.GetAxisRaw(verticalControllerInput);
        float moveYKeyboard = Input.GetAxisRaw(verticalKeyboardInput);
        moveInput = new Vector2(
            (Mathf.Abs(moveXController) > Mathf.Abs(moveXKeyboard)) ? moveXController : moveXKeyboard,
            (Mathf.Abs(moveYController) > Mathf.Abs(moveYKeyboard)) ? moveYController : moveYKeyboard
            );
        if (moveInput.magnitude < deadzone) moveInput = Vector2.zero;

        attackInput = Input.GetButton(attackControllerInput) || Input.GetButton(attackKeyboardInput);

    }
    
    public static PlayerControls BuildPlayer1Controls()
    {
        PlayerControls controls = new PlayerControls();
        controls.verticalControllerInput = "VerticalC";
        controls.verticalKeyboardInput = "VerticalK";
        controls.horizontalControllerInput = "HorizontalC";
        controls.horizontalKeyboardInput = "HorizontalK";
        controls.attackControllerInput = "AttackC";
        controls.attackKeyboardInput = "AttackK";
        return controls;
    }

    public static PlayerControls BuildPlayer2Controls()
    {
        PlayerControls controls = new PlayerControls();
        controls.verticalControllerInput = "VerticalC_2";
        controls.verticalKeyboardInput = "VerticalK_2";
        controls.horizontalControllerInput = "HorizontalC_2";
        controls.horizontalKeyboardInput = "HorizontalK_2";
        controls.attackControllerInput = "AttackC_2";
        controls.attackKeyboardInput = "AttackK_2";
        return controls;
    }

}
