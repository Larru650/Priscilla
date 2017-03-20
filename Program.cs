using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;



namespace Priscilla
{
    class Program
    {
        //create a speechsynth and make him talk, greeting the user
        private static SpeechSynthesizer synth = new SpeechSynthesizer(); //we make it static so we don't
                                                                      //have to create an instance      


        static void Main(string[] args)
        {//start of the program


            List<string> cpuMaxedOutMessages = new List<string>(); //the list is strongly typed
            cpuMaxedOutMessages.Add("WARNING: Your cpu is about to catch fire!");
            cpuMaxedOutMessages.Add("WARNING! You should not run your cpu that hard");
            cpuMaxedOutMessages.Add("WARNING: Stop, this is getting now serious!");
            cpuMaxedOutMessages.Add("RED ALERT! RED ALERT! RED ALERT! RED ALERT! ");

            //the dice
            Random rand = new Random();


            #region Greeting speech

            //synth.Speak("Welcome to Perfomance Counter v 0.1 Alpha");
            #endregion

            #region My Performance Counters
            //This will pull the current CPU load in percentage
            PerformanceCounter PerfCpuCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            PerfCpuCounter.NextValue();

            //This will pull the current available mem in mb
            PerformanceCounter PerfMemCounter = new PerformanceCounter("Memory", "Available MBytes"); //categories inside parenthesis are 
            PerfMemCounter.NextValue();                            
                                                                  
           
            //This will get us the system up time since it was last powered on (in seconds)
            PerformanceCounter PerfUpTime = new PerformanceCounter("System", "System Up Time");
            PerfUpTime.NextValue();
            #endregion

            #region Methods
            TimeSpan upTimeSpan = TimeSpan.FromSeconds(PerfUpTime.NextValue()); //creates the seconds from perfuptime to days,h,min,sec.

            string systemUpTimeMessage = string.Format("The current system up time is {0} days {1} hours {2} minutes {3} seconds", 
                   (int)upTimeSpan.TotalDays,
                   (int)upTimeSpan.Hours,
                   (int)upTimeSpan.Minutes,
                   (int)upTimeSpan.Seconds
                    );

            //tell the user what the current system uptime is
           Speak(systemUpTimeMessage, VoiceGender.Male, 4);

            int speechSpeed = 1;

            #endregion
            #region logic
            //loop


            bool isChromeAlreadyOpen = false;

            while (true)
            {

                

                //we have to declare the values that have been calculated from the first cpu and memory reports
                //to store them in the speaking string

                int currentCpuPercentage = (int)PerfCpuCounter.NextValue(); //casting int to convert them to integers
                int currentAvailableMemory = (int) PerfMemCounter.NextValue();
                




                //every 1 sec print the cpu load % to the screen
                Thread.Sleep(1000);
                Console.WriteLine("Cpu Load              : {0}%", currentCpuPercentage); 
                Console.WriteLine("Available Memory      : {0}MB", currentAvailableMemory);
                
                    
                    ;



                if (currentCpuPercentage > 50)

                    {

                    

                    if (currentCpuPercentage == 100)
                    {
                        if(speechSpeed < 3) //we will cap the speed to  5, basically we will only increase it
                                            //if its below 5
                        {
                            speechSpeed++;
                        }

                        

                      

                        

                        string cpuLoadVocalMessage = cpuMaxedOutMessages[rand.Next(5)];
                        Speak(cpuLoadVocalMessage, VoiceGender.Female, speechSpeed);//we use the created method of speak
                    }

                    

                    else {

                        string cpuLoadVocalMessage = string.Format("Your Cpu Level is {0} percent", currentCpuPercentage);
                        Speak(cpuLoadVocalMessage, VoiceGender.Male, 5);

                        if (isChromeAlreadyOpen == false)
                        {


                            OpenWebsite("https://www.youtube.com/watch?v=okd6cd6cFrA");
                            isChromeAlreadyOpen = true;
                        }

                    }
                }

                Thread.Sleep(1000);
                if (currentAvailableMemory < 1024)
                {

                    string memoryLoadVocalMessage = string.Format("You currently have {0} Gigabytes of memory available", currentAvailableMemory / 1024); //we want to convert it to GB for
                    Speak(memoryLoadVocalMessage,VoiceGender.Male);
                }
                //create strings for the synth to speak the values
                //the speech  
                //make synth speak                                                                                                                                  
                



            }
            //end of loop


            #endregion

        }
        /// <summary>
        /// speaks with a selected voice
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voicegender"></param>
        public static void Speak(string message, VoiceGender voicegender)
        {

            synth.SelectVoiceByHints(voicegender);
            synth.Speak(message);


        }
        /// <summary>
        /// Speaks with a selected voice at a selected speed (Alberts speak v2.0)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voicegender"></param>
        /// <param name="rate"></param>
        public static void Speak(string message, VoiceGender voicegender, int rate)
        {
            synth.Rate = rate;
            Speak(message, voicegender);
            //with this new function we can now if we want add a 3rd variable into the speak method!
        }



        public static void OpenWebsite(string URL)
        {
            Process p1 = new Process();
            p1.StartInfo.FileName = "chrome.exe";
            p1.StartInfo.Arguments = URL;
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            p1.Start();
        }

    }
}
