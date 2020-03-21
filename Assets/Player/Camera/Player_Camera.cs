using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    public Transform referenceTransform;
    public float collisionOffset = 0.35f; //To prevent Camera from clipping through Objects

    int PlayerLayerMask = 10;
    Vector3 defaultPos;
    Vector3 directionNormalized;
    Transform parentTransform;
    float defaultDistance;
    int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.localPosition;
        directionNormalized = defaultPos.normalized;
        parentTransform = transform.parent;
        defaultDistance = Vector3.Distance(defaultPos, Vector3.zero);
        layerMask = ~(1 << PlayerLayerMask);
    }

    // FixedUpdate for physics calculations
    void FixedUpdate()
    {
        Vector3 currentPos = defaultPos;
        RaycastHit hit;
        Vector3 dirTmp = parentTransform.TransformPoint(defaultPos) - referenceTransform.position;
        if (Physics.SphereCast(referenceTransform.position, collisionOffset, dirTmp, out hit, defaultDistance, layerMask))
        {
            currentPos = (directionNormalized * (hit.distance - collisionOffset));
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * 15f);
    }
}
