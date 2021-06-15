using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBallComponent : BaseItem
{
    private Transform ball;

    private void Awake() {
        ball = transform.Find("Ball").transform;
    }

    protected override void OnItemStateChanged() {
        Debug.Log(gameObject.name + " itemState changed: " + itemStats.state);
        switch(itemStats.state) {
            case ItemState.USED:
                ball.GetComponent<Renderer>().material.SetColor("_TintColor", Color.yellow);
                break;
            case ItemState.DECAYING:
                ball.GetComponent<Renderer>().material.SetColor("_TintColor", Color.green);
                break;
        }
    }
}
