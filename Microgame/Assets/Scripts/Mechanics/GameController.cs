using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        DialogManager dialog;
        Dialog.Callback dialogCallback;

        private void Awake()
        {
            dialog = DialogManager.Instance;
        }

        void Start() {
            dialogCallback = this.DialogCallback;
            //SystemDialog dialog = dialogManager.CallSystemDialog("My Title", "Hello World!", SystemDialog.AppearanceType.Default, dialogCallback);
        }

        private void DialogCallback(Dialog dialog = null, int result = 0)
        {
            Debug.Log("DialogManager Result:" + result);
        }

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
        }
    }
}