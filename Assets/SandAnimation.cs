using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandAnimation : MonoBehaviour
{

    public GameObject sandturtle;

    private SpriteRenderer turtleSpriteRenderer;

    private Vector3 turtle_initpos;
    private Vector3 turtle_finalpos1;
    private Vector3 turtle_finalpos2;

    public float turtle_speed = 10;

    public GameObject minicursor1;
    public GameObject minisandline1;
    public GameObject minicursor2;
    public GameObject minisandline2;
    
    private Vector3 minicursor1_finalpos;
    private Vector3 minicursor1_initpos;

    private Vector3 minicursor2_finalpos;
    private Vector3 minicursor2_initpos;    

    public GameObject sandcursor;
    public GameObject sandlines;

    public SpriteRenderer sandlinesSpriteRenderer;

    public Sprite[] sandlineSpriteArray;

    private Vector3 sandlines_initpos;


    public float cursor_speed = 5;
    private Vector3 cursor_initpos;
    private Vector3 cursor_finalpos;
    private int cursor_direction = 1;

    float alpha = 1f;


    // ------------------------- for speed animation

    // public GameObject speedturtle1;
    // public GameObject speedturtle2;
    // public GameObject speedcursor1;
    // public GameObject speedcursor2;

    // public GameObject speedline1;
    // public GameObject speedline2;

    // public Vector3 speedturtle1_initpos;
    // public Vector3 speedturtle1_finalpos;

    // public Vector3 speedturtle2_initpos;
    // public Vector3 speedturtle2_finalpos;

    // public Vector3 speedcursor1_initpos;
    // public Vector3 speedcursor1_finalpos;

    // public Vector3 speedcursor2_initpos;
    // public Vector3 speedcursor2_finalpos;
    // public Vector3 speedline2_finalpos;

    // float alpha1 = 1f;
    // float alpha2 = 1f;

    // public float period = 0.0f;



    void Start()
    {

        sandlinesSpriteRenderer = sandlines.GetComponent<SpriteRenderer>();
        sandlines_initpos = sandlines.transform.position;

        cursor_initpos = sandcursor.transform.position;
        cursor_finalpos = sandcursor.transform.position + new Vector3(13, 0, 0);

        turtleSpriteRenderer = sandturtle.GetComponent<SpriteRenderer>();
        turtle_initpos = sandturtle.transform.position;
        turtle_finalpos1 = sandturtle.transform.position + new Vector3(12, 12, 0);
        turtle_finalpos2 = turtle_finalpos1 + new Vector3(-12, 12, 0);

        minicursor1_finalpos = minicursor1.transform.position + new Vector3(5, 5, 0);
        minicursor1_initpos = minicursor1.transform.position;

        minicursor2_finalpos = minicursor2.transform.position + new Vector3(-5, 5, 0);
        minicursor2_initpos = minicursor2.transform.position;

        // // speed animation start
        // speedturtle1_initpos = speedturtle1.transform.position;
        // speedturtle1_finalpos = speedturtle1_initpos + new Vector3(0, 11, 0);

        // speedturtle2_initpos = speedturtle2.transform.position;
        // speedturtle2_finalpos = speedturtle2_initpos + new Vector3(0, 19, 0);

        // speedcursor1_initpos = speedcursor1.transform.position;
        // speedcursor1_finalpos = speedcursor1_initpos + new Vector3(0, 3.5f, 0);

        // speedcursor2_initpos = speedcursor2.transform.position;
        // speedcursor2_finalpos = speedcursor2_initpos + new Vector3(0, 3.5f, 0);

        
    }

    // Update is called once per frame
    void Update()
    {
        float cursor_step = cursor_speed * Time.deltaTime;

        float turtle_step = turtle_speed * Time.deltaTime;

        if(sandturtle.transform.position.y < turtle_finalpos1.y) {
            alpha += 0.7f * Time.deltaTime;
            turtleSpriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            minicursor1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            minisandline1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            minicursor1.transform.position = Vector3.MoveTowards(minicursor1.transform.position, minicursor1_finalpos, turtle_step);

            if(minicursor1.transform.position == minicursor1_finalpos) {
                minicursor1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                minisandline1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                sandturtle.transform.position = Vector3.MoveTowards(sandturtle.transform.position, turtle_finalpos1, turtle_step);
            }


            if(minicursor1.transform.position.y > minicursor1_finalpos.y - 2) {
                minisandline1.GetComponent<SpriteRenderer>().sprite = sandlineSpriteArray[1];
            }

            if(sandturtle.transform.position.y > turtle_finalpos1.y - 2) {
                
                minicursor2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                minisandline2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                minicursor2.transform.position = Vector3.MoveTowards(minicursor2.transform.position, minicursor2_finalpos, turtle_step);

                alpha = 1f;

                
            }

        } else if (sandturtle.transform.position.y < turtle_finalpos2.y) {
            minisandline1.GetComponent<SpriteRenderer>().sprite = sandlineSpriteArray[0];
            sandturtle.transform.rotation = Quaternion.Euler(0, 0, 45);
            sandturtle.transform.position = Vector3.MoveTowards(sandturtle.transform.position, turtle_finalpos2, turtle_step);

            alpha -= 0.1f * Time.deltaTime;
            turtleSpriteRenderer.color = new Color(1f, 1f, 1f, alpha);

            
            minicursor1.transform.position = minicursor1_initpos;
            minicursor2.transform.position = minicursor2_initpos;
            minicursor2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            minisandline2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            

        } else {
            turtleSpriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            sandturtle.transform.rotation = Quaternion.Euler(0, 0, -45);
            sandturtle.transform.position = turtle_initpos;

        }

        if(sandcursor.transform.position == cursor_finalpos) {
            cursor_direction = -1;
            cursor_speed = 15;
        }
        if(sandcursor.transform.position == cursor_initpos) {
            cursor_direction = 1;
            cursor_speed = 5;
        }

        if(cursor_direction == 1) {
            sandcursor.transform.position = Vector3.MoveTowards(sandcursor.transform.position, cursor_finalpos, cursor_step);
        } else {
            sandcursor.transform.position = Vector3.MoveTowards(sandcursor.transform.position, cursor_initpos, cursor_step);
        }

        if(sandcursor.transform.position.x >= 12) {
            sandlinesSpriteRenderer.sprite = sandlineSpriteArray[2];
            sandlines.transform.position = sandlines_initpos + new Vector3(3.7f, 0, 0);
        } else if(sandcursor.transform.position.x >= 7) {
            sandlinesSpriteRenderer.sprite = sandlineSpriteArray[1];
            sandlines.transform.position = sandlines_initpos + new Vector3(1.8f, 0, 0);
        } else {
            sandlinesSpriteRenderer.sprite = sandlineSpriteArray[0];
            sandlines.transform.position = sandlines_initpos;
        }



        // // speed animation

        // Debug.Log(period);

        // if (period > 5f)
        // {
        //     alpha1 = 1f;
        //     alpha2 = 1f;

        //     speedturtle1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha1);
        //     speedturtle1.transform.position = speedturtle1_initpos;
        //     speedturtle2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha1);
        //     speedturtle2.transform.position = speedturtle2_initpos;

        //     speedcursor1.transform.position = speedcursor1_initpos;
        //     speedcursor1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        //     speedcursor2.transform.position = speedcursor2_initpos;
        //     speedcursor2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        //     speedline1.GetComponent<SpriteRenderer>().color = new Color(255f, 59f, 136f, 0f);
        //     speedline2.GetComponent<SpriteRenderer>().color = new Color(255f, 59f, 136f, 0f);

        //     period = 0f;
                
        // }

        // if(speedturtle1.transform.position.y < speedturtle1_finalpos.y && speedcursor1.transform.position.y == speedcursor1_finalpos.y) {
        //         speedturtle1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha1);
        //         alpha1 -= 0.8f * Time.deltaTime;
        //         speedturtle1.transform.position = Vector3.MoveTowards(speedturtle1.transform.position, speedturtle1_finalpos, turtle_step);
        // }

        // if(speedturtle2.transform.position.y < speedturtle2_finalpos.y && speedcursor2.transform.position.y == speedcursor2_finalpos.y) {
        //     speedturtle2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha2);
        //     alpha2 -= 0.4f * Time.deltaTime;
        //     speedturtle2.transform.position = Vector3.MoveTowards(speedturtle2.transform.position, speedturtle2_finalpos, turtle_step);
        // }

        // if(speedcursor1.transform.position.y < speedcursor1_finalpos.y) {
        //     speedcursor1.transform.position = Vector3.MoveTowards(speedcursor1.transform.position, speedcursor1_finalpos, turtle_step);
        // } else {
        //     speedcursor1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        // }

        // if(speedcursor2.transform.position.y < speedcursor2_finalpos.y) {
        //     speedcursor2.transform.position = Vector3.MoveTowards(speedcursor2.transform.position, speedcursor2_finalpos, turtle_step);
        // } else {
        //     speedcursor2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        // }

        // if(speedcursor2.transform.position.y < speedcursor2_finalpos.y - 2) {
        //     speedline2.GetComponent<SpriteRenderer>().color = new Color(255f, 59f, 136f, 1f);
        //     speedline2.GetComponent<SpriteRenderer>().sprite = sandlineSpriteArray[1];
        // } else if (speedcursor2.transform.position.y < speedcursor2_finalpos.y) {
        //     speedline2.GetComponent<SpriteRenderer>().color = new Color(59f, 103f, 144f, 1f);
        //     speedline2.GetComponent<SpriteRenderer>().sprite = sandlineSpriteArray[0];
        // }

        // period += Time.deltaTime;

        
        
    }



    
}
