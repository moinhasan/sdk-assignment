using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a VictoryZone, usually used to end the current game level.
    /// </summary>
    public class VictoryZone : MonoBehaviour
    {
        DialogManager dialog;
        AdMobController adMobController;
        FirebaseAnalyticsManager firebaseAnalytics;
        Dialog.Callback dialogCallback;

        private void Awake()
        {
            dialog = DialogManager.Instance;
            adMobController = AdMobController.Instance;
            firebaseAnalytics = FirebaseAnalyticsManager.Instance;
        }
        void Start()
        {
            dialogCallback = this.DialogCallback;
        }
        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                var ev = Schedule<PlayerEnteredVictoryZone>();
                ev.victoryZone = this;
            }

            firebaseAnalytics.AnalyticsLevelComplete();
            dialog.CallSystemDialog("Congratulations!", "Successfully tracked Level Complete event", SystemDialog.AppearanceType.Default, dialogCallback);
        }
        //Callback on button OK
        private void DialogCallback(Dialog dialog = null, int result = 0)
        {
            adMobController.ShowInterstitialAd();
        }

    }
}