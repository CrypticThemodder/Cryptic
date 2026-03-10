using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Cryptic.Mods
{
    internal class Fun
    {
        public static void TPose()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f || Input.GetKey(KeyCode.T))
            {
                GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position = new Vector3(0f, 0f, 1f);
            }
        }
    }
}
