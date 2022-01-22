using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class player : MonoBehaviour
{
    private bool firstturn = true;
    public int hod;
    public Sprite cross;
    //public int hod;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickPlayer();
           // Debug.Log("tap");
        }
    }

    void ClickPlayer()
    {
        Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickPoint, Vector2.zero);

        if (hit.collider)
        {
            Transform hitCell = hit.transform;
           // Debug.Log(/*hitCell.GetComponent<cell>().bigcell + "" + */hitCell.GetComponent<cell>().smallcell);
            hod = hitCell.GetComponent<cell>().smallcell;

           // Debug.Log(hod);
            //Debug.Log(GameObject.Find("enemy").GetComponent<enemy>().hodo);
            int bf = GameObject.Find("enemy").GetComponent<enemy>().hodo;
            if (firstturn == false && GameObject.Find("Game").GetComponent<game>().Bgmasson(bf-1) == '-')
            {
               // Debug.Log("notfirstturn");
                if (!hitCell.GetComponent<SpriteRenderer>().sprite && !Turn.turn && !Turn.Pause
                             && hitCell.GetComponent<cell>().bigcell == GameObject.Find("enemy").GetComponent<enemy>().hodo)
                        {
                            hitCell.GetComponent<SpriteRenderer>().sprite = cross;
                            GameObject.Find("Game").GetComponent<game>().zapgamemass('x', hod - 1, hitCell.GetComponent<cell>().bigcell - 1);
                            Turn.turn = true;
                            StartCoroutine(Turn.SetPouse());
                        }
            }
            else if (firstturn == true || GameObject.Find("Game").GetComponent<game>().Bgmasson(bf-1) != '-')
            {
               // Debug.Log("firstturn");
                 if (!hitCell.GetComponent<SpriteRenderer>().sprite && !Turn.turn && !Turn.Pause)
                        {
                            hitCell.GetComponent<SpriteRenderer>().sprite = cross;
                            GameObject.Find("Game").GetComponent<game>().zapgamemass('x', hod - 1, hitCell.GetComponent<cell>().bigcell - 1);
                            Turn.turn = true;
                            firstturn = false;
                            StartCoroutine(Turn.SetPouse());
                        }
            }
              

            
                  
            
        }
    }
}
