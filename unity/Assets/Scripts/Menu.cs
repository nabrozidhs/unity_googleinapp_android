using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GoogleInAppPlugin))]
public class Menu : MonoBehaviour {

    private static string INAPP_ITEM = "REPLACE_INADD_ITEM";

    private static Vector2 BUTTON_SIZE = new Vector2(100, 50);
    private static Vector2 TEXT_SIZE = 2 * BUTTON_SIZE;

    private Rect buttonPosition;
    private Rect textPosition;

    private GoogleInAppPlugin inApp;
    private bool purchased;

    void Start() {
        buttonPosition = new Rect(
            (Screen.width - BUTTON_SIZE.x) / 2,
            (Screen.height - BUTTON_SIZE.y) / 2,
            BUTTON_SIZE.x, BUTTON_SIZE.y);
        textPosition = new Rect(
            (Screen.width - TEXT_SIZE.x) / 2,
            buttonPosition.y + 2 * BUTTON_SIZE.y,
            TEXT_SIZE.x, TEXT_SIZE.y);

        #if UNITY_ANDROID
        inApp = GetComponent<GoogleInAppPlugin>();
        GoogleInAppPlugin.BindComplete += HandleBindComplete;
        inApp.Bind();
        #endif
    }

    void OnGUI () {
        GUI.TextArea(
            textPosition,
            "Item " + INAPP_ITEM + (purchased ? " purchased." : " not purchased."));

        if (GUI.Button(buttonPosition, "Purchase")) {
            #if UNITY_ANDROID
            inApp.PurchaseItem(INAPP_ITEM);
            #endif
        }
    }

    void OnApplicationFocus(bool focused) {
        if (focused) {
            #if UNITY_ANDROID
            purchased = inApp.IsItemPurchased(INAPP_ITEM);
            #endif
        }
    }

    /// <summary>
    /// Handles the bind complete event from GoogleInAppPlugin.
    /// </summary>
    public void HandleBindComplete() {
        #if UNITY_ANDROID
        purchased = inApp.IsItemPurchased(INAPP_ITEM);
        #endif
    }
}
