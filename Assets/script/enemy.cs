using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int hodo;
    public Sprite round;
    private List<Transform> freeCells;
    // Use this for initialization
    void Start ()
    {
        InitArrayCells();

    }
	
	// Update is called once per frame
	void Update ()
    {
        //transform tempObj = transform.GetComponent<player>().hod;
        if (Turn.turn && !Turn.Pause)
            TurnEnemy();
    }

    void InitArrayCells()
    {
        freeCells = new List<Transform>();
        GameObject[] tempArr = GameObject.FindGameObjectsWithTag("cell");

        foreach (GameObject obj in tempArr)
            freeCells.Add(obj.GetComponent<Transform>());
    }


    void TurnEnemy()
    {
        while (Turn.turn)
        {
            if (GameObject.Find("Game").GetComponent<game>().WinGame() == true)
                break;
            int bufer;
            if (GameObject.Find("Game").GetComponent<game>().Bgmasson(GameObject.Find("player").GetComponent<player>().hod - 1) == '-')
            {
                int randomIndex = GameObject.Find("Game").GetComponent<game>().FindTurn(GameObject.Find("player").GetComponent<player>().hod - 1);
                //int randomIndex = (int)Random.Range(1, 10);
                bufer = (GameObject.Find("player").GetComponent<player>().hod - 1) * 9 + randomIndex;   //(int)Random.Range(0, freeCells.Count - 1);
                if (!freeCells[bufer].GetComponent<SpriteRenderer>().sprite)
                {
                    
                    freeCells[bufer].GetComponent<SpriteRenderer>().sprite = round;
                    hodo = randomIndex + 1;
                    Debug.Log(hodo);
                    GameObject.Find("Game").GetComponent<game>().zapgamemass('o', hodo - 1, GameObject.Find("player").GetComponent<player>().hod - 1);
                    

                    Turn.turn = false;
                    StartCoroutine(Turn.SetPouse());

                } 
            }
            else
            {
                int bgcell = GameObject.Find("Game").GetComponent<game>().FindTurnBIGcell();
                int randomIndex = GameObject.Find("Game").GetComponent<game>().FindTurn(bgcell);
                bufer = bgcell * 9 + randomIndex;
                //Debug.Log(bufer + "буфер");
                //Debug.Log(randomIndex + "индекс");
                //int ggg = randomIndex / 9;
                if (!freeCells[bufer].GetComponent<SpriteRenderer>().sprite/* && GameObject.Find("Game").GetComponent<game>().Bgmasson(ggg) == '-'*/)
                {
                    
                    freeCells[bufer].GetComponent<SpriteRenderer>().sprite = round;
                    hodo = randomIndex + 1;
                    Debug.Log(hodo);
                    GameObject.Find("Game").GetComponent<game>().zapgamemass('o', hodo-1, bgcell);
                    //Debug.Log(ggg);
                    
                    Turn.turn = false;
                    StartCoroutine(Turn.SetPouse());

                }
            }
            
            
            //freeCells.RemoveAt(randomIndex);
            /*int randomIndex = (int)Random.Range(0, freeCells.Count - 1);
            if (!freeCells[randomIndex].GetComponent<SpriteRenderer>().sprite)
            {
                freeCells[randomIndex].GetComponent<SpriteRenderer>().sprite = round;
                Turn.turn = false;
                StartCoroutine(Turn.SetPouse());

            }

            freeCells.RemoveAt(randomIndex);*/
        }
    }
}

public static class Turn
{
    public static bool turn = false;
    static bool pause = false;

    public static bool Pause
    {
        get { return pause; }
    }

    public static IEnumerator SetPouse()
    {
        pause = true;
        yield return new WaitForSeconds(0.1f);
        pause = false;
    }
}