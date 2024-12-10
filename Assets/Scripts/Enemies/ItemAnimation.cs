using UnityEngine;
using System.Collections;

public class ItemAnimation : MonoBehaviour
{
    public bool isAnimated = false;

    public bool isRotating = false;
    public bool isFloating = false;
    public bool isScaling = false;

    public Vector3 rotationAngle;
    public float rotationSpeed;

    public float floatSpeed;
    private bool goingUp = true;
    public float floatRate;
    private float floatTimer;

    public Vector3 startScale;
    public Vector3 endScale;

    private bool scalingUp = true;
    public float scaleSpeed;
    public float scaleRate;
    private float scaleTimer;

    private ItemType itemType; // 아이템 타입 (골드 또는 젬)
    private int amount; // 아이템 수량

    void Start()
    {
        StartCoroutine(DestroyAfterTime(3f)); // 3초 후 아이템 파괴
    }

    void Update()
    {
        if (isAnimated)
        {
            if (isRotating)
            {
                transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
            }

            if (isFloating)
            {
                floatTimer += Time.deltaTime;
                Vector3 moveDir = new Vector3(0.0f, 0.0f, floatSpeed);
                transform.Translate(moveDir);

                if (goingUp && floatTimer >= floatRate)
                {
                    goingUp = false;
                    floatTimer = 0;
                    floatSpeed = -floatSpeed;
                }

                else if (!goingUp && floatTimer >= floatRate)
                {
                    goingUp = true;
                    floatTimer = 0;
                    floatSpeed = +floatSpeed;
                }
            }

            if (isScaling)
            {
                scaleTimer += Time.deltaTime;

                if (scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
                }
                else if (!scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
                }

                if (scaleTimer >= scaleRate)
                {
                    scalingUp = !scalingUp;
                    scaleTimer = 0;
                }
            }
        }
    }

    public void Initialize(ItemType type, int amt)
    {
        itemType = type;
        amount = amt;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AddResource();
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        AddResource();
        Destroy(gameObject);
    }

    private void AddResource()
    {
        if (itemType == ItemType.Gold)
        {
            ResourceManager.Instance.AddGold(amount);
        }
        else if (itemType == ItemType.Gem)
        {
            ResourceManager.Instance.AddGem(amount);
        }
    }
}
