using System.Collections.Generic;
using UnityEngine;


public class ClassementFromDB : MonoBehaviour
{
   [SerializeField] List<ClassementItem> items;
   [SerializeField] ClassementItem currentUserItem;
    private ISessionService sessionService;


    void Start()
    {
        sessionService = ServiceLocator.Instance.GetService<ISessionService>();
        GetUsersRanking();   
    }

    private void GetUsersRanking()
    {
        List<DBRank> ranking = sessionService.Query.GetAllUsersRanking();

        int i = 0;

        foreach (DBRank rank in ranking)
        {
            if (i < items.Count)
            {
                items[i].Name.text = rank.Username;
                items[i].Fragments.text = rank.TotalFragments.ToString();

            }

            if (IsCurrentUserRanking(rank))
            {
                HighlightCurrentUserRank(rank, i);
            }

            i++;
        }
    }

    bool IsCurrentUserRanking(DBRank rank)
    {
        return rank.Username == sessionService.UserData.CurrentUser;
    }

    /// <summary>
    /// Met le rank de l'utilisateur actuel en bas du classement et en couleur.
    /// </summary>
    /// <param name="rank"></param>
    /// <param name="i"></param>
    void HighlightCurrentUserRank(DBRank rank, int i)
    {
            currentUserItem.Rank.text = "Votre place : " + (i + 1).ToString() + ".";
            currentUserItem.Name.text = rank.Username;
            currentUserItem.Fragments.text = rank.TotalFragments.ToString();
    }

}
