﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Animator _animator = null;
    private int _horizontalHash = 0;
    private int _verticalHash = 0;
    private int _attackHash = 0;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _horizontalHash = Animator.StringToHash("Horizontal");
        _verticalHash = Animator.StringToHash("Vertical");
        _attackHash = Animator.StringToHash("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal") * 2.32f; // Max Turn Speed
        float yAxis = Input.GetAxis("Vertical") * 5.66f; // Run Speed

        if (Input.GetMouseButtonDown(0)) _animator.SetTrigger(_attackHash);

        _animator.SetFloat(_horizontalHash, xAxis, 0.1f, Time.deltaTime);
        _animator.SetFloat(_verticalHash, yAxis, 1.0f, Time.deltaTime);
    }
}
