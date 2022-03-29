using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseAnalyticsManager : Singleton<FirebaseAnalyticsManager>
{
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool firebaseInitialized = false;

#if !UNITY_EDITOR && UNITY_ANDROID
    // When the app starts, check for the required dependencies
    public virtual void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        Debug.Log("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        Debug.Log("Set user properties.");
        // Set the user's sign up method.
        FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod,"Google");
        // Set the user ID.
        FirebaseAnalytics.SetUserId("user_1");
        // Set default session duration values.
        FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        firebaseInitialized = true;
    }

    public void AnalyticsLevelComplete()
    {
        // Log an event with multiple parameters.
        Debug.Log("Logging a level up event.");
        FirebaseAnalytics.LogEvent(
          "level_complete",
          new Parameter(FirebaseAnalytics.ParameterLevel, UnityEngine.Random.Range(2, 5)),
          new Parameter(FirebaseAnalytics.ParameterScore, UnityEngine.Random.Range(200, 500)),
          new Parameter("jump_accuracy", UnityEngine.Random.Range(1.0f, 100.0f)));
    }

    // Reset analytics data for this app instance.
    public void ResetAnalyticsData()
    {
        Debug.Log("Reset analytics data.");
        FirebaseAnalytics.ResetAnalyticsData();
    }

#else
    public void AnalyticsLevelComplete()
    {
        Debug.Log("Fake event for editor");
    }
#endif
}
