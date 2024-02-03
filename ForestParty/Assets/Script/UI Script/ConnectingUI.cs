using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{

    private void Start()
    {
        ForestPartyMultiplayer.Instance.OnTryingToJoin += ForestPartyMultiplayer_OnTryingToJoin;
        ForestPartyMultiplayer.Instance.OnFailedToJoin += ForestPartyMultiplayer_OnFailedToJoin;
        Hide();
    }

    private void ForestPartyMultiplayer_OnFailedToJoin(object sender, EventArgs e)
    {
        Hide();
    }

    private void ForestPartyMultiplayer_OnTryingToJoin(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        ForestPartyMultiplayer.Instance.OnTryingToJoin -= ForestPartyMultiplayer_OnTryingToJoin;
        ForestPartyMultiplayer.Instance.OnFailedToJoin += ForestPartyMultiplayer_OnFailedToJoin;
    }
}
