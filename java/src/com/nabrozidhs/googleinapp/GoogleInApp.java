package com.nabrozidhs.googleinapp;

import android.app.Activity;
import android.app.PendingIntent;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.IntentSender.SendIntentException;
import android.content.ServiceConnection;
import android.os.Bundle;
import android.os.IBinder;
import android.os.RemoteException;
import android.util.Log;

import com.android.vending.billing.IInAppBillingService;
import com.unity3d.player.UnityPlayer;

public final class GoogleInApp {

    private static final String TAG = GoogleInApp.class.getSimpleName();
    
    private static final int API_VERSION = 3;
    
    private static final String TYPE_INAPP = "inapp";
    
    private static final String RESPONSE_CODE = "RESPONSE_CODE";
    private static final String RESPONSE_BUY_INTENT = "BUY_INTENT";
    private static final String RESPONSE_INAPP_ITEM_LIST = "INAPP_PURCHASE_ITEM_LIST";
    
    private static final int BILLING_RESPONSE_RESULT_OK = 0;
    
    private final Activity mActivity;
    private final String mCallbackName;
    private IInAppBillingService mService;
    private final ServiceConnection mServiceConnection = new ServiceConnection() {
        @Override
        public void onServiceDisconnected(final ComponentName name) {
            mService = null;
        }
        
        @Override
        public void onServiceConnected(final ComponentName name, final IBinder service) {
            mService = IInAppBillingService.Stub.asInterface(service);
            
            try {
                if (mService.isBillingSupported(API_VERSION, mActivity.getPackageName(),
                        TYPE_INAPP) == BILLING_RESPONSE_RESULT_OK
                        && mCallbackName != null) {
                    onBindComplete();
                }
            } catch (RemoteException e) {
                Log.e(TAG, "" + e.getMessage(), e);
            }
        }
    };
    
    public GoogleInApp(final Activity activity, final String callbackName) {
        mActivity = activity;
        mCallbackName = callbackName;
        
        mActivity.bindService(
                new Intent("com.android.vending.billing.InAppBillingService.BIND"),
                mServiceConnection, Context.BIND_AUTO_CREATE);
    }
    
    /**
     * Unbinds the billing service.
     */
    public void dispose() {
        if (mServiceConnection != null) {
            mActivity.unbindService(mServiceConnection);
        }
    }
    
    /**
     * Checks if the user owns the specified item.
     * 
     * @param sku the item's sku.
     * 
     * @return true if the user owns the specified item, false otherwise.
     */
    public boolean isItemPurchased(final String sku) {
    	if (mService == null) {
    		return false;
    	}

        try {
            final Bundle purchases = mService.getPurchases(API_VERSION,
                    mActivity.getPackageName(), TYPE_INAPP, null);
            final int responseCode = getResponseCodeFromBundle(purchases);
            
            if (responseCode == BILLING_RESPONSE_RESULT_OK) {
                // Check if the specified sku is on the user's purchased item list.
                return purchases.getStringArrayList(RESPONSE_INAPP_ITEM_LIST).contains(sku);
            }
        } catch (RemoteException e) {
            Log.e(TAG, "" + e.getMessage(), e);
        }
        
        return false;
    }
    
    /**
     * Launches a purchase intent for the specified item.
     * 
     * @param sku the item's sku.
     */
    public void purchaseItem(final String sku) {
    	if (mService == null) {
    		return;
    	}
    	
        try {
            final Bundle buyIntentBundle = mService.getBuyIntent(API_VERSION,
                    mActivity.getPackageName(), sku, TYPE_INAPP, "");
            
            final int responseCode = getResponseCodeFromBundle(buyIntentBundle);
            
            if (responseCode == BILLING_RESPONSE_RESULT_OK) {
                final PendingIntent pendingIntent = buyIntentBundle.getParcelable(
                        RESPONSE_BUY_INTENT);
                mActivity.startIntentSenderForResult(pendingIntent.getIntentSender(),
                        responseCode, new Intent(), Integer.valueOf(0),
                        Integer.valueOf(0), Integer.valueOf(0));
            }
        } catch (RemoteException e) {
            Log.e(TAG, "" + e.getMessage(), e);
        } catch (SendIntentException e) {
            Log.e(TAG, "" + e.getMessage(), e);
        }
    }
    
    /**
     * Workaround to bug where sometimes response codes come as Long instead of Integer.
     */
    private static int getResponseCodeFromBundle(final Bundle b) {
        Object o = b.get(RESPONSE_CODE);
        if (o == null) {
            // Bundle with null response code, assuming OK (known issue).
            return BILLING_RESPONSE_RESULT_OK;
        } else if (o instanceof Integer) {
            return ((Integer)o).intValue();
        } else if (o instanceof Long) {
            return ((Long)o).intValue();
        } else {
            // Unexpected type for bundle response code.
            throw new RuntimeException(
                    "Unexpected type for bundle response code: "
                            + o.getClass().getName());
        }
    }
    
    /**
     * Notifies Unity that the billing service is ready.
     */
    private void onBindComplete() {
        if (mCallbackName != null) {
            UnityPlayer.UnitySendMessage(mCallbackName, "OnBindComplete", "");
        }
    }
}
