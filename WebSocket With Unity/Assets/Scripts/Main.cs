using UnityEngine;
using WebSocketSharp;

public class Main : MonoBehaviour
{
    [SerializeField] private GameObject NObj;
    [SerializeField] private GameObject RObj;

    [SerializeField] private RectTransform StartPanl;
    [SerializeField] private RectTransform BackButton;

    private WebSocket ws;
    private string lastMsg;

    private bool sender = false;

    void Start()
    {
        ws = new WebSocket("ws://pepper-sincere-cap.glitch.me//ws");
        ws.OnMessage += Ws_OnMessage;
        ws.OnOpen += Ws_OnOpen;
        //ws.Connect();
    }

    private void Ws_OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("Web Socket connected");
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        lastMsg = e.Data;
    }

    private void OnDestroy()
    {
        if (ws != null) ws.Close();
    }

    void Update()
    {
        if (lastMsg != null && !sender)
        {
            Debug.Log(lastMsg);
            var tokens = lastMsg.Split('|');
            float x = float.Parse(tokens[0]);
            float z = float.Parse(tokens[1]);
            RObj.transform.position = new Vector3(x, 0.5f, z);
            lastMsg = null;
        }

        if (Input.GetMouseButtonDown(0) && sender)
        {
            //cast a ray to the plane
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Ray, out hit))
            {
                //instanciate a brush
                NObj.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                string ObjPos = NObj.transform.position.x + "|" + NObj.transform.position.z;
                //Debug.Log(ObjPos);
                ws.Send(ObjPos);
            }

        }

    }

    public void OnNormalButton()
    {
        sender = true;
        ws.Connect();
        BackButton.gameObject.SetActive(true);
        NObj.SetActive(true);
        StartPanl.gameObject.SetActive(false);
    }

    public void OnReflectButton()
    {
        ws.Connect();
        BackButton.gameObject.SetActive(true);
        RObj.SetActive(true);
        StartPanl.gameObject.SetActive(false);
    }

    public void Back()
    {
        sender = false;
        BackButton.gameObject.SetActive(false);
        NObj.SetActive(false);
        NObj.transform.position = new Vector3(0, 0.5f, 0);
        RObj.SetActive(false);
        RObj.transform.position = new Vector3(0, 0.5f, 0);
        StartPanl.gameObject.SetActive(true);
        if (ws != null) ws.Close();
    }

}
