using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoonTutorial : MonoBehaviour
{
    [SerializeField] Canvas tutorialCanvas;
    [SerializeField] Image tutorialBaloon;
    [SerializeField] TMP_Text tutorialText;

    Color tutBaloonStartingColor;
    Color tutTextStartingColor;

    [SerializeField] float fadeOutTime = 0f;
    float elapsedTime = 0f;

    private void Start()
    {
        tutBaloonStartingColor = tutorialBaloon.color;
        tutTextStartingColor = tutorialText.faceColor;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPoint = GetMousePosition();
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0);
            if (hit && hit.transform.gameObject.Equals(this.gameObject))
            {
                StartCoroutine(FadeOut());
            }
        }

    }

    private IEnumerator FadeOut()
    {
        while (elapsedTime < fadeOutTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);
            Color tutBaloonColor = new Color(tutBaloonStartingColor.r, tutBaloonStartingColor.g,
                tutBaloonStartingColor.b, alpha);
            Color textBaloonColor = new Color(tutTextStartingColor.r, tutTextStartingColor.g,
                tutTextStartingColor.b, alpha);
            tutorialBaloon.color = tutBaloonColor;
            tutorialText.faceColor = textBaloonColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        tutorialCanvas.gameObject.SetActive(false);
        yield return null;
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }
}
