using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMovement : MonoBehaviour
{
    private Vector2 swipeStartPos;
    private Vector2 swipeEndPos;

    //public int buffer = 20;
    public float speed = 2;

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

    bool validSwipe = false;
    Image shadow;

    public float swipeBuffer = 0.05f;

    //private bool fingerDown;
    Rigidbody2D rb;

    public float maxVelocity = 58.0f;
    float sqrMaxVelocity;

    LineRenderer line;

    float maxMagnitude;

    Color baseLine;
    Color inactiveLine;
    public Camera cam;
    public GameObject camParent;
    // public float camZoomSpeed = 1.5f;
    // public float camMoveSpeed = 1f;

    public float startOrthoSize;

    //Direction Lines
    public float maxLineDistance = 3f;
    public LayerMask trajectoryLayer;
    public GameObject trajectoryCircle;
    public int pointNum = 5;
    GameObject[] trajCirArray;

    // ANIMATION START
    public GameObject playerSprite;
    public GameObject head;
    public GameObject rightarm;
    public GameObject leftarm;
    public GameObject rightleg;
    public GameObject leftleg;
    public GameObject tail;

    int setTurtleAnimation = 0;

    Vector3 saveHeadPos;
    Vector3 saveTailPos;
    Vector2 directionGlobal;
    public GameObject Mud1;
    public GameObject Mud2;
    public GameObject Mud3;
    // ANIMATION END


    public int crumbleBlocks_colliding;

    public bool riverActive = false;

    LayerMask nothingmask;

    LayerMask playermask;

    struct DirectionVector {
        public Vector2 coordinates;
        public Vector2 direction;
        public DirectionVector(Vector2 _coordinates, Vector2 _direction) {
            coordinates = _coordinates;
            direction = _direction;
        }
    }

    DirectionVector[] directions = new DirectionVector[8];


    void SetMaxVelocity(float maxVelocity){
        this.maxVelocity = maxVelocity;
        sqrMaxVelocity = maxVelocity * maxVelocity;
    }

    // Start is called before the first frame update
    void Start()
    {
        nothingmask = LayerMask.GetMask("Nothing");
        playermask = LayerMask.GetMask("Player");
        
        //TODO: Move animation and visual components into new script somewhere else
        Mud1.SetActive(false);
        Mud2.SetActive(false);
        Mud3.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        SetMaxVelocity(maxVelocity);
        Time.timeScale = 1f;
        //TODO: Replace with GM Ref
        shadow = GameManager.G.ui.shadow.GetComponent<Image>();
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

        // TODO: Replace Camera With Game Manager Ref    uhhh maybe not i mightve messed something up - nicole
        // cam = FindObjectOfType<Camera>();
        // camParent = GameObject.Find("Camera Main/Camera Tilt");
        // cam = GameManager.G.mainCamera;
        // camParent = GameManager.G.cameraTilt;

        // startOrthoSize = cam.orthographicSize;

        //Target Line
        trajCirArray = new GameObject[pointNum];
        for (int i = 0; i < pointNum; ++i)
        {
            trajCirArray[i] = Instantiate(trajectoryCircle, transform);
            trajCirArray[i].SetActive(false);
        }

        //Direction Vectors
        directions[0] = new DirectionVector(new Vector2(3, 1), (new Vector2(0f, 1f)).normalized); //North
        directions[1] = new DirectionVector(new Vector2(2, 2), (new Vector2(0.707f, 0.707f)).normalized); //Northeast
        directions[2] = new DirectionVector(new Vector2(1, 3), (new Vector2(1f, 0f)).normalized); //East
        directions[3] = new DirectionVector(new Vector2(2, 4), (new Vector2(0.707f, -0.707f)).normalized); //Southeast
        directions[4] = new DirectionVector(new Vector2(3, 5), (new Vector2(0f, -1f)).normalized); //South
        directions[5] = new DirectionVector(new Vector2(4, 4), (new Vector2(-0.707f, -0.707f)).normalized); //Southwest
        directions[6] = new DirectionVector(new Vector2(5, 3), (new Vector2(-1f, 0f)).normalized); //West     
        directions[7] = new DirectionVector(new Vector2(4, 2), (new Vector2(-0.707f, 0.707f)).normalized); //Northwest

        saveHeadPos = head.transform.localPosition;
        saveTailPos = tail.transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {

        //Start Swiping
        //TODO: Replace Input with more flexible input manager at some point
        if (Input.GetMouseButtonDown(0) && 
            (GameManager.G.player.state == PlayerState.Active || GameManager.G.player.state == PlayerState.PreGame))
        {
            swipeStartPos = Input.mousePosition;

            // Vector3 mouseVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // mouseVec.z = 0f;
            // line.SetPosition(0, mouseVec);

            //Activate Points
            //TODO: Move to the right place
            for (int i = 0; i < pointNum; ++i)
            {
                trajCirArray[i].transform.localPosition = Vector3.zero;
                trajCirArray[i].SetActive(true);
            }
            //TODO: Replace with GM Reference
            GameManager.G.audio.Play("player_select");

            GameManager.G.player.SwapState((GameManager.G.player.state == PlayerState.PreGame ? 
                PlayerState.FirstHeld : PlayerState.Held));
        }

        //Get Button Up
        if (Input.GetMouseButtonUp(0) && 
            (GameManager.G.player.state == PlayerState.Held || GameManager.G.player.state == PlayerState.FirstHeld))
        {
            validSwipe = false;

            swipeEndPos = Input.mousePosition;
            Vector2 direction = swipeEndPos - swipeStartPos;

            //Get direction as percentage of screen 
            float percentage = direction.magnitude / maxMagnitude;
            percentage = Mathf.Pow(percentage, 0.33f);

            if (percentage >= swipeBuffer)
            {
                GameObject nowfx = Instantiate(fx, this.transform.position, this.transform.rotation);
                Destroy(nowfx, 1f);
                direction = clampedDirection(direction);
                if (direction != Vector2.zero) {
                    GameManager.G.audio.Play("player_release");
                    rb.velocity = Vector2.zero;
                    rb.AddForce(direction * speed);
                }
                directionGlobal = direction;
            }

            GameManager.G.player.SwapState(PlayerState.Active);
        }

        if (GameManager.G.player.state == PlayerState.Held || GameManager.G.player.state == PlayerState.FirstHeld)
        {

            // Vector3 mouseVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // mouseVec.z = 0f;
            
            // line.SetPosition(1, mouseVec);

            swipeEndPos = Input.mousePosition;
            Vector2 direction = swipeEndPos - swipeStartPos;
            float percentage = direction.magnitude / maxMagnitude;
            direction = clampedDirection(direction);
            directionGlobal = direction;

            // MAKES DIFFERENT SWIPE TYPES HAVE DIFFERENT DRAG
            float adjustedPercentage = (percentage - swipeBuffer) / (1 - swipeBuffer);
            if (adjustedPercentage >= 0.5) rb.drag = 0.2f;
            else if (adjustedPercentage >= 0.2) rb.drag = 0.4f;
            else rb.drag = 0.9f;

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

            //TODO: Move to UI Manager or Player Manager
            {
                List<Vector2> points = new List<Vector2>();

                AddPoints(points, transform.position, direction, direction.magnitude * maxLineDistance);

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
                            direction.magnitude * maxLineDistance * (inc + inc * i));
                    }

                }
            }   
        }

        //TODO: Move to Player Animation Script
        float step = 1.4f * Time.deltaTime;
            
        float step2 = 2.5f * Time.deltaTime;

        if(setTurtleAnimation == 1) {
            rightleg.transform.Rotate(0, 0, -150 * Time.deltaTime);
            leftleg.transform.Rotate(0, 0, 150 * Time.deltaTime);
            rightarm.transform.Rotate(0, 0, 120 * Time.deltaTime);
            leftarm.transform.Rotate(0, 0, -120 * Time.deltaTime);
            head.transform.position = Vector3.MoveTowards(head.transform.position, playerSprite.transform.position, step);
            tail.transform.position = Vector3.MoveTowards(tail.transform.position, playerSprite.transform.position, step);
            if(Vector3.Distance (head.transform.position, playerSprite.transform.position) <= 0.01f) {
                setTurtleAnimation = 2;
            }
        } else if(setTurtleAnimation == 2) {
            rightleg.transform.rotation = Quaternion.RotateTowards(rightleg.transform.rotation, head.transform.rotation, 250 * Time.deltaTime);
            leftleg.transform.rotation = Quaternion.RotateTowards(leftleg.transform.rotation, head.transform.rotation, 250 * Time.deltaTime);
            rightarm.transform.rotation = Quaternion.RotateTowards(rightarm.transform.rotation, head.transform.rotation, 250 * Time.deltaTime);
            leftarm.transform.rotation = Quaternion.RotateTowards(leftarm.transform.rotation, head.transform.rotation, 250 * Time.deltaTime);
            head.transform.localPosition = Vector3.MoveTowards(head.transform.localPosition, saveHeadPos, step2);
            tail.transform.localPosition = Vector3.MoveTowards(tail.transform.localPosition, saveTailPos, step2);
            if(Vector3.Distance(head.transform.localPosition, saveHeadPos) <= 0.01f && leftleg.transform.eulerAngles.z <= 1) {
                setTurtleAnimation = 0;
            }
        }

        if (GameManager.G.state != GameState.MainMenu && GameManager.G.state != GameState.LevelSelect)
        {
            if (crumbleBlocks_colliding >= 1 && riverActive == true)
            {

                foreach (GameObject river in LevelManager.rivers)
                {
                    river.GetComponent<AreaEffector2D>().colliderMask = nothingmask;
                }
                riverActive = false;

            }
            else if (crumbleBlocks_colliding <= 0 && riverActive == false)
            {
                foreach (GameObject river in LevelManager.rivers)
                {
                    river.GetComponent<AreaEffector2D>().colliderMask = playermask;
                }
                riverActive = true;

            }
        }
            
    }

    Vector2 clampedDirection(Vector2 original) {
        float percentage = original.magnitude / maxMagnitude;
        original = original.normalized;

        //Get length of 0, 1, 2, 3
        if (percentage <= swipeBuffer) {
            return new Vector2(0, 0);
        }
        float length;
        //Get the percentage 

        float adjustedPercentage = (percentage - swipeBuffer) / (1 - swipeBuffer);
        if (adjustedPercentage >= 0.3) length = 3.5f;
        else if (adjustedPercentage >= 0.1) length = 2.3f;
        else length = 1.5f;

        //Group the X from 1-5, group the Y from 1-5, pick a direction based on the coordinates
        int x_cat;
        if (original.x > 0.92388f) x_cat = 1;
        else if (original.x > 0.38268f) x_cat = 2;
        else if (original.x > -0.38268f) x_cat = 3;
        else if (original.x > -0.92388f) x_cat = 4;
        else x_cat = 5;
        
        int y_cat;
        if (original.y > 0.92388f) y_cat = 1;
        else if (original.y > 0.38268f) y_cat = 2;
        else if (original.y > -0.38268f) y_cat = 3;
        else if (original.y > -0.92388f) y_cat = 4;
        else y_cat = 5;

        Vector2 coordinates = new Vector2(x_cat, y_cat);
        
        for (int i = 0; i < 8; ++i) {
            if (coordinates == directions[i].coordinates) {
                return directions[i].direction * ((float)length / 3f); 
            }
        }

        Debug.LogError("Direction Vector Invalid: " + coordinates);
        return new Vector2(0, 0);
    }

    //TODO: Time Slowdown should be moved??
    private void FixedUpdate()
    {

        if (GameManager.G.player.state == PlayerState.Held || 
            GameManager.G.player.state == PlayerState.FirstHeld)
        {
            //Speed
            if ((currentTime - slowdownLerp) > timeBuffer)
            {
                currentTime = Mathf.Lerp(currentTime, timeSlowedFactor, slowdownLerp);
                Time.timeScale = currentTime;
            }

            //Shadow
            if ((shadowFadeFactor - currentFade) > shadowBuffer)
            {
                currentFade = Mathf.Lerp(currentFade, shadowFadeFactor, shadowLerp);
                shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, currentFade);

            }

            swipeEndPos = Input.mousePosition;
            Vector2 direction = swipeEndPos - swipeStartPos;
            float percentage = direction.magnitude / maxMagnitude;

            // float size = startOrthoSize - percentage / 3;
            // cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, size, camZoomSpeed);

            
            // Vector3 camPos = this.transform.position - cam.transform.position;
            // camPos *= percentage / 4;
            // camPos.z = -10;

            // camParent.transform.localPosition = Vector3.Lerp(camParent.transform.localPosition, 
            //     camParent.transform.localPosition + camPos, camMoveSpeed);

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

            // cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, startOrthoSize, camZoomSpeed * 0.7f);
            // camParent.transform.localPosition = Vector3.Lerp(camParent.transform.localPosition, 
            //     Vector3.zero, camMoveSpeed * 0.7f);
        }

        if (directionGlobal != Vector2.zero) {
        	float angle = Mathf.Atan2(directionGlobal.y, directionGlobal.x) * Mathf.Rad2Deg;
        	playerSprite.transform.rotation = Quaternion.AngleAxis((angle - 90f), playerSprite.transform.forward);
        }

        var v = rb.velocity;
        // Clamp the velocity, if necessary
        // Use sqrMagnitude instead of magnitude for performance reasons.
        if(v.sqrMagnitude > sqrMaxVelocity){ // Equivalent to: rigidbody.velocity.magnitude > maxVelocity, but faster.
            // Vector3.normalized returns this vector with a magnitude 
            // of 1. This ensures that we're not messing with the 
            // direction of the vector, only its magnitude.
            rb.velocity = v.normalized * maxVelocity;
        }   
    }

    public void DeactivateUIElements() {
        line.SetPosition(0, Vector2.zero);
        line.SetPosition(1, Vector2.zero);

        //Deactivate Points
        for (int i = 0; i < pointNum; ++i)
        {
            trajCirArray[i].transform.localPosition = Vector3.zero;
            trajCirArray[i].SetActive(false);
        }
    }


    //TODO: Move to Other Script or PM
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

    //TODO: Move to player?
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

    //Not sure what this is doing
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mud") {

            if(speed > 1700f) {
                Vector2 direction = swipeEndPos - swipeStartPos;
                direction = clampedDirection(direction);
                if (direction != Vector2.zero) rb.AddForce(direction * (speed / 2 * -1));   
            }

            rb.drag = 1.0f;
            rb.angularDrag = 1.0f;
            rb.velocity *= 0.5f;

            StartCoroutine(ShowAndHide(Mud1, 1.5f));
            StartCoroutine(ShowAndHide(Mud2, 1.0f));
            StartCoroutine(ShowAndHide(Mud3, 0.5f));
        }

        if (collision.gameObject.tag == "CrumbleBlock") {
            crumbleBlocks_colliding += 1;
        }

    }

    //Move to Animation (or Object Fade?)
    IEnumerator ShowAndHide(GameObject obj, float delay)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        setTurtleAnimation = 1;
        GameManager.G.audio.Play("player_impact");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       if (collision.gameObject.tag == "Mud") {
            rb.drag = 0.0f;
            rb.angularDrag = 0.15f;
        }

        if (collision.gameObject.tag == "CrumbleBlock") {
            crumbleBlocks_colliding -= 1;
        }
    }
}


