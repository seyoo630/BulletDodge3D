using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs; // 캐릭터 프리팹 배열
    public int[] gradeChances; // 각 등급의 확률 (0: Common, 1: Rare, 2: Epic, 3: Legendary, 4: Mythical, 5: Transcendent)
    public Transform spawnPoint; // 캐릭터 소환 위치
    public Button spawnButton; // 소환 버튼
    public int spawnCost = 50; // 소환 비용

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
        rb.useGravity = true; // 중력을 적용하여 떨어지도록 설정

        CharacterController characterController = spawnedCharacter.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false; // 초기에는 조작 불가능하게 설정
            characterController.grade = GetRandomGrade(); // 랜덤 등급 설정

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

        return CharacterGrade.Common; // 기본 등급
    }


}
