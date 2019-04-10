using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinchAPI;

namespace SentryBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Zach Van Nes
            // Finch Sentry Bot
            // April 8th 2019

            Menu();
        }

        static void Menu()
        {
            Finch myFinch = new Finch(); 

            bool keepGoing = true;
            string menuChoice;
            double lowerTempThreshold = 0;
            double upperLightThreshold = 0;

            while (keepGoing == true)
            {
                Console.WriteLine("SentryBot Menu");
                Console.WriteLine();
                Console.WriteLine("1: Set Up SentryBot");
                Console.WriteLine("2: Activate SentryBot");
                Console.WriteLine("E: Exit");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        lowerTempThreshold = SetupTemp(myFinch);
                        upperLightThreshold = SetupLight(myFinch);
                        Console.Clear();
                        break;
                    case "2":
                        ActivateSentryBot(lowerTempThreshold, upperLightThreshold, myFinch);
                        break;                    
                    case "E":
                    case "e":
                        keepGoing = false;
                        Console.WriteLine("Thank You For Using The Application");
                        Pause();
                        myFinch.disConnect();
                        break;
                    default:
                        Console.WriteLine("Please Enter A Valid Menu Choice");
                        Pause();
                        Console.Clear();
                        break;
                }
            }
            
        }

        static double SetupTemp(Finch myFinch)
        {
            double temperatureDifference;
            double lowerTempThreshold;
            double ambientTemp;

            myFinch.connect();

            Console.WriteLine("Set Up The SentryBot");
            Console.WriteLine();
            Console.WriteLine("Enter Temperature Change");
            while (! double.TryParse(Console.ReadLine(), out temperatureDifference))
            {
                Console.WriteLine("Please Enter A Valid Response");
                Console.Clear();
            }
            ambientTemp = myFinch.getTemperature();

            lowerTempThreshold = ambientTemp - temperatureDifference;

            return lowerTempThreshold;
        }

        static double SetupLight(Finch myFinch)
        {
            double lightDifference;
            double upperLightThreshold;
            double ambientLight;

            Console.WriteLine();
            Console.WriteLine("Enter Light Change");
            
            while (!double.TryParse(Console.ReadLine(), out lightDifference))
            {
                Console.WriteLine("Please Enter A Valid Response");
                Console.Clear();
            }
            ambientLight = CalculateAmbientLight(myFinch);

            upperLightThreshold = ambientLight + lightDifference;

            Pause();


            return upperLightThreshold;

        }

        static double CalculateAmbientLight(Finch myFinch)
        {
            double ambientLight;
            int[] ambientLightArray = new int[1];

            ambientLightArray = myFinch.getLightSensors();
            ambientLight = ambientLightArray.Average();

            return ambientLight;
        }

        static bool TempBelowThreshold(double lowerTempThreshold, Finch myFinch)
        {
            if (myFinch.getTemperature() <= lowerTempThreshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool LightAboveThreshold(double upperLightThreshold, Finch myFinch)
        {
            double ambientLight = CalculateAmbientLight(myFinch);

            if (ambientLight >= upperLightThreshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void ActivateSentryBot(double lowerTempThreshold, double upperLightThreshold, Finch myFinch)
        {
            bool currentTemp= false;
            bool currentLight = false;

            Console.Clear();
            Console.WriteLine("Monitoring Temperature And Light");
            while (!TempBelowThreshold(lowerTempThreshold, myFinch) && !LightAboveThreshold(upperLightThreshold, myFinch))
            {
                FlashNormalLigh(myFinch);

                currentTemp = TempBelowThreshold(lowerTempThreshold, myFinch);
                currentLight = LightAboveThreshold(upperLightThreshold, myFinch);
            }

            myFinch.noteOn(750);
            myFinch.setLED(0, 0, 255);
            Console.WriteLine("Press Any Key To Turn Off Alarm");
            Console.ReadKey();
            Console.WriteLine();
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            if (currentTemp == true)
            {
                Console.WriteLine("Tempurture Dropped Below Threshold");
                Pause();
                Console.Clear();
            }
            else if (currentLight == true)
            {
                Console.WriteLine("Light Went Above Threshold");
                Pause();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("An Error Occured");
                Pause();
                Console.Clear();
            }


        }

        static void FlashNormalLigh(Finch myFinch)
        {
            myFinch.setLED(0, 255, 0);
            myFinch.wait(500);
            myFinch.setLED(0, 0, 0);
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();
        }
    }
}
