﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject buildingPrefab;

    public Transform player;
    public float maxDistanceFromCenter = 7f;

    public Sprite[] groundSprites;
    public int maxWidth = 25;
    public int maxHeight = 25;

    public int key = 0xFF;

    private Grid<Tile> grid;

    #region Unity API

    void Start()
    {
        grid = new Grid<Tile>(maxWidth, maxHeight);

        InitializeGrid();
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) > maxDistanceFromCenter)
        {
            RedrawGrid();
        }
    }

    void LateUpdate()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), Time.deltaTime * 3.0f);
    }

    #endregion

    void InitializeGrid()
    {
        var offset = new Vector3(0 - maxWidth / 2, 0 - maxHeight / 2, 0);
        grid.ForEach((x, y) =>
        {
            var tile = new GameObject();
            tile.transform.position = new Vector3(x, y, 0) + offset;

            var spriteRenderer = tile.gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = groundSprites[RandomHelper.Range(x, y, key, groundSprites.Length)];

            tile.name = $"Ground {x},{y}";
            tile.transform.parent = transform;

            grid.SetCell(x, y, new Tile { gameObject = tile, spriteRenderer = spriteRenderer });
        });
    }

    void RedrawGrid()
    {
        transform.position = new Vector3((int)player.position.x, (int)player.position.y, 0);
        var offset = new Vector3(transform.position.x - maxWidth / 2, transform.position.y - maxHeight / 2);

        grid.ForEach((x, y) =>
        {
            var tile = grid.GetCell(x, y);
            tile.spriteRenderer.sprite = groundSprites[RandomHelper.Range((int)offset.x + x, (int)offset.y + y, key, groundSprites.Length)];

            tile.gameObject.name = $"Ground {offset.x + x},{ offset.y + y}";
        });
    }
}