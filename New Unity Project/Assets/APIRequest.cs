using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class APIRequest : MonoBehaviour
{
    private const string REQUEST_URL = "https://api.pubg.com/tournaments";
    private const string API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJq" +
        "dGkiOiI0MWQ1ZjkyMC1jNGE2LTAxMzgtOTQ2OS0xOTdlNDVlMjM0OWUiLCJpc3MiOiJn" +
        "YW1lbG9ja2VyIiwiaWF0IjoxNTk3ODgxNzA0LCJwdWIiOiJibHVlaG9sZSIsInRpdGxl" +
        "IjoicHViZyIsImFwcCI6InNoYWRpYXRvci05Mi1nIn0.Dx6XjHy1BUJpbsVz9RHUw58E" +
        "_wNJqSUYPC5gYm5D1H0";

    private string jsonData;
    [SerializeField]
    private Text id, date;
    [SerializeField]
    private GameObject ListItemPrefab;
    private GameObject newListItem;
    private GameObject[] listItems = null;

    public void OnRequest()
    {
        if(listItems == null)
        StartCoroutine(OnResponse());
    }

    public IEnumerator OnResponse()
    {
        UnityWebRequest request = UnityWebRequest.Get(REQUEST_URL);
        request.SetRequestHeader("Authorization", "Bearer " + API_KEY);
        request.SetRequestHeader("Accept", "application/vnd.api+json");

        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            string jsonString = request.downloadHandler.text;
            JSONObject jObject = (JSONObject)JSON.Parse(jsonString);

            if (jObject == null)
            {
                //print("No Data");
            }
            else
            {
                ListOfTournaments(jObject);
            }

        }

    }

    public void ListOfTournaments(JSONObject obj)
    {
        for (int i = 0; i < obj[0].Count; i++)
        {
            listItems = new GameObject[obj[0].Count];

            newListItem = Instantiate(ListItemPrefab) as GameObject;
            listItems[i] = newListItem;
            newListItem.SetActive(true);
            newListItem.transform.SetParent(ListItemPrefab.transform.parent, false);

            newListItem.transform.Find("id").GetComponent<Text>().text += "\n" + obj[0][i][1].ToString();
            newListItem.transform.Find("date").GetComponent<Text>().text += "\n" + obj[0][i][2][0].ToString();
            float x, y;
            x = newListItem.GetComponent<RectTransform>().anchoredPosition.x;
            y = newListItem.GetComponent<RectTransform>().anchoredPosition.y;
            Vector2 newPos = new Vector2(x, y - 65 * i);
            newListItem.GetComponent<RectTransform>().anchoredPosition = newPos;

        }

    }

}
