﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace ParameterLister
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> BagOfCandies = new List<string>();
            if (args.Length > 0)
            {
                Console.WriteLine("Hoy Captain! We have some parameter there!");
                if (args.Length == 1)
                {
                    Console.WriteLine("Urm Captain, only one in fact!");
                    string sadNSoloParameter = args[0];
                    if (IsValidPath(sadNSoloParameter).IsFile)
                    {
                        Console.WriteLine("That was a file in fact!");
                        string[] BagOfBiscuit = File.ReadAllLines(sadNSoloParameter);
                        foreach (var Twix in BagOfBiscuit)
                        {
                            BagOfCandies.Add(Twix);
                        }
                    }
                    else
                    {
                        Console.WriteLine("... Only one link there!");
                        BagOfCandies.Add(sadNSoloParameter);
                    }
                }
                else
                {
                    Console.WriteLine("Captain! Many parameter there!");
                    var TheCrewOfParam = args;
                    foreach (var soloParam in TheCrewOfParam)
                    {
                        Console.WriteLine("They even put file on it!");
                        if (IsValidPath(soloParam).IsFile)
                        {
                            string[] BagOfBiscuit = File.ReadAllLines(soloParam);
                            foreach (var Mars in BagOfBiscuit)
                            {
                                BagOfCandies.Add(Mars);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Tiny link there!");
                            BagOfCandies.Add(soloParam);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Print URL list (seperated by space) or file with link you wanna scan here:");
                string userGivingMeTheInfo = Console.ReadLine();
                string[] CrewOfUserGaveInfo = userGivingMeTheInfo.Split(" ");
                foreach (string IAmBoredOfUsingVarName in CrewOfUserGaveInfo)
                {
                    if (IsValidPath(IAmBoredOfUsingVarName).IsFile)
                    {
                        string[] BagOfBiscuit = File.ReadAllLines(IAmBoredOfUsingVarName);
                        foreach (var Mars in BagOfBiscuit)
                        {
                            BagOfCandies.Add(Mars);
                        }
                    }
                    else
                        BagOfCandies.Add(IAmBoredOfUsingVarName);
                }
            }
            string pattern = @"(?:(http(?:s|)\:\/\/(?:[^?]*))|)(?:\&|\?)([^=]*)\=([^&]*)";
            /*
             * (?:(http(?:s|)\:\/\/(?:[^?]*))|) --> Capture the url
             * (?:\&|\?)([^=]*)\= --> Capture the argument
             * ([^&]*) --> capture the value
             */
            RegexOptions options = RegexOptions.Multiline;


            var argList = new List<string>();
            var valList = new List<string>();
            var redudancyList = new List<int>();

            /*TODO:
             * Rearrange data
             * So I'll be able to display data correctly. Right now I've no clue on how to 
             * draw gathered info...
            */

            foreach (string candie in BagOfCandies)
            {
                var Lollipops = Regex.Matches(candie, pattern, options);
                for (int i = 0; i < Lollipops.Count; i++)
                {
                    Match lollipop = Lollipops[i];
                    if (argList.Contains(lollipop.Groups[2].Value))
                    {
                        int j = 0;
                        while (argList[j] != lollipop.Groups[2].Value)
                        {
                            j++;
                        }
                        if (j != 0)
                        {
                            valList[j] += "," + lollipop.Groups[3].Value;
                            redudancyList[j] += 1;
                        }
                    }
                    else
                    {
                        argList.Add(lollipop.Groups[2].Value);
                        valList.Add(lollipop.Groups[3].Value);
                        redudancyList.Add(1);
                    }
                }
            }
            //Write to file
        }

        static (string URI, bool IsAbsoluteUri, bool IsFile) IsValidPath(string path) //based on drzaus on stackoverflow
        {
            Uri u;
            try
            {
                u = new Uri(path);
            }
            catch (Exception)
            {
                return (
                    URI: path,
                    IsAbsoluteUri: false,
                    IsFile: true
                );
            }

            return (
                    URI: path,
                    u.IsAbsoluteUri,
                    u.IsFile
                );
        }
    }
}