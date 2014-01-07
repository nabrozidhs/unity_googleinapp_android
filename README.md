# Unity Google InApp for Android


## Overview

(Really) Basic Unity3D plugin for Google Play In-app billing Version 3 API.


## Integration

### Importing

Just import the file `googleinapp.unitypackage` in your Unity project.

### Project setup

You need to add the BILLING permission `<uses-permission android:name="com.android.vending.BILLING" />`
as specified on the [Android documentation](http://developer.android.com/google/play/billing/billing_integrate.html#billing-permission).

### Examples

The Unity package provides an example script on how to use this plugin.


## Further plugin development

### Java

#### Installation

1. Make sure that Android SDK is correctly installed and that the `Google Play Billing Library`
package is downloaded.
2. Copy `IInAppBillingService.aidl` from `<ANDROID_SDK_DIR>/extras/google/play_billing`
to `<PLUGIN_DIR>/java/src/com/android/vending/billing`.
3. Copy `classes.jar` from `<UNITY_DIR>/Editor/Data/PlaybackEngines/androidplayer/bin`
to `<PLUGIN_DIR>/java/libs`.

The project should be able to build without errors on Eclipse.

### Unity

After succesfully building the Android plugin you can copy the library jar file from the
bin folder to your Unity project at `Assets/Plugins/Android`.
