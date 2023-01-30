using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorAction : GameAction
{
    public float DissolveEffectTime = 2;

    public GameAction[] FinishedAction;

    public GameObject pivot;

    void Start()
    {

        //Desactivar para evitar que la función Update sea llamada. Al ser llamada por el GameTrigger se reactivará
        enabled = false;
    }

    void Update()
    {
            foreach (var gameAction in FinishedAction)
            {
                gameAction.Activated();
            }

        

            var playerkey = Controller.Instance.key;
            if (playerkey == true && gameObject.CompareTag("Door"))
            {

                Quaternion newRotation = Quaternion.AngleAxis(90, Vector3.up);
                pivot.transform.rotation = Quaternion.Slerp(pivot.transform.rotation, newRotation, .05f);

            }

    }

    public override void Activated()
    {
        enabled = true;
    }
}
