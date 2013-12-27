Unity Google InApp for Android
=============================


Overview
--------

(Really) Basic Unity3D plugin for using Google Play In-app billing Version 3 API.


Integration
===========

Importing
---------

Just import the file `googleinapp.unitypackage` into your project.

Project setup
-------------

You need to add a new permission on your Android project. Edit your `AndroidManifest.xml`
file by adding the BILLING permission `<uses-permission android:name="com.android.vending.BILLING" />`,
as specified on the [Android documentation](http://developer.android.com/google/play/billing/billing_integrate.html#billing-permission).


Examples
--------




Further plugin development
==========================

Java
----

### Installation

1. Copy `IInAppBillingService.aidl` from `<ANDROID_SDK_DIR>/extras/google/play_billing`
to `<PLUGIN_DIR>/java/src/com/android/vending/billing`, before doing this you need to
download the `Google Play Billing Library` from the Android SDK Manager.
2. Copy `classes.jar` from `<UNITY_DIR>/Editor/Data/PlaybackEngines/androidplayer/bin`
to `<PLUGIN_DIR>/java/libs`.

Those two steps should make the project build on Eclipse.

Unity
-----

After succesfully building the Android plugin on Eclipse you can copy the jar generated
on the bin folder to your Unity project at `Assets/Plugins/Android`.
