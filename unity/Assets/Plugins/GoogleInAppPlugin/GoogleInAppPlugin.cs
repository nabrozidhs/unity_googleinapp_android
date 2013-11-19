using System;
using UnityEngine;

public sealed class GoogleInAppPlugin : MonoBehaviour {

    private const string CLASS_NAME = "com.nabrozidhs.googleinapp.GoogleInApp";

    private const string CALL_PURCHASEITEM = "purchaseItem";
    private const string CALL_ITEMPURCHASED = "isItemPurchased";

    public static event Action BindComplete = delegate {};

    private AndroidJavaObject plugin;

    /// <summary>
    /// Bind this instance.
    /// </summary>
    public void Bind() {
        plugin = new AndroidJavaObject(CLASS_NAME, GetAndroidActivity(), gameObject.name);
    }

    /// <summary>
    /// Launches a store purchase for the specified product.
    /// </summary>
    /// <param name='sku'>
    /// The product's sku.
    /// </param>
    public void PurchaseItem(string sku) {
        if (plugin != null) {
            plugin.Call(CALL_PURCHASEITEM, sku);
        }
    }

    /// <summary>
    /// Checks if the user owns the specified product.
    /// </summary>
    /// <returns>
    /// True if the user owns the specified product, false otherwise.
    /// </returns>
    /// <param name='sku'>
    /// The product's sku.
    /// </param>
    public bool IsItemPurchased(string sku) {
        if (plugin != null) {
            return plugin.Call<bool>(CALL_ITEMPURCHASED, sku);
        }

        return false;
    }

    /// <summary>
    /// Returns android activity.
    /// </summary>
    /// <returns>
    /// The plugin's java class.
    /// </returns>
    private static AndroidJavaObject GetAndroidActivity() {
        return new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity");
    }

    public void OnBindComplete() {
        BindComplete();
    }
}
