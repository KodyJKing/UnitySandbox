using UnityEngine;
using System.Collections;

public class Sticky : MonoBehaviour {

    private void OnCollisionStay( Collision other ) {
        if ( other.rigidbody == null )
            return;

        GameObject otherObject = other.gameObject;

        FixedJoint[] joints = GetComponents<FixedJoint>();

        foreach ( var joint in joints ) {
            if ( joint.connectedBody == other.rigidbody )
                return;
        }

        {
            gameObject.AddComponent<FixedJoint>();
            joints = GetComponents<FixedJoint>();
            FixedJoint joint = joints[ joints.Length - 1 ];
            joint.connectedBody = other.rigidbody;
            joint.breakForce = 50;
        }
    }

}
