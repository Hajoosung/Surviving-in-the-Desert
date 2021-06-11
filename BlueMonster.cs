using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMonster : WeakAnimal
{
   


    

    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }
    private void RandomAction()
    {
        RandomSound();
        int _random = Random.Range(0, 3); // 대기, 두리번, 걷기.

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();

        else if (_random == 2)
            TryWalk();
    }

 

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("풀뜯기");
    }
    
}