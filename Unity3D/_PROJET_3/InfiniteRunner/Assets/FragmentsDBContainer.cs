using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentsDBContainer : MonoBehaviour
{
    [SerializeField] List<FragmentCanvasItem> fragments;
    public List<FragmentCanvasItem> Fragments { get { return fragments; } }
    [SerializeField] bool ShowAllUsersFragments;
    [SerializeField] DBConstant.Time time;
    [SerializeField] GameObject FragmentSpotPrefab;

    private void Start()
    {
        GetAllUsersRandomFragments();
    }

    public void GetAllUsersRandomFragments()
    {
        List<DBFragment> fragment = ServiceLocator.Instance.GetService<UserSessionData>().GetAllUsersRandomFragmentsByCount(10);

        foreach (DBFragment frag in fragment)
        {
            Debug.Log("_______________________________"+frag.TimeId+"--------------");
            GameObject newPanel = Instantiate(FragmentSpotPrefab, transform);
            FragmentCanvasItem cItem = newPanel.GetComponent<FragmentCanvasItem>();
            fragments.Add(cItem);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(frag.Date);
            string date = dateTimeOffset.ToString("dd/MM/yyyy");
            cItem.Title.text = frag.Title;
            cItem.Content.text = frag.Content;
            cItem.Time.text = ("Epoque " + frag.timeName).ToUpper();
            cItem.OrangeInformation.text = frag.username;
            cItem.Img.sprite = cItem.ImgSprite[frag.TimeId-1];
            cItem.TextOverOrangeInformation.text = "Ce fragment a été trouvé le " + date + " par ";
        }

        foreach (FragmentCanvasItem elem in fragments)
        {
            if (elem != fragments[0])
            {
                elem.gameObject.SetActive(false);
            }
            else
            {
                elem.gameObject.SetActive(true);
            }
       
        }
    }

    void GetAllCurrentUserFragments()
    {

    }

    void GetCurrentUserFragmentsAtTime(DBConstant.Time time)
    {

    }

    void GetAllUsersFragmentsAtTime(DBConstant.Time time)
    {

    }

}
