using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassicProgressBar : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color m_MainColor = Color.white;
    [SerializeField] private Color m_FillColor = Color.green;

    [Header("General")]
    [SerializeField] private int m_NumberOfSegments = 5;
    [SerializeField] private float m_SizeOfNotch = 5;
    [Range(0, 1f)][SerializeField] private float m_FillAmount = 0.0f;

    private RectTransform m_RectTransform;
    private Image m_Image;
    private Image m_FillImage; // Fill 이미지
    private List<Image> m_ProgressToFill = new List<Image>();
    private float m_SizeOfSegment;

    public void Awake()
    {
        // get rect transform
        m_RectTransform = GetComponent<RectTransform>();

        // get image
        m_Image = transform.Find("Image").GetComponent<Image>();
        m_Image.color = m_MainColor;

        // get fill image
        m_FillImage = transform.Find("Fill").GetComponent<Image>();
        m_FillImage.color = m_FillColor;
        m_ProgressToFill.Add(m_FillImage);

        // count size of segments
        m_SizeOfSegment = m_RectTransform.sizeDelta.x / m_NumberOfSegments;

        // Create additional segments if necessary
        for (int i = 1; i < m_NumberOfSegments; i++)
        {
            GameObject currentSegment = Instantiate(m_Image.gameObject, transform);
            currentSegment.SetActive(true);

            Image segmentImage = currentSegment.GetComponent<Image>();
            segmentImage.fillAmount = m_SizeOfSegment;

            RectTransform segmentRectTransform = segmentImage.GetComponent<RectTransform>();
            segmentRectTransform.sizeDelta = new Vector2(m_SizeOfSegment, segmentRectTransform.sizeDelta.y);
            segmentRectTransform.localPosition += new Vector3(i * (m_SizeOfSegment + m_SizeOfNotch), 0, 0) - new Vector3(m_SizeOfSegment * (m_NumberOfSegments / 2), 0, 0);

            Transform fillTransform = currentSegment.transform.Find("Fill");
            if (fillTransform != null)
            {
                Image segmentFillImage = fillTransform.GetComponent<Image>();
                segmentFillImage.color = m_FillColor;
                m_ProgressToFill.Add(segmentFillImage);
                segmentFillImage.GetComponent<RectTransform>().sizeDelta = new Vector2(m_SizeOfSegment, segmentFillImage.GetComponent<RectTransform>().sizeDelta.y);
            }
            else
            {
                Debug.LogError("Segment does not have a child image for filling");
            }
        }
    }

    public void Update()
    {
        for (int i = 0; i < m_NumberOfSegments; i++)
        {
            if (i < m_ProgressToFill.Count)
            {
                m_ProgressToFill[i].fillAmount = Mathf.Clamp01(m_NumberOfSegments * m_FillAmount - i);
            }
        }
    }

    public void SetFillAmount(float fillAmount)
    {
        m_FillAmount = fillAmount;
    }

    public void SetFillColor(Color color)
    {
        m_FillColor = color;
        foreach (var segment in m_ProgressToFill)
        {
            segment.color = color;
        }
    }
}
