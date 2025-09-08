using UnityEngine;

public class UIManager : BaseManager<UIManager>
{
    public GamePanel gamePanel;
    public MenuPanel menuPanel;
    public LoosePanel loosePanel;


    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        //menuPanel.gameObject.SetActive(true);
        //loosePanel.gameObject.SetActive(false);
        //gamePanel.gameObject.SetActive(false);
    }

    

}
