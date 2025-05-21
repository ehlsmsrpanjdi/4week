using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    Camera mainCamera;
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 10f))
        {
            if(hit.collider.gameObject.CompareTag("Item") == true)
            {
                ItemData data = hit.collider.GetComponent<SpawnItem>()._ItemData;
                UIManager.Instance.SetText(data);
            }
            else
            {
                UIManager.Instance.ResetText();
            }
        }
        else
        {
            UIManager.Instance.ResetText();
        }
    }

}
