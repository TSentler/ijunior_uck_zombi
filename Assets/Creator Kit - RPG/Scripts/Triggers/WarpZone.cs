using RPGM.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZone : MonoBehaviour
{
    [Header("DestinationWarpZone")]
    /// warp destination
    [SerializeField] private WarpZone _destination;
    private List<Transform> _ignoreList = new List<Transform>();
    [SerializeField] private Vector2 _moveDirection; //zero = any direction


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the object that collides with the teleporter is on its ignore list, we do nothing and exit.
        if (_ignoreList.Contains(collision.transform))
        {
            return;
        }

        if (collision.TryGetComponent<CharacterController2D>(out var character) //Если игрок
            && (_moveDirection == Vector2.zero //направление не указано
                || Mathf.Approximately(1f, Vector2.Dot(_moveDirection, character.nextMoveCommand.normalized)))) //игрок движется в указанную сторону
        {
            Warp(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }

    private void Warp(Collider2D collision)
    {
        if (_destination == null)
            return;

        Camera.main.transform.position = collision.transform.position = _destination.transform.position;
        _ignoreList.Remove(collision.transform);
        _destination.AddToIgnoreList(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_ignoreList.Contains(collision.transform))
        {
            _ignoreList.Remove(collision.transform);
        }
    }

    /// <summary>
    /// Adds an object to the ignore list, which will prevent that object to be moved by the teleporter while it's in that list
    /// </summary>
    /// <param name="objectToIgnore">Object to ignore.</param>
    private void AddToIgnoreList(Transform objectToIgnore) => 
        _ignoreList.Add(objectToIgnore);
}
