using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CanvasClocks
{
    public class Clock : MonoBehaviour
    {
        [Header("Text Displays")]
        [SerializeField]
        private Text hoursText;
        [SerializeField]
        private Text minutesText;
        [SerializeField]
        private Text secondsText;

        [Header("Audio")]
        [SerializeField]
        private AudioSource audioSource;

        string hourString;
        string minuteString;
        string secondString;

        private int clockHours = 0;
        private int clockMinutes = 0;
        //Seconds are decremented by update loop, so they have to be a float.
        private float clockSeconds = 0;

        public enum ClockMode { Clock, StopWatch, CountDown };
        [HideInInspector]
        public ClockMode currentMode = ClockMode.Clock;

        private bool paused = false;

        //Methods can will be called by UI buttons to alter variables.
        #region ButtonOnClickedMethods
        public void AlterHours(int change)
        {
            clockHours += change;
            if (clockHours >= 24) clockHours = 0;
            if (clockHours < 0) clockHours = 23;
            UpdateTimeDisplay();
        }

        public void AlterMinutes(int change)
        {
            clockMinutes += change;
            if (clockMinutes >= 60) clockMinutes = 0;
            if (clockMinutes < 0) clockMinutes = 59;
            UpdateTimeDisplay();
        }

        public void AlterSeconds(int change)
        {
            clockSeconds += change;
            if (clockSeconds >= 60) clockSeconds = 0;
            if (clockSeconds < 0) clockSeconds = 59;
            UpdateTimeDisplay();
        }

        public void SetToTimeMode()
        {
            currentMode = ClockMode.Clock;
        }

        public void SetToStopWatch()
        {
            currentMode = ClockMode.StopWatch;
            ResetTimer();
            paused = true;
        }

        public void SetToCountDown()
        {
            currentMode = ClockMode.CountDown;
            ResetTimer();
            paused = true;
        }

        public void StartTimer()
        {
            paused = false;
        }

        public void StopTimer()
        {
            paused = true;
        }

        public void ResetTimer()
        {
            clockHours = 0;
            clockMinutes = 0;
            clockSeconds = 0;
            UpdateTimeDisplay();
        }

        public void SetTimeToCurrent()
        {
            clockHours = System.DateTime.Now.Hour;
            clockMinutes = System.DateTime.Now.Minute;
            clockSeconds = System.DateTime.Now.Second;
            UpdateTimeDisplay();
        }

        public void RemoveClock()
        {
            //Reset the clock values for future re-use
            ResetTimer();
            //Notify the clock spawner via a static method that a clock has been disabled.
            ClockSpawner.OnClockRemoved();
        }

        #endregion
        /// <summary>
        /// Run the logic and update relevant displays.
        /// </summary>
        private void UpdateTimeDisplay()
        {
            //Get string version of hours and adjust it with a 0 if neccessary.
            hourString = clockHours.ToString();
            hoursText.text = (hourString.Length == 1) ? "0" + hourString : hourString;
            //Same process for minutes
            minuteString = clockMinutes.ToString();
            minutesText.text = (minuteString.Length == 1) ? "0" + minuteString : minuteString;
            //Same process for seconds
            secondString = Mathf.FloorToInt(clockSeconds).ToString();
            secondsText.text = (secondString.Length == 1) ? "0" + secondString : secondString;
        }

        private void Update()
        {
            if (!paused)
            {
                //Increment the clock for clock and stopwatch modes.
                if (currentMode != ClockMode.CountDown)
                {
                    clockSeconds += Time.deltaTime;
                    if (clockSeconds >= 60)
                    {
                        clockSeconds = 0;
                        //Prevent clock overflow by checking if maximum value has been reached for stopwatch use.
                        if (currentMode == ClockMode.StopWatch && clockHours == 23 && clockMinutes == 59)
                        {
                            clockSeconds = 59;
                            return;
                        }
                        //Increment the minutes.
                        clockMinutes += 1;
                        if (clockMinutes >= 60)
                        {
                            clockMinutes = 0;
                            clockHours += 1;
                            //Reset clock hours if necessary.
                            if (clockHours > 23) clockHours = 0;
                        }
                    }
                }
                //Decrement clock for countdown.
                else
                {
                    clockSeconds -= Time.deltaTime;
                    if (clockSeconds < 0)
                    {
                        //Prevent clock overflow by checking if maximum value has been reached.
                        if (clockHours == 0 && clockMinutes == 0)
                        {
                            clockSeconds = 0;
                            //Play Sound
                            audioSource.Play();
                            paused = true;
                            return;
                        }
                        clockSeconds = 59;
                        //Decrement the minutes.
                        clockMinutes -= 1;
                        if (clockMinutes < 0)
                        {
                            clockMinutes = 59;
                            clockHours -= 1;
                        }
                    }
                }

                UpdateTimeDisplay();
            }
        }
    }
}

