using System;
using UnityEngine;

public sealed class GoogleInAppPlugin : MonoBehaviour {

    private const string CLASS_NAME = "com.nabrozidhs.googleinapp.GoogleInApp";

    private const string CALL_PURCHASEITEM = "purchaseItem";
    private const string CALL_ITEMPURCHASED = "isItemPurchased";
    private const string CALL_DISPOSE = "dispose";

    public static event Action BindComplete = delegate {};

#if UNITY_ANDROID
    private AndroidJavaObject plugin;
#endif

    /// <summary>
    /// Bind this instance.
    /// </summary>
    public void Bind() {
#if UNITY_ANDROID
        plugin = new AndroidJavaObject(
            CLASS_NAME,
            new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"),
            gameObject.name);
#endif
    }

    /// <summary>
    /// Launches a store purchase for the specified product.
    /// </summary>
    /// <param name='sku'>
    /// The product's sku.
    /// </param>
    public void PurchaseItem(string sku) {
#if UNITY_ANDROID
        if (plugin != null) {
            plugin.Call(CALL_PURCHASEITEM, sku);
        }
#endif
    }

    /// <summary>
    /// Unbinds the in app service.
    /// </summary>
    public void Dispose() {
#if UNITY_ANDROID
        if (plugin != null) {
            plugin.Call(CALL_DISPOSE, sku);
        }
#endif
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
#if UNITY_ANDROID
        if (plugin != null) {
            return plugin.Call<bool>(CALL_ITEMPURCHASED, sku);
        }
#endif
        return false;
    }

    public void OnBindComplete() {
        BindComplete();
    }

    void OnDestroy() {
        Dispose();
    }
}
