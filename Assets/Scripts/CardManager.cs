using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{

    public static CardManager Instance;
    //public static int gameSize = 2;
    public static int gameSizeX = 2;
    public static int gameSizeY = 2;
    [SerializeField]private GameObject prefab;
    [SerializeField]private GameObject cardList;
    [SerializeField]private Sprite cardBack;
    // all possible sprite
    [SerializeField]private Sprite[] sprites;
    // list of card
    private CardMatch[] cards;

    //we place card on this panel
    [SerializeField]private GameObject panel;
    [SerializeField]private GameObject info;
    [SerializeField]private CardMatch spritePreload;
    [SerializeField]private Text sizeLabel;
    // [SerializeField]private Slider sizeSlider;
    [SerializeField] private Slider sizeSliderX;
    [SerializeField] private Slider sizeSliderY;

    private int spriteSelected;
    private int cardSelected;
    private int cardLeft;
    private bool gameStart;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //the purpose is to allow preloading of panel, so that it does not lag when it loads
        gameStart = false;
        panel.SetActive(false);
    }
    private void PreloadCardImage()
    {
        for (int i = 0; i < sprites.Length; i++)
            spritePreload.SpriteID = i;
        spritePreload.gameObject.SetActive(false);
    }
    public void StartCardGame()
    {
        if (gameStart) return;
        gameStart = true;
        panel.SetActive(true);
        info.SetActive(false);
        SetGamePanel();
        cardSelected = spriteSelected = -1;
        cardLeft = cards.Length;
        SpriteCardAllocation();
        StartCoroutine(HideFace());
    }
    private void SetGamePanel(){

        int isOdd = gameSizeX % 2 ;

        cards = new CardMatch[gameSizeX * gameSizeX - isOdd];
        foreach (Transform child in cardList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        RectTransform panelsize = panel.transform.GetComponent(typeof(RectTransform)) as RectTransform;
        float row_size = panelsize.sizeDelta.x;
        float col_size = panelsize.sizeDelta.y;
        float scale = 1.0f/ gameSizeX;
        float xInc = row_size/ gameSizeX;
        float yInc = col_size/ gameSizeX;
        float curX = -xInc * (float)(gameSizeX / 2);
        float curY = -yInc * (float)(gameSizeX / 2);

        if(isOdd == 0) {
            curX += xInc / 2;
            curY += yInc / 2;
        }
        float initialX = curX;
        for (int i = 0; i < gameSizeX; i++)
        {
            curX = initialX;
            for (int j = 0; j < gameSizeX; j++)
            {
                GameObject c;
                if (isOdd == 1 && i == (gameSizeX - 1) && j == (gameSizeX - 1))
                {
                    int index = gameSizeX / 2 * gameSizeX + gameSizeX / 2;
                    c = cards[index].gameObject;
                }
                else
                {
                    c = Instantiate(prefab);
                    c.transform.parent = cardList.transform;

                    int index = i * gameSizeX + j;
                    cards[index] = c.GetComponent<CardMatch>();
                    cards[index].ID = index;
                    c.transform.localScale = new Vector3(scale, scale);
                }
                c.transform.localPosition = new Vector3(curX, curY, 0);
                curX += xInc;

            }
            curY += yInc;
        }

    }
    void ResetFace()
    {
        for (int i = 0; i < gameSizeX; i++)
            cards[i].ResetRotation();
    }

    IEnumerator HideFace()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < cards.Length; i++)
            cards[i].Flip();
        yield return new WaitForSeconds(0.5f);
    }

    private void SpriteCardAllocation()
    {
        int i, j;
        int[] selectedID = new int[cards.Length / 2];
        for (i = 0; i < cards.Length/2; i++)
        {
            int value = Random.Range(0, sprites.Length - 1);
            for (j = i; j > 0; j--)
            {
                if (selectedID[j - 1] == value)
                    value = (value + 1) % sprites.Length;
            }
            selectedID[i] = value;
        }

        for (i = 0; i < cards.Length; i++)
        {
            cards[i].Active();
            cards[i].SpriteID = -1;
            cards[i].ResetRotation();
        }
        for (i = 0; i < cards.Length / 2; i++)
            for (j = 0; j < 2; j++)
            {
                int value = Random.Range(0, cards.Length - 1);
                while (cards[value].SpriteID != -1)
                    value = (value + 1) % cards.Length;

                cards[value].SpriteID = selectedID[i];
            }

    }
    public void SetGameSize() {
        gameSizeX = (int)sizeSliderX.value;
        gameSizeY = (int)sizeSliderY.value;
        sizeLabel.text = gameSizeX + " X " + gameSizeY;
        /* gameSize = (int)sizeSlider.value;
         sizeLabel.text = gameSize + " X " + gameSize;*/
    }
    public Sprite GetSprite(int spriteId)
    {
        return sprites[spriteId];
    }
    public Sprite CardBack()
    {
        return cardBack;
    }
    public bool canClick()
    {
        if (!gameStart)
            return false;
        return true;
    }
    public void cardClicked(int spriteId, int cardId)
    {
        if (spriteSelected == -1)
        {
            spriteSelected = spriteId;
            cardSelected = cardId;
        }
        else
        {
            if (spriteSelected == spriteId)
            {
                cards[cardSelected].Inactive();
                cards[cardId].Inactive();
                cardLeft -= 2;
                CheckGameWin();
            }
            else
            {
                cards[cardSelected].Flip();
                cards[cardId].Flip();
                GameController.Instance.wrongCount += 1; //LOSS LEVEL
                
            }
            cardSelected = spriteSelected = -1;
        }
    }
    private void CheckGameWin()
    {
        //WIN LEVEL
        if (cardLeft == 0)
        {
            UIManager.Instance.LevelWin();
            AudioPlayer.Instance.PlayAudio(1);
        }
    }
    private void EndGame()
    {
        gameStart = false;
        panel.SetActive(false);
    }
    public void Return()
    {
        EndGame();
    }
    public void DisplayInfo(bool i)
    {
        info.SetActive(i);
    }

}
