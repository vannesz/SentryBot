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
                Console.WriteLine("3:");
                Console.WriteLine("E: Exit");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        lowerTempThreshold = SetupTemp(myFinch);
                        upperLightThreshold = SetupLight(myFinch);
                        break;
                    case "2":
                        ActivateSentryBot(lowerTempThreshold, myFinch);
                        break;
                    case "3":
                        break;
                    case "E":
                    case "e":
                        keepGoing = false;
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
            double.TryParse(Console.ReadLine(), out temperatureDifference);

            ambientTemp = myFinch.getTemperature();

            lowerTempThreshold = ambientTemp - temperatureDifference;

            Pause();
            Console.Clear();

            return lowerTempThreshold;
        }

        static double SetupLight(Finch myFinch)
        {
            double lightDifference;
            double upperLightThreshold;
            double ambientLight;
            int[] ambientLightArray = new int[1];

            myFinch.connect();

            Console.WriteLine("Set Up The SentryBot");
            Console.WriteLine();
            Console.WriteLine("Enter Temperature Change");
            double.TryParse(Console.ReadLine(), out lightDifference);

            ambientLightArray = myFinch.getLightSensors();
            ambientLight = ambientLightArray.Average();

            upperLightThreshold = ambientLight + lightDifference;

            Pause();
            Console.Clear();

            return upperLightThreshold;
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
            // have the if/else check for light level average

            if (myFinch.getTemperature() >= upperLightThreshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void ActivateSentryBot(double lowerTempThreshold, Finch myFinch)
        {
            Console.Clear();
            Console.WriteLine("Monitoring Temperature");
            while (!TempBelowThreshold(lowerTempThreshold, myFinch))
            {
                FlashNormalTempLigh(myFinch);
            }
        }

        static void FlashNormalTempLigh(Finch myFinch)
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
