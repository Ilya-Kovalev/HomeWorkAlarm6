using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Alarm : MonoBehaviour
{
    [SerializeField] private AudioSource _alarm;

    private float _startVolume = 0;
    private float _maxVolume = 1;
    private float _duration = 3;
    private Coroutine _currentCorutine = null;

    private void OnTriggerEnter2D()
    {
        _alarm.volume = _startVolume;
        _alarm.PlayOneShot(_alarm.clip);

        if(_currentCorutine != null) 
        {
            StopCoroutine(_currentCorutine);
            _currentCorutine = StartCoroutine(DetermineSound(_maxVolume));
        }
        else 
        {
            _currentCorutine = StartCoroutine(DetermineSound(_maxVolume));
        }       
    }

    private void OnTriggerExit2D()
    {
        if(_currentCorutine != null) 
        {
            StopCoroutine(_currentCorutine);
            _currentCorutine = StartCoroutine(DetermineSound(_startVolume));         
        }
    }    

    private float FindNumberOfChanges(float Duration) 
    {
        float numberOfChanges = Duration / Time.deltaTime;
        return numberOfChanges;
    }
   
    private IEnumerator DetermineSound(float target) 
    {
        float numberOfChanges = FindNumberOfChanges(_duration);
        float sizeOfChange = _maxVolume / numberOfChanges;
        int actionPointer;

        if(target == _startVolume) 
        {
            actionPointer = -1;
        }
        else 
        {
            actionPointer = 1;
        }

        while (_alarm.volume != target) 
        {
           _alarm.volume += sizeOfChange * actionPointer;         

            yield return null;
        }         

        if(target == _startVolume && _alarm.volume == _startVolume) 
        {
            _alarm.Stop();
        }
    }
}

