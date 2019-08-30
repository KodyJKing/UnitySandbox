using UnityEngine;

public class Drag : MonoBehaviour {

    GameObject target;
    Vector3 localGrabPoint;
    float grabDistance;
    Ray mouseRay;

    // Update is called once per frame
    void Update() {
        Camera cam = GetComponent<Camera>();
        mouseRay = cam.ScreenPointToRay( Input.mousePosition );

        if ( Input.GetMouseButtonDown( 0 ) ) {
            RaycastHit hit;
            if ( Physics.Raycast( mouseRay, out hit ) ) {
                target = hit.rigidbody.gameObject;
                localGrabPoint = hit.transform.worldToLocalMatrix.MultiplyPoint( hit.point );
                grabDistance = hit.distance;
            }
        } else if ( Input.GetMouseButtonUp( 0 ) ) {
            target = null;
        } else if ( Input.GetMouseButton( 0 ) && target != null ) {
            Vector3 gp = grabPoint;
            Vector3 mp = mousePoint;
            Vector3 diff = mp - gp;
            Rigidbody body = target.GetComponent<Rigidbody>();
            body.AddForceAtPosition( diff * 10, gp );
            body.AddForce( body.velocity * -10 );
        }

        if ( Input.GetKey( KeyCode.E ) )
            grabDistance += 0.1f;
        if ( Input.GetKey( KeyCode.Q ) )
            grabDistance -= 0.1f;
    }

    Vector3 grabPoint {
        get {
            return target.transform.localToWorldMatrix.MultiplyPoint( localGrabPoint );
        }
    }

    Vector3 mousePoint {
        get {
            return mouseRay.origin + mouseRay.direction * grabDistance;
        }
    }

    void OnDrawGizmos() {
        if ( target != null )
            Gizmos.DrawSphere( grabPoint, 0.1f );
    }

}
