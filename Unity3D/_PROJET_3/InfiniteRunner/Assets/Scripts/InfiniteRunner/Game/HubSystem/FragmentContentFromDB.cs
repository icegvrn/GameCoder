using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet de param�trer le type de contenu de fragment qu'on veut afficher et d'aller les chercher dans la BDD
/// </summary>
public class FragmentContentFromDB : MonoBehaviour
{
    [Header("Panel � instancier")]
    [SerializeField] GameObject FragmentSpotPrefab;

    [Header("Configuration du spot")]
    [SerializeField] bool ShowAllUsersFragments;
    [SerializeField] bool Randomize;
    [SerializeField] DBConstant.Time time;

    // Les panels actuellement instanci�s
    [SerializeField] private List<FragmentCanvasDecomposer> fragmentsPanels;
    public List<FragmentCanvasDecomposer> FragmentsPanels { get { return fragmentsPanels; } }

    //Database
    private SQLiteSessionDataQuery db;

    private void Start()
    {
        InitDataBase();
        LoadData();
    }

    private void InitDataBase()
    {
        db = ServiceLocator.Instance.GetService<ISessionService>().Query;
    }
   
    public void LoadData()
    {
        List<DBFragment> fragment = GetFragment();
        UpdateFragments(fragment);
    }

    /// <summary>
    /// Choisi la requ�te � effectuer en fonction du param�trage du spot
    /// </summary>
    private List<DBFragment> GetFragment()
    {
        List<DBFragment> fragment;

        if (ShowAllUsersFragments)
        {
            if (Randomize)
            {
                if (time == 0) { fragment = db.GetAllUsersRandomFragmentsByCount(10); } else { fragment = db.GetAllUsersRandomFragmentsAtTimeByCount((int)time, 10); }
            }
            else
            {
                if (time == 0) { fragment = db.GetAllUsersFragmentsByCount(10); } else { fragment = db.GetAllUsersFragmentsAtTimeByCount((int)time, 10); }
            }
        }
        else
        {
            if (Randomize)
            {
                if (time == 0)
                { fragment = db.GetAllCurrentUserRandomFragments(); }
                else { fragment = db.GetAllCurrentUserRandomFragmentsAtTime((int)time); }
            }

            else
            {
                if (time == 0)
                { fragment = db.GetAllCurrentUserFragments(); }
                else { fragment = db.GetAllCurrentUserFragmentsAtTime((int)time); }
            }
        }
        return fragment;
    }

    /// <summary>
    /// Appel la m�thode de cr�ation et de remplissage d'un panel pour chaque fragment trouv�.
    /// </summary>
    /// <param name="fragments"></param>
    private void UpdateFragments(List<DBFragment> fragments)
    {
        if (fragments.Count > 0)
        {
            CreateFragmentPanels(fragments);
            ShowFirstFragmentPanel();
        }

        else
        {
            CreateEmptyFragmentPanel();
        }
    }

    /// <summary>
    /// Instancie un panel dans lequel mettre le continue du fragment. Ce contenu est ensuite appel� par ConfigureFragmentPanel.
    /// </summary>
    /// <param name="fragments"></param>
    private void CreateFragmentPanels(List<DBFragment> fragments)
    {
        foreach (DBFragment frag in fragments)
        {
            FragmentCanvasDecomposer panel = CreateEmptyFragmentPanel();
            ConfigureFragmentPanel(panel, frag);
        }
    }

    /// <summary>
    /// M�thode qui vient modifier le contenu du canvas en remplissant les champs avec les informations de la db.
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="frag"></param>
    private void ConfigureFragmentPanel(FragmentCanvasDecomposer panel, DBFragment frag)
    {
        panel.Title.text = frag.Title;
        panel.Content.text = frag.Content;
        panel.Time.text = ("Epoque " + frag.timeName).ToUpper();

        if (ShowAllUsersFragments)
        {
            panel.OrangeInformation.text = frag.username;
            panel.TextOverOrangeInformation.text = "Ce fragment a �t� trouv� le " + GetDate(frag.Date) + " par ";
        }
        else
        {
            panel.OrangeInformation.text = GetDate(frag.Date);
            panel.TextOverOrangeInformation.text = "Vous avez trouv� ce fragment le ";
        }
        panel.Img.sprite = panel.ImgSprite[frag.TimeId - 1];
    }

    private void ShowFirstFragmentPanel()
    {
        foreach (var panel in fragmentsPanels)
        {
            panel.gameObject.SetActive(panel == fragmentsPanels[0] ? true : false);
        }
    }

    /// <summary>
    /// Cr�e un panel par d�faut.
    /// </summary>
    private FragmentCanvasDecomposer CreateEmptyFragmentPanel()
    {
        GameObject fragmentItem = Instantiate(FragmentSpotPrefab, transform);
        FragmentCanvasDecomposer panel = fragmentItem.GetComponent<FragmentCanvasDecomposer>();
        fragmentsPanels.Add(panel);
        return panel;
    }

    /// <summary>
    /// Converti la date Unix contenue dans la db en date FR
    /// </summary>
    /// <param name="frag_date"></param>
    /// <returns></returns>
    private string GetDate(int frag_date)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(frag_date);
        string date = dateTimeOffset.ToString("dd/MM/yyyy");
        return date;
    }



}