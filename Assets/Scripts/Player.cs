using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Vector2 swipeStartPos;
    private Vector2 swipeEndPos;

    //public int buffer = 20;
    public float speed = 2;
    public float slowdown = 0.4f;

    float currentTime = 1f;
    public float timeSlowedFactor = 0.3f;
    public float slowdownLerp = 0.6f;
    public float speedupLerp = 0.9f;

    float currentFade = 0f;
    public float shadowFadeFactor = 180f;
    public float shadowLerp = 0.2f;

    public float lerpBuffer = 0.02f;
    float timeBuffer;
    float shadowBuffer;

    public GameObject fx;

    bool swiping = false;
    bool validSwipe = false;
    Image shadow;

    public float swipeBuffer = 0.05f;

    //private bool fingerDown;
    Rigidbody2D rb;
    LineRenderer line;

    float maxMagnitude;

    Color baseLine;
    Color inactiveLine;
    Camera cam;
    GameObject camParent;
    public float camZoomSpeed = 1.5f;
    public float camMoveSpeed = 1f;

    Vector3 originalPos = new Vector3(0f, 0f, -10f);

    //Direction Lines
    public float maxLineDistance = 3f;
    public LayerMask trajectoryLayer;
    public GameObject trajectoryCircle;
    public int pointNum = 5;
    GameObject[] trajCirArray;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1f;
        shadow = GameObject.Find("Shadow").GetComponent<Image>();
        shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, currentFade);

        timeBuffer = Mathf.Abs(currentTime - slowdownLerp) * lerpBuffer;
        shadowFadeFactor /= 255;
        shadowBuffer = shadowFadeFactor * lerpBuffer;

        line = FindObjectOfType<LineRenderer>();
        line.SetPosition(0, Vector2.zero);
        line.SetPosition(1, Vector2.zero);

        //Maximum Screen Size
        maxMagnitude = (new Vector2(Screen.width, Screen.height) - Vector2.zero).magnitude;
        baseLine = new Color(1, 1, 1, 0.6f);
        inactiveLine = new Color(0.5f, 0.5f, 0.5f, 0.6f);
        line.startColor = inactiveLine;
        line.endColor = inactiveLine;

        cam = FindObjectOfType<Camera>();
        camParent = GameObject.Find("Cam Object");

        //Target Line
        trajCirArray = new GameObject[pointNum];
        for (int i = 0; i < pointNum; ++i)
        {
            trajCirArray[i] = Instantiate(trajectoryCircle, transform);
            trajCirArray[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Using Input.GetMouseButton, will work on mobile but not a long term solution

        //Get Button Down
        if (Input.GetMouseButtonDown(0))
        {
            swipeStartPos = Input.mousePosition;

            Vector3 mouseVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseVec.z = 0f;
            line.SetPosition(0, mouseVec);
            swiping = true;
            //Time.timeScale = timeSlowedFactor;

            //Activate Points
            for (int i = 0; i < pointNum; ++i)
            {
                trajCirArray[i].transform.localPosition = Vector3.zero;
                //trajCirArray[i].GetComponent<s>
                trajCirArray[i].SetActive(true);
            }
        }

        //Get Button Up
        if (Input.GetMouseButtonUp(0) && swiping)
        {
            swiping = false;
            validSwipe = false;

            swipeEndPos = Input.mousePosition;
            Vector2 direction = swipeEndPos - swipeStartPos;

            //Get direction as percentage of screen 
            float percentage = direction.magnitude / maxMagnitude;
            percentage = Mathf.Pow(percentage, 0.33f);
            print(percentage);

            if (percentage >= swipeBuffer)
            {
                line.SetPosition(0, Vector2.zero);
                line.SetPosition(1, Vector2.zero);
                GameObject nowfx = Instantiate(fx, this.transform.position, this.transform.rotation);
                Destroy(nowfx, 1f);
                rb.velocity = rb.velocity * slowdown;
                rb.AddForce(direction.normalized * percentage * speed);
            }
            else
            {
                //print("buffered");
                line.SetPosition(0, Vector2.zero);
                line.SetPosition(1, Vector2.zero);
            }

            //Deactivate Points
            for (int i = 0; i < pointNum; ++i)
            {
                trajCirArray[i].transform.localPosition = Vector3.zero;
                //trajCirArray[i].GetComponent<s>
                trajCirArray[i].SetActive(false);
            }
            //cam.orthographicSize = 5f;
            //cam.transform.position = new Vector3(0f, 0f, -10f);
        }

        if (swiping)
        {
            Vector3 mouseVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseVec.z = 0f;
            line.SetPosition(1, mouseVec);

            swipeEndPos = Input.mousePosition;
            Vector2 direction = swipeEndPos - swipeStartPos;
            float percentage = direction.magnitude / maxMagnitude;

            if (!validSwipe && percentage > swipeBuffer)
            {
                validSwipe = true;

                line.startColor = baseLine;
                line.endColor = baseLine;
            }
            else if (validSwipe && percentage <= swipeBuffer)
            {
                validSwipe = false;

                line.startColor = inactiveLine;
                line.endColor = inactiveLine;
            }

            {
                List<Vector2> points = new List<Vector2>();

                //first point is the start point
                //Loop:
                //check for collision at the distance
                //if collision
                //add collision point to the list
                //calculate start point + trajectory
                //if no collision
                //end loop

                AddPoints(points, transform.position, new Vector2(direction.x,
                    direction.y).normalized, percentage * maxLineDistance);

                for (int i = 1; i < points.Count; ++i)
                {
                    Debug.DrawLine(points[i - 1], points[i]);
                }

                float inc = 1 / (float)pointNum;
                for (int i = 0; i < pointNum; ++i)
                {
                    if (!validSwipe)
                    {
                        trajCirArray[i].transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        trajCirArray[i].transform.position = ReturnPoint(points,
                            percentage * maxLineDistance * (inc + inc * i));
                    }

                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (swiping)
        {
            //Speed
            if ((currentTime - slowdownLerp) > timeBuffer)
            {
                currentTime = Mathf.Lerp(currentTime, timeSlowedFactor, slowdownLerp);
                Time.timeScale = currentTime;
            }
            else {
                print("reached");
            }

            //Shadow
            if ((shadowFadeFactor - currentFade) > shadowBuffer)
            {
                currentFade = Mathf.Lerp(currentFade, shadowFadeFactor, shadowLerp);
                shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, currentFade);

            }

            //
            swipeEndPos = Input.mousePosition;
            Vector2 direction = swipeEndPos - swipeStartPos;
            float percentage = direction.magnitude / maxMagnitude;

            float size = 5 - percentage / 3;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, size, camZoomSpeed);

            Vector3 camPos = this.transform.position - cam.transform.position;
            camPos *= percentage / 5;
            camPos.z = -10;

            camParent.transform.position = Vector3.Lerp(camParent.transform.position, camPos, camMoveSpeed);

        }
        else
        {
            //Speed
            if ((1 - currentTime) > 0.02f)
            {
                currentTime = Mathf.Lerp(currentTime, 1f, speedupLerp);
                if ((1 - currentTime) <= 0.02f)
                {
                    currentTime = 1f;
                }
                Time.timeScale = currentTime;
            }

            //Shadow
            if ((currentFade) > shadowFadeFactor * 0.02f)
            {
                currentFade = Mathf.Lerp(currentFade, 0, speedupLerp);
                if (currentFade <= shadowFadeFactor * 0.02)
                {
                    currentFade = 0f;
                }
                shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, currentFade);

            }

            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5f, camZoomSpeed * 0.7f);
            camParent.transform.position = Vector3.Lerp(camParent.transform.position, originalPos, camMoveSpeed * 0.7f);
        }
    }


    void AddPoints(List<Vector2> list, Vector2 startPos, Vector2 direction, float distance)
    {

        list.Add(startPos);

        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, distance, trajectoryLayer);
        if (hit.collider == null) //No Hit
        {
            list.Add(startPos + (direction.normalized * distance));
        }
        else
        { //Hit
            Vector2 impactPoint = startPos + (direction.normalized * (hit.distance * 0.97f));
            Vector2 newDirection = Vector2.Reflect(direction, hit.normal);
            float newDistance = distance - hit.distance;

            if (newDistance <= 0.1f || startPos == impactPoint) return;

            AddPoints(list, impactPoint, newDirection.normalized, newDistance);
        }
    }

    Vector2 ReturnPoint(List<Vector2> list, float distance)
    {


        Vector2 result = list[list.Count - 1];

        for (int i = 1; i < list.Count; ++i)
        {
            float segmentDistance = Vector2.Distance(list[i - 1], list[i]);
            if (distance <= segmentDistance)
            {
                float percent = distance / segmentDistance;
                result = list[i - 1] + ((list[i] - list[i - 1]) * percent);
                break;
            }
            else
            {
                distance -= segmentDistance;
            }
        }
        return result;
    }
}
