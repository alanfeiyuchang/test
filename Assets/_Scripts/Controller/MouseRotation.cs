using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using WooHooFly.NodeSystem;

public class MouseRotation : MonoBehaviour
{
    //public Transform Compass;
    public float rotateAngle;
    private Vector3 rotationAngle;
    private Vector3 targetAngle;
    private float TurnDuration = 1f;
    [SerializeField] private Transform Axis;
    
    public RotationLink[] rotationLinks;
    public UnityEvent rotationEvent;

    public static MouseRotation instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        targetAngle = transform.eulerAngles;
        UpdateOrientation(targetAngle.y);
    }
    void Update()
    {
        if (GameManager.instance.CurrentState != GameManager.GameState.playing)
            return;
        //rotationAngle = Compass.transform.up * angle;
        rotationAngle = this.transform.up * rotateAngle;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            targetAngle = Input.GetAxis("Mouse ScrollWheel") * rotationAngle * 10;
            //transform.eulerAngles = clampAngle(targetAngle);
            transform.RotateAround(Axis.position, Vector3.up, targetAngle.y);
            //StartCoroutine(RotateMap(Compass.transform.up, angle));

            //float angle = Vector3.Angle(this.transform.forward, Vector3.forward);
            //float angle2 = Vector3.Angle(this.transform.forward, Vector3.right);
            //currentAngle = (angle2 > 90) ? 360 - angle : angle;

            UpdateOrientation(Mathf.RoundToInt(transform.eulerAngles.y));
            UpdateLinkers(Mathf.RoundToInt(transform.eulerAngles.y));

            if (rotationEvent != null)
                rotationEvent.Invoke();
        }
    }

    // How the whole level is rotated related to world axis
    private void UpdateOrientation(float angle)
    {
        //Debug.Log(angle);
        if (angle > 0 && angle <= 90)
        {
            GameManager.instance.levelDirection = Direction.Right;
        }
        else if (angle > 90 && angle <= 180)
        {
            GameManager.instance.levelDirection = Direction.Backward;
        }
        else if (angle > 180 && angle <= 270)
        {
            GameManager.instance.levelDirection = Direction.Left;
        }
        else if (angle > 270 && angle <= 360)
        {
            GameManager.instance.levelDirection = Direction.Forward;
        }
    }

    /// <summary>
    /// Enable/Disable linker when rotate to certain angle
    /// </summary>
    /// <param name="angle"></param>
    private void UpdateLinkers (float angle)
    {
        foreach(RotationLink link in rotationLinks)
        {
            if(link.checkNodeATransform == null && link.checkNodeBTransform == null)
            {
                if (angle == link.activeAngle)
                {
                    EnableLink(link.nodeA, link.nodeB, true);
                }
                else
                {
                    EnableLink(link.nodeA, link.nodeB, false);
                }
            }
            else
            {
                if (link.checkNodeATransform != null)
                {
                    if(link.checkNodeATransform.transform.localPosition == link.transform)
                    {
                        if (angle == link.activeAngle)
                        {
                            EnableLink(link.nodeA, link.nodeB, true);
                        }
                        else
                        {
                            EnableLink(link.nodeA, link.nodeB, false);
                        }
                    }   
                }
                else if (link.checkNodeBTransform != null)
                {
                    if (link.checkNodeBTransform.transform.localPosition == link.transform)
                    {
                        if (angle == link.activeAngle)
                        {
                            EnableLink(link.nodeA, link.nodeB, true);
                        }
                        else
                        {
                            EnableLink(link.nodeA, link.nodeB, false);
                        }
                    }
                }
            }
        }
    }

    private void EnableLink(Node nodeA, Node nodeB, bool state)
    {
        if (nodeA == null || nodeB == null)
            return;
        if (TutorialManager.current != null) {
            if (state) {
                TutorialManager.current.HighlightPath();
            }
        }
        
        nodeA.EnableTransitEdge(nodeB, state);
        nodeB.EnableTransitEdge(nodeA, state);
    }

    private Vector3 clampAngle(Vector3 target)
    {
        float x;
        if (target.x > 180)
            x = target.x - 360;
        else if (target.x < -180)
            x = target.x + 360;
        else
            x = target.x;

        float y;
        if (target.y > 180)
            y = target.y - 360;
        else if (target.y < -180)
            y = target.y + 360;
        else
            y = target.y;

        float z;
        if (target.z > 180)
            z = target.z - 360;
        else if (target.z < -180)
            z = target.z + 360;
        else
            z = target.z;
        return new Vector3(x, y, z);
    }

    public void RotateMapCW()
    {
        if (GameManager.instance.CurrentState != GameManager.GameState.playing)
            return;
        StartCoroutine(RotateMapAnim(rotateAngle));
    }

    public void RotateMapCCW()
    {
        if (GameManager.instance.CurrentState != GameManager.GameState.playing)
            return;
        StartCoroutine(RotateMapAnim(-1 * rotateAngle));
    }


    IEnumerator RotateMapAnim(float angle)
    {
        Debug.Log(angle);
        GameManager.instance.CurrentState = GameManager.GameState.rotating;
        float t = 0;
        Vector3 startAngleY = transform.eulerAngles;
        Vector3 endAngle = transform.eulerAngles;
        endAngle[1] += angle;
        endAngle = clampAngle(endAngle);
        while (t < TurnDuration)
        {
            float turnAngle = (Time.deltaTime / TurnDuration) * angle;
            t += Time.deltaTime;
            //float rotationAngle = Mathf.Lerp(0, angle, t / TurnDuration);
            transform.RotateAround(Axis.position, Vector3.up, turnAngle);
            yield return null;
        }
        transform.eulerAngles = endAngle;
        GameManager.instance.CurrentState = GameManager.GameState.playing;

        UpdateOrientation(Mathf.RoundToInt(transform.eulerAngles.y));
        UpdateLinkers(Mathf.RoundToInt(transform.eulerAngles.y));

        if (rotationEvent != null)
            rotationEvent.Invoke();
    }
}
