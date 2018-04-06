using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerControls {

    public static PlayerControls player1Control;
    public static PlayerControls player2Control;

    public static void LoadSettings() {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OTSU/Decapite/Input";
        if (File.Exists(path + "1.xml") && File.Exists(path + "2.xml")) { // load existing settings
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(PlayerControls));
            StreamReader file1 = new StreamReader(path+"1.xml");
            player1Control = (PlayerControls)reader.Deserialize(file1);
            file1.Close();
            StreamReader file2 = new StreamReader(path + "2.xml");
            player2Control = (PlayerControls)reader.Deserialize(file2);
            file2.Close();
        } else { // load default settings
            player1Control = BuildPlayer1Controls();
            player2Control = BuildPlayer2Controls();
        }
    }

    public static void SaveSettings() {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OTSU/Decapite";
        Directory.CreateDirectory(path);
        System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(PlayerControls));
        FileStream file1 = System.IO.File.Create(path + "/Input1.xml");
        writer.Serialize(file1, player1Control);
        file1.Close();
        FileStream file2 = System.IO.File.Create(path + "/Input2.xml");
        writer.Serialize(file2, player2Control);
        file2.Close();
    }

    public string name = "Player X";
    public PlayerSlot playerSlot;


    public ControlType controlType;

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
        if (controlType == ControlType.Controller) {
            if (playerSlot == PlayerSlot.Player1) {
                attackKey = KeyCode.Joystick1Button0;
            } else {
                attackKey = KeyCode.Joystick2Button0;
            }
        } else {
            if (playerSlot == PlayerSlot.Player1) {
                attackKey = KeyCode.Mouse0;
            } else {
                attackKey = KeyCode.Space;
            }
        }
        if (playerSlot == PlayerSlot.Player1) {
            leftKey = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            upKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
            horizontalAxisName = "Horizontal1";
            verticalAxisName = "Vertical1";
        } else {
            leftKey = KeyCode.Q;
            rightKey = KeyCode.D;
            upKey = KeyCode.Z;
            downKey = KeyCode.S;
            horizontalAxisName = "Horizontal2";
            verticalAxisName = "Vertical2";
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
