using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs; // ĳ���� ������ �迭
    public int[] gradeChances; // �� ����� Ȯ�� (0: Common, 1: Rare, 2: Epic, 3: Legendary, 4: Mythical, 5: Transcendent)
    public Transform spawnPoint; // ĳ���� ��ȯ ��ġ
    public Button spawnButton; // ��ȯ ��ư
    public int spawnCost = 50; // ��ȯ ���

    private GameObject spawnedCharacter;

    void Start()
    {
        spawnButton.onClick.AddListener(TrySpawnCharacter);
    }

    void TrySpawnCharacter()
    {
        if (ResourceManager.Instance.Gold >= spawnCost)
        {
            ResourceManager.Instance.AddGold(-spawnCost);
            SpawnCharacter();
        }
        else
        {
            Debug.Log("Not enough gold to spawn character.");
        }
    }

    void SpawnCharacter()
    {
        int randomIndex = Random.Range(0, characterPrefabs.Length);
        GameObject characterPrefab = characterPrefabs[randomIndex];
        spawnedCharacter = Instantiate(characterPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody rb = spawnedCharacter.GetComponent<Rigidbody>();
        rb.useGravity = true; // �߷��� �����Ͽ� ���������� ����

        CharacterController characterController = spawnedCharacter.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false; // �ʱ⿡�� ���� �Ұ����ϰ� ����
            characterController.grade = GetRandomGrade(); // ���� ��� ����

        }
        else
        {
            Debug.LogError("CharacterController component not found on " + characterPrefab.name);
        }
    }

    CharacterGrade GetRandomGrade()
    {
        int totalChance = 0;
        foreach (int chance in gradeChances)
        {
            totalChance += chance;
        }

        int randomValue = Random.Range(0, totalChance);
        int cumulativeChance = 0;

        for (int i = 0; i < gradeChances.Length; i++)
        {
            cumulativeChance += gradeChances[i];
            if (randomValue < cumulativeChance)
            {
                return (CharacterGrade)i;
            }
        }

        return CharacterGrade.Common; // �⺻ ���
    }


}
