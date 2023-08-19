using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DBConstant;

public class FragmentsDBContainer : MonoBehaviour
{
    [SerializeField] List<FragmentCanvasItem> fragments;
    public List<FragmentCanvasItem> Fragments { get { return fragments; } }
    [SerializeField] bool ShowAllUsersFragments;
    [SerializeField] bool Randomize;
    [SerializeField] DBConstant.Time time;
    [SerializeField] GameObject FragmentSpotPrefab;

    private void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        if (ShowAllUsersFragments)
        {
            if (Randomize)
            {
                if (time == 0) { GetAllUsersRandomFragments(); } else { GetAllUsersRandomFragmentsAtTime(time); }
            }
            else
            {
                if (time == 0) { GetAllUsersFragments();} else { GetAllUsersFragmentsAtTime(time); }
            }
        }
        else
        {
            if (Randomize)
            {
                if (time == 0)
                { GetAllCurrentUserRandomFragments(); } else { GetCurrentUserRandomFragmentsAtTime(time); }
            }

            else
            {
                if (time == 0)
                { GetAllCurrentUserFragments();} else { GetCurrentUserFragmentsAtTime(time); }
            }
        }

    }

    void GetAllUsersFragments()
    {
        List<DBFragment> fragment = ServiceLocator.Instance.GetService<ISessionService>().Query.GetAllUsersFragmentsByCount(10);
        UpdateFragments(fragment);
    }

    public void GetAllUsersRandomFragments()
    {
        List<DBFragment> fragment = ServiceLocator.Instance.GetService<ISessionService>().Query.GetAllUsersRandomFragmentsByCount(10);
        UpdateFragments(fragment);
    }

    void GetAllUsersRandomFragmentsAtTime(DBConstant.Time time)
    {
        List<DBFragment> fragment = ServiceLocator.Instance.GetService<ISessionService>().Query.GetAllUsersRandomFragmentsAtTimeByCount((int)time, 10);
        UpdateFragments(fragment);
    }

    void GetAllUsersFragmentsAtTime(DBConstant.Time time)
    {
        List<DBFragment> fragment = ServiceLocator.Instance.GetService<ISessionService>().Query.GetAllUsersFragmentsAtTimeByCount((int)time, 10);
        UpdateFragments(fragment);
    }

    void GetAllCurrentUserFragments()
    {
       List<DBFragment> fragment = ServiceLocator.Instance.GetService<ISessionService>().Query.GetAllCurrentUserFragments();
       UpdateFragments(fragment);
    }

   

    void GetAllCurrentUserRandomFragments() 
    {
        List<DBFragment> fragment = ServiceLocator.Instance.GetService<ISessionService>().Query.GetAllCurrentUserRandomFragments();
        UpdateFragments(fragment);
    }

    void GetCurrentUserRandomFragmentsAtTime(DBConstant.Time time)
    {
        List<DBFragment> fragment = ServiceLocator.Instance.GetService<ISessionService>().Query.GetAllCurrentUserRandomFragmentsAtTime((int)time);
        UpdateFragments(fragment);
    }


    void GetCurrentUserFragmentsAtTime(DBConstant.Time time)
    {
        List<DBFragment> fragment = ServiceLocator.Instance.GetService<ISessionService>().Query.GetAllCurrentUserFragmentsAtTime((int)time);
        UpdateFragments(fragment);

    }

    private void UpdateFragments(List<DBFragment> fragment)
    {
        if (fragment.Count > 0)
        {
            foreach (DBFragment frag in fragment)
            {
                GameObject newPanel = Instantiate(FragmentSpotPrefab, transform);
                FragmentCanvasItem cItem = newPanel.GetComponent<FragmentCanvasItem>();
                fragments.Add(cItem);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(frag.Date);
                string date = dateTimeOffset.ToString("dd/MM/yyyy");
                cItem.Title.text = frag.Title;
                cItem.Content.text = frag.Content;
                cItem.Time.text = ("Epoque " + frag.timeName).ToUpper();

                if (ShowAllUsersFragments)
                {
                    cItem.OrangeInformation.text = frag.username;
                    cItem.Img.sprite = cItem.ImgSprite[frag.TimeId - 1];
                    cItem.TextOverOrangeInformation.text = "Ce fragment a été trouvé le " + date + " par ";
                }
                else
                {
                    cItem.OrangeInformation.text = date;
                    cItem.Img.sprite = cItem.ImgSprite[frag.TimeId - 1];
                    cItem.TextOverOrangeInformation.text = "Vous avez trouvé ce fragment le ";
                }

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
        else
        {
            GameObject newPanel = Instantiate(FragmentSpotPrefab, transform);
            FragmentCanvasItem cItem = newPanel.GetComponent<FragmentCanvasItem>();
            fragments.Add(cItem);
        }
    }

}
