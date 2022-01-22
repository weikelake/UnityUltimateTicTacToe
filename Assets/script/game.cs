using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game : MonoBehaviour
{
    public Transform cell;                  //Префаб-триггер ячейки
    public Transform cellsParent;           //Объект-папка для ячеек
    private Transform[] sprites;          //Массив ячеек поля

    public Transform Bcell;                  //Префаб-триггер ячейки
    public Transform BcellsParent;           //Объект-папка для ячеек
    private Transform[] Bsprites = new Transform[9];          //Массив ячеек поля

    public Sprite krest;
    public Sprite nol;
    public Sprite square;
    //Bsprites = new Transform[9];

    private bool GameWin;

    //private int[] massII = new int[9];
    private char[,] gamemass = new char[9, 9];
    //public char[] Biggamemass = new char[10];
    private char[] BGmass = new char[9];
    private int[] massTurn = new int[9];
    private int[] massTurnBIG = new int[9];
    private float x;
    private float y;
    //private float X;
    //private float Y;
    
    private List<Transform> freeBCells;

    public bool WinGame()
    {
        return GameWin;
    }

    //поиск поля в которое будем ходить
    public int FindTurnBIGcell()
    {
        for (int i = 0; i < massTurnBIG.Length; i++)
        {
            //если клетка свободна
            if (BGmass[i] == '-')
                massTurnBIG[i] = 100;
            else
                massTurnBIG[i] = -50;
        }
        for (int i = 0; i < massTurnBIG.Length; i++)
        {
            //свитч на возможные пары
            switch (soCloseBIG(i))
            {
                case 1: //выиграет х
                    massTurnBIG[i] += 10;
                    break;
                case 2://выиграет о
                    massTurnBIG[i] += 30;
                    break;
            }
        }
        Debug.Log("" +
            massTurnBIG[0] + ' ' + massTurnBIG[1] + ' ' + massTurnBIG[2] + '\n' +
            massTurnBIG[3] + ' ' + massTurnBIG[4] + ' ' + massTurnBIG[5] + '\n' +
            massTurnBIG[6] + ' ' + massTurnBIG[7] + ' ' + massTurnBIG[8]);

        // ПОИСК КЛЕТКИ С МАКСИМАЛЬНЫМ КОЛИЧЕСТВОМ ОЧКОВ
        int bufer = 0;
        int index = 0;
        for (int i = 0; i < massTurnBIG.Length; i++)
        {
            if (massTurnBIG[i] > bufer)
            {
                bufer = massTurnBIG[i];
                index = i;
            }
        }
        bool check = false;
        //выбор рандомной клетки (из тех, что с самым большим количеством очков)
        while (check == false)
        {
            int i = (int)Random.Range(0, 9);
            if (massTurnBIG[i] == bufer)
            {
                check = true;
                index = i;
            }
        }
        //int count;
        return index; //возвращает индекс клетки, в которую будет сделан ход
    }

    //поиск возможной пары (ход сделан в выигранное поле)
    private int soCloseBIG(int bigcell)
    {
        for (int i = 0; i < massTurn.Length; i++)
        {
            //если клетка свободна
            if (gamemass[bigcell, i] == '-')
            {
                gamemass[bigcell, i] = 'x'; //временно присваиваем клетке х
                if (win('x', bigcell)) //если поле выигрывает
                {
                    gamemass[bigcell, i] = '-'; //обратно присваиваем клетке -
                    return 1;
                }
                gamemass[bigcell, i] = 'o';//временно присваиваем клетке о
                if (win('o', bigcell))//если поле выигрывает
                {
                    gamemass[bigcell, i] = '-'; // обратно присваиваем клетке -
                    return 2;
                }
                gamemass[bigcell, i] = '-'; // обратно присваиваем клетке -
            }
        }
        return 0;
    }
    //поиск клетки, в которую будем ходить
    public int FindTurn(int bigcell)
    {
        for (int i = 0; i < massTurn.Length; i++)
        {
                //если клетка свободна
                if (gamemass[bigcell, i] == '-')
                    massTurn[i] = 100; 
                else
                    massTurn[i] = -50;
        }
       
        for (int i = 0; i < massTurn.Length; i++)
        {
            //если поле, соответствующее клетке выиграно
            if (BGmass[i] != '-')
            {
                massTurn[i] += -29;
            }
        }
        for (int i = 0; i < massTurn.Length; i++)
        {
            //если поле и клетка свободны
            if (BGmass[i] == '-' && gamemass[bigcell, i] == '-')
            {
                //если есть пара крестов в следующем поле
                if (soClosefuturekrest(i))
                    massTurn[i] += -20;
                //если есть пара нолей в следующем поле
                if (soClosefuturenol(i))
                    massTurn[i] += -15;
            }
        }


        soClose(bigcell); //функция на определение пар в текущем поле

        Debug.Log(""+
            massTurn[0] + ' ' +  massTurn[1] + ' ' + massTurn[2] + '\n' +
            massTurn[3] + ' ' + massTurn[4] + ' ' + massTurn[5] + '\n' +
            massTurn[6] + ' ' + massTurn[7] + ' ' + massTurn[8]);

        // ПОИСК КЛЕТКИ С МАКСИМАЛЬНЫМ КОЛИЧЕСТВОМ ОЧКОВ
        int bufer = 0;
        int index = 0;
        for (int i = 0; i < massTurn.Length; i++)
        {
            if (massTurn[i] > bufer)
            {
                bufer = massTurn[i];
                index = i;
            }
        }
        bool check = false;
        //выбор рандомной клетки (из тех, что с самым большим количеством очков)
        while (check == false)
        {
            int i = (int)Random.Range(0, 9);
            if (massTurn[i] == bufer)
            {
                check = true;
                index = i;
            }
        }
        //int count;
        return index; //возвращает индекс клетки, в которую будет сделан ход
   }

    //поиск пары в текущей клетке
   private void soClose(int bigcell)
    {
        for(int i=0; i<massTurn.Length; i++) // массив по всем клеткам в поле 
        {
            if (gamemass[bigcell,i] == '-') // если клетка пуста
            {
                gamemass[bigcell, i] = 'x'; //временно присваиваем клетке х
                if (win('x', bigcell)) //если поле выигрывает
                    massTurn[i] += 10; //+10 к удаче
                gamemass[bigcell, i] = 'o';//временно присваиваем клетке о
                if (win('o', bigcell)) //если поле выигрывает
                    massTurn[i] += 30; //+30 к здоровью
                gamemass[bigcell, i] = '-';
            }
        }
    }
    
    //проверка возможных следующих полей на парные нолики

    private bool soClosefuturenol(int bigcell)
    {
        bool check = false;
        for (int i = 0; i < massTurn.Length; i++)
        {
            if (gamemass[bigcell, i] == '-')// если клетка пуста
            {
                gamemass[bigcell, i] = 'o';//временно присваиваем клетке о
                if (win('o', bigcell)) //если выигрывает, то гг
                    check = true;
                gamemass[bigcell, i] = '-';
            }
        }
        return check;
    }
    //проверка возможных следующих полей на парные нолики
    private bool soClosefuturekrest(int bigcell)
    {
        bool check = false;
        for (int i = 0; i < massTurn.Length; i++)
        {
            if (gamemass[bigcell, i] == '-')// если клетка пуста
            {
                gamemass[bigcell, i] = 'x';//временно присваиваем клетке х
                if (win('x', bigcell)) //если выигрывает, то гг
                    check = true;      
                gamemass[bigcell, i] = '-';
            }
        }
        return check;
    }
    public char Bgmasson(int numbermass) //вообще бесполезная фигня, но без неё не работает
    {
        return BGmass[numbermass];
    }

    public void gmmasson(int one, int two, char player) //вообще бесполезная фигня, но без неё не работает
    {
        gamemass[one, two] = player;
    }

    
    void InitArrayCellsgame()
    {
        freeBCells = new List<Transform>();
        GameObject[] tempArr = GameObject.FindGameObjectsWithTag("bigcell");

        foreach (GameObject obj in tempArr)
            freeBCells.Add(obj.GetComponent<Transform>());
    }

    void Start()
    {
        for (int i = 0; i <= 8; i++)
        {
            for (int j = 0; j <= 8; j++)
            {

                gamemass[i, j] = '-';
                //if (i==0)
                //{
                //    gamemass[i, 0] = '-';
                //    gamemass[i, 1] = 'o';
                //    gamemass[i, 2] = 'o';
                //    gamemass[i, 3] = 'o';
                //    gamemass[i, 4] = 'x';
                //    gamemass[i, 5] = 'x';
                //    gamemass[i, 6] = 'x';
                //    gamemass[i, 7] = 'x';
                //    gamemass[i, 8] = 'o';
                //}
            }
        }
        for (int i = 0; i < BGmass.Length; i++)
        {
            BGmass[i] = '-';
        }
        //Debug.Log(" " +
        //   Biggamemass[0] + ' ' + Biggamemass[1] + ' ' + Biggamemass[2] + ' ' +
        //   Biggamemass[3] + ' ' + Biggamemass[4] + ' ' + Biggamemass[5] + ' ' +
        //   Biggamemass[6] + ' ' + Biggamemass[7] + ' ' + Biggamemass[8]);
        InitArrayCellsgame();
        GameWin = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void gameover()
    {
        for (int i = 0; i < 9; i++)
        {
            freeBCells[i].GetComponent<Transform>().position = new Vector3(freeBCells[i].transform.position.x, freeBCells[i].transform.position.y, -1);
        }
    }
    public void zapgamemass(char player, int smallcell, int bigcell)
    {
        
        //gamemass[bigcell, smallcell] = player;
        gmmasson(bigcell, smallcell, player);
        if (win(player, bigcell))
        {
            //Debug.Log(bigcell);
            BGmass[bigcell] = player;
            
            if (player == 'x')
            {
                freeBCells[bigcell].GetComponent<SpriteRenderer>().sprite = krest;
                freeBCells[bigcell].GetComponent<Transform>().position = new Vector3(freeBCells[bigcell].transform.position.x, freeBCells[bigcell].transform.position.y, -1);
                if (bigwin(player))
                {
                    Debug.Log("Выиграл Х");
                    gameover();
                    GameWin = true;
                }

            }
            else
            {
                freeBCells[bigcell].GetComponent<SpriteRenderer>().sprite = nol;
                freeBCells[bigcell].GetComponent<Transform>().position = new Vector3(freeBCells[bigcell].transform.position.x, freeBCells[bigcell].transform.position.y, -1);
                if (bigwin(player))
                {
                    Debug.Log("Выиграл О");
                    gameover();
                    GameWin = true;
                }
            }
           /* Debug.Log(" " +
           Biggamemass[0] + ' ' + Biggamemass[1] + ' ' + Biggamemass[2] + ' ' +
           Biggamemass[3] + ' ' + Biggamemass[4] + ' ' + Biggamemass[5] + ' ' +
           Biggamemass[6] + ' ' + Biggamemass[7] + ' ' + Biggamemass[8]);*/
        }
        else if (nich(bigcell))
        {
            BGmass[bigcell] = 's';
            freeBCells[bigcell].GetComponent<SpriteRenderer>().sprite = square;
            freeBCells[bigcell].GetComponent<Transform>().position = new Vector3(freeBCells[bigcell].transform.position.x, freeBCells[bigcell].transform.position.y, -1);
        }

        if (bignich() && (bigwin(player) == false))
        {
            Debug.Log("Ничья");
            gameover();
            GameWin = true;
        }
        //Debug.Log("\n" +
        //    gamemass[bigcell, 0] + ' ' + gamemass[bigcell, 1] + ' ' + gamemass[bigcell, 2] + ' ' + "\n" +
        //   gamemass[bigcell, 3] + ' ' + gamemass[bigcell, 4] + ' ' + gamemass[bigcell, 5] + ' ' + "\n" +
        //    gamemass[bigcell, 6] + ' ' + gamemass[bigcell, 7] + ' ' + gamemass[bigcell, 8]);
    }

    void InitField()
    {
        float onex = -3.5f;
        float oney = 4.48f;

        int arrbigcell = 0;
        for (int j = 1; j <= 9; j++)
        {
            int arrsmallcell = 0;//, arrCell = 0;
            sprites = new Transform[9];
            x = onex;
            y = oney;
            for (int i = 1; i <= 9; i++)
            {
                Transform tempObj = (Transform)Instantiate(cell, new Vector3(x, y, 0), Quaternion.identity);
                sprites[arrsmallcell] = tempObj;
                tempObj.transform.SetParent(cellsParent.transform);
                tempObj.GetComponent<cell>().smallcell = arrsmallcell + 1;
                //tempObj.GetComponent<cell>().posColl = arrColl;
                tempObj.GetComponent<cell>().bigcell = arrbigcell + 1;
                arrsmallcell++;
                x += 1f;

                if (i % 3 == 0)
                {
                    x = onex;
                    y -= 1f;
                }
            }
            onex = onex + 3f;
            arrbigcell++;
            if (j % 3 == 0)
            {
                oney = oney - 3f;
                onex = onex - 9f;
            }
        }
    }
    void InitFieldBIG()
    {
        float X = -2.46f;
        float Y = 3.49f;
        // int arrcell = 0;

        for (int i = 1; i <= 9; i++)
        {
            Transform tempOb = (Transform)Instantiate(Bcell, new Vector3(X, Y, 1), Quaternion.identity);
            Bsprites[i - 1] = tempOb;
            tempOb.transform.SetParent(BcellsParent.transform);
            tempOb.GetComponent<bigcell>().BIGCELL = i;
            //tempObj.GetComponent<BIGcell>().BIGCELL = arrcell + 1;
            //tempObj.GetComponent<cell>().bigcell = arrbigcell + 1;
            //arrcell++;
            X += 3f;

            if (i % 3 == 0)
            {
                X = -2.46f;
                Y -= 3f;
            }
        }
    }
    void Awake()
    {
        InitField();
        InitFieldBIG();
    }

    //проверка на выигрыш
    bool win(char xod //x или о
        , int bigcell)//номер поля
    {
        //массив выигрышных комбинаций
        int[,] wins = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };
        for (int i = 0; i < 8; i++)
        {
            int count = 0;
            for (int j = 0; j < 3; j++)
            {
                if (gamemass[bigcell, wins[i, j]] == xod) // игровой массив
                    count++;
            }
            if (count == 3) //если три одинаковых символа в ряд
                return true;
        }
        return false;
    }

    bool bigwin(char xod)
    {
        int[,] wins = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };
        for (int i = 0; i < 8; i++)
        {
            int count = 0;
            for (int j = 0; j < 3; j++)
            {
                if (BGmass[wins[i, j]] == xod)
                    count++;
            }
            if (count == 3)
                return true;
        }
        return false;
    }
    bool bignich()
    {
        int count = 0;
        for (int i = 0; i < 9; i++)
        {
           if (BGmass[i] != '-')
            {
                count++;
            }
        }
        if (count == 9)
            return true;
        else
            return false;  
    }
   bool nich(int cell)
    {
        int count = 0;
        for (int i = 0; i < 9; i++)
        {
            if (gamemass[cell, i] != '-')
                count++;
        }
        if (count == 9)
            return true;
        else
            return false;
    }
}
